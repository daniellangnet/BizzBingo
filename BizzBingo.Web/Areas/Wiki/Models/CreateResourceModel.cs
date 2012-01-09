using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBingo.Web.Areas.Wiki.Models
{
    public class CreateResourceModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        [AllowHtml]
        public string EmbedCode { get; set; }
        public string Type { get; set; }
        public Guid TermId { get; set; }
        public string ViaSource { get; set; }
    }
}