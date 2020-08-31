using HTTAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Contracts
{
    /// <summary>
    /// Role contract
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        ///  Returns roles
        /// </summary>
        /// <returns></returns>
        Task<List<Role>> GetRoles();
    }
}
