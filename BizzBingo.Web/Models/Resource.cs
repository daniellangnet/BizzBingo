namespace BizzBingo.Web.Models
{
    using System;

    public class Resource
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }

}