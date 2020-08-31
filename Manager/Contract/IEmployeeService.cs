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

    }
}
