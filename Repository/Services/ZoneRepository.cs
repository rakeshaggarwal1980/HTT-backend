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
    public class ZoneRepository : IZoneRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Ctor
        /// context injection\creation
        /// </summary>
        /// <param name="context"></param>
        public ZoneRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns list of symptoms
        /// </summary>
        /// <returns></returns>
        public async Task<List<Zone>> GetZones()
        {
            return await _context.Zone.ToListAsync();
        }
    }
}
