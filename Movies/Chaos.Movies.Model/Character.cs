//-----------------------------------------------------------------------
// <copyright file="Character.cs">
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

    /// <summary>Represents a character in a movie.</summary>
    public class Character
    {
        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="name">The name of the character.</param>
        public Character(string name)
        {
            this.Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        public Character(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of the character.</summary>
        public int Id { get; private set; }

        /// <summary>Gets or sets name of the character.</summary>
        public string Name { get; set; }
        
        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images
        {
            get { return this.images.AsReadOnly(); }
        }

        public static IEnumerable<Character> Get(IEnumerable<int> idList)
        {
            var characters = new List<Character>();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharactersGet", connection))
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
                        characters.Add(new Character(reader));
                    }

                    // ToDo: Get other stuff
                }
            }

            return characters;
        }

        /// <summary>Saves this character to the database.</summary>
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Validates that the <paramref name="character"/> is valid to be saved.</summary>
        /// <param name="character">The character to validate.</param>
        private static void ValidateSaveCandidate(Character character)
        {
            if (string.IsNullOrEmpty(character.Name))
            {
                throw new InvalidSaveCandidateException("The 'Name' can not be empty.");
            }
        }

        /// <summary>Updates a character from a record.</summary>
        /// <param name="character">The character to update.</param>
        /// <param name="record">The record containing the data for the character.</param>
        private static void ReadFromRecord(Character character, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "CharacterId", "Name" });
            character.Id = (int)record["CharacterId"];
            character.Name = record["Name"].ToString();
        }

        /// <summary>Saves a character to the database.</summary>
        /// <param name="character">The character to save.</param>
        private static void SaveToDatabase(Character character)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharacterSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@characterId", character.Id));
                command.Parameters.Add(new SqlParameter("@name", character.Name));
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(character, reader);
                    }
                }
            }
        }
    }
}
