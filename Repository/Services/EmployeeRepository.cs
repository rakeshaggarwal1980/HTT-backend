using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        /// <param name="includeRole"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployeeById(int empId)
        {
            return await _context.Employee
                    .Where(e => e.Id == empId).Include(e => e.EmployeeRoles).ThenInclude(e=>e.Role).SingleOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var db = _context.Employee.Where(e => e.Id == employee.Id).Include(e => e.EmployeeRoles).SingleOrDefault();
            employee.Password = db.Password;
            _context.Entry(db).CurrentValues.SetValues(employee);
            // delete / clear subset1 from database
            foreach (var dbSubset1 in db.EmployeeRoles.ToList())
            {
                if (!employee.EmployeeRoles.Any(i => i.Id == dbSubset1.Id))
                    _context.EmployeeRole.Remove(dbSubset1);
            }
            foreach (var newSubset1 in employee.EmployeeRoles)
            {
                var dbSubset1 = db.EmployeeRoles.SingleOrDefault(i => i.Id == newSubset1.Id);
                if (dbSubset1 != null)
                    // update Subset1
                    _context.Entry(dbSubset1).CurrentValues.SetValues(newSubset1);
                else
                    db.EmployeeRoles.Add(newSubset1);
            }
            _context.Employee.Update(db);
            await _context.SaveChangesAsync();
            return employee;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployee(Employee employee)
        {
            var result = await _context.Employee
                   .Where(e => e.Email == employee.Email && e.Password == employee.Password).Include(e => e.EmployeeRoles)
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
                  .Where(e => e.Email == email).Include(e => e.EmployeeRoles).ThenInclude(e => e.Role).SingleOrDefaultAsync();
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
        public async Task<List<Employee>> GetEmployeeDetailsByRole(string roleName)
        {
            var filteredEmployees = _context.Employee.Where(e => e.EmployeeRoles.Any(r => r.Role.Name == roleName));
            return await filteredEmployees.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Employee.Include(e => e.EmployeeRoles).ThenInclude(e=>e.Role).ToListAsync();
        }

        //public async Task<Employee> GetEmployeeByToken(string token)
        //{
        //    return await _context.Employee.SingleOrDefaultAsync(x =>
        //       x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow);
        //}
    }
}



