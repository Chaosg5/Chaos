//-----------------------------------------------------------------------
// <copyright file="GameController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;
    using Chaos.Wedding.Models;
    using Chaos.Wedding.Models.Games;

    using Contract = Chaos.Wedding.Models.Games.Contract;

    /// <inheritdoc />
    /// <summary>The <see cref="Controller"/> for <see cref="Models.Games"/>.</summary>
    public class GameController : Controller
    {
        private enum NotificationLevel
        {
            Danger,
            Warning,
            Info,
            Success
        }

        private enum ResponseAction
        {
            Redirect,
            Notification
        }

        public async Task<ActionResult> Index()
        {
            // if (this.Session["Team"] != null)
            if (this.Session["GameId"] != null && int.TryParse(this.Session["GameId"].ToString(), out var gameId))
            {
                this.RedirectToAction("PlayGame", new { GameId = gameId });
            }

            return this.View();
        }

        public async Task<ActionResult> Help()
        {
            return this.View();
        }

        public async Task<ActionResult> Score()
        {
            return this.View();
        }

        /// <summary>View to choose <see cref="CultureInfo"/> for the current <see cref="HttpSessionState"/>.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ActionResult> Language()
        {
            await this.SetSystemData();
            return this.View();
        }

        /// <summary>Sets the <see cref="CultureInfo"/> for the current <see cref="HttpSessionState"/> to the <paramref name="language"/>.</summary>
        /// <param name="language">The <see cref="CultureInfo.Name"/> of the <see cref="CultureInfo"/> to set.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="language"/> is <see langword="null"/></exception>
        public ActionResult SetUserLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException(nameof(language));
            }

            var culture = new CultureInfo(language);
            this.Session["UserLanguage"] = culture.Name;
            return this.Json(
                this.JsonResult(
                    ResponseAction.Notification,
                    NotificationLevel.Success,
                    $"The language has been successfully set to {culture.NativeName}."));
        }

        /// <summary>Edits all <see cref="Game"/>s.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task<ActionResult> EditIndex()
        {
            await this.SetSystemData();
            var games = await Game.Static.GetAllAsync(await SessionHandler.GetSessionAsync());
            return this.View(games.Select(g => g.ToContract(this.GetUserLanguage())));
        }

        /// <summary>Edits a <see cref="Game"/>.</summary>
        /// <param name="gameId">The <see cref="Game.Id"/> to edit.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ActionResult> EditGame(int gameId = 0)
        {
            await this.SetSystemData();
            if (gameId == 0)
            {
                // ReSharper disable once ExceptionNotDocumented
                var newGame = new Game(string.Empty, 0, 0, string.Empty);
                return this.View(newGame.ToContract(this.GetUserLanguage()));
            }

            var game = await GameCache.GameGetAsync(gameId);
            return this.View(game.ToContract(this.GetUserLanguage()));
        }

        /// <summary>Edits a <see cref="Zone"/>.</summary>
        /// <param name="zoneId">The <see cref="Zone.Id"/>.</param>
        /// <param name="gameId">The <see cref="Zone.GameId"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Can't create a zone without a game. <paramref name="gameId"/></exception>
        public async Task<ActionResult> EditZone(int zoneId, int gameId = 0)
        {
            await this.SetSystemData();
            if (zoneId == 0)
            {
                if (gameId == 0)
                {
                    throw new ArgumentNullException(nameof(gameId), "Can't create a zone without a game.");
                }

                // ReSharper disable once ExceptionNotDocumented
                var newZone = new Zone(gameId, 0, 0, 0, 0, string.Empty, string.Empty);
                return this.View(newZone.ToContract(this.GetUserLanguage()));
            }

            var zone = await GameCache.ZoneGetAsync(zoneId);
            return this.View(zone.ToContract(this.GetUserLanguage()));
        }

        /// <summary>Edits a <see cref="Challenge"/>.</summary>
        /// <param name="challengeId">The <see cref="Challenge.Id"/>.</param>
        /// <param name="zoneId">The <see cref="Challenge.ZoneId"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Can't create a challenge without a zone. <paramref name="zoneId"/></exception>
        public async Task<ActionResult> EditChallenge(int challengeId, int zoneId = 0)
        {
            await this.SetSystemData();
            if (challengeId == 0)
            {
                if (zoneId == 0)
                {
                    throw new ArgumentNullException(nameof(zoneId), "Can't create a challenge without a zone.");
                }

                // ReSharper disable once ExceptionNotDocumented
                var newChallenge = new Challenge(zoneId, null, null, null, string.Empty);
                return this.View(newChallenge.ToContract(this.GetUserLanguage()));
            }

            var challenge = await GameCache.ChallengeGetAsync(challengeId);
            return this.View(challenge.ToContract(this.GetUserLanguage()));
        }

        /// <summary>Edits a <see cref="Question"/>.</summary>
        /// <param name="questionId">The <see cref="Question.Id"/>.</param>
        /// <param name="challengeId">The <see cref="Question.ChallengeId"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Can't create a question without a challenge. <paramref name="challengeId"/></exception>
        public async Task<ActionResult> EditQuestion(int questionId, int challengeId = 0)
        {
            await this.SetSystemData();
            if (questionId == 0)
            {
                if (challengeId == 0)
                {
                    throw new ArgumentNullException(nameof(challengeId), "Can't create a question without a challenge.");
                }

                // ReSharper disable once ExceptionNotDocumented
                var newQuestion = new Question(challengeId, null, null, null, string.Empty, string.Empty);
                return this.View(newQuestion.ToContract(this.GetUserLanguage()));
            }

            var question = await GameCache.QuestionGetAsync(questionId);
            return this.View(question.ToContract(this.GetUserLanguage()));
        }

        /// <summary>Edits a <see cref="Alternative"/>.</summary>
        /// <param name="alternativeId">The <see cref="Alternative.Id"/>.</param>
        /// <param name="questionId">The <see cref="Alternative.QuestionId"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Can't create a alternative without a question. <paramref name="questionId"/></exception>
        public async Task<ActionResult> EditAlternative(int alternativeId, int questionId = 0)
        {
            await this.SetSystemData();
            if (alternativeId == 0)
            {
                if (questionId == 0)
                {
                    throw new ArgumentNullException(nameof(questionId), "Can't create a alternative without a question.");
                }

                // ReSharper disable once ExceptionNotDocumented
                var newAlternative = new Alternative(questionId, 0, 0, false, 0, string.Empty, string.Empty, string.Empty);
                return this.View(newAlternative.ToContract(this.GetUserLanguage()));
            }

            var alternative = await GameCache.AlternativeGetAsync(alternativeId);
            return this.View(alternative.ToContract(this.GetUserLanguage()));
        }

        /// <summary>Saves a <see cref="Game"/>.</summary>
        /// <param name="gameId">The <see cref="Game.Id"/>.</param>
        /// <param name="imageId">The <see cref="Game.ImageId"/>.</param>
        /// <param name="width">The <see cref="Game.Width"/>.</param>
        /// <param name="height">The <see cref="Game.Height"/>.</param>
        /// <param name="titles">The <see cref="Game.Titles"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public async Task<ActionResult> SaveGame(int gameId, string imageId, short width, short height, string titles)
        {
            if (gameId == 0)
            {
                var newGame = new Game(imageId, width, height, titles);
                await newGame.SaveAsync(await SessionHandler.GetSessionAsync());
                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The game has been successfully saved.",
                        Url.Action("EditGame", "Game", new { gameId = newGame.Id })));
            }

            var game = await GameCache.GameGetAsync(gameId);
            await game.UpdateAsync(
                new Contract.Game
                {
                    Id = gameId,
                    ImageId = imageId,
                    Width = width,
                    Height = height,
                    Titles = new LanguageDescriptionCollection(titles).ToContract()
                },
                await SessionHandler.GetSessionAsync());
            return this.Json(
                this.JsonResult(
                    ResponseAction.Notification,
                    NotificationLevel.Success,
                    "The game has been successfully updated."));
        }

        /// <summary>Saves a <see cref="Zone"/>.</summary>
        /// <param name="zoneId">The <see cref="Zone.Id"/>.</param>
        /// <param name="gameId">The <see cref="Zone.GameId"/>.</param>
        /// <param name="width">The <see cref="Zone.Width"/>.</param>
        /// <param name="height">The <see cref="Zone.Height"/>.</param>
        /// <param name="positionX">The <see cref="Zone.PositionX"/>.</param>
        /// <param name="positionY">The <see cref="Zone.PositionY"/>.</param>
        /// <param name="imageId">The <see cref="Zone.ImageId"/>.</param>
        /// <param name="titles">The <see cref="Zone.Titles"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<ActionResult> SaveZone(
            int zoneId,
            int gameId,
            short width,
            short height,
            short positionX,
            short positionY,
            string imageId,
            string titles)
        {
            if (zoneId == 0)
            {
                var newZone = new Zone(gameId, width, height, positionX, positionY, imageId, titles);
                await newZone.SaveAsync(await SessionHandler.GetSessionAsync());
                await GameCache.ZoneAddedAsync(newZone);
                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The zone has been successfully created.",
                        Url.Action("EditZone", "Game", new { zoneId = newZone.Id, gameId = newZone.GameId })));
            }

            var zone = await GameCache.ZoneGetAsync(zoneId);
            await zone.UpdateAsync(
                new Contract.Zone
                {
                    Id = zoneId,
                    GameId = gameId,
                    Width = width,
                    Height = height,
                    PositionX = positionX,
                    PositionY = positionY,
                    ImageId = imageId,
                    Titles = new LanguageDescriptionCollection(titles).ToContract()
                },
                await SessionHandler.GetSessionAsync());
            return this.Json(
                this.JsonResult(
                    ResponseAction.Notification,
                    NotificationLevel.Success,
                    "The zone has been successfully updated."));
        }

        /// <summary>Saves a <see cref="Challenge"/>.</summary>
        /// <param name="challengeId">The <see cref="Challenge.Id"/>.</param>
        /// <param name="zoneId">The <see cref="Challenge.ZoneId"/>.</param>
        /// <param name="challengeTypeId">The <see cref="Challenge.Type"/>.</param>
        /// <param name="challengeSubjectId">The <see cref="Challenge.Subject"/>.</param>
        /// <param name="difficultyId">The <see cref="Challenge.Difficulty"/>.</param>
        /// <param name="titles">The <see cref="Challenge.Titles"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<ActionResult> SaveChallenge(int challengeId, int zoneId, int challengeTypeId, int challengeSubjectId, int difficultyId, string titles)
        {
            if (challengeId == 0)
            {
                var newChallenge = new Challenge(
                    zoneId,
                    await GameCache.ChallengeTypeGetAsync(challengeTypeId),
                    await GameCache.ChallengeSubjectGetAsync(challengeSubjectId),
                    await GameCache.DifficultyGetAsync(difficultyId),
                    titles);
                await newChallenge.SaveAsync(await SessionHandler.GetSessionAsync());
                await GameCache.ChallengeAddedAsync(newChallenge);
                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The challenge has been successfully created.",
                        Url.Action("EditChallenge", "Game", new { challengeId = newChallenge.Id, zoneId = newChallenge.ZoneId })));
            }

            var challenge = await GameCache.ChallengeGetAsync(challengeId);
            await challenge.UpdateAsync(
                new Contract.Challenge
                {
                    Id = challengeId,
                    ZoneId = zoneId,
                    Type = (await GameCache.ChallengeTypeGetAsync(challengeTypeId)).ToContract(),
                    Subject = (await GameCache.ChallengeSubjectGetAsync(challengeSubjectId)).ToContract(),
                    Difficulty = (await GameCache.DifficultyGetAsync(difficultyId)).ToContract(),
                    Titles = new LanguageDescriptionCollection(titles).ToContract()
                },
                await SessionHandler.GetSessionAsync());

            return this.Json(
                this.JsonResult(
                    ResponseAction.Notification,
                    NotificationLevel.Success,
                    "The challenge has been successfully updated."));
        }

        /// <summary>Saves a <see cref="Question"/>.</summary>
        /// <param name="questionId">The <see cref="Question.Id"/>.</param>
        /// <param name="challengeId">The <see cref="Question.ChallengeId"/>.</param>
        /// <param name="challengeTypeId">The <see cref="Question.Type"/>.</param>
        /// <param name="challengeSubjectId">The <see cref="Question.Subject"/>.</param>
        /// <param name="difficultyId">The <see cref="Question.Difficulty"/>.</param>
        /// <param name="imageId">The <see cref="Question.ImageId"/>.</param>
        /// <param name="titles">The <see cref="Question.Titles"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<ActionResult> SaveQuestion(
            int questionId,
            int challengeId,
            int challengeTypeId,
            int challengeSubjectId,
            int difficultyId,
            string imageId,
            string titles)
        {
            if (questionId == 0)
            {
                var newQuestion = new Question(
                    challengeId,
                    await GameCache.ChallengeTypeGetAsync(challengeTypeId),
                    await GameCache.ChallengeSubjectGetAsync(challengeSubjectId),
                    await GameCache.DifficultyGetAsync(difficultyId),
                    imageId,
                    titles);
                await newQuestion.SaveAsync(await SessionHandler.GetSessionAsync());
                await GameCache.QuestionAddedAsync(newQuestion);
                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The question has been successfully created.",
                        Url.Action("EditQuestion", "Game", new { questionId = newQuestion.Id, challengeId = newQuestion.ChallengeId })));
            }

            var question = await GameCache.QuestionGetAsync(questionId);
            await question.UpdateAsync(
                new Contract.Question
                {
                    Id = questionId,
                    ChallengeId = challengeId,
                    Type = (await GameCache.ChallengeTypeGetAsync(challengeTypeId)).ToContract(),
                    Subject = (await GameCache.ChallengeSubjectGetAsync(challengeSubjectId)).ToContract(),
                    Difficulty = (await GameCache.DifficultyGetAsync(difficultyId)).ToContract(),
                    ImageId = imageId,
                    Titles = new LanguageDescriptionCollection(titles).ToContract()
                },
                await SessionHandler.GetSessionAsync());
            return this.Json(
                this.JsonResult(
                    ResponseAction.Notification,
                    NotificationLevel.Success,
                    "The question has been successfully updated."));
        }

        /// <summary>Saves a <see cref="Alternative"/>.</summary>
        /// <param name="alternativeId">The <see cref="Alternative.Id"/>.</param>
        /// <param name="questionId">The <see cref="Alternative.QuestionId"/>.</param>
        /// <param name="correctRow">The <see cref="Alternative.CorrectRow"/>.</param>
        /// <param name="correctColumn">The <see cref="Alternative.CorrectColumn"/>.</param>
        /// <param name="isCorrect">The <see cref="Alternative.IsCorrect"/>.</param>
        /// <param name="scoreValue">The <see cref="Alternative.ScoreValue"/>.</param>
        /// <param name="correctAnswer">The <see cref="Alternative.CorrectAnswer"/>.</param>
        /// <param name="imageId">The <see cref="Alternative.ImageId"/>.</param>
        /// <param name="titles">The <see cref="Alternative.Titles"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<ActionResult> SaveAlternative(
            int alternativeId,
            int questionId,
            byte correctRow,
            byte correctColumn,
            bool isCorrect,
            byte scoreValue,
            string correctAnswer,
            string imageId,
            string titles)
        {
            if (alternativeId == 0)
            {
                var newAlternative = new Alternative(questionId, correctRow, correctColumn, isCorrect, scoreValue, correctAnswer, imageId, titles);
                await newAlternative.SaveAsync(await SessionHandler.GetSessionAsync());
                await GameCache.AlternativeAddedAsync(newAlternative);
                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The alternative has been successfully created.",
                        Url.Action("EditQuestion", "Game", new { questionId = newAlternative.QuestionId })));
            }

            var alternative = await GameCache.AlternativeGetAsync(alternativeId);
            await alternative.UpdateAsync(
                new Contract.Alternative
                {
                    Id = alternativeId,
                    QuestionId = questionId,
                    CorrectRow = correctRow,
                    CorrectColumn = correctColumn,
                    IsCorrect = isCorrect,
                    ScoreValue = scoreValue,
                    CorrectAnswer = correctAnswer,
                    ImageId = imageId,
                    Titles = new LanguageDescriptionCollection(titles).ToContract()
                },
                await SessionHandler.GetSessionAsync());
            return this.Json(
                this.JsonResult(
                    ResponseAction.Redirect,
                    NotificationLevel.Success,
                    "The alternative has been successfully updated.",
                    Url.Action("EditQuestion", "Game", new { questionId = alternative.QuestionId })));
        }

        public async Task<ActionResult> PlayGame(int gameId)
        {
            var game = await GameCache.GameGetAsync(gameId);
            return this.View(game);
        }

        public async Task<ActionResult> PlayZone()
        {
            return this.View();
        }

        public async Task<ActionResult> PlayChallenge()
        {
            return this.View();
        }

        private static int GetDelayFromLevel(NotificationLevel level)
        {
            switch (level)
            {
                case NotificationLevel.Danger:
                    return 60000;
                case NotificationLevel.Warning:
                case NotificationLevel.Info:
                    return 30000;
                case NotificationLevel.Success:
                default:
                    return 15000;
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="url"/> is <see langword="null"/></exception>
        private object JsonResult(ResponseAction action, NotificationLevel level, string title, string url = "", string message = "")
        {
            var delay = GetDelayFromLevel(level);
            switch (action)
            {
                case ResponseAction.Redirect:
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        // ToDo: Change to warning notification
                        throw new ArgumentNullException(nameof(url), $"No URL was specified for the redirect. {title}");
                    }

                    this.Session["NotificationScript"] = $"window.notification('{level.ToString().ToLower()}', '{title}', '{message}', '{delay}');";
                    return new { action = action.ToString().ToLower(), url };
                case ResponseAction.Notification:
                default:
                    return new { action = action.ToString().ToLower(), level = level.ToString().ToLower(), title, message, delay };
            }
        }

        private async Task SetSystemData()
        {
            if (this.Session["Languages"] == null)
            {
                this.Session["Languages"] = new List<string> { "sv-SE", "en-US", "pt-BR" };
            }

            if (this.Session["UserLanguage"] == null)
            {
                this.Session["UserLanguage"] = "sv-SE";
            }

            if (this.Session["NotificationScript"] != null)
            {
                this.ViewBag.NotificationScript = (string)this.Session["NotificationScript"];
                this.Session["NotificationScript"] = null;
            }

            var userLanguage = this.GetUserLanguage();
            if (this.Session["SystemData"] == null || ((SystemData)this.Session["SystemData"]).UserLanguage != userLanguage)
            {
                var systemData = new SystemData
                {
                    UserLanguage = userLanguage,
                    ChallengeTypes = (await GameCache.ChallengeTypesGetAllAsync()).Select(t => t.ToContract(userLanguage)),
                    ChallengeSubjects = (await GameCache.ChallengeSubjectsGetAllAsync()).Select(s => s.ToContract(userLanguage)),
                    Difficulties = (await GameCache.DifficultiesGetAllAsync()).Select(d => d.ToContract(userLanguage))
                };
                this.Session["SystemData"] = systemData;
            }
        }

        private string GetUserLanguage()
        {
            if (this.Session["UserLanguage"] == null)
            {
                return "sv-SE";
            }

            return (string)this.Session["UserLanguage"];
        }
    }
}