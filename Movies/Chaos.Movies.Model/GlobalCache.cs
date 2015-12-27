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
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Provides a global cache of objects.</summary>
    public static class GlobalCache
    {
        /// <summary>Private part of the <see cref="Departments"/> property.</summary>
        private static readonly List<Department> DepartmentsField = new List<Department>();

        /// <summary>Private part of the <see cref="MovieSeriesTypes"/> property.</summary>
        private static readonly List<MovieSeriesType> MovieSeriesTypesField = new List<MovieSeriesType>();

        /// <summary>Private part of the <see cref="Roles"/> property.</summary>
        private static readonly List<Role> RolesField = new List<Role>();

        /// <summary>Private part of the <see cref="User"/> property.</summary>
        private static User UserField;
        
        /// <summary>Initializes a new instance of the <see cref="GlobalCache"/> class.</summary>
        /// <param name="user">The current user.</param>
        public static void InitCache(User user)
        {
            UserField = user;
            MovieSeriesTypesLoad();
            RolesLoad();
            DepartmentsLoad();
        }

        /// <summary>Gets all available movie departments.</summary>
        public static ReadOnlyCollection<Department> Departments => DepartmentsField.AsReadOnly();

        /// <summary>Gets all available movie series types.</summary>
        public static ReadOnlyCollection<MovieSeriesType> MovieSeriesTypes => MovieSeriesTypesField.AsReadOnly();
        
        /// <summary>Gets all available person roles.</summary>
        public static ReadOnlyCollection<Role> Roles => RolesField.AsReadOnly();

        /// <summary>Gets the id of the current user.</summary>
        public static User User
        {
            get
            {
                if (UserField == null)
                {
                    throw new CacheInitializationException("The user has not been initialized.");
                }

                return UserField;
            }
        }

        /// <summary>Adds the specified <paramref name="type"/> to the current list of movie series types.</summary>
        /// <param name="type">The movie series type to add.</param>
        public static void AddMovieSeriesType(MovieSeriesType type)
        {
           MovieSeriesTypesField.RemoveAll(t => t.Id == type.Id);
           MovieSeriesTypesField.Add(type);
        }

        /// <summary>Gets the specified department.</summary>
        /// <param name="id">The id of the department to get.</param>
        /// <returns>The specified department.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the department with the specified id doesn't exist.</exception>
        public static Department GetDepartment(int id)
        {
            var department = DepartmentsField.Find(d => d.Id == id);
            if (department == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return department;
        }

        /// <summary>Gets the specified movie series type.</summary>
        /// <param name="id">The id of the movie series type to get.</param>
        /// <returns>The specified movie series type.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the movie series type with the specified id doesn't exist.</exception>
        public static MovieSeriesType GetMovieSeriesType(int id)
        {
            var type = MovieSeriesTypesField.Find(t => t.Id == id);
            if (type == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return type;
        }

        /// <summary>Gets the specified role.</summary>
        /// <param name="id">The id of the role to get.</param>
        /// <returns>The specified role.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the role with the specified id doesn't exist.</exception>
        public static Role GetRole(int id)
        {
            var role = RolesField.Find(d => d.Id == id);
            if (role == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return role;
        }

        private static void DepartmentsLoad()
        {
            DepartmentsField.Clear();
            DepartmentsField.AddRange(Department.GetAll());
        }

        /// <summary>Loads all movie series types from the database.</summary>
        private static void MovieSeriesTypesLoad()
        {
            MovieSeriesTypesField.Clear();
            MovieSeriesTypesField.AddRange(MovieSeriesType.GetAll());
        }

        private static void RolesLoad()
        {
            RolesField.Clear();
            RolesField.AddRange(Role.GetAll());
        }
    }
}
