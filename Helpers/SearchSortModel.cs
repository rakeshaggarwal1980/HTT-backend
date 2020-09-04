using HTTAPI.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTAPI.Helpers
{
    public class SearchSortModel
    {

        public string PropertyName { get; set; }

        public string SearchString { get; set; }

        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; } = SortDirection.Desc;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        [BindNever]
        public int? TotalRecords { get; set; }

        [BindNever]
        public dynamic SearchResult { get; set; }

        //public List<Filter> Filters { get; set; } = new List<Filter>();

    }


    public class Filter
    {
        public string PropertyName { get; set; }
        public ExpressionOperation ExpOperation { get; set; }
        public List<string> Value { get; set; }
    }
}
