using HTTAPI.Enums;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
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
            return healthTrack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeedId"></param>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        public async Task<HealthTrack> GetSelfDeclarationByEmployeeForRequest(int employeedId, string requestNumber)
        {
            // when employee enters office , set entity status to finished
            return await _context.HealthTrack.Include("Employee").Include("HealthTrackSymptoms").Include("HealthTrackQuestions")
                .Where(x => x.EmployeeId == employeedId && x.Status == EntityStatus.Active
                && x.RequestNumber == requestNumber).SingleOrDefaultAsync();
        }

    }
}



