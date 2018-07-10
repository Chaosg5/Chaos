//-----------------------------------------------------------------------
// <copyright file="HomeController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.WebCore.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            return View();
        }

        public async Task<IActionResult> About()
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<IActionResult> Contact()
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            ViewData["Message"] = "Your contact page.";

            return View();
        }
        
        public async Task<IActionResult> Movie(int movieId)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            var movie = await Model.Movie.Static.GetAsync(session, movieId);
            return this.View(movie.ToContract());
        }

        public IActionResult Error()
        {
            return View();
        }

        /// <summary>Validates the current user's session.</summary>
        /// <returns>The <see cref="UserSession"/>.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private async Task<UserSession> ValidateSessionAsync()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("SessionId"), out var sessionId))
            {
                return null;
            }

            try
            {
                var session = await UserSession.GetSessionAsync(sessionId);
                await session.ValidateSessionAsync();
                return session;
            }
            catch (InvalidSessionException)
            {
                return null;
            }
        }
    }
}
