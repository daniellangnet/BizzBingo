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
        public string OAuthToken { get; set; }
        public string OAuthSecret { get; set; }
        public string OAuthAccessToken { get; set; }
        public string TwitterId { get; set; }
    }
}