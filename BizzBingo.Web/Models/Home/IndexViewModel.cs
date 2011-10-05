using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizzBingo.Web.Models.Home
{
    public class IndexViewModel
    {
        public List<Word> Top { get; set; }
        public List<Word> Newest { get; set; }
    }
}