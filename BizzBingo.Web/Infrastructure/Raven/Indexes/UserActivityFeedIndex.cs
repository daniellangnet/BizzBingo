using System;
using System.Linq;
using BizzBingo.Web.Models;
using Raven.Client.Indexes;

namespace BizzBingo.Web.Infrastructure.Raven.Indexes
{
    public class UserActivityFeedIndex : AbstractMultiMapIndexCreationTask<UserActivityFeedIndex.UserFeedResult>
    {
        public class UserFeedResult
        {
            public string UserId { get; set; }
            public string Name { get; set; }
            public string ActionRelatedTermTitle { get; set; }
            public string ActionRelatedTermSlug { get; set; }
            public DateTime Time { get; set; }
        }

        public UserActivityFeedIndex()
        {
            AddMap<User>(users => from user in users
                                  from action in user.ActionFeed
                                  select new
                                             {
                                                 ActionRelatedTermSlug = (string)null,
                                                 ActionRelatedTermTitle = (string)null,
                                                 Time = action.Time,
                                                 Name = user.Name,
                                                 UserId = user.Id
                                             });

            AddMap<Term>(terms => from term in terms
                                  select new
                                             {
                                                 ActionRelatedTermSlug = term.Slug,
                                                 ActionRelatedTermTitle = term.Title,
                                                 UserId = term.SharedByUserId,
                                                 Time = DateTime.MinValue,
                                                 Name = (string)null
                                             });

            Reduce = results => from result in results
                                group result by result.UserId
                                into g
                                select new
                                           {
                                               Name = g.Select(x => x.Name),
                                               UserId = g.Select(x => x.UserId),
                                               ActionRelatedTermTitle = g.Select(x => x.ActionRelatedTermTitle).Where(x => x != null).FirstOrDefault(),
                                               ActionRelatedTermSlug = g.Select(x => x.ActionRelatedTermSlug).Where(x => x != null).FirstOrDefault(),
                                               Time = g.Select(x => x.Time).Where(x => x != DateTime.MinValue).FirstOrDefault(),
                                           };
        }
    }
}