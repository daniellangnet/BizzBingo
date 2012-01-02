using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBingo.Web.Controllers;
using BizzBingo.Web.Infrastructure.Raven;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BizzBingo.Web.Infrastructure
{
    public class WebInstaller : IWindsorInstaller
    {
        private readonly LifestyleType _lifestyleType;
        
        public WebInstaller(LifestyleType lifestyleType)
        {
            this._lifestyleType = lifestyleType;
        }
        
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            this.InstallControllers(container);
            this.InstallInfrastructure(container);
        }
        
        private void InstallControllers(IWindsorContainer container)
        {
            var controllerTypes = from t in typeof(HomeController).Assembly.GetTypes()
                                  where typeof(IController).IsAssignableFrom(t)
                                  select t;
            // workaround http://stackoverflow.com/questions/2784846/castle-windsor-controller-factory-and-renderaction
            foreach (var t in controllerTypes)
            {
                // register controllers and append logging interceptor
                container.Register(Component.For(t).LifeStyle.Is(LifestyleType.Transient).Named(t.FullName));
            }
        }
        
        /// <summary>
        /// Register (web) infrastructure.
        /// </summary>
        /// <param name="container">
        /// The IoC Container.
        /// </param>
        private void InstallInfrastructure(IWindsorContainer container)
        {
            container.Register(
                AllTypes.FromAssembly(typeof(DocumentStoreHolder).Assembly).Pick().WithService.FirstInterface().Configure(
                    x => x.LifeStyle.Is(this._lifestyleType)));
            // for integrationtests with the IoC configuration - HttpContext would always be null
            // just plug a mock into the container for unit testing
            if (HttpContext.Current != null)
            {
                container.Register(
                    Component.For<HttpContextBase>().LifeStyle.Is(this._lifestyleType).UsingFactoryMethod(
                        () => new HttpContextWrapper(HttpContext.Current)));
                container.Register(
                    Component.For<HttpRequestBase>().LifeStyle.Is(this._lifestyleType).UsingFactoryMethod(
                        () => new HttpRequestWrapper(HttpContext.Current.Request)));
            }
        }
    }
}