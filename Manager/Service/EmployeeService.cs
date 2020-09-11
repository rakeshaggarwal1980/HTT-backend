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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

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

        IRoleRepository _roleRepository;

        /// <summary>
        /// 
        /// </summary>
        public AppEmailHelper appEmailHelper;
        /// <summary>
        /// 
        /// </summary>
        private readonly ClaimsPrincipal _principal;
        /// <summary>
        /// EmployeeService constructor
        /// </summary>
        /// <param name="employeeRepository"></param>
        /// <param name="configuration"></param>      
        public EmployeeService(IEmployeeRepository employeeRepository, IConfiguration configuration, IPrincipal principal,
            IRoleRepository roleRepository)
        {
            _employeeRepository = employeeRepository;
            _configuration = configuration;
            _principal = principal as ClaimsPrincipal;
            _roleRepository = roleRepository;

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
                    employeeModel.Password = BC.HashPassword(signUpViewModel.Password);
                    // by default user registers as employee
                    var role = await _roleRepository.GetRoleByName(EmployeeRolesEnum.Employee.ToString());
                    employeeModel.EmployeeRoles = new List<EmployeeRole>{
                        new EmployeeRole {  Employee = employeeModel,  Role = role }};
                    employeeModel = await _employeeRepository.CreateEmployee(employeeModel);
                    employeeViewModel.MapFromModel(employeeModel);

                    // uncomment if required
                    //var roles = new List<EmployeeRoleViewModel>();
                    //roles = employeeModel.EmployeeRoles.Select(t =>
                    //{
                    //    var role = new EmployeeRoleViewModel();
                    //    role.Id = t.Id;
                    //    role.RoleId = t.RoleId;
                    //    role.RoleName = t.Role.Name;
                    //    role.EmployeeId = t.EmployeeId;
                    //    return role;
                    //}).ToList();

                    // send email to HR for approval
                    var mailresult = await SendRegisterationRequestEmail(employeeViewModel);
                    result.Message = mailresult.Message;
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
                    var mailResult = await SendAccountStatusEmail(employeeVm, MailTemplate.UserConfirmation);
                    result.Message = mailResult.Message;
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
                    //  var emp = new Employee();
                    var emp = await _employeeRepository.GetEmployeeById(employeeViewModel.Id);
                    if (emp != null)
                    {
                        emp.MapFromViewModel(employeeViewModel);

                        // add new emp roles
                        var empRoles = new List<EmployeeRole>();
                        empRoles = employeeViewModel.Roles.Select(r =>
                        {
                            var empRole = new EmployeeRole
                            {
                                Id = r.Id,
                                RoleId = r.RoleId,
                                EmployeeId = r.EmployeeId
                            };
                            return empRole;
                        }).ToList();
                        emp.EmployeeRoles = empRoles;
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
                    var employeeModel = await _employeeRepository.GetEmployeeByEmail(loginModel.Email);
                    if (employeeModel == null)
                    {
                        result.Body = null;
                        result.Message = "Account with " + loginModel.Email + " does not exists.";
                        result.Status = Status.Fail;
                        result.StatusCode = HttpStatusCode.NotFound;
                        return result;
                    }
                    else
                    {
                        if (!BC.Verify(loginModel.Password, employeeModel.Password))
                        {
                            result.Body = null;
                            result.Message = "Password is incorrect";
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.Forbidden;
                            return result;
                        }
                        // check if employee is confirmed or not
                        if (employeeModel.Status == EntityStatus.Accept)
                        {
                            var roles = new List<EmployeeRoleViewModel>();
                            roles = employeeModel.EmployeeRoles.Select(t =>
                            {
                                var role = new EmployeeRoleViewModel();
                                role.Id = t.Id;
                                role.RoleId = t.RoleId;
                                role.RoleName = t.Role.Name;
                                role.EmployeeId = t.EmployeeId;
                                return role;
                            }).ToList();

                            UserViewModel userView = new UserViewModel()
                            {
                                Email = employeeModel.Email,
                                EmployeeCode = employeeModel.EmployeeCode,
                                Name = employeeModel.Name,
                                UserId = employeeModel.Id,
                                Token = GenerateToken(employeeModel.Name, employeeModel.Email),
                                Roles = roles
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
                    return result;
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.BadRequest;
                    return result;
                }
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
                        var roles = new List<EmployeeRoleViewModel>();
                        roles = employeeModel.EmployeeRoles.Select(t =>
                        {
                            var role = new EmployeeRoleViewModel();
                            role.Id = t.Id;
                            role.RoleId = t.RoleId;
                            role.RoleName = t.Role.Name;
                            role.EmployeeId = t.EmployeeId;
                            return role;
                        }).ToList();
                        employeeVm.Roles = roles;
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
                        var roles = new List<EmployeeRoleViewModel>();
                        roles = employeeModel.EmployeeRoles.Select(t =>
                        {
                            var role = new EmployeeRoleViewModel();
                            role.Id = t.Id;
                            role.RoleId = t.RoleId;
                            role.RoleName = t.Role.Name;
                            role.EmployeeId = t.EmployeeId;
                            return role;
                        }).ToList();
                        employeeVm.Roles = roles;
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
                        var roles = new List<EmployeeRoleViewModel>();
                        roles = t.EmployeeRoles.Select(r =>
                        {
                            var role = new EmployeeRoleViewModel();
                            role.Id = r.Id;
                            role.RoleId = r.RoleId;
                            role.RoleName = r.Role.Name;
                            role.EmployeeId = r.EmployeeId;
                            return role;
                        }).ToList();

                        employeeViewModel.Roles = roles;
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
                var mailResult = await SendForgotPasswordEmail(employee);
                result.Message = mailResult.Message;
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
                // in case of change pwd , will recieve email
                var employee = new Employee();
                if (!string.IsNullOrEmpty(model.email))
                {
                    employee = await _employeeRepository.GetEmployeeByEmail(model.email);
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.BadRequest;
                    return result;
                }
                if (employee == null)
                {
                    result.Body = null;
                    result.Message = "Invalid Request";
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.BadRequest;
                    return result;
                }

                // update password
                employee.Password = BC.HashPassword(model.Password);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IResult> DeleteEmployee(int Id)
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                var employee = await _employeeRepository.GetEmployeeById(Id);
                if (employee == null)
                {
                    result.Status = Status.Error;
                    result.Message = "Employee Id does not exists"; 
                    result.StatusCode = HttpStatusCode.NotFound;
                    return result;
                }
                await _employeeRepository.DeleteEmployee(employee);
                result.Message = "Employee deleted";
                result.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
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

        private async Task<IResult> SendRegisterationRequestEmail(EmployeeViewModel empViewModel)
        {
            var result = new Result
            {
                Operation = Operation.SendEmail,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                appEmailHelper = new AppEmailHelper();
                var hrEmployeeList = await _employeeRepository.GetEmployeeDetailsByRole(EmployeeRolesEnum.HRManager.ToString());
                if (hrEmployeeList.Count > 0)
                {
                    foreach (var hrEmployee in hrEmployeeList)
                    {
                        appEmailHelper.ToMailAddresses.Add(new MailAddress(hrEmployee.Email, hrEmployee.Name));
                    }
                }
                appEmailHelper.CCMailAddresses.Add(new MailAddress(empViewModel.Email, empViewModel.Name));
                appEmailHelper.MailTemplate = MailTemplate.UserRegisterationRequest;
                appEmailHelper.Subject = "Request for Registeration";
                EmployeeRegisterationEmailViewModel emailVm = new EmployeeRegisterationEmailViewModel();
                emailVm.MapFromViewModel(empViewModel);
                emailVm.LinkUrl = $"{ _configuration["AppUrl"]}users";
                emailVm.HRName = "";
                appEmailHelper.MailBodyViewModel = emailVm;
                await appEmailHelper.InitMailMessage();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
                result.StatusCode = HttpStatusCode.InternalServerError;

            }
            return result;
        }

        private async Task<IResult> SendAccountStatusEmail(EmployeeViewModel empViewModel, MailTemplate mailTemplate)
        {
            var result = new Result
            {
                Operation = Operation.SendEmail,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                appEmailHelper = new AppEmailHelper();
                var activeUserEmailId = ((ClaimsIdentity)_principal.Identity).GetActiveUserEmailId();
                var activeUserName = ((ClaimsIdentity)_principal.Identity).GetActiveUserName();
                if (!string.IsNullOrEmpty(activeUserEmailId))
                {
                    appEmailHelper.FromMailAddress = new MailAddress(activeUserEmailId, activeUserName ?? "HR");
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
                emailVm.HRName = activeUserName ?? "HR";
                appEmailHelper.MailBodyViewModel = emailVm;
                await appEmailHelper.InitMailMessage();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }

        private async Task<IResult> SendForgotPasswordEmail(Employee emp)
        {
            var result = new Result
            {
                Operation = Operation.SendEmail,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                appEmailHelper = new AppEmailHelper();
                appEmailHelper.ToMailAddresses.Add(new MailAddress(emp.Email, emp.Name));
                appEmailHelper.MailTemplate = MailTemplate.PasswordReset;

                EmployeePasswordResetEmailViewModel emailVm = new EmployeePasswordResetEmailViewModel();
                emailVm.Name = emp.Name;
                string password = GeneratePassword();
                emailVm.Password = password;
                appEmailHelper.Subject = "New Password";
                appEmailHelper.MailBodyViewModel = emailVm;
                await appEmailHelper.InitMailMessage();
                result.Message = "Password has been sent to your email.";
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        private string GeneratePassword()
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            Random rand = new Random();
            string temp = "";
            for (int i = 0; i < Convert.ToInt32(9); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }

        #endregion


    }
}