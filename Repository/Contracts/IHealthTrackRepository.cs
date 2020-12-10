using HTTAPI.Helpers;
using HTTAPI.Models;
using System;
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
        List<HealthTrack> GetAllDeclarations(SearchSortModel search);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<HealthTrack> GetDeclarationsByUserId(SearchSortModel search);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<HealthTrack> GetDeclarations(SearchSortModel search);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="covidHealthTrack"></param>
        /// <returns></returns>
        Task<CovidHealthTrack> CreateCovidHealthTrack(CovidHealthTrack covidHealthTrack);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="confirmationDate"></param>
        /// <returns></returns>
        Task<CovidHealthTrack> GetExistingCovidDeclaration(DateTime confirmationDate, int employeeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<CovidHealthTrack> GetCovidDeclarations(SearchSortModel search);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeedId"></param>
        /// <returns></returns>
        Task<List<CovidHealthTrack>> GetCovidDeclarationByEmployee(int employeedId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="declarationId"></param>
        /// <returns></returns>
        Task<List<CovidHealthTrack>> GetCovidDeclaration(int declarationId);
    }
}
