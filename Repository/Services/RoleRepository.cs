﻿using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        private readonly Context _context;

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="context"></param>
        public RoleRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns list of roles
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoles()
        {
            return await _context.Role.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Role> GetRoleByName(string name)
        {
            return await _context.Role.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task DeleteEmployeeRoles(int employeeId)
        {
            var emp = _context.Employee.Where(x => x.Id == employeeId).Include(e=>e.EmployeeRoles).SingleOrDefault();
            if (emp != null)
            {
                foreach (var role in emp.EmployeeRoles)
                {
                    emp.EmployeeRoles.Remove(role);

                }
            }
            await _context.SaveChangesAsync();

        }
    }
}
