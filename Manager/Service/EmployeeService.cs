using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HTTAPI.Manager.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeService : IEmployeeService
    {

        /// <summary>
        /// logger EmployeeService
        /// </summary>
        readonly ILogger<EmployeeService> _logger;
        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// contract with EmployeeRepository
        /// </summary>
        IEmployeeRepository _employeeRepository;


        /// <summary>
        /// Claim Identity
        /// </summary>
        private readonly ClaimsPrincipal _principal;

        /// <summary>
        /// EmployeeService constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="principal"></param>
        /// <param name="employeeRepository"></param>
        /// <param name="configuration"></param>      
        public EmployeeService(ILogger<EmployeeService> logger, IPrincipal principal,
            IEmployeeRepository employeeRepository, IConfiguration configuration)
        {
            _logger = logger;
            _principal = principal as ClaimsPrincipal;
            _employeeRepository = employeeRepository;
            _configuration = configuration;

        }

        /// <summary>
        /// Create Employee
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <returns></returns>
        public async Task<IResult> CreateEmployee(EmployeeViewModel employeeViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (employeeViewModel != null)
                {
                    var employeeModel = new Employee();
                    // To map employee detail
                    employeeModel.MapFromViewModel(employeeViewModel);

                    employeeModel = await _employeeRepository.CreateEmployee(employeeModel);
                    employeeViewModel.Id = employeeModel.Id;
                    result.Body = employeeViewModel;
                    return result;
                }
                result.Status = Status.Fail;
                result.StatusCode = HttpStatusCode.BadRequest;
                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
                return result;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<IResult> GetEmployee(UserLoginViewModel loginModel)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (loginModel != null)
                {
                    var employeeModel = new Employee();
                    // To map employee detail
                    employeeModel.MapFromViewModel(loginModel);

                    employeeModel = await _employeeRepository.GetEmployee(employeeModel);
                    if (employeeModel != null && employeeModel.Id != 0)
                    {
                        // employee exists
                        UserViewModel userView = new UserViewModel()
                        {
                            Email = employeeModel.Email,
                            EmployeeCode = employeeModel.EmployeeCode,
                            Name = employeeModel.Name,
                            UserId = employeeModel.Id,
                            Token = GenerateToken(employeeModel.Name, employeeModel.Email)
                        };

                        result.Body = userView;
                    }
                    else
                    {
                        result.Body = null;
                        result.Message = "No Employee exists";
                        result.Status = Status.Fail;
                        result.StatusCode = HttpStatusCode.Unauthorized;
                    }
                    return result;
                }
                result.Status = Status.Fail;
                result.StatusCode = HttpStatusCode.BadRequest;
                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
                return result;
            }
        }
        #region Private methods

        private string GenerateToken(string name, string email)
        {
            var claims = new Claim[]
            {
                new Claim("Name", name),
                new Claim(ClaimTypes.Email,email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SigningKey"])),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
