﻿using HTTAPI.Helpers;
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
        List<ComeToOfficeRequest> GetRequestsList(SearchSortModel search);


        /// <summary>
        ///  Get Requests of a user
        /// </summary>
        /// <returns></returns>
        List<ComeToOfficeRequest> GetRequestsListByUserId(SearchSortModel search);


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
        Task<List<ComeToOfficeRequest>> GetRequestsByEmployee(int employeeId);


        /// <summary>
        /// Get request by request number
        /// </summary>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        Task<ComeToOfficeRequest> GetRequestByNumber(string requestNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<List<ComeToOfficeRequest>> GetRequestsByUserId(int employeeId);
    }

}
