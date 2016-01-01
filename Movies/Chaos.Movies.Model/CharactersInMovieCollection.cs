//-----------------------------------------------------------------------
// <copyright file="CharactersInMovie.cs">
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

    /// <summary>Represents <see cref="Character"/>s in a movie.</summary>
    public class CharactersInMovieCollection : IReadOnlyCollection<CharacterInMovie>
    {
        /// <summary>The list of <see cref="Character"/>s in this <see cref="CharactersInMovieCollection"/>.</summary>
        private readonly List<CharacterInMovie> characters = new List<CharacterInMovie>();

        /// <summary>Private part of the <see cref="MovieId"/> property.</summary>
        private int movieId;

        /// <summary>Initializes a new instance of the <see cref="CharactersInMovieCollection"/> class.</summary>
        public CharactersInMovieCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CharactersInMovieCollection"/> class.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="CharactersInMovieCollection"/> belongs to.</param>
        public CharactersInMovieCollection(int movieId)
        {
            this.MovieId = movieId;
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="CharactersInMovieCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Gets id of the <see cref="Movie"/> which this <see cref="CharactersInMovieCollection"/> belongs to.</summary>
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
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (this.movieId != 0)
                {
                    throw new ValueLogicalReadOnlyException("The id of the movie can't be changed once set.");
                }

                this.movieId = value;
            }
        }

        /// <summary>Gets the number of elements contained in this <see cref="CharactersInMovieCollection"/>.</summary>
        public int Count => this.characters.Count;

        /// <summary>Returns an enumerator which iterates through this <see cref="CharactersInMovieCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<CharacterInMovie> GetEnumerator()
        {
            return this.characters.GetEnumerator();
        }

        /// <summary>Sets the id of the <see cref="Movie"/> this <see cref="CharactersInMovieCollection"/> belongs to.</summary>
        /// <param name="id">The id of the <see cref="Movie"/> which this <see cref="CharactersInMovieCollection"/> belongs to.</param>
        public void SetMovieId(int id)
        {
            this.MovieId = id;
        }

        /// <summary>Removes specified <paramref name="characterInMovie"/> item to the list.</summary>
        /// <param name="characterInMovie">The item to add.</param>
        public void AddCharacter(CharacterInMovie characterInMovie)
        {
            if (characterInMovie == null)
            {
                throw new ArgumentNullException(nameof(characterInMovie));
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
        public void AddCharacterAndSave(CharacterInMovie characterInMovie)
        {
            this.AddCharacter(characterInMovie);
            this.ValidateMovieId();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharacterInMovieAdd", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieId", this.MovieId));
                command.Parameters.Add(new SqlParameter("@characterId", characterInMovie.Character.Id));
                command.Parameters.Add(new SqlParameter("@personId", characterInMovie.Person.Id));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Removes specified <paramref name="characterInMovie"/> item from the list.</summary>
        /// <param name="characterInMovie">The item to remove.</param>
        public bool RemoveCharacter(CharacterInMovie characterInMovie)
        {
            if (characterInMovie == null)
            {
                throw new ArgumentNullException(nameof(characterInMovie));
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
                command.Parameters.Add(new SqlParameter("@movieId", this.MovieId));
                command.Parameters.Add(new SqlParameter("@characterId", characterInMovie.Character.Id));
                command.Parameters.Add(new SqlParameter("@personId", characterInMovie.Person.Id));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Saves all <see cref="CharacterInMovie"/> to the database.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
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
                    command.Parameters.Add(new SqlParameter("@movieId", this.MovieId));
                    command.Parameters.Add(new SqlParameter("@characters", table));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Loads <see cref="Character"/>s for the current movie.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="MovieId"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void LoadCharacters()
        {
            this.ValidateMovieId();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharactersInMovieGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieId", this.movieId));
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
                this.characters.AddRange(loadData.Select(c => new CharacterInMovie(GlobalCache.GetPerson(c.PersonId), GlobalCache.GetCharacter(c.CharacterId))));
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
            public CharacterLoadShell(IDataRecord record)
            {
                ReadFromRecord(this, record);
            }

            /// <summary>Gets the id of the <see cref="Character"/>.</summary>
            public int CharacterId { get; private set; }

            /// <summary>Gets the id of the <see cref="Person"/> playing the <see cref="Character"/>.</summary>
            public int PersonId { get; private set; }

            /// <summary>Updates a <see cref="CharacterLoadShell"/> from a record.</summary>
            /// <param name="character">The <see cref="CharacterLoadShell"/> to update.</param>
            /// <param name="record">The record containing the data for the <see cref="CharacterLoadShell"/>.</param>
            private static void ReadFromRecord(CharacterLoadShell character, IDataRecord record)
            {
                Persistent.ValidateRecord(record, new[] { "CharacterId", "PersonId" });
                character.CharacterId = (int)record["CharacterId"];
                character.PersonId = (int)record["PersonId"];
            }
        }
    }
}
