//-----------------------------------------------------------------------
// <copyright file="Character.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public class Character : IReadOnlyCharacter
    {
        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name;

        /// <summary>Private part of the <see cref="ImdbId"/> property.</summary>
        private string imdbId = string.Empty;

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
        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="value"/> is <see langword="null" />.</exception>
        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }

                this.name = value;
            }
        }

        /// <summary>Gets or sets the id of the character in IMDB.</summary>
        public string ImdbId
        {
            get { return this.imdbId; }
            private set { this.imdbId = value ?? string.Empty; }
        }

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images
        {
            get { return this.images.AsReadOnly(); }
        }

        ////public static async Character Get(int idList)
        ////{
        ////    var characters = 
        ////}

        /// <summary>Gets the specified <see cref="Character"/>s.</summary>
        /// <param name="idList">The list of ids of the <see cref="Character"/>s to get.</param>
        /// <remarks>
        /// Uses stored procedure <c>CharactersGet</c>.
        /// Result 1 columns: CharacterId, Name
        /// </remarks>
        /// <returns>The list of <see cref="Character"/>s.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        /// <exception cref="InvalidOperationException">Cannot open a connection without specifying a data source or server.orThe connection is already open.</exception>
        /// <exception cref="ConfigurationErrorsException">There are two entries with the same name in the &lt;localdbinstances&gt; section.</exception>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The <c>system.data.localdb</c> tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        public static async Task<IEnumerable<Character>> Get(IEnumerable<int> idList)
        {
            var characters = new List<Character>();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharactersGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idList", Persistent.CreateIntCollectionTable(idList));
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        throw new MissingResultException(1, "Characters");
                    }

                    while (reader.Read())
                    {
                        characters.Add(new Character(reader));
                    }

                    if (!reader.NextResult())
                    {
                        throw new MissingResultException(2, "IconsInCharacters");
                    }

                    while (reader.Read())
                    {

                    }
                }
            }

            return characters;
        }

        /// <summary>Saves this character to the database.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Character"/> is not valid to be saved.</exception>
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Validates that the <paramref name="character"/> is valid to be saved.</summary>
        /// <param name="character">The character to validate.</param>
        /// <exception cref="InvalidSaveCandidateException">The <paramref name="character"/> is not valid to be saved.</exception>
        private static void ValidateSaveCandidate(Character character)
        {
            if (string.IsNullOrEmpty(character.Name))
            {
                throw new InvalidSaveCandidateException("The character's name can not be empty.");
            }
        }

        /// <summary>Updates a character from a record.</summary>
        /// <param name="character">The character to update.</param>
        /// <param name="record">The record containing the data for the character.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private static void ReadFromRecord(Character character, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "CharacterId", "Name", "ImdbId" });
            character.Id = (int)record["CharacterId"];
            character.Name = record["Name"].ToString();
            character.ImdbId = record["ImdbId"].ToString();
        }

        /// <summary>Saves a character to the database.</summary>
        /// <param name="character">The character to save.</param>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The <c>system.data.localdb</c> tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private static void SaveToDatabase(Character character)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharacterSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@characterId", character.Id);
                command.Parameters.AddWithValue("@name", character.Name);
                command.Parameters.AddWithValue("@imdbId", character.ImdbId);
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

        /// <summary></summary>
        /// <param name="characterToKeep"></param>
        /// <param name="characterIdToMerge"></param>
        private static void MergeCharacters(Character characterToKeep, int characterIdToMerge)
        {
            
        }
    }
}
