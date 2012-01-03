using System;

namespace BizzBingo.Web.Models
{
    public class Action
    {
        public DateTime Time { get; set; }
        public ActionType Type { get; set; }
        public Guid TermIdContext { get; set; }
        public Guid ReactionIdContext { get; set; }
        public Guid ResourceIdContext { get; set; }
    }
}