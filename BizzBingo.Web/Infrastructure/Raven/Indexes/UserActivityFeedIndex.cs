using System;
using System.Linq;
using BizzBingo.Web.Models;
using Raven.Client.Indexes;

namespace BizzBingo.Web.Infrastructure.Raven.Indexes
{
    public class UserActivityFeedIndex : AbstractIndexCreationTask<User>
    {
        public class Result
        {
            public string UserId { get; set; }
            public string Name { get; set; }
            public DateTime Time { get; set; }
            public string ActionRelatedTermTitle { get; set; }
            public string ActionRelatedTermSlug { get; set; }
        }

        public UserActivityFeedIndex()
        {
            Map = users => from user in users
                           select new
                                      {
                                          UserId = user.Id.ToString(),
                                          Name = user.Name
                                      };

            TransformResults =
                (database, users) => from user in users
                                     from action in user.ActionFeed
                                     let relatedTerm = database.Load<Term>("terms/" + action.TermIdContext.ToString())
                                     select new
                                                {
                                                    UserId = user.Id,
                                                    Name = user.Name,
                                                    Time = action.Time,
                                                    ActionRelatedTermTitle = relatedTerm.Title,
                                                    ActionRelatedTermSlug = relatedTerm.Slug
                                                };
        }
    }
}