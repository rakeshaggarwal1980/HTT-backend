using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTAPI.Helpers
{
    public static class PagingExtensions
    {
        /// <summary>
        /// Pages a LINQ query to return just the subset of rows from the database. Use as follows:
        /// 
        /// var query = from s in _context.Table
        ///             orderby s.Id ascending
        ///             select s;
        ///
        /// return _myRepository.Find(query, page, pageSize).ToList();
        /// </summary>
        /// <typeparam name="TSource">Entity</typeparam>
        /// <param name="source">LINQ query</param>
        /// <param name="page">Page Index</param>
        /// <param name="pageSize">Number of Rows</param>
        /// <returns>IQueryable</returns>
        public static IQueryable<TSource> Page<TSource>(IQueryable<TSource> source, int page, int pageSize)
        {
            if (page == 0 && pageSize == 0)
            {
                return source;
            }
            return source.Skip(((page == 0 ? 1 : page) - 1) * pageSize).Take(pageSize == 0 ? 10 : pageSize);
        }
    }

}
