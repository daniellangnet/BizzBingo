using System.Collections.Generic;

namespace BizzBingo.Web.Models.Home
{
    public class IndexViewModel
    {
        public List<Term> Top { get; set; }
        public List<Term> Newest { get; set; }
        public List<Term> MostResources { get; set; }
    }
}