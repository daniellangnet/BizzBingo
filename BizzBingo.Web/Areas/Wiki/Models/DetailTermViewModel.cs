using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizzBingo.Web.Infrastructure;

namespace BizzBingo.Web.Areas.Wiki.Models
{
    using Web.Models;

    public class DetailTermViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public string Slug { get; set; }
        public int Views { get; set; }
        public string CreatedOn { get; set; }
        public List<DetailResourceViewModel> Resources { get; set; }
        public List<Reaction> PositiveReaction { get; set; }
        public List<Reaction> NegativeReaction { get; set; }
        public CurrentUserInformation CurrentUserInformation { get; set; }
    }
}