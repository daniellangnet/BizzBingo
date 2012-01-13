using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizzBingo.Web.Infrastructure.Raven.Indexes;
using BizzBingo.Web.Models;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Xunit;
using Action = BizzBingo.Web.Models.Action;

namespace BizzBingo.Tests.Indexes
{
    public class UserActivityFeedIndexTest : RavenTest
    {
        [Fact]
        public void CanDoSimpleQuery()
        {
            using (var documentStore = GetDatabase())
            {
                IndexCreation.CreateIndexes(typeof(UserActivityFeedIndex).Assembly, documentStore);

                using (var documentSession = documentStore.OpenSession())
                {
                    var userId = Guid.NewGuid();
                    var term1Id = Guid.NewGuid();
                    var term2Id = Guid.NewGuid();

                    // seed some data
                    documentSession.Store(new Term
                                              {
                                                  Id = term1Id,
                                                  Title = "term1",
                                                  Slug = "term1-slug"
                                              });
                    documentSession.Store(new Term
                                              {
                                                  Id = term2Id,
                                                  Title = "term2",
                                                  Slug = "term2-slug"
                                              });
                    documentSession.Store(new User
                                              {
                                                  Id = Guid.NewGuid(),
                                                  Name = "daniel",
                                                  ActionFeed =
                                                      new List<Action>(new Action[]
                                                          {
                                                              new Action
                                                                  {
                                                                      Time = new DateTime(2012, 1, 1),
                                                                      TermIdContext = term1Id
                                                                  },
                                                              new Action
                                                                  {
                                                                      Time = new DateTime(2012, 1, 2),
                                                                      TermIdContext = term2Id
                                                                  }
                                                          })
                                              });

                    documentSession.SaveChanges();

                    var userActivityFeedItems = documentSession.Query<User, UserActivityFeedIndex>()
                        .Customize(x => x.WaitForNonStaleResults())
                        .As<UserActivityFeedIndex.Result>()
                        .ToList();
                    
                    Assert.Equal(2, userActivityFeedItems.Count);

                    Assert.Equal("daniel", userActivityFeedItems[0].Name);
                    Assert.Equal(new DateTime(2012, 1, 1), userActivityFeedItems[0].Time);
                    Assert.Equal("term1", userActivityFeedItems[0].ActionRelatedTermTitle);
                    Assert.Equal("term1-slug", userActivityFeedItems[0].ActionRelatedTermSlug);

                    Assert.Equal("daniel", userActivityFeedItems[1].Name);
                    Assert.Equal(new DateTime(2012, 1, 2), userActivityFeedItems[1].Time);
                    Assert.Equal("term2", userActivityFeedItems[1].ActionRelatedTermTitle);
                    Assert.Equal("term2-slug", userActivityFeedItems[1].ActionRelatedTermSlug);
                }
            }
        }
    }
}
