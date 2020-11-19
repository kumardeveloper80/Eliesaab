using Elisab.WebApp.Helper;
using System;
using System.Web.Http;
using System.Web.Http.WebHost;

namespace Elisab.WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
