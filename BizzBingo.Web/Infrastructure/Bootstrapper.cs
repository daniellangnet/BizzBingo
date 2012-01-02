using Castle.Core;
using Castle.Windsor;

namespace BizzBingo.Web.Infrastructure
{
    public static class Bootstrapper
    {
        public static IWindsorContainer Container { get; set; }
     
        public static void InstallDependencies(LifestyleType lifestyleType)
        {
            Container = new WindsorContainer();
            Container.Install(new WebInstaller(lifestyleType));
        }
    }
}