//-----------------------------------------------------------------------
// <copyright file="PeopleInMovie.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>Represents all <see cref="Person"/>s in a movie.</summary>
    public class PeopleInMovie : IReadOnlyCollection<PersonInMovie>
    {
        /// <summary>The id of the <see cref="Movie"/> that the <see cref="Character"/> belongs to.</summary>
        private readonly int movieId;

        /// <summary>Private part of the <see cref="People"/> property.</summary>
        private readonly List<PersonInMovie> people = new List<PersonInMovie>();
        
        public PeopleInMovie(int movieId)
        {
            this.movieId = movieId;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this.people.Count;

        public IEnumerator<PersonInMovie> GetEnumerator()
        {
            return this.people.GetEnumerator();
        }

        public void AddPerson(PersonInMovie personInMovie)
        {
            if (this.people.Exists(p => p.Person.Id == personInMovie.Person.Id && p.Department.Id == personInMovie.Department.Id && p.Role.Id == personInMovie.Role.Id))
            {
                return;
            }

            this.people.Add(personInMovie);
        }

        public void AddPersonAndSave(PersonInMovie personInMovie)
        {
            this.AddPerson(personInMovie);

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("PersonInMovieAdd", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieId", this.movieId));
                command.Parameters.Add(new SqlParameter("@personId", personInMovie.Person.Id));
                command.Parameters.Add(new SqlParameter("@departmentId", personInMovie.Department.Id));
                command.Parameters.Add(new SqlParameter("@roleId", personInMovie.Role.Id));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
