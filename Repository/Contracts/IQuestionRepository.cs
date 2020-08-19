using HTTAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQuestionRepository
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Question>> GetQuestions();
    }
}
