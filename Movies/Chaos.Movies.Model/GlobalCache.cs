//-----------------------------------------------------------------------
// <copyright file="GlobalCache.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;

    /// <summary>Provides a global cache of objects.</summary>
    public static class GlobalCache
    {
        /// <summary>Gets all available movies.</summary>
        private static readonly AsyncCache<int, Movie> Movies = new AsyncCache<int, Movie>(i => Movie.Static.GetAsync(session, i));

        /// <summary>Gets all available characters.</summary>
        private static readonly AsyncCache<int, Character> Characters = new AsyncCache<int, Character>(i => Character.Static.GetAsync(session, i));

        /// <summary>Gets all available movie departments.</summary>
        private static readonly AsyncCache<int, Department> Departments = new AsyncCache<int, Department>(i => Department.Static.GetAsync(session, i));

        /// <summary>Gets all available movie series types.</summary>
        private static readonly AsyncCache<int, MovieSeriesType> MovieSeriesTypes = new AsyncCache<int, MovieSeriesType>(i => MovieSeriesType.Static.GetAsync(session, i));

        /// <summary>Gets all available movie series types.</summary>
        private static readonly AsyncCache<int, MovieType> MovieTypes = new AsyncCache<int, MovieType>(i => MovieType.Static.GetAsync(session, i));

        /// <summary>Gets all available people.</summary>
        private static readonly AsyncCache<int, Person> People = new AsyncCache<int, Person>(i => Person.Static.GetAsync(session, i));

        /// <summary>Gets all available person roles.</summary>
        private static readonly AsyncCache<int, Role> Roles = new AsyncCache<int, Role>(i => Role.Static.GetAsync(session, i));

        /// <summary>Gets all available external sources.</summary>
        private static readonly AsyncCache<int, ExternalSource> ExternalSources = new AsyncCache<int, ExternalSource>(i => ExternalSource.Static.GetAsync(session, i));

        /// <summary>Gets all available icon types.</summary>
        private static readonly AsyncCache<int, IconType> IconTypes = new AsyncCache<int, IconType>(i => IconType.Static.GetAsync(session, i));

        /// <summary>Gets all available icon types.</summary>
        private static readonly AsyncCache<int, RatingType> RatingTypes = new AsyncCache<int, RatingType>(i => RatingType.Static.GetAsync(session, i));

        /// <summary>Gets all available icon types.</summary>
        private static readonly AsyncCache<int, WatchType> WatchTypes = new AsyncCache<int, WatchType>(i => WatchType.Static.GetAsync(session, i));

        ////public static User SystemUser { get; private set; } = new User {Id = 1};

        /// <summary>The session.</summary>
        private static UserSession session;

        private static bool IsInitiated = true;

        /// <summary>Gets the default system language.</summary>
        public static CultureInfo DefaultLanguage { get; } = new CultureInfo("en-US");

        internal static async Task<string> GetServerIpAsync()
        {
            return (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList[0].ToString();
        }

        /// <summary>Clears all cache objects.</summary>
        public static void ClearAllCache()
        {
        }

        /// <summary>Gets the specified <see cref="Movie"/>.</summary>
        /// <param name="id">The id of the <see cref="Movie"/> to get.</param>
        /// <returns>The specified <see cref="Movie"/>.</returns>
        public static async Task<Movie> GetMovieAsync(int id)
        {
            await InitCacheAsync();
            return await Movies.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Character"/>.</summary>
        /// <param name="id">The id of the <see cref="Character"/> to get.</param>
        /// <returns>The specified <see cref="Character"/>.</returns>
        public static async Task<Character> GetCharacterAsync(int id)
        {
            await InitCacheAsync();
            return await Characters.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Department"/>.</summary>
        /// <param name="id">The id of the <see cref="Department"/> to get.</param>
        /// <returns>The specified <see cref="Department"/>.</returns>
        public static async Task<Department> GetDepartmentAsync(int id)
        {
            await InitCacheAsync();
            return await Departments.GetValue(id);
        }

        /////// <summary>Gets the specified <see cref="Department"/>.</summary>
        /////// <param name="departmentTitle">The title of the <see cref="Department"/> to find.</param>
        /////// <param name="language">The language of the <see cref="departmentTitle"/>.</param>
        /////// <returns>The specified <see cref="Department"/>.</returns>
        /////// <exception cref="ArgumentOutOfRangeException">If the <see cref="Department"/> with the specified <paramref name="departmentTitle"/> does not exists.</exception>
        ////public static Department GetDepartment(string departmentTitle, CultureInfo language)
        ////{
        ////    var department = Departments.Find(d => d.Titles.GetTitle(language).Title == departmentTitle);
        ////    if (department == null)
        ////    {
        ////        throw new ArgumentOutOfRangeException(nameof(departmentTitle));
        ////    }

        ////    return department;
        ////}

        /// <summary>Gets the specified <see cref="MovieSeriesType"/>.</summary>
        /// <param name="id">The id of the <see cref="MovieSeriesType"/> to get.</param>
        /// <returns>The specified <see cref="MovieSeriesType"/>.</returns>
        public static async Task<MovieSeriesType> GetMovieSeriesTypeAsync(int id)
        {
            await InitCacheAsync();
            return await MovieSeriesTypes.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="MovieType"/>.</summary>
        /// <param name="id">The id of the <see cref="MovieType"/> to get.</param>
        /// <returns>The specified <see cref="MovieType"/>.</returns>
        public static async Task<MovieType> GetMovieTypeAsync(int id)
        {
            await InitCacheAsync();
            return await MovieTypes.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Person"/>.</summary>
        /// <param name="id">The id of the <see cref="Person"/> to get.</param>
        /// <returns>The specified <see cref="Person"/>.</returns>
        public static async Task<Person> GetPersonAsync(int id)
        {
            await InitCacheAsync();
            return await People.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Role"/>.</summary>
        /// <param name="id">The id of the <see cref="Role"/> to get.</param>
        /// <returns>The specified <see cref="Role"/>.</returns>
        public static async Task<Role> GetRoleAsync(int id)
        {
            await InitCacheAsync();
            return await Roles.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="IconType"/>.</summary>
        /// <param name="id">The id of the <see cref="IconType"/> to get.</param>
        /// <returns>The specified <see cref="IconType"/>.</returns>
        public static async Task<IconType> GetIconTypeAsync(int id)
        {
            await InitCacheAsync();
            return await IconTypes.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="RatingType"/>.</summary>
        /// <param name="id">The id of the <see cref="RatingType"/> to get.</param>
        /// <returns>The specified <see cref="RatingType"/>.</returns>
        public static async Task<RatingType> GetRatingTypeAsync(int id)
        {
            await InitCacheAsync();
            return await RatingTypes.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="WatchType"/>.</summary>
        /// <param name="id">The id of the <see cref="WatchType"/> to get.</param>
        /// <returns>The specified <see cref="WatchType"/>.</returns>
        public static async Task<WatchType> GetWatchTypeAsync(int id)
        {
            await InitCacheAsync();
            return await WatchTypes.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="ExternalSource"/>.</summary>
        /// <param name="id">The id of the <see cref="ExternalSource"/> to get.</param>
        /// <returns>The specified <see cref="ExternalSource"/>.</returns>
        public static async Task<ExternalSource> GetExternalSourceAsync(int id)
        {
            await InitCacheAsync();
            return await ExternalSources.GetValue(id);
        }

        /// <summary>Initializes a new instance of the <see cref="GlobalCache"/> class.</summary>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task InitCacheAsync()
        {
            if (session == null || session.ActiveTo < DateTime.Now)
            {
                session = await GetSessionAsync();
            }

            if (!IsInitiated)
            {
                await MovieSeriesTypesLoadAllAsync();
                await DepartmentsLoadAllAsync();
                await RolesLoadAllAsync();
                await IconTypesLoadAllAsync();
                await RatingTypesLoadAllAsync();
                await WatchTypesLoadAllAsync();
            }
        }

        private static async Task<UserSession> GetSessionAsync()
        {
            return await UserSession.Static.CreateSessionAsync(
                new UserLogin(
                    Properties.Settings.Default.SystemUserName,
                    Properties.Settings.Default.SystemPassword,
                    await GetServerIpAsync()));
        }

        /// <summary>Loads all <see cref="Department"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task DepartmentsLoadAllAsync()
        {
            Departments.Clear();
            foreach (var department in await Department.Static.GetAllAsync(session))
            {
                Departments.SetValue(department.Id, department);
            }
        }

        /// <summary>Loads all <see cref="MovieSeriesType"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task MovieSeriesTypesLoadAllAsync()
        {
            MovieSeriesTypes.Clear();
            foreach (var movieSeriesType in await MovieSeriesType.Static.GetAllAsync(session))
            {
                MovieSeriesTypes.SetValue(movieSeriesType.Id, movieSeriesType);
            }
        }

        /// <summary>Loads all <see cref="MovieType"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task MovieTypesLoadAllAsync()
        {
            MovieTypes.Clear();
            foreach (var movieType in await MovieType.Static.GetAllAsync(session))
            {
                MovieTypes.SetValue(movieType.Id, movieType);
            }
        }

        /// <summary>Loads all <see cref="Role"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task RolesLoadAllAsync()
        {
            Roles.Clear();
            foreach (var role in await Role.Static.GetAllAsync(session))
            {
                Roles.SetValue(role.Id, role);
            }
        }

        /// <summary>Loads all <see cref="IconType"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task IconTypesLoadAllAsync()
        {
            IconTypes.Clear();
            foreach (var iconType in await IconType.Static.GetAllAsync(session))
            {
                IconTypes.SetValue(iconType.Id, iconType);
            }
        }

        /// <summary>Loads all <see cref="RatingType"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task RatingTypesLoadAllAsync()
        {
            RatingTypes.Clear();
            foreach (var ratingType in await RatingType.Static.GetAllAsync(session))
            {
                RatingTypes.SetValue(ratingType.Id, ratingType);
            }
        }

        /// <summary>Loads all <see cref="WatchType"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private static async Task WatchTypesLoadAllAsync()
        {
            WatchTypes.Clear();
            foreach (var ratingType in await WatchType.Static.GetAllAsync(session))
            {
                WatchTypes.SetValue(ratingType.Id, ratingType);
            }
        }
    }
}
