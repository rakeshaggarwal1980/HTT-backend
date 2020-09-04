using HTTAPI.Models;
using System.Collections.Generic;
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
        Task<Employee> UpdateEmployee(Employee employee);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        Task<Employee> GetEmployeeById(int empId);

        /// <summary>
        ///  Returns list of all employees
        /// </summary>
        /// <returns></returns>
        Task<List<Employee>> GetAllEmployees();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Employee> GetEmployeeByToken(string token);
    }
}
