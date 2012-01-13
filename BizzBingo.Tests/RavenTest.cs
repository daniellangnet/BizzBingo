using Raven.Client;
using Raven.Client.Embedded;

namespace BizzBingo.Tests
{
    public abstract class RavenTest
    {
        protected IDocumentStore GetDatabase()
        {
            var documentStore = new EmbeddableDocumentStore
                                    {
                                        RunInMemory = true
                                    };
            documentStore.Initialize();

            return documentStore;
        }
    }
}