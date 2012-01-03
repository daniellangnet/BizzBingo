namespace BizzBingo.Web.Models
{
    using System.Collections.Generic;
    using System;

    public class Term
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string LcId { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public string Slug { get; set; }
        public int Views { get; set; }
        public DateTime CreatedOn { get; set; }
        public IList<Resource> Resources { get; set; }
        public IList<Reaction> Reactions { get; set; }
        public Guid SharedByUserId { get; set; }
    }
}
