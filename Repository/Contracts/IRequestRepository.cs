using HTTAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<ComeToOfficeRequest>> GetRequestsList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<ComeToOfficeRequest> GetRequestById(int requestId);

    }
}
