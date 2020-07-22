//-----------------------------------------------------------------------
// <copyright file="GlobalCache.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;
    using Chaos.Wedding.Models.Games;

    /// <summary>Provides a global cache of objects.</summary>
    public static class GameCache
    {
        /// <summary>Gets all available <see cref="Alternative"/>s.</summary>
        private static readonly AsyncCache<int, Alternative> Alternatives = new AsyncCache<int, Alternative>(i => Alternative.Static.GetAsync(session, i));

        /// <summary>Gets all available <see cref="Challenge"/>s.</summary>
        private static readonly AsyncCache<int, Challenge> Challenges = new AsyncCache<int, Challenge>(i => Challenge.Static.GetAsync(session, i));

        /// <summary>Gets all available <see cref="ChallengeSubject"/>s.</summary>
        private static readonly AsyncCache<int, ChallengeSubject> ChallengeSubjects = new AsyncCache<int, ChallengeSubject>(i => ChallengeSubject.Static.GetAsync(session, i));

        /// <summary>Gets all available <see cref="ChallengeType"/>s.</summary>
        private static readonly AsyncCache<int, ChallengeType> ChallengeTypes = new AsyncCache<int, ChallengeType>(i => ChallengeType.Static.GetAsync(session, i));

        /// <summary>Gets all available <see cref="Difficulty"/>s.</summary>
        private static readonly AsyncCache<int, Difficulty> Difficulties = new AsyncCache<int, Difficulty>(i => Difficulty.Static.GetAsync(session, i));

        /// <summary>Gets all available <see cref="Game"/>s.</summary>
        private static readonly AsyncCache<int, Game> Games = new AsyncCache<int, Game>(i => Game.Static.GetAsync(session, i));

        /// <summary>Gets all available <see cref="Question"/>s.</summary>
        private static readonly AsyncCache<int, Question> Questions = new AsyncCache<int, Question>(i => Question.Static.GetAsync(session, i));

        /// <summary>Gets all available <see cref="Team"/>s.</summary>
        private static readonly AsyncCache<int, Team> Teams = new AsyncCache<int, Team>(i => Team.Static.GetAsync(session, i));

        /// <summary>Gets all available <see cref="Zone"/>s.</summary>
        private static readonly AsyncCache<int, Zone> Zones = new AsyncCache<int, Zone>(i => Zone.Static.GetAsync(session, i));
        
        private static readonly AsyncCache<Tuple<int, int>, TeamChallenge> TeamChallenges = new AsyncCache<Tuple<int, int>, TeamChallenge>(i => TeamChallenge.EnsureTeamChallengeAsync(i.Item1, i.Item2, session));
        
        /// <summary>The session.</summary>
        private static UserSession session;

        /// <summary>Gets the default system language.</summary>
        public static CultureInfo BaseLanguage { get; } = new CultureInfo("en-US");
        
        /// <summary>Gets or sets a value indicating whether this <see cref="GameCache"/> is initiated.</summary>
        private static bool IsInitiated { get; set; }

        /// <summary>Clears all cache objects.</summary>
        public static void ClearAllCache()
        {
        }

        /// <summary>Gets the specified <see cref="Alternative"/>.</summary>
        /// <param name="id">The id of the <see cref="Alternative"/> to get.</param>
        /// <returns>The specified <see cref="Alternative"/>.</returns>
        public static async Task<Alternative> AlternativeGetAsync(int id)
        {
            await InitCacheAsync();
            return await Alternatives.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Challenge"/>.</summary>
        /// <param name="id">The id of the <see cref="Challenge"/> to get.</param>
        /// <returns>The specified <see cref="Challenge"/>.</returns>
        public static async Task<Challenge> ChallengeGetAsync(int id)
        {
            await InitCacheAsync();
            return await Challenges.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="ChallengeSubject"/>.</summary>
        /// <param name="id">The id of the <see cref="ChallengeSubject"/> to get.</param>
        /// <returns>The specified <see cref="ChallengeSubject"/>.</returns>
        public static async Task<ChallengeSubject> ChallengeSubjectGetAsync(int id)
        {
            await InitCacheAsync();
            return await ChallengeSubjects.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="ChallengeType"/>.</summary>
        /// <param name="id">The id of the <see cref="ChallengeType"/> to get.</param>
        /// <returns>The specified <see cref="ChallengeType"/>.</returns>
        public static async Task<ChallengeType> ChallengeTypeGetAsync(int id)
        {
            await InitCacheAsync();
            return await ChallengeTypes.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Difficulty"/>.</summary>
        /// <param name="id">The id of the <see cref="Difficulty"/> to get.</param>
        /// <returns>The specified <see cref="Difficulty"/>.</returns>
        public static async Task<Difficulty> DifficultyGetAsync(int id)
        {
            await InitCacheAsync();
            return await Difficulties.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Game"/>.</summary>
        /// <param name="id">The id of the <see cref="Game"/> to get.</param>
        /// <returns>The specified <see cref="Game"/>.</returns>
        public static async Task<Game> GameGetAsync(int id)
        {
            await InitCacheAsync();
            return await Games.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Question"/>.</summary>
        /// <param name="id">The id of the <see cref="Question"/> to get.</param>
        /// <returns>The specified <see cref="Question"/>.</returns>
        public static async Task<Question> QuestionGetAsync(int id)
        {
            await InitCacheAsync();
            return await Questions.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Team"/>.</summary>
        /// <param name="id">The id of the <see cref="Team"/> to get.</param>
        /// <returns>The specified <see cref="Team"/>.</returns>
        public static async Task<Team> TeamGetAsync(int id)
        {
            await InitCacheAsync();
            return await Teams.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="TeamChallenge"/>.</summary>
        /// <param name="teamId">The id of the <see cref="TeamChallenge.TeamId"/> to get.</param>
        /// <param name="challengeId">The id of the <see cref="TeamChallenge.ChallengeId"/> to get.</param>
        /// <returns>The specified <see cref="TeamChallenge"/>.</returns>
        public static async Task<TeamChallenge> TeamChallengeGetAsync(int teamId, int challengeId)
        {
            await InitCacheAsync();
            return await TeamChallenges.GetValue(new Tuple<int, int>(teamId, challengeId));
        }

        /// <summary>Gets the specified <see cref="Zone"/>.</summary>
        /// <param name="id">The id of the <see cref="Zone"/> to get.</param>
        /// <returns>The specified <see cref="Zone"/>.</returns>
        public static async Task<Zone> ZoneGetAsync(int id)
        {
            await InitCacheAsync();
            return await Zones.GetValue(id);
        }

        /// <summary>Gets all <see cref="ChallengeSubject"/>s.</summary>
        /// <returns>The specified <see cref="ChallengeSubject"/>.</returns>
        public static async Task<IEnumerable<ChallengeSubject>> ChallengeSubjectsGetAllAsync()
        {
            await InitCacheAsync();
            var challengeSubjects = new List<ChallengeSubject>();
            foreach (var pair in ChallengeSubjects)
            {
                challengeSubjects.Add(await pair.Value);
            }

            return challengeSubjects;
        }

        /// <summary>Gets all <see cref="ChallengeType"/>s.</summary>
        /// <returns>The specified <see cref="ChallengeType"/>.</returns>
        public static async Task<IEnumerable<ChallengeType>> ChallengeTypesGetAllAsync()
        {
            await InitCacheAsync();
            var challengeTypes = new List<ChallengeType>();
            foreach (var pair in ChallengeTypes)
            {
                challengeTypes.Add(await pair.Value);
            }

            return challengeTypes;
        }

        /// <summary>Gets all <see cref="Difficulty"/>s.</summary>
        /// <returns>The specified <see cref="Difficulty"/>.</returns>
        public static async Task<IEnumerable<Difficulty>> DifficultiesGetAllAsync()
        {
            await InitCacheAsync();
            var difficulties = new List<Difficulty>();
            foreach (var pair in Difficulties)
            {
                difficulties.Add(await pair.Value);
            }

            return difficulties;
        }

        /// <summary>Updates the cache with the new <see cref="Zone"/>.</summary>
        /// <param name="zone">New <see cref="Zone"/> to add to it's cached parent <see cref="Game"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Zone"/> is not valid to be saved.</exception>
        public static async Task ZoneAddedAsync(Zone zone)
        {
            await InitCacheAsync();
            if (zone == null)
            {
                throw new ArgumentNullException(nameof(zone));
            }

            if (zone.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The zone needs to be saved before cached.");
            }
            
            zone.ValidateSaveCandidate();
            var game = await Games.GetValue(zone.GameId);
            game.Zones.Add(zone);
        }

        /// <summary>Updates the cache with the new <see cref="Challenge"/>.</summary>
        /// <param name="challenge">New <see cref="Challenge"/> to add to it's cached parent <see cref="Zone"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Challenge"/> is not valid to be saved.</exception>
        public static async Task ChallengeAddedAsync(Challenge challenge)
        {
            await InitCacheAsync();
            if (challenge == null)
            {
                throw new ArgumentNullException(nameof(challenge));
            }

            if (challenge.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The challenge needs to be saved before cached.");
            }

            challenge.ValidateSaveCandidate();
            var zone = await Zones.GetValue(challenge.ZoneId);
            zone.Challenges.Add(challenge);
        }

        /// <summary>Updates the cache with the new <see cref="Question"/>.</summary>
        /// <param name="question">New <see cref="Question"/> to add to it's cached parent <see cref="Challenge"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Question"/> is not valid to be saved.</exception>
        public static async Task QuestionAddedAsync(Question question)
        {
            await InitCacheAsync();
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            if (question.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The question needs to be saved before cached.");
            }

            question.ValidateSaveCandidate();
            var challenge = await Challenges.GetValue(question.ChallengeId);
            challenge.Questions.Add(question);
        }

        /// <summary>Updates the cache with the new <see cref="Alternative"/>.</summary>
        /// <param name="alternative">New <see cref="Alternative"/> to add to it's cached parent <see cref="Question"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Alternative"/> is not valid to be saved.</exception>
        public static async Task AlternativeAddedAsync(Alternative alternative)
        {
            await InitCacheAsync();
            if (alternative == null)
            {
                throw new ArgumentNullException(nameof(alternative));
            }

            if (alternative.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The alternative needs to be saved before cached.");
            }

            alternative.ValidateSaveCandidate();
            var question = await Questions.GetValue(alternative.QuestionId);
            question.Alternatives.Add(alternative);
        }

        /// <summary>The get server ip async.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public static async Task<string> GetServerIpAsync()
        {
            // ReSharper disable ExceptionNotDocumented
            return (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList[0].ToString();
            // ReSharper restore ExceptionNotDocumented
        }

        /// <summary>Initializes a new instance of the <see cref="GameCache"/> class.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task InitCacheAsync()
        {
            if (session == null || session.ActiveTo < DateTime.Now)
            {
                // ReSharper disable once ExceptionNotDocumented
                session = await GetSessionAsync();
            }

            if (!IsInitiated)
            {
                IsInitiated = true;
                try
                {
                    await ChallengeSubjectsLoadAllAsync();
                    await ChallengeTypesLoadAllAsync();
                    await DifficultiesLoadAllAsync();
                }
                catch
                {
                    IsInitiated = false;
                    // ReSharper disable once ExceptionNotDocumented
                    throw;
                }
            }
        }

        /// <summary>Gets a local system session.</summary>
        /// <returns>The <see cref="UserSession"/>.</returns>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task<UserSession> GetSessionAsync()
        {
            return await UserSession.Static.CreateSessionAsync(
                new UserLogin(
                    Movies.Model.Properties.Settings.Default.SystemUserName,
                    Movies.Model.Properties.Settings.Default.SystemPassword,
                    await GetServerIpAsync()));
        }

        /// <summary>Loads all <see cref="ChallengeSubject"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task ChallengeSubjectsLoadAllAsync()
        {
            ChallengeSubjects.Clear();
            foreach (var challengeSubject in await ChallengeSubject.Static.GetAllAsync(session))
            {
                ChallengeSubjects.SetValue(challengeSubject.Id, challengeSubject);
            }
        }

        /// <summary>Loads all <see cref="ChallengeTypes"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task ChallengeTypesLoadAllAsync()
        {
            ChallengeTypes.Clear();
            foreach (var challengeType in await ChallengeType.Static.GetAllAsync(session))
            {
                ChallengeTypes.SetValue(challengeType.Id, challengeType);
            }
        }

        /// <summary>Loads all <see cref="Difficulties"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task DifficultiesLoadAllAsync()
        {
            Difficulties.Clear();
            foreach (var difficulty in await Difficulty.Static.GetAllAsync(session))
            {
                Difficulties.SetValue(difficulty.Id, difficulty);
            }
        }
    }
}
