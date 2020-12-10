using HTTAPI.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace HTTAPI.Helpers
{
    public class SearchSortModel
    {

        public string PropertyName { get; set; }

        public string SearchString { get; set; }

        public string SortColumn { get; set; } = "id";

        public SortDirection SortDirection { get; set; } = SortDirection.Desc;

        public int userId { get; set; }
        public int roleId { get; set; }

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
