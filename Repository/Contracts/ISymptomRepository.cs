using HTTAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISymptomRepository
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Symptom>> GetSymptoms();
    }
}
