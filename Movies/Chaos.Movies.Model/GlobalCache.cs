//-----------------------------------------------------------------------
// <copyright file="GlobalCache.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>Provides a global cache of objects.</summary>
    internal static class GlobalCache
    {
        /// <summary>Gets all available movies.</summary>
        private static readonly AsyncCache<int, Movie> Movies = new AsyncCache<int, Movie>(i => Movie.Static.GetAsync(session, i));

        /// <summary>Gets all available characters.</summary>
        private static readonly AsyncCache<int, Character> Characters = new AsyncCache<int, Character>(i => Character.Static.GetAsync(session, i));
        
        /// <summary>Gets all available movie departments.</summary>
        private static readonly AsyncCache<int, Department> Departments = new AsyncCache<int, Department>(i => Department.Static.GetAsync(session, i));

        /// <summary>Gets all available movie series types.</summary>
        private static readonly AsyncCache<int, MovieSeriesType> MovieSeriesTypes = new AsyncCache<int, MovieSeriesType>(i => MovieSeriesType.Static.GetAsync(session, i));

        /// <summary>Gets all available people.</summary>
        private static readonly AsyncCache<int, Person> People = new AsyncCache<int, Person>(i => Person.Static.GetAsync(session, i));

        /// <summary>Gets all available person roles.</summary>
        private static readonly AsyncCache<int, Role> Roles = new AsyncCache<int, Role>(i => Role.Static.GetAsync(session, i));

        /// <summary>Gets all available external sources.</summary>
        private static readonly AsyncCache<int, ExternalSource> ExternalSources = new AsyncCache<int, ExternalSource>(i => ExternalSource.Static.GetAsync(session, i));

        /// <summary>Gets all available icon types.</summary>
        private static readonly AsyncCache<int, IconType> IconTypes = new AsyncCache<int, IconType>(i => IconType.Static.GetAsync(session, i));
        
        ////public static User SystemUser { get; private set; } = new User {Id = 1};

        /// <summary>The session.</summary>
        private static UserSession session;

        /// <summary>Gets the default system language.</summary>
        public static CultureInfo DefaultLanguage { get; } = new CultureInfo("en-US");

        /// <summary>Initializes a new instance of the <see cref="GlobalCache"/> class.</summary>
        /// <param name="userSession">The session.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        public static async Task InitCacheAsync(UserSession userSession)
        {
            await MovieSeriesTypesLoadAllAsync();
            await RolesLoadAllAsync();
            await IconTypesLoadAllAsync();
            await DepartmentsLoadAllAsync();
        }

        /// <summary>Clears all cache objects.</summary>
        public static void ClearAllCache()
        {
        }

        // ToDo: Remove this
        /////// <summary>Adds the specified <paramref name="type"/> to the current list of movie series types.</summary>
        /////// <param name="type">The movie series type to add.</param>
        ////public static void AddMovieSeriesType(MovieSeriesType type)
        ////{
        ////    if (type == null)
        ////    {
        ////        throw new ArgumentNullException("type");
        ////    }

        ////    if (type.Id <= 0)
        ////    {
        ////        throw new PersistentObjectRequiredException("The movie series type needs to be saved before added to the cache.");
        ////    }

        ////    MovieSeriesTypesField.RemoveAll(t => t.Id == type.Id);
        ////    MovieSeriesTypesField.Add(type);
        ////}

        /// <summary>Gets the specified <see cref="Movie"/>.</summary>
        /// <param name="id">The id of the <see cref="Movie"/> to get.</param>
        /// <returns>The specified <see cref="Movie"/>.</returns>
        public static async Task<Movie> GetMovieAsync(int id)
        {
            return await Movies.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Character"/>.</summary>
        /// <param name="id">The id of the <see cref="Character"/> to get.</param>
        /// <returns>The specified <see cref="Character"/>.</returns>
        public static async Task<Character> GetCharacterAsync(int id)
        {
            return await Characters.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Department"/>.</summary>
        /// <param name="id">The id of the <see cref="Department"/> to get.</param>
        /// <returns>The specified <see cref="Department"/>.</returns>
        public static async Task<Department> GetDepartmentAsync(int id)
        {
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
            return await MovieSeriesTypes.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Person"/>.</summary>
        /// <param name="id">The id of the <see cref="Person"/> to get.</param>
        /// <returns>The specified <see cref="Person"/>.</returns>
        public static async Task<Person> GetPersonAsync(int id)
        {
            return await People.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="Role"/>.</summary>
        /// <param name="id">The id of the <see cref="Role"/> to get.</param>
        /// <returns>The specified <see cref="Role"/>.</returns>
        public static async Task<Role> GetRoleAsync(int id)
        {
            return await Roles.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="IconType"/>.</summary>
        /// <param name="id">The id of the <see cref="IconType"/> to get.</param>
        /// <returns>The specified <see cref="IconType"/>.</returns>
        public static async Task<IconType> GetIconTypeAsync(int id)
        {
            return await IconTypes.GetValue(id);
        }

        /// <summary>Gets the specified <see cref="ExternalSource"/>.</summary>
        /// <param name="id">The id of the <see cref="ExternalSource"/> to get.</param>
        /// <returns>The specified <see cref="ExternalSource"/>.</returns>
        public static async Task<ExternalSource> GetExternalSourceAsync(int id)
        {
            return await ExternalSources.GetValue(id);
        }

        /////// <summary>Gets the specified <see cref="Role"/> by title and <see cref="Department"/> title.</summary>
        /////// <param name="roleTitle">The title of the <see cref="Role"/> to get.</param>
        /////// <param name="departmentTitle">The title of the <see cref="Department"/> that the role belongs to.</param>
        /////// <param name="language">The language of the titles.</param>
        /////// <returns>The found role.</returns>
        /////// <exception cref="ArgumentOutOfRangeException">If the role or department wasn't found.</exception>
        ////public static Role GetRole(string roleTitle, string departmentTitle, CultureInfo language)
        ////{
        ////    var role = GetDepartment(departmentTitle, language).Roles.First(r => r.Titles.GetTitle(language).Title == roleTitle);
        ////    if (role == null)
        ////    {
        ////        throw new ArgumentOutOfRangeException(nameof(roleTitle));
        ////    }

        ////    return role;
        ////}

        /// <summary>Loads the <see cref="Character"/>s with the specified ids from the database.</summary>
        /// <param name="idList">The list of ids of the <see cref="Character"/>s to load.</param>
        private static void CharactersLoad(IEnumerable<int> idList)
        {
            foreach (var person in Person.Get(idList))
            {
                People.SetValue(person.Id, person);
            }
        }

        /// <summary>Loads the <see cref="Person"/>s with the specified ids from the database.</summary>
        /// <param name="idList">The list of ids of the <see cref="Person"/>s to load.</param>
        private static void PeopleLoad(IEnumerable<int> idList)
        {
            foreach (var person in Person.Get(idList))
            {
                People.SetValue(person.Id, person);
            }
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
        private static async Task MovieSeriesTypesLoadAllAsync()
        {
            MovieSeriesTypes.Clear();
            foreach (var movieSeriesType in await MovieSeriesType.Static.GetAllAsync(session))
            {
                MovieSeriesTypes.SetValue(movieSeriesType.Id, movieSeriesType);
            }
        }

        /// <summary>Loads all <see cref="Role"/>s from the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
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
        private static async Task IconTypesLoadAllAsync()
        {
            IconTypes.Clear();
            foreach (var iconType in await IconType.Static.GetAllAsync(session))
            {
                IconTypes.SetValue(iconType.Id, iconType);
            }
        }
    }
}
