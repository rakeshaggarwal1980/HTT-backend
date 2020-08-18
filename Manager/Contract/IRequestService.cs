using HTTAPI.Helpers;
using HTTAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
