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
    /// 
    /// </summary>
    public class RequestRepository : IRequestRepository
    {
        private readonly Context _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public RequestRepository(Context context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ComeToOfficeRequest> CreateRequest(ComeToOfficeRequest request)
        {
            _context.ComeToOfficeRequest.Add(request);
            await _context.SaveChangesAsync();

            _context.Entry(request).Reference(c => c.Employee).Load();
            return request;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ComeToOfficeRequest> UpdateRequest(ComeToOfficeRequest request)
        {
            var officeRequest = _context.ComeToOfficeRequest.Include(t => t.Employee).FirstOrDefault(req => req.Id == request.Id && req.Status == EntityStatus.Active);
            if (officeRequest == null)
            {
                throw new ArgumentException("Update failed: No such request found");
            }

            // other fields can not be edited
            officeRequest.IsApproved = request.IsApproved;
            officeRequest.IsDeclined = request.IsDeclined;
            officeRequest.Status = request.Status;
            await _context.SaveChangesAsync();

            return officeRequest;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ComeToOfficeRequest> GetRequestsList(SearchSortModel search)
        {

            // Employe can have only 1 request "Active" at a time
            // when entered office it is set to "finished"

            //return (roleId == 1 ? await _context.ComeToOfficeRequest.Include(r => r.Employee).ThenInclude(r => r.EmployeeRoles).
            //      Where(x => x.Status == EntityStatus.Active && x.EmployeeId != userId && (
            //      x.Employee.EmployeeRoles.Any(role => role.RoleId == 2 || role.RoleId == 3))).ToListAsync()
            //      : await _context.ComeToOfficeRequest.Include(r => r.Employee).ThenInclude(r => r.EmployeeRoles).
            //      Where(x => x.Status == EntityStatus.Active && x.EmployeeId != userId).ToListAsync());


            var query = _context.ComeToOfficeRequest.Include(r => r.Employee).ThenInclude(e => e.EmployeeRoles).AsQueryable();

            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "requestnumber")
            {
                query = query.Where(t => t.RequestNumber.ToLower().Contains(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "employeename")
            {
                query = query.Where(t => t.Employee.Name.ToLower().Contains(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "employeeid")
            {
                query = query.Where(t => t.Employee.EmployeeCode == Convert.ToInt32(search.SearchString));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "daterange")
            {
                string[] dates = search.SearchString.Split("(to)");
                query = query.Where(t => t.FromDate.Date >= Convert.ToDateTime(dates[0]).Date && (t.ToDate.Date == Convert.ToDateTime(dates[1]).Date || (t.FromDate.Date < Convert.ToDateTime(dates[1]).Date && t.ToDate.Date < Convert.ToDateTime(dates[1]).Date)));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "status")
            {
                if (search.SearchString.ToLower() == "pending")
                {
                    query = query.Where(t => t.IsApproved == false && t.IsDeclined == false && t.Status == EntityStatus.Active);
                }
                else if (search.SearchString.ToLower() == "approved")
                {
                    query = query.Where(t => t.IsApproved == true && t.IsDeclined == false && t.Status == EntityStatus.Active);
                }
                else if (search.SearchString.ToLower() == "declined")
                {
                    query = query.Where(t => t.IsApproved == false && t.IsDeclined == true && t.Status == EntityStatus.Active);
                }

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
                    else if (search.SortColumn.ToLower() == "fromdate")
                    {
                        query = query.OrderBy(dec => dec.FromDate);
                    }
                    else if (search.SortColumn == "todate")
                    {
                        query = query.OrderBy(dec => dec.ToDate);
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
                    else if (search.SortColumn.ToLower() == "fromdate")
                    {
                        query = query.OrderByDescending(dec => dec.FromDate);
                    }
                    else if (search.SortColumn == "todate")
                    {
                        query = query.OrderByDescending(dec => dec.ToDate);
                    }
                    else
                    {
                        query = query.OrderByDescending(dec => dec.CreatedDate);

                    }
                    break;
            }

            query = PagingExtensions.Page(query, search.Page, search.PageSize);

            var data = query
                .Select(request => new ComeToOfficeRequest
                {
                    Id = request.Id,
                    CreatedBy = request.CreatedBy,
                    FromDate = request.FromDate,
                    HRComments = request.HRComments,
                    IsApproved = request.IsApproved,
                    IsDeclined = request.IsDeclined,
                    ModifiedBy = request.ModifiedBy,
                    CreatedDate = request.CreatedDate,
                    ModifiedDate = request.ModifiedDate,
                    EmployeeId = request.EmployeeId,
                    RequestNumber = request.RequestNumber,
                    Status = request.Status,
                    ToDate = request.ToDate,
                    Employee = request.Employee
                })
                .ToList();

            return data;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ComeToOfficeRequest> GetRequestsListByUserId(SearchSortModel search)
        {

            //return await _context.ComeToOfficeRequest.Include("Employee").Where(x => x.EmployeeId == userId).ToListAsync();

            var query = _context.ComeToOfficeRequest.Include("Employee");

            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "requestnumber")
            {
                query = query.Where(t => t.RequestNumber.ToLower().Contains(search.SearchString));

            }

            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "daterange")
            {
                string[] dates = search.SearchString.Split("(to)");
                query = query.Where(t => t.FromDate.Date >= Convert.ToDateTime(dates[0]).Date && (t.ToDate.Date == Convert.ToDateTime(dates[1]).Date || (t.FromDate.Date < Convert.ToDateTime(dates[1]).Date && t.ToDate.Date < Convert.ToDateTime(dates[1]).Date)));

            }
            if (!string.IsNullOrEmpty(search.SearchString) && search.PropertyName.ToLower() == "status")
            {
                if (search.SearchString.ToLower() == "pending")
                {
                    query = query.Where(t => t.IsApproved == false && t.IsDeclined == false && t.Status == EntityStatus.Active);
                }
                else if (search.SearchString.ToLower() == "approved")
                {
                    query = query.Where(t => t.IsApproved == true && t.IsDeclined == false && t.Status == EntityStatus.Active);
                }
                else if (search.SearchString.ToLower() == "declined")
                {
                    query = query.Where(t => t.IsApproved == false && t.IsDeclined == true && t.Status == EntityStatus.Active);
                }

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
                    else if (search.SortColumn.ToLower() == "fromdate")
                    {
                        query = query.OrderBy(dec => dec.FromDate);
                    }
                    else if (search.SortColumn == "todate")
                    {
                        query = query.OrderBy(dec => dec.ToDate);
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
                    else if (search.SortColumn.ToLower() == "fromdate")
                    {
                        query = query.OrderByDescending(dec => dec.FromDate);
                    }
                    else if (search.SortColumn == "todate")
                    {
                        query = query.OrderByDescending(dec => dec.ToDate);
                    }
                    else
                    {
                        query = query.OrderByDescending(dec => dec.CreatedDate);

                    }
                    break;
            }

            query = PagingExtensions.Page(query, search.Page, search.PageSize);

            var data = query
                .Select(request => new ComeToOfficeRequest
                {
                    Id = request.Id,
                    CreatedBy = request.CreatedBy,
                    FromDate = request.FromDate,
                    HRComments = request.HRComments,
                    IsApproved = request.IsApproved,
                    IsDeclined = request.IsDeclined,
                    ModifiedBy = request.ModifiedBy,
                    CreatedDate = request.CreatedDate,
                    ModifiedDate = request.ModifiedDate,
                    EmployeeId = request.EmployeeId,
                    RequestNumber = request.RequestNumber,
                    Status = request.Status,
                    ToDate = request.ToDate,
                    Employee = request.Employee
                })
                .ToList();

            return data;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task<ComeToOfficeRequest> GetRequestById(int requestId)
        {
            return await _context.ComeToOfficeRequest.Include("Employee").Where(x => x.Id == requestId).SingleOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        public async Task<ComeToOfficeRequest> GetRequestByNumber(string requestNumber)
        {
            return await _context.ComeToOfficeRequest.Include("Employee").Where(x => x.RequestNumber == requestNumber).SingleOrDefaultAsync();
        }


        /// <summary>
        /// Checks if Employee has already submitted request
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<ComeToOfficeRequest>> GetRequestsByEmployee(int employeeId)
        {
            return await _context.ComeToOfficeRequest.Where(x => x.EmployeeId == employeeId
                                                            && x.Status == EntityStatus.Active
                                                            && x.ToDate.Date >= DateTime.Now.Date).ToListAsync();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<ComeToOfficeRequest>> GetRequestsByUserId(int employeeId)
        {

            return await _context.ComeToOfficeRequest.Include("Employee").Where(x => x.EmployeeId == employeeId).ToListAsync();
        }

    }

}