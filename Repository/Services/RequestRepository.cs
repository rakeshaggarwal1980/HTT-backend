using HTTAPI.Enums;
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
        public async Task<List<ComeToOfficeRequest>> GetRequestsList()
        {
            // Employe can have only 1 request "Active" at a time
            // when entered office it is set to "finished"
            return await _context.ComeToOfficeRequest.Include("Employee").Where(x => x.Status == EntityStatus.Active).ToListAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComeToOfficeRequest>> GetRequestsListByUserId(int userId)
        {
            return await _context.ComeToOfficeRequest.Include("Employee").Where(x => x.EmployeeId == userId).ToListAsync();
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
    }

}