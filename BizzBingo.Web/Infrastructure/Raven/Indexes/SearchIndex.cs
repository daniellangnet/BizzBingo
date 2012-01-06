using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizzBingo.Web.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace BizzBingo.Web.Infrastructure.Raven.Indexes
{
    public class SearchIndex : AbstractIndexCreationTask<Term>
    {
        public SearchIndex()
        {
            Map = terms => from term in terms
                            select new { term.Title };

            Index(x => x.Title, FieldIndexing.Analyzed);
        }
    }
}