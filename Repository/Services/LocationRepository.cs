using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Ctor
        /// context injection\creation
        /// </summary>
        /// <param name="context"></param>
        public LocationRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns list of locations
        /// </summary>
        /// <returns></returns>
        public async Task<List<Location>> GetLocations()
        {
            return await _context.Location.ToListAsync();
        }
    }
}
