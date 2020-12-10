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
        public List<HealthTrack> GetAllDeclarations(SearchSortModel search)
        {
            var query = _context.HealthTrack.Include(s => s.HealthTrackSymptoms).ThenInclude(s => s.Symptom).Include(q => q.HealthTrackQuestions).ThenInclude(q => q.Question).Include("Location").Include("Zone").Include(x => x.Employee).ThenInclude(x => x.EmployeeRoles).AsQueryable();

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
            if (search.userId != 0 && search.roleId != 0)
            {
                if (search.roleId == 1)
                {
                    query = query.Where(t => t.EmployeeId != search.userId && (
                     t.Employee.EmployeeRoles.Any(role => role.RoleId == 2 || role.RoleId == 3)));
                }

            }

            search.TotalRecords = query.Count();

            switch (search.SortDirection.ToString().ToLower())
            {
                case "asc":
                    if (search.SortColumn == "id")
                    {
                        query = query.OrderBy(dec => dec.Id);
                    }
                    else if (search.SortColumn == "employeename")
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
                    Employee = declaration.Employee,
                    HealthTrackQuestions = declaration.HealthTrackQuestions,
                    HealthTrackSymptoms = declaration.HealthTrackSymptoms
                })
                .ToList();

            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<HealthTrack> GetDeclarations(SearchSortModel search)
        {
            var query = _context.HealthTrack.Include(h => h.Employee).ThenInclude(e => e.EmployeeRoles).Include("Location").Include("Zone");

            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "employeename")
            {
                query = query.Where(t => t.Employee.Name.ToLower().Contains(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "employeeid")
            {
                query = query.Where(t => t.Employee.EmployeeCode == Convert.ToInt32(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "date")
            {
                query = query.Where(t => t.CreatedDate.Date == Convert.ToDateTime(search.SearchString).Date);

            }
            if (search.userId != 0 && search.roleId != 0)
            {
                if (search.roleId == 1)
                {
                    query = query.Where(t => t.EmployeeId != search.userId && (
                     t.Employee.EmployeeRoles.Any(role => role.RoleId == 2 || role.RoleId == 3)));
                }

            }

            search.TotalRecords = query.Count();

            switch (search.SortDirection.ToString().ToLower())
            {
                case "asc":
                    if (search.SortColumn == "id")
                    {
                        query = query.OrderBy(dec => dec.Id);
                    }
                    else if (search.SortColumn == "employeename")
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<HealthTrack> GetDeclarationsByUserId(SearchSortModel search)
        {
            var query = _context.HealthTrack.Include(h => h.Employee).Include("Location").Include("Zone");

            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "date")
            {
                query = query.Where(t => t.CreatedDate.Date == Convert.ToDateTime(search.SearchString).Date);

            }
            if (search.userId != 0)
            {
                query = query.Where(t => t.EmployeeId == search.userId);

            }

            search.TotalRecords = query.Count();

            switch (search.SortDirection.ToString().ToLower())
            {
                case "asc":
                    if (search.SortColumn == "id")
                    {
                        query = query.OrderBy(dec => dec.Id);
                    }
                    else if (search.SortColumn == "employeename")
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


        /// <summary>
        /// This method is used to Create Health Track
        /// </summary>       
        public async Task<CovidHealthTrack> CreateCovidHealthTrack(CovidHealthTrack covidHealthTrack)
        {
            _context.CovidHealthTrack.Add(covidHealthTrack);
            await _context.SaveChangesAsync();
            _context.Entry(covidHealthTrack).Reference(c => c.Employee).Load();
            return covidHealthTrack;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="confirmationDate"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<CovidHealthTrack> GetExistingCovidDeclaration(DateTime confirmationDate, int employeeId)
        {
            return await _context.CovidHealthTrack.Where(d => d.CovidConfirmationDate.Date == confirmationDate.Date && d.EmployeeId == employeeId).SingleOrDefaultAsync();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<CovidHealthTrack> GetCovidDeclarations(SearchSortModel search)
        {
            var query = _context.CovidHealthTrack.Include(h => h.Employee).ThenInclude(e => e.EmployeeRoles).Include("Location");

            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "employeename")
            {
                query = query.Where(t => t.Employee.Name.ToLower().Contains(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "employeeid")
            {
                query = query.Where(t => t.Employee.EmployeeCode == Convert.ToInt32(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "date")
            {
                query = query.Where(t => t.CreatedDate.Date == Convert.ToDateTime(search.SearchString).Date);

            }
            if (search.userId != 0 && search.roleId != 0)
            {
                if (search.roleId == 1)
                {
                    query = query.Where(t => t.EmployeeId != search.userId && (
                     t.Employee.EmployeeRoles.Any(role => role.RoleId == 2 || role.RoleId == 3)));
                }

            }

            search.TotalRecords = query.Count();

            switch (search.SortDirection.ToString().ToLower())
            {
                case "asc":
                    if (search.SortColumn == "id")
                    {
                        query = query.OrderBy(dec => dec.Id);
                    }
                    else if (search.SortColumn == "employeename")
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
                .Select(declaration => new CovidHealthTrack
                {
                    Id = declaration.Id,
                    CovidConfirmationDate = declaration.CovidConfirmationDate,
                    DateOfSymptoms = declaration.DateOfSymptoms,
                    FamilyMembersCount = declaration.FamilyMembersCount,
                    HospitalizationNeed = declaration.HospitalizationNeed,
                    OfficeLastDay = declaration.OfficeLastDay,
                    OthersInfectedInFamily = declaration.OthersInfectedInFamily,
                    Status = declaration.Status,
                    LocationId = declaration.LocationId,
                    CreatedDate = declaration.CreatedDate,
                    EmployeeId = declaration.EmployeeId,
                    Employee = declaration.Employee
                })
                .ToList();

            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeedId"></param>
        /// <returns></returns>
        public async Task<List<CovidHealthTrack>> GetCovidDeclarationByEmployee(int employeedId)
        {
            // request can have one or more declarations
            // when employee enters office , set entity status to finished
            return await _context.CovidHealthTrack.Include("Employee").Where(x => x.EmployeeId == employeedId && x.Status == EntityStatus.Active).ToListAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="declarationId"></param>
        /// <returns></returns>
        public async Task<List<CovidHealthTrack>> GetCovidDeclaration(int declarationId)
        {
            // request can have one or more declarations
            // when employee enters office , set entity status to finished
            return await _context.CovidHealthTrack.Include("Employee").Where(x => x.Id == declarationId && x.Status == EntityStatus.Active).ToListAsync();
        }
    }
}



