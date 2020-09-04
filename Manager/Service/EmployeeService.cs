using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public AppEmailHelper appEmailHelper;
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
                    employeeViewModel.RoleId = employeeModel.Role.Id;
                    employeeViewModel.RoleName = employeeModel.Role.Name;

                    // send email to HR for approval
                    await SendRegisterationRequestEmail(employeeViewModel, MailTemplate.UserRegisterationRequest);

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
        /// <param name="employeeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IResult> UpdateAccountStatus(int employeeId, EntityStatus status)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (employeeId != 0)
                {
                    var employeeVm = new EmployeeViewModel();
                    var emp = await _employeeRepository.GetEmployeeById(employeeId);
                    if (emp != null)
                    {
                        emp.Status = status;
                        emp = await _employeeRepository.UpdateEmployee(emp);
                        employeeVm.MapFromModel(emp);
                    }
                    result.Message = emp.Status == EntityStatus.Accept ? "Confirmed" : "Declined";
                    // send email to employee via hR
                    await SendAccountStatusEmail(employeeVm, MailTemplate.UserConfirmation);
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
        /// Update Employee
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <returns></returns>
        public async Task<IResult> UpdateEmployee(EmployeeViewModel employeeViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (employeeViewModel != null)
                {
                    var emp = await _employeeRepository.GetEmployeeById(employeeViewModel.Id);
                    if (emp != null)
                    {
                        emp.MapFromViewModel(employeeViewModel);
                        emp = await _employeeRepository.UpdateEmployee(emp);
                    }
                    employeeViewModel.MapFromModel(emp);
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
                    if (employeeModel != null)
                    {
                        // check if employee is confirmed or not
                        if (employeeModel.Status == EntityStatus.Accept)
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
                        else if (employeeModel.Status == EntityStatus.Deny)
                        {
                            result.Body = null;
                            result.Message = "Your account has declined by the HR";
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.Forbidden;
                        }
                        else
                        {
                            result.Body = null;
                            result.Message = "Your account has not confirmed yet by the HR";
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.Unauthorized;
                        }
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
                        employeeVm.RoleId = employeeModel.Role.Id;
                        employeeVm.RoleName = employeeModel.Role.Name;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<IResult> GetEmployeeById(int employeeId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (employeeId > 0)
                {
                    var employeeVm = new EmployeeViewModel();
                    var employeeModel = await _employeeRepository.GetEmployeeById(employeeId);
                    if (employeeModel != null)
                    {
                        employeeVm.MapFromModel(employeeModel);
                        employeeVm.RoleId = employeeModel.Role.Id;
                        employeeVm.RoleName = employeeModel.Role.Name;
                        result.Body = employeeVm;
                    }
                    else
                    {
                        result.Body = null;
                        result.Message = "Employee Id does not exists";
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



        /// <summary>
        /// Returns employees
        /// </summary>
        /// <returns></returns>
        public async Task<IResult> GetEmployeeList()
        {
            var employeeViewModels = new List<EmployeeViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var employees = await _employeeRepository.GetAllEmployees();
                if (employees.Any())
                {
                    employeeViewModels = employees.Select(t =>
                    {
                        var employeeViewModel = new EmployeeViewModel();
                        employeeViewModel.MapFromModel(t);
                        employeeViewModel.RoleId = t.Role.Id;
                        employeeViewModel.RoleName = t.Role.Name;
                        return employeeViewModel;
                    }).ToList();
                }
                else
                {
                    result.Message = "No records found";
                }
                result.Body = employeeViewModels;

            }
            catch (Exception ex)
            {
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var employee = await _employeeRepository.GetEmployeeByEmail(model.Email);
                if (employee == null)
                {
                    result.Body = null;
                    result.Message = "Employee email does not exists";
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.NotFound;
                    return result;
                }
                // create reset token that expires after 1 day
                employee.ResetToken = randomTokenString();
                employee.ResetTokenExpires = DateTime.Now.AddDays(1);
                employee = await _employeeRepository.UpdateEmployee(employee);

                await SendPasswordResetEmail(employee);
                result.Message = "Please check your email for password reset instructions";

            }
            catch (Exception ex)
            {
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IResult> ResetPassword(ResetPasswordViewModel model)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var employee = await _employeeRepository.GetEmployeeByToken(model.Token);
                if (employee == null)
                {
                    result.Body = null;
                    result.Message = "Invalid token";
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.BadRequest;
                    return result;
                }

                // update password and remove reset token
                employee.Password = model.Password;
                employee.ResetToken = null;
                employee.ResetTokenExpires = null;
                employee = await _employeeRepository.UpdateEmployee(employee);
                result.Message = "Password changed successfully.";
                return result;

            }
            catch (Exception ex)
            {
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
           
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

        private async Task SendRegisterationRequestEmail(EmployeeViewModel empViewModel, MailTemplate mailTemplate)
        {
            appEmailHelper = new AppEmailHelper();
            var hrEmployee = await _employeeRepository.GetEmployeeDetailsByRole(EmployeeRoles.HRManager.ToString());

            appEmailHelper.ToMailAddresses.Add(new MailAddress(hrEmployee.Email, hrEmployee.Name));
            appEmailHelper.CCMailAddresses.Add(new MailAddress(empViewModel.Email, empViewModel.Name));
            appEmailHelper.MailTemplate = mailTemplate;
            appEmailHelper.Subject = "Request for Registeration";
            EmployeeRegisterationEmailViewModel emailVm = new EmployeeRegisterationEmailViewModel();
            emailVm.MapFromViewModel(empViewModel);
            emailVm.LinkUrl = $"{ _configuration["AppUrl"]}users";
            emailVm.HRName = hrEmployee.Name;
            appEmailHelper.MailBodyViewModel = emailVm;
            await appEmailHelper.InitMailMessage();
        }

        private async Task SendAccountStatusEmail(EmployeeViewModel empViewModel, MailTemplate mailTemplate)
        {
            appEmailHelper = new AppEmailHelper();
            var hrEmployee = await _employeeRepository.GetEmployeeDetailsByRole(EmployeeRoles.HRManager.ToString());
            if (hrEmployee != null)
            {
                appEmailHelper.FromMailAddress = new MailAddress(hrEmployee.Email, hrEmployee.Name);
            }
            appEmailHelper.ToMailAddresses.Add(new MailAddress(empViewModel.Email, empViewModel.Name));
            appEmailHelper.MailTemplate = mailTemplate;
            EmployeeRegisterationEmailViewModel emailVm = new EmployeeRegisterationEmailViewModel();
            emailVm.MapFromViewModel(empViewModel);
            if (empViewModel.Status == EntityStatus.Accept)
            {
                appEmailHelper.Subject = "Account confirmation";
            }
            else if (empViewModel.Status == EntityStatus.Deny)
            {
                appEmailHelper.Subject = "Account not confirmed";
            }

            emailVm.LinkUrl = $"{ _configuration["AppUrl"]}login";
            emailVm.HRName = hrEmployee.Name;
            appEmailHelper.MailBodyViewModel = emailVm;
            await appEmailHelper.InitMailMessage();
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }


        private async Task SendPasswordResetEmail(Employee emp)
        {
            appEmailHelper = new AppEmailHelper();
            appEmailHelper.ToMailAddresses.Add(new MailAddress(emp.Email, emp.Name));
            appEmailHelper.MailTemplate = MailTemplate.PasswordReset;

            EmployeePasswordResetEmailViewModel emailVm = new EmployeePasswordResetEmailViewModel();
            emailVm.Name = emp.Name;
            emailVm.ResetUrl = $"{ _configuration["AppUrl"]}account/resetpassword?token={emp.ResetToken}";
            appEmailHelper.Subject = "Reset Password";
            appEmailHelper.MailBodyViewModel = emailVm;
            await appEmailHelper.InitMailMessage();

        }

        #endregion
    }
}
