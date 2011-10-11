using System;
using System.Linq;
using System.Web;

namespace BizzBingo.Web.Models
{
    public class Word
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LcId { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public string Slug { get; set; }
        public int Views { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}