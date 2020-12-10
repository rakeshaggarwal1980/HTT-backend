using HTTAPI.Helpers;
using HTTAPI.ViewModels;
using System.Threading.Tasks;

namespace HTTAPI.Manager.Contract
{
    /// <summary>
    /// interface for ContentDetailService 
    /// </summary>
    public interface IHealthTrackService
    {

        /// <summary>
        ///  Create the CreateHealthTrack
        /// </summary>
        /// <param name="healthTrackViewModel"></param>
        /// <returns></returns>
        Task<IResult> CreateHealthTrack(HealthTrackViewModel healthTrackViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IResult> GetDeclarationFormData();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeedId"></param>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        Task<IResult> GetSelfDeclarationByEmployeeForRequest(int employeedId, string requestNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IResult GetAllDeclarations(SearchSortModel search);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetDeclarations(SearchSortModel search);

        /// <summary>
        /// get declarations by userId
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetDeclarationsByUserId(SearchSortModel search);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="covidHealthTrackViewModel"></param>
        /// <returns></returns>
        Task<IResult> CreateCovidHealthTrack(CovidHealthTrackViewModel covidHealthTrackViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetCovidDeclarations(SearchSortModel search);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeedId"></param>
        /// <returns></returns>
        Task<IResult> GetCovidDeclaration(int declarationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeedId"></param>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        Task<IResult> GetExistingSelfDeclarationOfEmployee(int employeedId);
    }

}
