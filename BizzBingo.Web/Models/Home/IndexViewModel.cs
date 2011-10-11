using System.Collections.Generic;

namespace BizzBingo.Web.Models.Home
{
    public class IndexViewModel
    {
        public List<Word> Top { get; set; }
        public List<Word> Newest { get; set; }
    }
}