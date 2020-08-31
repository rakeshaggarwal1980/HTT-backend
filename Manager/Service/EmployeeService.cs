using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
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
        /// 
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// contract with EmployeeRepository
        /// </summary>
        IEmployeeRepository _employeeRepository;

        /// <summary>
        /// EmployeeService constructor
        /// </summary>
        /// <param name="employeeRepository"></param>
        /// <param name="configuration"></param>      
        public EmployeeService(IEmployeeRepository employeeRepository, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _configuration = configuration;

        }

        /// <summary>
        /// Create Employee
        /// </summary>
        /// <param name="signUpViewModel"></param>
        /// <returns></returns>
        public async Task<IResult> CreateEmployee(UserSignUpViewModel signUpViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var employeeViewModel = new EmployeeViewModel();
                if (signUpViewModel != null)
                {
                    // email and employee code should be unique
                    var employee1 = await _employeeRepository.GetEmployeeByEmail(signUpViewModel.Email);
                    if (employee1 != null)
                    {
                        result.Status = Status.Fail;
                        result.StatusCode = HttpStatusCode.NotAcceptable;
                        result.Message = "Employee Email Id already exists";
                        return result;
                    }

                    var employee2 = await _employeeRepository.GetEmployeeByEmpCode(signUpViewModel.EmployeeCode);
                    if (employee2 != null)
                    {
                        result.Status = Status.Fail;
                        result.StatusCode = HttpStatusCode.NotAcceptable;
                        result.Message = "Employee Code already exists";
                        return result;
                    }

                    var employeeModel = new Employee();
                    employeeModel.MapFromViewModel(signUpViewModel);
                    employeeModel = await _employeeRepository.CreateEmployee(employeeModel);

                    employeeViewModel.MapFromModel(employeeModel);

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
                        RoleViewModel roleVM = new RoleViewModel();
                        roleVM.MapFromModel(employeeModel.Role);
                        UserViewModel userView = new UserViewModel()
                        {
                            Email = employeeModel.Email,
                            EmployeeCode = employeeModel.EmployeeCode,
                            Name = employeeModel.Name,
                            UserId = employeeModel.Id,
                            Token = GenerateToken(employeeModel.Name, employeeModel.Email, employeeModel.Role.Name),
                            Role = roleVM
                        };

                        result.Body = userView;
                    }
                    else
                    {
                        result.Body = null;
                        result.Message = "Wrong Email or password";
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IResult> GetEmployeeByEmail(string email)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var employeeVm = new EmployeeViewModel();
                    var employeeModel = await _employeeRepository.GetEmployeeByEmail(email);
                    if (employeeModel != null)
                    {
                        employeeVm.MapFromModel(employeeModel);
                        result.Body = employeeVm;
                    }
                    else
                    {
                        result.Body = null;
                        result.Message = "Email Id does not exists";
                        result.Status = Status.Fail;
                        result.StatusCode = HttpStatusCode.NotFound;
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
                result.StatusCode = HttpStatusCode.InternalServerError;
                return result;
            }
        }

        #region Private methods

        private string GenerateToken(string name, string email, string roleName)
        {
            var claims = new Claim[]
            {
                new Claim("Name", name),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Role,roleName),
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
