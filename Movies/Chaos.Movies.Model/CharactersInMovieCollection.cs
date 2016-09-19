//-----------------------------------------------------------------------
// <copyright file="CharactersInMovieCollection.cs">
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

    /// <summary>Represents <see cref="Character"/>s in a movie.</summary>
    public class CharactersInMovieCollection : IReadOnlyCollection<CharacterInMovie>
    {
        /// <summary>The list of <see cref="Character"/>s in this <see cref="CharactersInMovieCollection"/>.</summary>
        private readonly List<CharacterInMovie> characters = new List<CharacterInMovie>();

        /// <summary>Initializes a new instance of the <see cref="CharactersInMovieCollection"/> class.</summary>
        public CharactersInMovieCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CharactersInMovieCollection"/> class.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="CharactersInMovieCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The id of the <see cref="Movie"/> can't be changed once set.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="MovieId"/> is not valid.</exception>
        public CharactersInMovieCollection(int movieId)
        {
            this.SetMovieId(movieId);
        }

        /// <summary>Gets id of the <see cref="Movie"/> which this <see cref="CharactersInMovieCollection"/> belongs to.</summary>
        public int MovieId { get; private set; }

        /// <summary>Gets the number of elements contained in this <see cref="CharactersInMovieCollection"/>.</summary>
        public int Count
        {
            get { return this.characters.Count; }
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="CharactersInMovieCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="CharactersInMovieCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<CharacterInMovie> GetEnumerator()
        {
            return this.characters.GetEnumerator();
        }

        /// <summary>Sets the id of the <see cref="Movie"/> this <see cref="CharactersInMovieCollection"/> belongs to.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="CharactersInMovieCollection"/> belongs to.</param>
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

        /// <summary>Removes specified <paramref name="characterInMovie"/> item to the list.</summary>
        /// <param name="characterInMovie">The item to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="characterInMovie"/> is <see langword="null" />.</exception>
        public void AddCharacter(CharacterInMovie characterInMovie)
        {
            if (characterInMovie == null)
            {
                throw new ArgumentNullException("characterInMovie");
            }

            if (this.characters.Exists(c => c.Character.Id == characterInMovie.Character.Id))
            {
                return;
            }

            this.characters.Add(characterInMovie);

            // ToDo: this.AddPerson(new PersonInMovie(characterInMovie.Person, CastDepartment, ActorRole));
        }

        /// <summary>Removes specified <paramref name="characterInMovie"/> item from the list and saves the change to the database.</summary>
        /// <param name="characterInMovie">The item to remove.</param>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0.A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        public void AddCharacterAndSave(CharacterInMovie characterInMovie)
        {
            this.AddCharacter(characterInMovie);
            this.ValidateMovieId();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharacterInMovieAdd", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@movieId", this.MovieId);
                command.Parameters.AddWithValue("@characterId", characterInMovie.Character.Id);
                command.Parameters.AddWithValue("@personId", characterInMovie.Person.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Removes specified <paramref name="characterInMovie"/> item from the list.</summary>
        /// <param name="characterInMovie">The item to remove.</param>
        /// <returns><see langword="true"/> if the <paramref name="characterInMovie"/> was found and removed; <see langword="false"/> if it was not found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="characterInMovie"/> is <see langword="null"/>.</exception>
        public bool RemoveCharacter(CharacterInMovie characterInMovie)
        {
            if (characterInMovie == null)
            {
                throw new ArgumentNullException("characterInMovie");
            }

            if (!this.characters.Contains(characterInMovie))
            {
                return false;
            }

            this.characters.Remove(characterInMovie);

            // ToDo: this.RemovePersonAsActor(person);
            return true;
        }

        /// <summary>Removes specified <paramref name="characterInMovie"/> item from the list and saves the change to the database.</summary>
        /// <param name="characterInMovie">The item to remove.</param>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0.A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        public void RemoveCharacterAndSave(CharacterInMovie characterInMovie)
        {
            if (!this.RemoveCharacter(characterInMovie))
            {
                return;
            }

            this.ValidateMovieId();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharacterInMovieRemove", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@movieId", this.MovieId);
                command.Parameters.AddWithValue("@characterId", characterInMovie.Character.Id);
                command.Parameters.AddWithValue("@personId", characterInMovie.Person.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Saves all <see cref="CharacterInMovie"/> to the database.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The &lt;system.data.localdb&gt; tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        public void Save()
        {
            this.ValidateMovieId();
            using (var table = new DataTable())
            {
                table.Locale = CultureInfo.InvariantCulture;
                table.Columns.Add(new DataColumn("CharacterId"));
                table.Columns.Add(new DataColumn("PersonId"));
                foreach (var characterInMovie in this.characters)
                {
                    table.Rows.Add(characterInMovie.Character.Id, characterInMovie.Person.Id);
                }

                using (var connection = new SqlConnection(Persistent.ConnectionString))
                using (var command = new SqlCommand("CharactersInMovieSave", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@movieId", this.MovieId);
                    command.Parameters.AddWithValue("@characters", table);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Loads <see cref="Character"/>s for the current movie.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        /// <exception cref="SqlException">A connection-level error occurred while opening the connection. If the <see cref="P:System.Data.SqlClient.SqlException.Number" /> property contains the value 18487 or 18488, this indicates that the specified password has expired or must be reset. See the <see cref="M:System.Data.SqlClient.SqlConnection.ChangePassword(System.String,System.String)" /> method for more information.The &lt;system.data.localdb&gt; tag in the app.config file has invalid or unknown elements.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
        /// <exception cref="IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
        public void LoadCharacters()
        {
            this.ValidateMovieId();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharactersInMovieGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@movieId", this.MovieId);
                connection.Open();

                var loadData = new List<CharacterLoadShell>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loadData.Add(new CharacterLoadShell(reader));
                    }
                }

                this.characters.Clear();
                this.characters.AddRange(loadData.Select(c => new CharacterInMovie(GlobalCache.GetPerson(c.PersonId), GlobalCache.GetCharacter(c.CharacterId), c.UserRating)));
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

        /// <summary>Temporarily holds ids related to a <see cref="Character"/> in a <see cref="Movie"/> during loaded.</summary>
        private class CharacterLoadShell
        {
            /// <summary>Initializes a new instance of the <see cref="CharacterLoadShell" /> class.</summary>
            /// <param name="record">The record containing the data for the <see cref="CharacterLoadShell"/>.</param>
            /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
            public CharacterLoadShell(IDataRecord record)
            {
                ReadFromRecord(this, record);
            }

            /// <summary>Gets the id of the <see cref="Character"/>.</summary>
            public int CharacterId { get; private set; }

            /// <summary>Gets the id of the <see cref="Person"/> playing the <see cref="Character"/>.</summary>
            public int PersonId { get; private set; }

            /// <summary>Gets the current user's rating of the <see cref="Character"/> in the <see cref="Movie"/>.</summary>
            public int UserRating { get; private set; }

            /// <summary>Updates a <see cref="CharacterLoadShell"/> from a record.</summary>
            /// <param name="character">The <see cref="CharacterLoadShell"/> to update.</param>
            /// <param name="record">The record containing the data for the <see cref="CharacterLoadShell"/>.</param>
            /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
            private static void ReadFromRecord(CharacterLoadShell character, IDataRecord record)
            {
                Persistent.ValidateRecord(record, new[] { "CharacterId", "PersonId", "Rating" });
                character.CharacterId = (int)record["CharacterId"];
                character.PersonId = (int)record["PersonId"];
                character.UserRating = (int)record["Rating"];
            }
        }
    }
}
