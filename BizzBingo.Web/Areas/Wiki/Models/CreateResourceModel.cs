using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizzBingo.Web.Areas.Wiki.Models
{
    public class CreateResourceModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Url { get; set; }
        public Guid TermId { get; set; }
    }
}