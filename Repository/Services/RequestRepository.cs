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
            return request;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ComeToOfficeRequest> UpdateRequest(ComeToOfficeRequest request)
        {
            var officeRequest = _context.ComeToOfficeRequest.FirstOrDefault(req => req.Id == request.Id && req.Status == EntityStatus.Active);
            if (officeRequest == null)
            {
                throw new ArgumentException("Update failed: No such request found");
            }
            // other fields can not be edited
            officeRequest.IsApproved = request.IsApproved;
            officeRequest.IsDeclined = request.IsDeclined;
            officeRequest.Status = request.Status;
            await _context.SaveChangesAsync();
            return request;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComeToOfficeRequest>> GetRequestsList()
        {
            return await _context.ComeToOfficeRequest.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task<ComeToOfficeRequest> GetRequestById(int requestId)
        {
            return await _context.ComeToOfficeRequest.Where(x => x.Id == requestId).SingleOrDefaultAsync();
        }
    }

}