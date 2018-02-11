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
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a person.</summary>
    public class Person : Readable<Person, PersonDto>, IReadable<Person, PersonDto>
    {
        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<PersonUserRating<Person>> personUserRatings = new List<PersonUserRating<Person>>();

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

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Person Static { get; } = new Person();

        /// <summary>Gets the id of the person.</summary>
        public int Id { get; private set; }

        /// <summary>Gets or sets name of the person.</summary>
        public string Name { get; set; }

        /// <summary>Gets the list of images for the <see cref="Person"/> and their order.</summary>
        public IconCollection Images { get; } = new IconCollection();

        /// <summary>Gets the list ratings of this <see cref="Person"/> for the current <see cref="User"/>.</summary>
        public ReadOnlyCollection<PersonUserRating<Person>> PersonUserRatings
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
                       // people.Add(this.ReadFromRecordAsync(reader));
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

                        person.personUserRatings.Add(new PersonUserRating<Person>(reader));
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

        /// <inheritdoc />
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.MissingColumnException">A required column is missing in the <paramref name="record" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="record" /> is <see langword="null" />.</exception>
        public override Task<Person> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "PersonId", "Name" });
            return Task.FromResult(new Person
            {
                Id = (int)record["PersonId"],
                Name = record["Name"].ToString()
            });
        }

        /// <summary>Validates that this <see cref="Person"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Person"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException($"The {nameof(this.Name)} can not be empty.");
            }
        }

        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new NotImplementedException();
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
                       /// await this.ReadFromRecordAsync(reader);
                    }
                }
            }
        }

        protected override Task<IEnumerable<Person>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new NotImplementedException();
        }

        public PersonDto ToContract()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetAsync(UserSession session, int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new NotImplementedException();
        }

        public Task SaveAllAsync(UserSession session)
        {
            throw new NotImplementedException();
        }
    }
}
