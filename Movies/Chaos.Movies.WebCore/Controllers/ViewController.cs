//-----------------------------------------------------------------------
// <copyright file="ViewController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.WebCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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

            var userLanguage = "sv-SE";
            var movie = (await Model.Movie.Static.GetAsync(session, movieId)).ToContract(userLanguage);
            await Model.Movie.Static.GetUserItemDetailsAsync(movie, session, userLanguage);
            var s = new Tuple<MovieDto, ReadOnlyCollection<WatchTypeDto>>(
                movie,
                new ReadOnlyCollection<WatchTypeDto>((await GlobalCache.GetAllWatchTypesAsync()).Select(w => w.ToContract(userLanguage)).ToList()));
            return this.View(s);
        }

        public async Task<IActionResult> Character(int characterId)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            var userLanguage = "sv-SE";
            var character = (await Model.Character.Static.GetAsync(session, characterId)).ToContract(userLanguage);
            await Model.Character.Static.GetUserItemDetailsAsync(character, session, userLanguage);
            return this.View(character);
        }

        public async Task<IActionResult> Person(int personId)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            var userLanguage = "sv-SE";
            var person = (await Model.Person.Static.GetAsync(session, personId)).ToContract(userLanguage);
            await Model.Person.Static.GetUserItemDetailsAsync(person, session, userLanguage);
            return this.View(person);
        }

        #endregion

        #region Edit

        [HttpPost]
        public async Task<ActionResult> SaveMovieRating(int movieId, int ratingTypeId, int ratingValue)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            await Model.Movie.SaveUserRatingAsync(movieId, ratingTypeId, ratingValue, session);
            return Json("true");
        }

        [HttpPost]
        public async Task<ActionResult> SavePersonRating(int movieId, int personId, int roleId, int departmentId, int ratingValue)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            await Model.Person.SaveUserRatingAsync(personId, roleId, departmentId, movieId, ratingValue, session);
            return Json("true");
            //return this.RedirectToAction("Movie", new { movieId });
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
                return Json("true");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveWatchMovie(int movieId, DateTime watchDate, bool dateUncertain, int watchTypeId)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            await Model.Movie.SaveWatchMovieAsync(movieId, watchDate, dateUncertain, watchTypeId, session);
            return Json("true");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteWatchMovie(int movieId, int watchId, int watchTypeId)
        {
            var session = await this.ValidateSessionAsync();
            if (session == null)
            {
                return this.RedirectToAction("Index", "Login");
            }

            await Model.Movie.DeleteWatchMovieAsync(watchId, movieId, watchTypeId, session);
            return Json("true");
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
