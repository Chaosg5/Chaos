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
    using System.IO;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents <see cref="Person"/>s in a movie.</summary>
    public class PeopleInMovieCollection : IReadOnlyCollection<PersonInMovie>
    {
        /// <summary>The list of <see cref="Person"/>s in this <see cref="PeopleInMovieCollection"/>.</summary>
        private readonly List<PersonInMovie> people = new List<PersonInMovie>();

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

        /// <summary>Gets id of the <see cref="Movie"/> which this <see cref="PeopleInMovieCollection"/> belongs to.</summary>
        public int MovieId { get; private set; }

        /// <summary>Gets the number of elements contained in this <see cref="PeopleInMovieCollection"/>.</summary>
        public int Count
        {
            get { return this.people.Count; }
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="PeopleInMovieCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="PeopleInMovieCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<PersonInMovie> GetEnumerator()
        {
            return this.people.GetEnumerator();
        }

        /// <summary>Sets the id of the <see cref="Movie"/> this <see cref="PeopleInMovieCollection"/> belongs to.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="PeopleInMovieCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The id of the <see cref="Movie"/> can't be changed once set.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="movieId"/> is not valid.</exception>
        public void SetMovieId(int movieId)
        {
            if (movieId <= 0)
            {
                throw new ArgumentOutOfRangeException("movieId");
            }

            if (this.MovieId != 0)
            {
                throw new ValueLogicalReadOnlyException("The id of the movie can't be changed once set.");
            }

            this.MovieId = movieId;
        }

        /// <summary>Removes specified <paramref name="personInMovie"/> item to the list.</summary>
        /// <param name="personInMovie">The item to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="personInMovie"/> is <see langword="null" />.</exception>
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
        /// <exception cref="InvalidCastException">A value does not match its respective column type. </exception>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The &lt;system.data.localdb&gt; tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
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
                command.Parameters.AddWithValue("@movieId", this.MovieId);
                command.Parameters.AddWithValue("@personId", personInMovie.Person.Id);
                command.Parameters.AddWithValue("@departmentId", personInMovie.Department.Id);
                command.Parameters.AddWithValue("@roleId", personInMovie.Role.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Removes specified <paramref name="personInMovie"/> item from the list.</summary>
        /// <param name="personInMovie">The item to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="personInMovie"/> is <see langword="null" />.</exception>
        /// <returns><see langword="true"/> if the <paramref name="personInMovie"/> was found and removed; <see langword="false"/> if it was not found.</returns>
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
        /// <exception cref="InvalidCastException">A value does not match its respective column type. </exception>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The &lt;system.data.localdb&gt; tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
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
                command.Parameters.AddWithValue("@movieId", this.MovieId);
                command.Parameters.AddWithValue("@personId", personInMovie.Person.Id);
                command.Parameters.AddWithValue("@departmentId", personInMovie.Department.Id);
                command.Parameters.AddWithValue("@roleId", personInMovie.Role.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Saves all <see cref="PersonInMovie"/> to the database.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        /// <exception cref="InvalidCastException">A value does not match its respective column type. </exception>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The &lt;system.data.localdb&gt; tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
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
                    command.Parameters.AddWithValue("@movieId", this.MovieId);
                    command.Parameters.AddWithValue("@people", table);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Loads <see cref="Person"/>s for the current movie.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        /// <exception cref="ArgumentNullException">If any parameter is null.</exception>
        /// <exception cref="RelationshipException">If a <see cref="Role"/> isn't a part of it's <see cref="Department"/>.</exception>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The &lt;system.data.localdb&gt; tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public void LoadPeople()
        {
            this.ValidateMovieId();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("PeopleInMovieGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@movieId", this.MovieId);
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
                // ToDo:
                ////this.people.AddRange(
                ////    loadData.Select(
                ////        p =>
                ////            new PersonInMovie(
                ////                GlobalCache.GetPerson(p.PersonId),
                ////                GlobalCache.GetDepartment(p.DepartmentId),
                ////                GlobalCache.GetRole(p.RoleId))));
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
            /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
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
            /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
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
