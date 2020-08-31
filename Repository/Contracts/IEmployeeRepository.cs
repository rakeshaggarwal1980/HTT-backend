using HTTAPI.Models;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Contracts
{
    /// <summary>
    /// Employee Repo
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<Employee> CreateEmployee(Employee employee);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<Employee> GetEmployee(Employee employee);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<Employee> GetEmployeeDetailsByRole(string roleName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Employee> GetEmployeeByEmail(string email);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="empCode"></param>
        /// <returns></returns>
        Task<Employee> GetEmployeeByEmpCode(int empCode);
    }
}
