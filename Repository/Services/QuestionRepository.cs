using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTTAPI.Repository.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class QuestionRepository : IQuestionRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Ctor
        /// context injection\creation
        /// </summary>
        /// <param name="context"></param>
        public QuestionRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns list of questions
        /// </summary>
        /// <returns></returns>
        public async Task<List<Question>> GetQuestions()
        {
            return await _context.Question.ToListAsync();
        }
    }
}
