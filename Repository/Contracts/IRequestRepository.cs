using HTTAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ComeToOfficeRequest> CreateRequest(ComeToOfficeRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ComeToOfficeRequest> UpdateRequest(ComeToOfficeRequest request);


        /// <summary>
        /// Get all requests
        /// </summary>
        /// <returns></returns>
        Task<List<ComeToOfficeRequest>> GetRequestsList();

        /// <summary>
        ///  Get Request detail
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<ComeToOfficeRequest> GetRequestById(int requestId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<ComeToOfficeRequest> GetRequestByEmployee(int employeeId);

    }
}
