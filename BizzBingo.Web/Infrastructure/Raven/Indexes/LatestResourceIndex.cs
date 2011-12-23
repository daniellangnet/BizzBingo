using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizzBingo.Web.Models;
using global::Raven.Client.Indexes;

namespace BizzBingo.Web.Infrastructure.Raven.Indexes
{

    public class LatestResourceIndex : AbstractIndexCreationTask<Term, LatestResourceIndex.LatestResourceResult>
    {
        public class LatestResourceResult
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public DateTimeOffset CreatedOn { get; set; }
            public string TermTitle { get; set; }
            public string TermSlug { get; set; }
            public string Type { get; set; }
        }

        public LatestResourceIndex()
        {
            Map = terms => from term in terms
                           from resources in term.Resources
                           select new
                           {
                               Title = resources.Title,
                               CreatedOn = resources.CreatedOn,
                               Url = resources.Url,
                               TermTitle = term.Title,
                               TermSlug = term.Slug,
                               Type = resources.Type
                           };

            Reduce = results => from result in results
                                group result by result.Url
                                    into g
                                    select new
                                    {
                                        Title = g.Key,
                                        CreatedOn = g.Max(x => (DateTimeOffset)x.CreatedOn),
                                        Url = g.Select(x => x.Url).First(),
                                        TermTitle = g.Select(x => x.TermTitle).First(),
                                        TermSlug = g.Select(x => x.TermSlug).First(),
                                        Type = g.Select(x => x.Type).First()
                                    };

        }
    }
}