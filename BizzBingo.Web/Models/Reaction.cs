namespace BizzBingo.Web.Models
{
    using System;

    public class Reaction
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public bool IsPositive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid SharedByUserId { get; set; }
    }
}