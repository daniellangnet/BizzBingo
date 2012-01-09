using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using BizzBingo.Web.Models;
using Embedly;
using Embedly.OEmbed;
using Raven.Client.Document;

namespace BizzBingo.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var store = new DocumentStore { ConnectionStringName = "RavenDB", Conventions = new DocumentConvention() { MaxNumberOfRequestsPerSession = 1000 }}.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    var terms = session.Query<Term>()
                        .Take(128)
                        .ToList();

                    if (terms.Count == 0)
                        return;

                    int patched = 0;

                    foreach (var term in terms)
                    {
                        Console.WriteLine(term.Title);

                        if(term.Resources != null)
                        {
                            foreach (var resource in term.Resources)
                            {
                                 Console.WriteLine(resource.Title);
                                 if(string.IsNullOrWhiteSpace(resource.ThumbnailUrl))
                                 {
                                     Console.WriteLine("-> Fetch Meta");

                                     Embedly.Client client = new Client(ConfigurationManager.AppSettings["embedlyKey"], new TimeSpan(0,0,0,30));
                                     var result = client.GetOEmbed(new Uri(resource.Url));

                                     if(result.Exception != null)
                                         Console.WriteLine("Failed!");

                                     if(result.Response.Type == ResourceType.Video)
                                     {
                                         var video = result.Response.AsVideo;
                                         resource.ThumbnailUrl = video.ThumbnailUrl;
                                         resource.EmbedCode = video.Html;
                                         resource.Type = "video";
                                         patched += 1;
                                     }
                                     else if(result.Response.Type == ResourceType.Rich)
                                     {
                                         var rich = result.Response.AsRich;
                                         resource.ThumbnailUrl = rich.ThumbnailUrl;
                                         resource.EmbedCode = rich.Html;
                                         resource.Type = "rich";
                                         patched += 1;
                                     }
                                     session.SaveChanges();
                                 }
                            }
                        }
                    }

                    Console.WriteLine("Patched: {0}", patched);
                }
            }

            Console.ReadLine();
        }
    }
}
