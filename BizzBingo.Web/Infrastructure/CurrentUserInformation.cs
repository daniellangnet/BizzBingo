using System;

namespace BizzBingo.Web.Infrastructure
{
    public class CurrentUserInformation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TwitterId { get; set; }
        public DateTime RegisteredOn { get; set; }
        public DateTime LastSignIn { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}