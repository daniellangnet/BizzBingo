using System;

namespace BizzBingo.Web.Areas.Wiki.Models
{
    public class CreateReactionModel
    {
        public string Title { get; set; }
        public string Reason { get; set; }
        public string Name { get; set; }
        public bool IsPositive { get; set; }
        public Guid TermId { get; set; }
    }
}