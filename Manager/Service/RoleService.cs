using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HTTAPI.Manager.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleService : IRoleService
    {
        IRoleRepository _roleRepository;

        /// <summary>
        /// logger RoleService
        /// </summary>
        readonly ILogger<RoleService> _logger;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="roleRepository"></param>
        /// <param name="logger"></param>
        public RoleService(ILogger<RoleService> logger, IRoleRepository roleRepository)

        {
            _logger = logger;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Returns list of roles
        /// </summary>
        /// <returns></returns>
        public async Task<IResult> GetRoles()
        {
            var roleViewModels = new List<RoleViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var roles = await _roleRepository.GetRoles();
                if (roles.Any())
                {
                    roleViewModels = roles.Select(t =>
                    {
                        var roleViewModel = new RoleViewModel();
                        roleViewModel.MapFromModel(t);
                        return roleViewModel;
                    }).ToList();
                }
                else
                {
                    result.Message = "No roles found";
                }
                result.Body = roleViewModels;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }
    }
}
