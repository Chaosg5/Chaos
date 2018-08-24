//-----------------------------------------------------------------------
// <copyright file="ViewController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.WebCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class ViewController : Controller
    {
        #region Viwes

        public async Task<IActionResult> Movie(int movieId)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            var movie = await Model.Movie.Static.GetAsync(session, movieId);
            var userLanguage = "sv-SE";
            var result = movie.ToContract(userLanguage);
            await Model.Movie.Static.GetUserRatingsAsync(new List<MovieDto> { result }, session);
            await Model.Movie.Static.GetUserItemDetailsAsync(result, session, userLanguage);
            return this.View(result);
        }

        public async Task<IActionResult> Character(int movieId)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            var character = await Model.Character.Static.GetAsync(session, movieId);
            return this.View(character.ToContract());
        }

        public async Task<IActionResult> Person(int movieId)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            var person = await Model.Person.Static.GetAsync(session, movieId);
            return this.View(person.ToContract());
        }

        #endregion

        #region Edit
        
        [HttpPost]
        public async Task<ActionResult> SavePersonRating(int movieId, int personId, int roleId, int departmentId, int ratingValue)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            await Model.Person.SaveUserRatingAsync(personId, roleId, departmentId, movieId, ratingValue, session);
            return this.RedirectToAction("Movie", new { movieId });
        }

        [HttpPost]
        public async Task<ActionResult> SaveCharacterRating(int movieId, int characterId, int personId, int ratingValue)
        {
            try
            {
                var session = await this.ValidateSessionAsync();
                if (session == null)
                {
                    return this.RedirectToAction("Index", "Login");
                }

                await Model.Character.SaveUserRatingAsync(characterId, personId, movieId, ratingValue, session);
                return this.RedirectToAction("Movie", new { movieId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion


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
