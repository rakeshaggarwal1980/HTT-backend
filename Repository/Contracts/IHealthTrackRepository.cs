using HTTAPI.Helpers;
using HTTAPI.Models;
using System.Collections.Generic;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeedId"></param>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        Task<List<HealthTrack>> GetSelfDeclarationByEmployeeForRequest(int employeedId, string requestNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<HealthTrack>> GetAllDeclarations();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<HealthTrack> GetDeclarations(SearchSortModel search);
    }
}
