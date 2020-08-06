//-----------------------------------------------------------------------
// <copyright file="Global.asax.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Chaos.Wedding.Controllers;

    using NLog;

    /// <inheritdoc />
    /// <summary>The MVC application.</summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>The logger.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>The application start.</summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>Handles applications errors.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = string.Empty;
            var currentAction = string.Empty;
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (!string.IsNullOrEmpty(currentRouteData?.Values["controller"]?.ToString()))
            {
                currentController = currentRouteData.Values["controller"].ToString();
            }

            if (!string.IsNullOrEmpty(currentRouteData?.Values["action"]?.ToString()))
            {
                currentAction = currentRouteData.Values["action"].ToString();
            }

            var ex = Server.GetLastError();
            Logger.Error(ex);
            var routeData = new RouteData();
            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException exception ? exception.GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";
            routeData.Values["exception"] = new HandleErrorInfo(ex, currentController, currentAction);

            IController errorHandlerController = new ErrorController();
            var wrapper = new HttpContextWrapper(httpContext);
            var requestContext = new RequestContext(wrapper, routeData);
            errorHandlerController.Execute(requestContext);
        }
    }
}
