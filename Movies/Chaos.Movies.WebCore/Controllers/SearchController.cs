using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Chaos.Movies.WebCore.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;

    using Microsoft.AspNetCore.Http;

    public class SearchController : Controller
    {
        public async Task<IActionResult> Index(string searchText)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            var result = new List<MovieDto>();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var movies = await Movie.Static.SearchAsync(
                    new SearchParametersDto { RequireExactMatch = false, SearchLimit = 10, SearchText = searchText },
                    session);
                result.AddRange(movies.Select(m => m.ToContract()));
                await Movie.Static.GetUserRatingsAsync(result, session);
                result = result.OrderByDescending(m => m.ExternalRatings.FirstOrDefault()?.Value).ToList();
            }

            return this.View(result.AsReadOnly());
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