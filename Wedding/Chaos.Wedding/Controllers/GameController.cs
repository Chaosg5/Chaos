//-----------------------------------------------------------------------
// <copyright file="GameController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;
    using Chaos.Wedding.Models;
    using Chaos.Wedding.Models.Games;

    using NLog;

    using Contract = Chaos.Wedding.Models.Games.Contract;

    /// <inheritdoc />
    /// <summary>The <see cref="Controller"/> for <see cref="Models.Games"/>.</summary>
    public class GameController : Controller
    {
        /// <summary>The logger.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>The last <see cref="DateTime"/> when <see cref="Team.GameScores"/> was updated.</summary>
        private static DateTime lastGameScoreUpdate = DateTime.MinValue;

        /// <summary>The levels of user notifications.</summary>
        private enum NotificationLevel
        {
            /// <summary>An error message.</summary>
            Danger,

            /// <summary>A warning message</summary>
            Warning,

            /// <summary>An information message.</summary>
            Info,

            /// <summary>A success message.</summary>
            Success
        }

        /// <summary>The different ways of relaying the results to the user.</summary>
        private enum ResponseAction
        {
            /// <summary>Redirects the user to another page.</summary>
            Redirect,

            /// <summary>Shows a notification to the user.</summary>
            Notification
        }

        /// <summary>Sets up the <see cref="Game"/> and <see cref="Team"/> for the user and relays them to the <see cref="Game"/>.</summary>
        /// <param name="teamLookup">Optional <see cref="Team.LookupId"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public async Task<ActionResult> Index(string teamLookup = null)
        {
            // ToDo: System texts
            // ToDo: Score calculation for Sort
            // ToDo: Display max number of answers for MultiChoice
            // ToDo: Touch move center instead of corner

            // ToDo: Only show supported ChallengeTypes for admin?
            // ToDo: Validate Questions and alternatives when editing
            Team team = null;
            if (Guid.TryParse(teamLookup, out var teamId))
            {
                team = await Team.Static.GetAsync(await SessionHandler.GetSessionAsync(), teamId);
                this.Session["TeamId"] = team.Id;
                ViewBag.TeamLookup = team.LookupId.ToString("D").Substring(0, 4);
            }

            await this.SetSystemData();
            if (this.Session["TeamId"] == null)
            {
                return this.View();
            }

            team = team ?? await Team.Static.GetAsync(await SessionHandler.GetSessionAsync(), (int)this.Session["TeamId"]);
            if (team.GameScores.Count == 1)
            {
                this.Session["GameId"] = team.GameScores.Keys.First();
            }

            if (this.Session["GameId"] != null)
            {
                return this.RedirectToAction("PlayGame", new { GameId = (int)this.Session["GameId"] });
            }

            var games = await Game.Static.GetAllAsync(await SessionHandler.GetSessionAsync());
            return this.View(games.Select(g => g.ToContract(this.GetUserLanguage())));
        }

        /// <summary>Looks up a <see cref="Team"/> with the <paramref name="lookupShort"/>.</summary>
        /// <param name="lookupShort">The short for the <see cref="Team.LookupId"/> of the <see cref="Team"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> GetTeamLookupShort(string lookupShort)
        {
            try
            {
                var teams = (await Team.Static.SearchAsync(
                    new SearchParametersDto { SearchText = lookupShort },
                    await SessionHandler.GetSessionAsync())).ToList();
                if (!teams.Any())
                {
                    throw new MissingResultException(string.Format(CultureInfo.InvariantCulture, "The specified team '{0}' doesn't exist.", lookupShort));
                }

                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The team has been successfully logged in.",
                        Url.Action("Index", "Game", new { teamLookup = teams.First().LookupId })));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Plays a <see cref="Game"/>.</summary>
        /// <param name="gameId">The <see cref="Game.Id"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Team"/> is not valid to be saved.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public async Task<ActionResult> PlayGame(int gameId)
        {
            if (!int.TryParse(this.Session["TeamId"]?.ToString(), out var teamId))
            {
                return this.RedirectToAction("Index", "Game");
            }

            var game = await GameCache.GameGetAsync(gameId);
            this.Session["GameId"] = game.Id;
            var team = await GameCache.TeamGetAsync((int)this.Session["TeamId"]);
            if (!team.GameScores.ContainsKey(gameId))
            {
                team.GameScores.Add(gameId, 0);
                await team.SaveAsync(await SessionHandler.GetSessionAsync());
            }

            if (!game.TeamIds.Contains(team.Id))
            {
                game.TeamIds.Add(team.Id);
            }

            var contract = game.ToContract(this.GetUserLanguage());
            await TeamChallenge.AddTeamChallengesAsync(contract, teamId);
            return this.View(contract);
        }

        /// <summary>Plays a <see cref="Game"/>.</summary>
        /// <param name="zoneId">The <see cref="Zone.Id"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public async Task<ActionResult> PlayZone(int zoneId)
        {
            if (!int.TryParse(this.Session["TeamId"]?.ToString(), out var teamId))
            {
                return this.RedirectToAction("Index", "Game");
            }

            var zone = await GameCache.ZoneGetAsync(zoneId);
            var contract = zone.ToContract(this.GetUserLanguage());
            await TeamChallenge.AddTeamChallengesAsync(contract, teamId);
            await TeamZone.AddTeamZonesAsync(contract, teamId);
            return this.View(contract);
        }

        /// <summary>Plays a <see cref="Game"/>.</summary>
        /// <param name="challengeId">The <see cref="Challenge.Id"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> PlayChallenge(int challengeId)
        {
            if (!int.TryParse(this.Session["TeamId"]?.ToString(), out var teamId))
            {
                return this.RedirectToAction("Index", "Game");
            }

            var teamChallenge = await GameCache.TeamChallengeGetAsync(teamId, challengeId);
            ViewBag.ChallengeLocked = teamChallenge.IsLocked;
            var challenge = await GameCache.ChallengeGetAsync(challengeId);
            var model = teamChallenge.MergeAnswersIntoChallenge(challenge, this.GetUserLanguage());
            return this.View(model);
        }

        /// <summary>Locks the <paramref name="challengeId"/> for the current <see cref="Team"/>.</summary>
        /// <param name="challengeId">The <see cref="Challenge.Id"/> of the <see cref="Challenge"/> to lock.</param>
        /// <param name="unlock">If the <see cref="Challenge"/> should be unlocked instead.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> LockTeamChallenge(int challengeId, bool unlock = false)
        {
            try
            {
                if (!int.TryParse(this.Session["TeamId"]?.ToString(), out var teamId))
                {
                    throw new InvalidSaveCandidateException("A team needs to be specified.");
                }

                if (unlock && (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"]))
                {
                    throw new InvalidSaveCandidateException("Only an administrator can unlock a challenge.");
                }

                var session = await SessionHandler.GetSessionAsync();
                var challenge = await GameCache.ChallengeGetAsync(challengeId);
                var teamChallenge = await GameCache.TeamChallengeGetAsync(teamId, challenge.Id);
                teamChallenge.CalculateScore(challenge, this.GetUserLanguage());
                teamChallenge.IsLocked = !unlock;
                await teamChallenge.SaveAsync(session);

                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The challenge has been successfully locked.",
                        Url.Action("PlayChallenge", "Game", new { challengeId = challenge.Id })));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Locks all <see cref="Challenge"/>s all <see cref="Team"/>s for the current <see cref="Game"/>.</summary>
        /// <param name="unlock">If the <see cref="Challenge"/>s should be unlocked instead.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> LockAllChallenges(bool unlock = false)
        {
            try
            {
                if (!int.TryParse(this.Session["GameId"]?.ToString(), out var gameId))
                {
                    throw new InvalidSaveCandidateException("A game needs to be specified.");
                }

                if (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"])
                {
                    throw new InvalidSaveCandidateException("Only an administrator can lock all challenges.");
                }

                var session = await SessionHandler.GetSessionAsync();
                var game = await GameCache.GameGetAsync(gameId);
                foreach (var challenge in game.Zones.SelectMany(z => z.Challenges))
                {
                    foreach (var teamId in game.TeamIds)
                    {
                        var teamChallenge = await GameCache.TeamChallengeGetAsync(teamId, challenge.Id);
                        if (teamChallenge.IsLocked == !unlock)
                        {
                            continue;
                        }

                        teamChallenge.CalculateScore(challenge, this.GetUserLanguage());
                        teamChallenge.IsLocked = !unlock;
                        await teamChallenge.SaveAsync(session);
                    }
                }

                return this.Json(this.JsonResult(ResponseAction.Notification, NotificationLevel.Success, "All challenges have been successfully locked."));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Unlocks the <paramref name="zoneId"/> for the current <see cref="Team"/>.</summary>
        /// <param name="zoneId">The <see cref="Zone.Id"/> of the <see cref="Zone"/> to unlock.</param>
        /// <param name="lockCode">The <see cref="Zone.LockCode"/> to match.</param>
        /// <param name="unlock">If the <see cref="Zone"/> should be locked instead.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> UnlockTeamZone(int zoneId, string lockCode, bool unlock = true)
        {
            try
            {
                if (!int.TryParse(this.Session["TeamId"]?.ToString(), out var teamId))
                {
                    throw new InvalidSaveCandidateException("A team needs to be specified.");
                }

                if (!unlock && (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"]))
                {
                    throw new InvalidSaveCandidateException("Only an administrator can lock a zone.");
                }

                var session = await SessionHandler.GetSessionAsync();
                var zone = await GameCache.ZoneGetAsync(zoneId);
                if (!string.Equals(lockCode, zone.LockCode, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidSaveCandidateException(string.Format(CultureInfo.InvariantCulture, "The code '{0}' is not correct.", lockCode));
                }

                var teamZone = await GameCache.TeamZoneGetAsync(teamId, zone.Id);
                teamZone.Unlocked = unlock;
                await teamZone.SaveAsync(session);

                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The zone has been successfully unlocked.",
                        Url.Action("PlayZone", "Game", new { zoneId = zone.Id })));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Save an <see cref="TeamAnswer"/> for the <paramref name="alternativeId"/>.</summary>
        /// <param name="alternativeId">The <see cref="TeamAnswer.AlternativeId"/>.</param>
        /// <param name="isAnswered">The <see cref="TeamAnswer.IsAnswered"/>.</param>
        /// <param name="answeredRow">The <see cref="TeamAnswer.AnsweredRow"/>.</param>
        /// <param name="answeredColumn">The <see cref="TeamAnswer.AnsweredColumn"/>.</param>
        /// <param name="answer">The <see cref="TeamAnswer.Answer"/>.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> SaveTeamAnswer(
            int alternativeId,
            bool isAnswered,
            byte answeredRow = 0,
            byte answeredColumn = 0,
            string answer = "")
        {
            try
            {
                if (!int.TryParse(this.Session["TeamId"]?.ToString(), out var teamId))
                {
                    throw new InvalidSaveCandidateException("A team needs to be specified.");
                }

                var session = await SessionHandler.GetSessionAsync();
                var alternative = await GameCache.AlternativeGetAsync(alternativeId);
                var question = await GameCache.QuestionGetAsync(alternative.QuestionId);
                var teamChallenge = await GameCache.TeamChallengeGetAsync(teamId, question.ChallengeId);
                await teamChallenge.UpdateAnswerAsync(
                    new Contract.TeamAnswer
                    {
                        TeamId = teamId,
                        ChallengeId = question.ChallengeId,
                        QuestionId = alternative.QuestionId,
                        AlternativeId = alternativeId,
                        AnsweredRow = answeredRow,
                        AnsweredColumn = answeredColumn,
                        IsAnswered = isAnswered,
                        Answer = answer
                    },
                    session);

                return this.Json(
                    this.JsonResult(
                        ResponseAction.Notification,
                        NotificationLevel.Success,
                        "The answer has been successfully saved.",
                        string.Empty,
                        string.Empty,
                        0));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>The help page.</summary>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult Help()
        {
            return this.View();
        }

        /// <summary>The score page.</summary>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> Score()
        {
            if (this.Session["GameId"] == null)
            {
                return this.RedirectToAction("Index", "Game");
            }

            await this.SetSystemData();
            var game = await GameCache.GameGetAsync((int)this.Session["GameId"]);
            var teams = await Task.WhenAll(game.TeamIds.Select(async t => await GameCache.TeamGetAsync(t)));
            if ((DateTime.Now - lastGameScoreUpdate).TotalMinutes > 5)
            {
                foreach (var team in teams)
                {
                    await team.UpdateScoreAsync(await SessionHandler.GetSessionAsync());
                }

                lastGameScoreUpdate = DateTime.Now;
            }

            return this.View(teams.Select(t => t.ToContract(game.Id)));
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
        public ActionResult SetUserLanguage(string language)
        {
            try
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
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Edits all <see cref="Game"/>s.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task<ActionResult> EditIndex()
        {
            await this.SetSystemData();
            if (!(bool)this.Session["SystemAdmin"])
            {
                if (int.TryParse(this.Session["TeamId"]?.ToString(), out var teamId))
                {
                    return this.RedirectToAction("EditTeam", "Game", new { teamId });
                }

                return this.View(new List<Contract.Game>());
            }

            var games = await Game.Static.GetAllAsync(await SessionHandler.GetSessionAsync());
            return this.View(games.Select(g => g.ToContract(this.GetUserLanguage())));
        }

        /// <summary>Edits a <see cref="Team"/>.</summary>
        /// <param name="teamId">The <see cref="Team.Id"/> to edit.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public async Task<ActionResult> EditTeam(int teamId = 0)
        {
            await this.SetSystemData();
            if (!(bool)this.Session["SystemAdmin"])
            {
                if (!int.TryParse(this.Session["TeamId"]?.ToString(), out var currentTeamId) || currentTeamId != teamId)
                {
                    return this.RedirectToAction("Index", "Game");
                }
            }

            if (teamId == 0)
            {
                // ReSharper disable once ExceptionNotDocumented
                var newTeam = new Team(string.Empty);
                return this.View(newTeam.ToContract(this.GetUserLanguage()));
            }

            var team = await GameCache.TeamGetAsync(teamId);
            return this.View(team.ToContract(this.GetUserLanguage()));
        }

        /// <summary>Edits a <see cref="Game"/>.</summary>
        /// <param name="gameId">The <see cref="Game.Id"/> to edit.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ActionResult> EditGame(int gameId = 0)
        {
            await this.SetSystemData();
            if (!(bool)this.Session["SystemAdmin"])
            {
                return this.RedirectToAction("Index", "Game");
            }

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
            if (!(bool)this.Session["SystemAdmin"])
            {
                return this.RedirectToAction("Index", "Game");
            }

            if (zoneId == 0)
            {
                if (gameId == 0)
                {
                    throw new ArgumentNullException(nameof(gameId), "Can't create a zone without a game.");
                }

                // ReSharper disable once ExceptionNotDocumented
                var newZone = new Zone(gameId, 0, 0, 0, 0, string.Empty, string.Empty, string.Empty);
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
            if (!(bool)this.Session["SystemAdmin"])
            {
                return this.RedirectToAction("Index", "Game");
            }

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
            if (!(bool)this.Session["SystemAdmin"])
            {
                return this.RedirectToAction("Index", "Game");
            }
            
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
        /// <param name="correctColumn">The <see cref="Alternative.CorrectColumn"/>.</param>
        /// <param name="correctRow">The <see cref="Alternative.CorrectRow"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Can't create a alternative without a question. <paramref name="questionId"/></exception>
        public async Task<ActionResult> EditAlternative(int alternativeId, int questionId = 0, byte correctColumn = 0, byte correctRow = 0)
        {
            await this.SetSystemData();
            if (!(bool)this.Session["SystemAdmin"])
            {
                return this.RedirectToAction("Index", "Game");
            }
            
            if (alternativeId == 0)
            {
                if (questionId == 0)
                {
                    throw new ArgumentNullException(nameof(questionId), "Can't create a alternative without a question.");
                }

                // ReSharper disable once ExceptionNotDocumented
                var newAlternative = new Alternative(questionId, correctRow, correctColumn, true, 0, string.Empty, string.Empty, string.Empty);
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
        public async Task<ActionResult> SaveGame(int gameId, string imageId, short width, short height, string titles)
        {
            try
            {
                if (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"])
                {
                    throw new InvalidSessionException("Only a system administrator can edit.");
                }

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
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Saves a <see cref="Zone"/>.</summary>
        /// <param name="zoneId">The <see cref="Zone.Id"/>.</param>
        /// <param name="gameId">The <see cref="Zone.GameId"/>.</param>
        /// <param name="width">The <see cref="Zone.Width"/>.</param>
        /// <param name="height">The <see cref="Zone.Height"/>.</param>
        /// <param name="positionX">The <see cref="Zone.PositionX"/>.</param>
        /// <param name="positionY">The <see cref="Zone.PositionY"/>.</param>
        /// <param name="imageId">The <see cref="Zone.ImageId"/>.</param>
        /// <param name="lockCode">The <see cref="Zone.LockCode"/>.</param>
        /// <param name="titles">The <see cref="Zone.Titles"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ActionResult> SaveZone(
            int zoneId,
            int gameId,
            short width,
            short height,
            short positionX,
            short positionY,
            string imageId,
            string lockCode,
            string titles)
        {
            try
            {
                if (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"])
                {
                    throw new InvalidSessionException("Only a system administrator can edit.");
                }

                if (zoneId == 0)
                {
                    var newZone = new Zone(gameId, width, height, positionX, positionY, imageId, lockCode, titles);
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
                        LockCode = lockCode,
                        Titles = new LanguageDescriptionCollection(titles).ToContract()
                    },
                    await SessionHandler.GetSessionAsync());
                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The zone has been successfully updated.",
                        Url.Action("EditGame", "Game", new { gameId = zone.GameId })));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Saves a <see cref="Challenge"/>.</summary>
        /// <param name="challengeId">The <see cref="Challenge.Id"/>.</param>
        /// <param name="zoneId">The <see cref="Challenge.ZoneId"/>.</param>
        /// <param name="challengeTypeId">The <see cref="Challenge.Type"/>.</param>
        /// <param name="challengeSubjectId">The <see cref="Challenge.Subject"/>.</param>
        /// <param name="difficultyId">The <see cref="Challenge.Difficulty"/>.</param>
        /// <param name="titles">The <see cref="Challenge.Titles"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ActionResult> SaveChallenge(int challengeId, int zoneId, int challengeTypeId, int challengeSubjectId, int difficultyId, string titles)
        {
            try
            {
                if (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"])
                {
                    throw new InvalidSessionException("Only a system administrator can edit.");
                }

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
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The challenge has been successfully updated.",
                        Url.Action("EditZone", "Game", new { zoneId = challenge.ZoneId })));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
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
        public async Task<ActionResult> SaveQuestion(
            int questionId,
            int challengeId,
            int challengeTypeId,
            int challengeSubjectId,
            int difficultyId,
            string imageId,
            string titles)
        {
            try
            {
                if (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"])
                {
                    throw new InvalidSessionException("Only a system administrator can edit.");
                }

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
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The question has been successfully updated.",
                        Url.Action("EditChallenge", "Game", new { challengeId = question.ChallengeId })));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
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
            try
            {
                if (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"])
                {
                    throw new InvalidSessionException("Only a system administrator can edit.");
                }

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
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Saves a <see cref="Team"/>.</summary>
        /// <param name="teamId">The <see cref="Team.Id"/>.</param>
        /// <param name="name">The <see cref="Team.Name"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public async Task<ActionResult> SaveTeam(int teamId, string name)
        {
            try
            {
                if (this.Session["SystemAdmin"] == null || !(bool)this.Session["SystemAdmin"])
                {
                    if (!int.TryParse(this.Session["TeamId"]?.ToString(), out var currentTeamId) || currentTeamId != teamId)
                    {
                        throw new InvalidSessionException("Only a system administrator can edit.");
                    }
                }

                if (teamId == 0)
                {
                    var newTeam = new Team(name);
                    await newTeam.SaveAsync(await SessionHandler.GetSessionAsync());
                    return this.Json(
                        this.JsonResult(
                            ResponseAction.Redirect,
                            NotificationLevel.Success,
                            "The team has been successfully created.",
                            Url.Action("Index", "Game", new { teamLookup = newTeam.LookupId })));
                }

                var team = await GameCache.TeamGetAsync(teamId);
                await team.UpdateAsync(
                    new Contract.Team
                    {
                        Id = teamId,
                        Name = name
                    },
                    await SessionHandler.GetSessionAsync());
                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "The team has been successfully updated.",
                        Url.Action("Index", "Game", new { teamLookup = team.LookupId })));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Reset the <see cref="GameCache"/>, reloading a cached objects.</summary>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult ResetGameCache()
        {
            GameCache.ClearAllCache();
            lastGameScoreUpdate = DateTime.MinValue;
            return this.Json(this.JsonResult(ResponseAction.Notification, NotificationLevel.Success, "The cache has been successfully cleared."));
        }

        /// <summary>Logs out the current <see cref="Team"/>.</summary>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult LogOut()
        {
            this.Session["TeamId"] = null;
            return this.Json(
                this.JsonResult(
                    ResponseAction.Redirect,
                    NotificationLevel.Success,
                    "The team has been successfully logged out.",
                    Url.Action("Index", "Game")));
        }

        /// <summary>Logs out the current <see cref="Team"/>.</summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult AdminLogin(string username, string password)
        {
            try
            {
                if (username != Movies.Model.Properties.Settings.Default.SystemUserName
                || password != Movies.Model.Properties.Settings.Default.SystemPassword)
                {
                    throw new InvalidSessionException("The specified login is not valid.");
                }

                this.Session["SystemAdmin"] = true;
                return this.Json(
                    this.JsonResult(
                        ResponseAction.Redirect,
                        NotificationLevel.Success,
                        "Successfully logged in as administrator.",
                        Url.Action("EditIndex", "Game")));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return this.Json(this.JsonException(exception));
            }
        }

        /// <summary>Gets the delay time in milliseconds for the <paramref name="level"/>.</summary>
        /// <param name="level">The <see cref="NotificationLevel"/>.</param>
        /// <returns>The time in milliseconds before closing the notification.</returns>
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

        /// <summary>Creates a <see cref="JsonResult"/> from a result to show to the user.</summary>
        /// <param name="exception">The <see cref="Exception"/>.</param>
        /// <param name="delay">The delay.</param>
        /// <returns>The <see cref="object"/>.</returns>
        private object JsonException(Exception exception, int delay = 30000)
        {
            return this.JsonResult(ResponseAction.Notification, NotificationLevel.Danger, exception.Message, string.Empty, string.Empty, delay);
        }

        /// <summary>Creates a <see cref="JsonResult"/> from a result to show to the user.</summary>
        /// <param name="action">The <see cref="ResponseAction"/>.</param>
        /// <param name="level">The <see cref="NotificationLevel"/>.</param>
        /// <param name="title">The title of the message.</param>
        /// <param name="url">The URL for <see cref="ResponseAction.Redirect"/>.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="delay">The delay.</param>
        /// <returns>The JSON <see cref="object"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is <see langword="null"/></exception>
        private object JsonResult(ResponseAction action, NotificationLevel level, string title, string url = "", string message = "", int delay = -1)
        {
            if (delay == -1)
            {
                delay = GetDelayFromLevel(level);
            }

            switch (action)
            {
                case ResponseAction.Redirect:
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        throw new ArgumentNullException(nameof(url), $"No URL was specified for the redirect. {title}");
                    }

                    this.Session["NotificationScript"] = $"window.notification('{level.ToString().ToLower()}', '{title}', '{message}', '{delay}');";
                    return new { action = action.ToString().ToLower(), url };
                case ResponseAction.Notification:
                default:
                    return new { action = action.ToString().ToLower(), level = level.ToString().ToLower(), title, message, delay };
            }
        }

        /// <summary>Loads and stores system data.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
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

            if (this.Session["SystemAdmin"] == null)
            {
                this.Session["SystemAdmin"] = false;
            }

            var userLanguage = this.GetUserLanguage();
            if (this.Session["SystemData"] == null || ((SystemData)this.Session["SystemData"]).UserLanguage != userLanguage)
            {
                var systemData = new SystemData
                {
                    UserLanguage = userLanguage,
                    ChallengeTypes = (await GameCache.ChallengeTypesGetAllAsync()).Select(t => t.ToContract(userLanguage)),
                    ChallengeSubjects = (await GameCache.ChallengeSubjectsGetAllAsync()).Select(s => s.ToContract(userLanguage)),
                    Difficulties = (await GameCache.DifficultiesGetAllAsync()).Select(d => d.ToContract(userLanguage)),
                    SystemTexts = (await GameCache.SystemTextsGetAllAsync()).Select(t => t.ToContract(userLanguage)).ToDictionary(t => t.TextKey, t => t),
                    IsAdmin = (bool)this.Session["SystemAdmin"]
                };
                this.Session["SystemData"] = systemData;
            }
        }

        /// <summary>Gets the current user's selected <see cref="CultureInfo"/>.</summary>
        /// <returns>The <see cref="CultureInfo.Name"/>.</returns>
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