//-----------------------------------------------------------------------
// <copyright file="LoginController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.WebCore.Controllers
{
    using System.Threading.Tasks;

    using Chaos.Movies.Model;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>Handles user login.</summary>
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserLogin(string username, string password)
        {
            var userLogin = new Contract.UserLogin(username, password, this.HttpContext.Connection.RemoteIpAddress.ToString());
            var session = await UserSession.Static.CreateSessionAsync(userLogin);
            HttpContext.Session.SetString("SessionId", session.SessionId.ToString());
            return this.RedirectToAction("Index", "Home");
        }
    }
}