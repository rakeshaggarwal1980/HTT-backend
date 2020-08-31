using HTTAPI.Enums;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Services
{
    /// <summary>
    /// EmployeeRepository
    /// Here all method should be async
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Ctor
        /// context injection\creation
        /// </summary>
        /// <param name="context"></param>
        public EmployeeRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployeeById(int empId)
        {
            var result = await _context.Employee
                  .Where(e => e.Id == empId).Include(e => e.Role).SingleOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var emp = _context.Employee.Include(t => t.Role).FirstOrDefault(e => e.Id == employee.Id);
            if (emp == null)
            {
                throw new ArgumentException("Update failed: No such request found");
            }
            emp.Name = employee.Name;
            emp.RoleId = employee.RoleId;
            emp.Email = employee.Email;
            emp.EmployeeCode = employee.EmployeeCode;
            await _context.SaveChangesAsync();
            return emp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployee(Employee employee)
        {

            var result = await _context.Employee
                   .Where(e => e.Email == employee.Email && e.Password == employee.Password).Include(e => e.Role)
                   .SingleOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            var result = await _context.Employee
                  .Where(e => e.Email == email).SingleOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empCode"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployeeByEmpCode(int empCode)
        {
            var result = await _context.Employee
                  .Where(e => e.EmployeeCode == empCode).SingleOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Employee> GetEmployeeDetailsByRole(string roleName)
        {
            return await _context.Employee.Where(e => e.Role.Name == roleName).SingleOrDefaultAsync();
        }


    }
}



