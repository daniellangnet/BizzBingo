using System.Collections.Generic;
using BizzBingo.Web.Models;

namespace BizzBingo.Web.Areas.Wiki.Models
{
    public class SearchResultModel
    {
        public string SearchTerm { get; set; }
        public List<Term> Results { get; set; }
    }
}