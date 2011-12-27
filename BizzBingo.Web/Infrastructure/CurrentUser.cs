using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizzBingo.Web.Models;

namespace BizzBingo.Web.Infrastructure
{
    public class CurrentUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TwitterId { get; set; }
        public string OAuthAccessToken { get; set; }
        public DateTime RegisteredOn { get; set; }
        public DateTime LastSignIn { get; set; }
    }
}
