using betterDeckOfCards.Data;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace betterDeckOfCards
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IDeckRepository, DeckRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}