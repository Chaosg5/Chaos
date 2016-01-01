//-----------------------------------------------------------------------
// <copyright file="Person.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
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
        /// <param name="record">The record containing the data for the person.</param>
        public Person(IDataRecord record)
        {
            ReadFromRecord(this, record);
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
                command.Parameters.Add(new SqlParameter("@userId", GlobalCache.User));
                command.Parameters.Add(new SqlParameter("@idList", idList));
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        throw new MissingResultException(1);
                    }

                    while (reader.Read())
                    {
                        people.Add(new Person(reader));
                    }

                    if (!reader.NextResult() || !reader.HasRows)
                    {
                        throw new MissingResultException(2);
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
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Validates that the <paramref name="person"/> is valid to be saved.</summary>
        /// <param name="person">The person to validate.</param>
        private static void ValidateSaveCandidate(Person person)
        {
            if (string.IsNullOrEmpty(person.Name))
            {
                throw new InvalidSaveCandidateException("The 'Name' can not be empty.");
            }
        }

        /// <summary>Updates a person from a record.</summary>
        /// <param name="person">The person to update.</param>
        /// <param name="record">The record containing the data for the person.</param>
        private static void ReadFromRecord(Person person, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "PersonId", "Name" });
            person.Id = (int)record["PersonId"];
            person.Name = record["Name"].ToString();
        }

        /// <summary>Saves a person to the database.</summary>
        /// <param name="person">The person to save.</param>
        private static void SaveToDatabase(Person person)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("PersonSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@personId", person.Id));
                command.Parameters.Add(new SqlParameter("@name", person.Name));
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(person, reader);
                    }
                }
            }
        }
    }
}
