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
        /// <param name="employeeViewModel"></param>
        /// <returns></returns>
        Task<IResult> CreateEmployee(EmployeeViewModel employeeViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        Task<IResult> GetEmployee(UserLoginViewModel loginModel);
    }
}
