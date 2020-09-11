using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.ViewModels;
using System.Threading.Tasks;

namespace HTTAPI.Manager.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        ///  Create the CreateEmployee
        /// </summary>
        /// <param name="signUpViewModel"></param>
        /// <returns></returns>
        Task<IResult> CreateEmployee(UserSignUpViewModel signUpViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        Task<IResult> GetEmployee(UserLoginViewModel loginModel);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IResult> GetEmployeeByEmail(string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<IResult> GetEmployeeById(int employeeId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <returns></returns>
        Task<IResult> UpdateEmployee(EmployeeViewModel employeeViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IResult> GetEmployeeList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<IResult> UpdateAccountStatus(int employeeId, EntityStatus status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResult> ForgotPassword(ForgotPasswordViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResult> ResetPassword(ResetPasswordViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResult> DeleteEmployee(int Id);
    }
}
