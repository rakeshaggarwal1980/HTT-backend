using HTTAPI.Models;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Contracts
{
    /// <summary>
    /// HealthTrackRepository 
    /// </summary>
    public interface IHealthTrackRepository
    {

        /// <summary>
        ///  This method is used to save new health track
        /// </summary>
        /// <param name="healthTrack"></param>
        /// <returns></returns>
        Task<HealthTrack> CreateHealthTrack(HealthTrack healthTrack);

    }
}
