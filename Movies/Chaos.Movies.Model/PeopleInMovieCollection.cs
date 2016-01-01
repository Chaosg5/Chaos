//-----------------------------------------------------------------------
// <copyright file="PeopleInMovieCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents <see cref="Person"/>s in a movie.</summary>
    public class PeopleInMovieCollection : IReadOnlyCollection<PersonInMovie>
    {
        /// <summary>The list of <see cref="Person"/>s in this <see cref="PeopleInMovieCollection"/>.</summary>
        private readonly List<PersonInMovie> people = new List<PersonInMovie>();

        /// <summary>Private part of the <see cref="MovieId"/> property.</summary>
        private int movieId;

        /// <summary>Initializes a new instance of the <see cref="PeopleInMovieCollection"/> class.</summary>
        public PeopleInMovieCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PeopleInMovieCollection"/> class.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="PeopleInMovieCollection"/> belongs to.</param>
        public PeopleInMovieCollection(int movieId)
        {
            this.MovieId = movieId;
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="PeopleInMovieCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Gets id of the <see cref="Movie"/> which this <see cref="PeopleInMovieCollection"/> belongs to.</summary>
        public int MovieId
        {
            get
            {
                return this.movieId;
            }

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                if (this.movieId != 0)
                {
                    throw new ValueLogicalReadOnlyException("The id of the movie can't be changed once set.");
                }
                
                this.movieId = value;
            }
        }

        /// <summary>Gets the number of elements contained in this <see cref="PeopleInMovieCollection"/>.</summary>
        public int Count
        {
            get { return this.people.Count; }
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="PeopleInMovieCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<PersonInMovie> GetEnumerator()
        {
            return this.people.GetEnumerator();
        }

        /// <summary>Sets the id of the <see cref="Movie"/> this <see cref="PeopleInMovieCollection"/> belongs to.</summary>
        /// <param name="id">The id of the <see cref="Movie"/> which this <see cref="PeopleInMovieCollection"/> belongs to.</param>
        public void SetMovieId(int id)
        {
            this.MovieId = id;
        }

        /// <summary>Removes specified <paramref name="personInMovie"/> item to the list.</summary>
        /// <param name="personInMovie">The item to add.</param>
        public void AddPerson(PersonInMovie personInMovie)
        {
            if (personInMovie == null)
            {
                throw new ArgumentNullException("personInMovie");
            }

            if (this.people.Exists(p => p.Person.Id == personInMovie.Person.Id && p.Department.Id == personInMovie.Department.Id && p.Role.Id == personInMovie.Role.Id))
            {
                return;
            }

            this.people.Add(personInMovie);
        }

        /// <summary>Removes specified <paramref name="personInMovie"/> item from the list and saves the change to the database.</summary>
        /// <param name="personInMovie">The item to remove.</param>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void AddPersonAndSave(PersonInMovie personInMovie)
        {
            this.AddPerson(personInMovie);
            if (this.MovieId <= 0)
            {
                throw new PersistentObjectRequiredException("The movie needs to be saved before adding people.");
            }

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("PersonInMovieAdd", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieId", this.MovieId));
                command.Parameters.Add(new SqlParameter("@personId", personInMovie.Person.Id));
                command.Parameters.Add(new SqlParameter("@departmentId", personInMovie.Department.Id));
                command.Parameters.Add(new SqlParameter("@roleId", personInMovie.Role.Id));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Removes specified <paramref name="personInMovie"/> item from the list.</summary>
        /// <param name="personInMovie">The item to remove.</param>
        public bool RemovePerson(PersonInMovie personInMovie)
        {
            if (personInMovie == null)
            {
                throw new ArgumentNullException("personInMovie");
            }

            if (!this.people.Contains(personInMovie))
            {
                return false;
            }

            this.people.Remove(personInMovie);
            return true;
        }

        /// <summary>Removes specified <paramref name="personInMovie"/> item from the list and saves the change to the database.</summary>
        /// <param name="personInMovie">The item to remove.</param>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void RemovePersonAndSave(PersonInMovie personInMovie)
        {
            if (!this.RemovePerson(personInMovie))
            {
                return;
            }

            this.ValidateMovieId();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("PersonInMovieRemove", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieId", this.MovieId));
                command.Parameters.Add(new SqlParameter("@personId", personInMovie.Person.Id));
                command.Parameters.Add(new SqlParameter("@departmentId", personInMovie.Department.Id));
                command.Parameters.Add(new SqlParameter("@roleId", personInMovie.Role.Id));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Saves all <see cref="PersonInMovie"/> to the database.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void Save()
        {
            this.ValidateMovieId();
            using (var table = new DataTable())
            {
                table.Locale = CultureInfo.InvariantCulture;
                table.Columns.Add(new DataColumn("PersonId"));
                table.Columns.Add(new DataColumn("DepartmentId"));
                table.Columns.Add(new DataColumn("RoleId"));
                foreach (var personInMovie in this.people)
                {
                    table.Rows.Add(personInMovie.Person.Id, personInMovie.Department.Id, personInMovie.Role.Id);
                }

                using (var connection = new SqlConnection(Persistent.ConnectionString))
                using (var command = new SqlCommand("PeopleInMovieSave", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@movieId", this.MovieId));
                    command.Parameters.Add(new SqlParameter("@people", table));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Loads <see cref="Person"/>s for the current movie.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void LoadPeople()
        {
            this.ValidateMovieId();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("PeopleInMovieGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieId", this.movieId));
                connection.Open();

                var loadData = new List<PersonLoadShell>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loadData.Add(new PersonLoadShell(reader));
                    }
                }

                this.people.Clear();
                this.people.AddRange(
                    loadData.Select(
                        p =>
                            new PersonInMovie(
                                GlobalCache.GetPerson(p.PersonId),
                                GlobalCache.GetDepartment(p.DepartmentId),
                                GlobalCache.GetRole(p.RoleId))));
            }
        }

        /// <summary>Validates that the <see cref="MovieId"/> is set.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        private void ValidateMovieId()
        {
            if (this.MovieId <= 0)
            {
                throw new PersistentObjectRequiredException("The movie needs to be saved before adding characters.");
            }
        }

        /// <summary>Temporarily holds ids related to a <see cref="Person"/> in a <see cref="Movie"/> during loaded.</summary>
        private class PersonLoadShell
        {
            /// <summary>Initializes a new instance of the <see cref="PersonLoadShell" /> class.</summary>
            /// <param name="record">The record containing the data for the <see cref="PersonLoadShell"/>.</param>
            public PersonLoadShell(IDataRecord record)
            {
                ReadFromRecord(this, record);
            }

            /// <summary>Gets the id of the <see cref="Person"/>.</summary>
            public int PersonId { get; private set; }


            /// <summary>Gets the id of the <see cref="Person"/>'s <see cref="DepartmentId"/>.</summary>
            public int DepartmentId { get; private set; }


            /// <summary>Gets the id of the <see cref="Person"/>'s <see cref="RoleId"/>.</summary>
            public int RoleId { get; private set; }

            /// <summary>Updates a <see cref="PersonLoadShell"/> from a record.</summary>
            /// <param name="person">The <see cref="PersonLoadShell"/> to update.</param>
            /// <param name="record">The record containing the data for the <see cref="PersonLoadShell"/>.</param>
            private static void ReadFromRecord(PersonLoadShell person, IDataRecord record)
            {
                Persistent.ValidateRecord(record, new[] { "PersonId", "DepartmentId", "RoleId" });
                person.PersonId = (int)record["PersonId"];
                person.DepartmentId = (int)record["DepartmentId"];
                person.RoleId = (int)record["RoleId"];
            }
        }
    }
}
