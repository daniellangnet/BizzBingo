using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizzBingo.Web.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TwitterId { get; set; }
        public string OAuthAccessToken { get; set; }
        public DateTime RegisteredOn { get; set; }
        public DateTime LastSignIn { get; set; }
        public int ReputationPoints { get; set; }
        public List<Action> ActionFeed { get; set; } 
    }
}