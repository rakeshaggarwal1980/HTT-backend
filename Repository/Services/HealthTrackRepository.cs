using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
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
    }
}



