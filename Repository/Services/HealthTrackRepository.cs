using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Services
{
    /// <summary>
    ///HealthTrackRepository
    /// Here all method should be async
    /// </summary>
    public class HealthTrackRepository : IHealthTrackRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Ctor
        /// context injection\creation
        /// </summary>
        /// <param name="context"></param>
        public HealthTrackRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is used to Create Health Track
        /// </summary>       
        public async Task<HealthTrack> CreateHealthTrack(HealthTrack healthTrack)
        {
            _context.HealthTrack.Add(healthTrack);
            await _context.SaveChangesAsync();
            _context.Entry(healthTrack).Reference(c => c.Employee).Load();
            return healthTrack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeedId"></param>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        public async Task<List<HealthTrack>> GetSelfDeclarationByEmployeeForRequest(int employeedId, string requestNumber)
        {
            // request can have one or more declarations
            // when employee enters office , set entity status to finished
            return await _context.HealthTrack.Include("Employee").Include("HealthTrackSymptoms").Include("HealthTrackQuestions")
                .Where(x => x.EmployeeId == employeedId && x.Status == EntityStatus.Active
                && x.RequestNumber == requestNumber).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<HealthTrack>> GetAllDeclarations()
        {
            return await _context.HealthTrack.Include("Employee").Include(hs=>hs.HealthTrackSymptoms).ThenInclude(s=>s.Symptom)
                .Include(s=>s.HealthTrackQuestions).ThenInclude(s=>s.Question).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<HealthTrack> GetDeclarations(SearchSortModel search)
        {
            var query = _context.HealthTrack.Include("Employee").Include("Location").Include("Zone");

            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "employeename")
            {
                query = query.Where(t => t.Employee.Name.ToLower().Contains(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "employeeid")
            {
                query = query.Where(t => t.Id == Convert.ToInt32(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "date")
            {
                query = query.Where(t => t.CreatedDate == Convert.ToDateTime(search.SearchString));

            }

            search.TotalRecords = query.Count();

            switch (search.SortDirection.ToString().ToLower())
            {
                case "asc":
                    if (search.SortColumn == "id")
                    {
                        query = query.OrderBy(dec => dec.Id);
                    }
                    else if(search.SortColumn == "employeename")
                    {
                        query = query.OrderBy(dec => dec.Employee.Name);
                    }
                    else
                    {
                        query = query.OrderBy(dec => dec.CreatedDate);

                    }
                    break;
                default:
                    if (search.SortColumn == "id")
                    {
                        query = query.OrderByDescending(dec => dec.Id);
                    }
                    else if (search.SortColumn == "employeename")
                    {
                        query = query.OrderByDescending(dec => dec.Employee.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(dec => dec.CreatedDate);

                    }
                    break;
            }

            query = PagingExtensions.Page(query, search.Page, search.PageSize);

            var data = query
                .Select(declaration => new HealthTrack
                {
                    Id = declaration.Id,
                    ResidentialAddress = declaration.ResidentialAddress,
                    PreExistHealthIssue = declaration.PreExistHealthIssue,
                    ContactWithCovidPeople = declaration.ContactWithCovidPeople,
                    TravelOustSideInLast15Days = declaration.TravelOustSideInLast15Days,
                    RequestNumber = declaration.RequestNumber,
                    LocationId = declaration.LocationId,
                    CreatedDate = declaration.CreatedDate,
                    ZoneId = declaration.ZoneId,
                    EmployeeId = declaration.EmployeeId,
                    Employee = declaration.Employee
                })
                .ToList();

            return data;
        }

    }
}



