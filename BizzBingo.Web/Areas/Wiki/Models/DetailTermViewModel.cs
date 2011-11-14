using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizzBingo.Web.Areas.Wiki.Models
{
    using Web.Models;

    public class DetailTermViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public string Slug { get; set; }
        public int Views { get; set; }
        public string CreatedOn { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
    }
}