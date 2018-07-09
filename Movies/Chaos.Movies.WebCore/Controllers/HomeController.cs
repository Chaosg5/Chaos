//-----------------------------------------------------------------------
// <copyright file="HomeController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.WebCore.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Chaos.Movies.Model;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Login");
            }

            return View();
        }

        public async Task<IActionResult> About()
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Login");
            }

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<IActionResult> Contact()
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Login");
            }

            ViewData["Message"] = "Your contact page.";

            return View();
        }
        
        public IActionResult Login()
        {
            ViewData["Message"] = "Login.";

            return View();
        }

        public async Task<IActionResult> Movie()
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Login");
            }

            var movie = await Model.Movie.Static.GetAsync(session, 766949);
            return this.View(movie);
        }

        public IActionResult Error()
        {
            return View();
        }

        private async Task<UserSession> ValidateSessionAsync()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("SessionId"), out var sessionId))
            {
                return null;
            }

            return null;
        }
    }
}
