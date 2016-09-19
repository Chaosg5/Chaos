//-----------------------------------------------------------------------
// <copyright file="PersonInMovie.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a person in a movie.</summary>
    public class PersonInMovie
    {
        /// <summary>Initializes a new instance of the <see cref="PersonInMovie"/> class.</summary>
        /// <param name="person">The person in the movie.</param>
        /// <param name="department">The department which the <paramref name="person"/> belongs to.</param>
        /// <param name="role">The role of the person in the <paramref name="department"/>.</param>
        /// <param name="userRating">The current user's rating.</param>
        /// <exception cref="ArgumentNullException">If any parameter is null.</exception>
        /// <exception cref="PersistentObjectRequiredException">If any of the objects aren't saved.</exception>
        /// <exception cref="RelationshipException">If the <paramref name="role"/> isn't a part of the <paramref name="department"/>.</exception>
        public PersonInMovie(Person person, Department department, Role role, int userRating)
        {
            if (person == null)
            {
                throw new ArgumentNullException("person");
            }

            if (person.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The department needs to be saved before being added to a person.");
            }

            if (department == null)
            {
                throw new ArgumentNullException("department");
            }

            if (department.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The department needs to be saved before being added to a person.");
            }

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            if (role.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The role needs to be saved before being added to a person.");
            }

            if (department.Roles.All(r => r.Id != role.Id))
            {
                throw new RelationshipException("The role needs to be a part of the specified department before being added to a person.");
            }

            this.Person = person;
            this.Department = department;
            this.Role = role;
            this.UserRating = userRating;
        }

        /// <summary>Gets the person.</summary>
        public Person Person { get; private set; }

        /// <summary>Gets the role of the <see cref="Person"/>.</summary>
        public Role Role { get; private set; }

        /// <summary>Gets the department of the <see cref="Person"/>.</summary>
        public Department Department { get; private set; }

        /// <summary>Gets the current user's rating of the <see cref="Character"/> in the <see cref="Movie"/>.</summary>
        public int UserRating { get; private set; }
    }
}
