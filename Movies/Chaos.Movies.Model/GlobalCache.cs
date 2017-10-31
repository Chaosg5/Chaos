//-----------------------------------------------------------------------
// <copyright file="GlobalCache.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>Provides a global cache of objects.</summary>
    public static class GlobalCache
    {
        /// <summary>Private part of the <see cref="Characters"/> property.</summary>
        private static readonly List<Character> CharactersField = new List<Character>();

        /// <summary>Private part of the <see cref="Departments"/> property.</summary>
        private static readonly List<Department> DepartmentsField = new List<Department>();

        /// <summary>Private part of the <see cref="MovieSeriesTypes"/> property.</summary>
        private static readonly List<MovieSeriesType> MovieSeriesTypesField = new List<MovieSeriesType>();

        /// <summary>Private part of the <see cref="People"/> property.</summary>
        private static readonly List<Person> PeopleField = new List<Person>();

        /// <summary>Private part of the <see cref="Roles"/> property.</summary>
        private static readonly List<Role> RolesField = new List<Role>();

        /// <summary>Private part of the <see cref="DefaultLanguage"/> property.</summary>
        private static readonly CultureInfo DefaultLanguageField = new CultureInfo("en-US");

        /// <summary>Private part of the <see cref="User"/> property.</summary>
        private static User userField;

        ////public static AsyncCache<int, Character> Characters = new AsyncCache<int, Character>();

        /// <summary>Gets all available characters.</summary>
        public static ReadOnlyCollection<Character> Characters => CharactersField.AsReadOnly();

        /// <summary>Gets all available movie departments.</summary>
        public static ReadOnlyCollection<Department> Departments => DepartmentsField.AsReadOnly();

        /// <summary>Gets all available movie series types.</summary>
        public static ReadOnlyCollection<MovieSeriesType> MovieSeriesTypes => MovieSeriesTypesField.AsReadOnly();

        /// <summary>Gets all available people.</summary>
        public static ReadOnlyCollection<Person> People => PeopleField.AsReadOnly();

        /// <summary>Gets all available person roles.</summary>
        public static ReadOnlyCollection<Role> Roles => RolesField.AsReadOnly();

        /// <summary>Gets the id of the current user.</summary>
        /// <exception cref="InvalidOperationException" accessor="get">The user has not been initialized.</exception>
        public static User User
        {
            get
            {
                if (userField == null)
                {
                    // ToDo: Code Analysis remove this exception
                    throw new InvalidOperationException("The user has not been initialized.");
                }

                return userField;
            }
        }

        ////public static User SystemUser { get; private set; } = new User {Id = 1};

        /// <summary>Gets the default system language.</summary>
        public static CultureInfo DefaultLanguage => DefaultLanguageField;

        /// <summary>Initializes a new instance of the <see cref="GlobalCache"/> class.</summary>
        /// <param name="user">The current user.</param>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public static void InitCache(User user)
        {
            userField = user;
            MovieSeriesTypesLoad();
            RolesLoad();
            DepartmentsLoad();
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

        /// <summary>Gets the specified <see cref="Character"/>.</summary>
        /// <param name="id">The id of the <see cref="Character"/> to get.</param>
        /// <returns>The specified <see cref="Character"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="Character"/> with the specified id doesn't exist.</exception>
        public static Character GetCharacter(int id)
        {
            var character = CharactersField.Find(d => d.Id == id);
            if (character == null)
            {
                // ToDo:
                ////character = Character.GetAsync(new[] { id }).First();
                ////if (character == null)
                ////{
                ////    throw new ArgumentOutOfRangeException("id");
                ////}

                ////CharactersField.Add(character);
            }

            if (character == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return character;
        }

        /// <summary>Gets the specified <see cref="Department"/>.</summary>
        /// <param name="id">The id of the <see cref="Department"/> to get.</param>
        /// <returns>The specified <see cref="Department"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="Department"/> with the specified id doesn't exist.</exception>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public static Department GetDepartment(int id)
        {
            var department = DepartmentsField.Find(d => d.Id == id);
            if (department == null)
            {
                department = Department.Get(new[] { id }).First();
                if (department == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                DepartmentsField.Add(department);
            }

            if (department == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return department;
        }

        /// <summary>Gets the specified <see cref="Department"/>.</summary>
        /// <param name="departmentTitle">The title of the <see cref="Department"/> to find.</param>
        /// <param name="language">The language of the <see cref="departmentTitle"/>.</param>
        /// <returns>The specified <see cref="Department"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="Department"/> with the specified <paramref name="departmentTitle"/> does not exists.</exception>
        public static Department GetDepartment(string departmentTitle, CultureInfo language)
        {
            var department = DepartmentsField.Find(d => d.Titles.GetTitle(language).Title == departmentTitle);
            if (department == null)
            {
                throw new ArgumentOutOfRangeException(nameof(departmentTitle));
            }

            return department;
        }

        /// <summary>Gets the specified <see cref="MovieSeriesType"/>.</summary>
        /// <param name="id">The id of the <see cref="MovieSeriesType"/> to get.</param>
        /// <returns>The specified <see cref="MovieSeriesType"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="MovieSeriesType"/> with the specified id doesn't exist.</exception>
        public static MovieSeriesType GetMovieSeriesType(int id)
        {
            var type = MovieSeriesTypesField.Find(t => t.Id == id);
            if (type == null)
            {
                type = MovieSeriesType.Get(new[] { id }).First();
                if (type == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                MovieSeriesTypesField.Add(type);
            }

            if (type == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return type;
        }

        /// <summary>Gets the specified <see cref="Person"/>.</summary>
        /// <param name="id">The id of the <see cref="Person"/> to get.</param>
        /// <returns>The specified <see cref="Person"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="Person"/> with the specified id doesn't exist.</exception>
        public static Person GetPerson(int id)
        {
            var person = PeopleField.Find(d => d.Id == id);
            if (person == null)
            {
                person = Person.Get(new[] { id }).First();
                if (person == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                PeopleField.Add(person);
            }

            if (person == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return person;
        }

        /// <summary>Gets the specified <see cref="Role"/>.</summary>
        /// <param name="id">The id of the <see cref="Role"/> to get.</param>
        /// <returns>The specified <see cref="Role"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="Role"/> with the specified id doesn't exist.</exception>
        public static Role GetRole(int id)
        {
            var role = RolesField.Find(r => r.Id == id);
            if (role == null)
            {
                role = Role.Get(new[] { id }).First();
                if (role == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                RolesField.Add(role);
            }

            if (role == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return role;
        }

        /// <summary>Gets the specified <see cref="Role"/> by title and <see cref="Department"/> title.</summary>
        /// <param name="roleTitle">The title of the <see cref="Role"/> to get.</param>
        /// <param name="departmentTitle">The title of the <see cref="Department"/> that the role belongs to.</param>
        /// <param name="language">The language of the titles.</param>
        /// <returns>The found role.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the role or department wasn't found.</exception>
        public static Role GetRole(string roleTitle, string departmentTitle, CultureInfo language)
        {
            var role = GetDepartment(departmentTitle, language).Roles.First(r => r.Titles.GetTitle(language).Title == roleTitle);
            if (role == null)
            {
                throw new ArgumentOutOfRangeException(nameof(roleTitle));
            }

            return role;
        }

        /// <summary>Loads the <see cref="Character"/>s with the specified ids from the database.</summary>
        /// <param name="idList">The list of ids of the <see cref="Character"/>s to load.</param>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        private static void CharactersLoad(IEnumerable<int> idList)
        {
            CharactersField.RemoveAll(c => idList.Contains(c.Id));
            // ToDO:
            ////CharactersField.AddRange(Character.GetAsync(idList));
        }

        /// <summary>Loads all <see cref="Department"/>s from the database.</summary>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        private static void DepartmentsLoad()
        {
            DepartmentsField.Clear();
            DepartmentsField.AddRange(Department.GetAll());
        }

        /// <summary>Loads all <see cref="MovieSeriesType"/>s from the database.</summary>
        private static void MovieSeriesTypesLoad()
        {
            MovieSeriesTypesField.Clear();
            MovieSeriesTypesField.AddRange(MovieSeriesType.GetAll());
        }

        /// <summary>Loads the <see cref="Person"/>s with the specified ids from the database.</summary>
        /// <param name="idList">The list of ids of the <see cref="Person"/>s to load.</param>
        private static void PeopleLoad(IEnumerable<int> idList)
        {
            PeopleField.RemoveAll(p => idList.Contains(p.Id));
            PeopleField.AddRange(Person.Get(idList));
        }

        /// <summary>Loads all <see cref="Role"/>s from the database.</summary>
        private static void RolesLoad()
        {
            RolesField.Clear();
            RolesField.AddRange(Role.GetAll());
        }
    }
}
