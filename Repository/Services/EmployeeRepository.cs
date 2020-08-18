using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
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
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployee(Employee employee) {

           var result= await _context.Employee
                  .Where(e => e.Email == employee.Email && e.Password == employee.Password)
                  .SingleOrDefaultAsync();
            return result;
        } 
    }
}



