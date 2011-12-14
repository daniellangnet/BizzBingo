namespace BizzBingo.Web.Models
{
    using System;

    public class Reaction
    {
        public Guid Id { get; set; }
        public string Story { get; set; }
        public string Name { get; set; }
        public bool IsPositive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}