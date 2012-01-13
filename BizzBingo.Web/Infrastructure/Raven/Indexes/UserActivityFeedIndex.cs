using System;
using System.Linq;
using BizzBingo.Web.Models;
using Raven.Client.Indexes;

namespace BizzBingo.Web.Infrastructure.Raven.Indexes
{
    public class UserActivityFeedIndex : AbstractIndexCreationTask<User, UserActivityFeedIndex.MapResult>
    {
        public class MapResult
        {
            public string UserId { get; set; }
            public string Name { get; set; }
            public DateTime Time { get; set; }
            public string RelatedTermId { get; set; }
        }

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
                           from action in user.ActionFeed
                           select new
                                      {
                                          UserId = user.Id,
                                          Name = user.Name,
                                          Time = action.Time,
                                          RelatedTermId = action.TermIdContext.ToString()
                                      };

            TransformResults =
                (database, actions) => from action in actions
                                       let alias = database.Load<Term>(action.RelatedTermId)
                                       select
                                           new
                                               {
                                                   UserId = action.UserId,
                                                   Name = action.Name,
                                                   Time = action.Time,
                                                   ActionRelatedTermTitle = alias.Title,
                                                   ActionRelatedTermSlug = alias.Slug
                                               };
        }
    }
}