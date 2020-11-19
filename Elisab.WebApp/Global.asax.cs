using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elisab.BAL.AutoMapper;
using Elisab.WebApp.App_Start;
using Elisab.WebApp.Controllers;
using NLog;

namespace Elisab.WebApp
{
    public class MvcApplication : HttpApplication
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            Log.Info("Starting up...");
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();

            Log.Info("Routes and bundles registered");
            Log.Info("Started");
        }

        protected void Application_End()
        {
            Log.Info("Stopped");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Log.Error(exception, "Unhandled application exception");

            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.ClearError();

            if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            {
                return;
            }

            //ExecuteErrorController(httpContext, exception as HttpException);
        }

        private void ExecuteErrorController(HttpContext httpContext, HttpException exception)
        {
            var routeData = new RouteData();
            routeData.Values["controller"] = "Home";

            if (exception != null && exception.GetHttpCode() == (int)HttpStatusCode.NotFound)
            {
                routeData.Values["action"] = "Login";
            }
            else
            {
                routeData.Values["action"] = "Login";
            }

            using (Controller controller = new ErrorController())
            {
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
        }
    }
}
