using HTTAPI.Enums;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
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
        /// <param name="employeedId"></param>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        //public async  Task<ComeToOfficeRequest> GetEmployeeDeclarationsRequestInfo(int employeedId, string requestNumber)
        //{
        //    var request = await (from ht in _context.HealthTrack
        //                  join cr in _context.ComeToOfficeRequest
        //                  on ht.RequestNumber equals cr.RequestNumber
        //                  where (ht.EmployeeId == employeedId) &&
        //                        (ht.Status == EntityStatus.Active) &&
        //                        (ht.RequestNumber == requestNumber)
        //                  select cr).ToListAsync();
        //    return request;
       
        //}


    }
}



