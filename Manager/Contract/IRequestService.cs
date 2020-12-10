using HTTAPI.Helpers;
using HTTAPI.ViewModels;
using System.Threading.Tasks;

namespace HTTAPI.Manager.Contract
{
    /// <summary>
    ///  Contract for Come to office request
    /// </summary>
    public interface IRequestService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IResult GetRequestsList(SearchSortModel search);


        /// <summary>
        /// Get requests by UserId
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetRequestsListByUserId(SearchSortModel search);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        Task<IResult> CreateRequest(ComeToOfficeRequestViewModel requestViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        Task<IResult> UpdateRequest(ComeToOfficeRequestViewModel requestViewModel);

        /// <summary>
        /// Get request detail
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<IResult> GetRequestDetail(int requestId);
    }
}
