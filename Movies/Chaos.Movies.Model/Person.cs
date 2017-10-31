//-----------------------------------------------------------------------
// <copyright file="Person.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a person.</summary>
    public class Person
    {
        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<PersonUserRating> personUserRatings = new List<PersonUserRating>();

        /// <summary>Initializes a new instance of the <see cref="Person" /> class.</summary>
        public Person()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Person" /> class.</summary>
        /// <param name="name">The name of the person.</param>
        public Person(string name)
        {
            this.Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="Person" /> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="Person"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public Person(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the id of the person.</summary>
        public int Id { get; private set; }

        /// <summary>Gets or sets name of the person.</summary>
        public string Name { get; set; }

        /// <summary>Gets the list of images for the <see cref="Person"/> and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images
        {
            get { return this.images.AsReadOnly(); }
        }

        /// <summary>Gets the list ratings of this <see cref="Person"/> for the current <see cref="User"/>.</summary>
        public ReadOnlyCollection<PersonUserRating> PersonUserRatings
        {
            get { return this.personUserRatings.AsReadOnly(); }
        }

        /// <summary>Gets the specified <see cref="Person"/>s.</summary>
        /// <param name="idList">The list of ids of the <see cref="Person"/>s to get.</param>
        /// <remarks>
        /// Uses stored procedure <c>PeopleGet</c>.
        /// Result 1 columns: PersonId, Favorite
        /// Result 2 columns: PersonId, MovieId, Rating, Watches
        /// </remarks>
        /// <returns>The list of <see cref="Person"/>s.</returns>
        public static IEnumerable<Person> Get(IEnumerable<int> idList)
        {
            var people = new List<Person>();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("PeopleGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", GlobalCache.User);
                command.Parameters.AddWithValue("@idList", idList);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        throw new MissingResultException(1, "");
                    }

                    while (reader.Read())
                    {
                        people.Add(new Person(reader));
                    }

                    if (!reader.NextResult() || !reader.HasRows)
                    {
                        throw new MissingResultException(2, "");
                    }

                    while (reader.Read())
                    {
                        var personId = (int)reader["PersonId"];
                        var person = people.Find(p => p.Id == personId);
                        if (person == null)
                        {
                            throw new SqlResultSyncException(personId);
                        }

                        person.personUserRatings.Add(new PersonUserRating(reader));
                    }
                }
            }

            return people;
        }

        /// <summary>Saves this person to the database.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Person"/> is not valid to be saved.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public void Save()
        {
            this.ValidateSaveCandidate();
            this.SaveToDatabase();
        }

        /// <summary>Validates that this <see cref="Person"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Person"/> is not valid to be saved.</exception>
        private void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException($"The {nameof(this.Name)} can not be empty.");
            }
        }

        /// <summary>Updates this <see cref="Person"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Person"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "PersonId", "Name" });
            this.Id = (int)record["PersonId"];
            this.Name = record["Name"].ToString();
        }

        /// <summary>Saves this <see cref="Person"/> to the database.</summary>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private void SaveToDatabase()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("PersonSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@personId", this.Id);
                command.Parameters.AddWithValue("@name", this.Name);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.ReadFromRecord(reader);
                    }
                }
            }
        }
    }
}
