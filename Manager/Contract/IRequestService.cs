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
        Task<IResult> GetRequestsList();

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
