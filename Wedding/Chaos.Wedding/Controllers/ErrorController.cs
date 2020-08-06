//-----------------------------------------------------------------------
// <copyright file="ErrorController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Controllers
{
    using System.Web.Mvc;
    
    using NLog;

    /// <inheritdoc />
    /// <summary>Controller for errors.</summary>
    public class ErrorController : Controller
    {
        /// <summary>The logger.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>The index view for errors.</summary>
        /// <param name="exception">The exception thrown.</param>
        /// <returns>The index view.</returns>
        public ActionResult Index(HandleErrorInfo exception)
        {
            if (exception == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            Logger.Error(exception.Exception);
            return this.View(exception);
        }
    }
}