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
            var challengeSubjects = new List<Difficulty>();
            foreach (var pair in Difficulties)
            {
                challengeSubjects.Add(await pair.Value);
            }

            return challengeSubjects;
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
    }
}
