using System;

namespace BizzBingo.Web.Areas.Wiki.Models
{
    public class CreateReactionModel
    {
        public string Story { get; set; }
        public string Name { get; set; }
        public bool IsPositive { get; set; }
        public Guid TermId { get; set; }
    }
}