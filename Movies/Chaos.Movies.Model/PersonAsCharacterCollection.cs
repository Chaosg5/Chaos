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
    using System.Linq;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents <see cref="Character"/>s in a <see cref="Parent"/>.</summary>
    public class PersonAsCharacterCollection : IReadOnlyCollection<PersonAsCharacter>
    {
        /// <summary>The list of <see cref="Character"/>s in this <see cref="PersonAsCharacterCollection"/>.</summary>
        private readonly List<PersonAsCharacter> characters = new List<PersonAsCharacter>();

        /// <summary>Gets the id and type of the parent which this <see cref="PersonAsCharacterCollection"/> belongs to.</summary>
        private Parent parent;

        /// <summary>Initializes a new instance of the <see cref="PersonAsCharacterCollection"/> class.</summary>
        public PersonAsCharacterCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PersonAsCharacterCollection"/> class.</summary>
        /// <param name="parent">The parent which this <see cref="PersonAsCharacterCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        internal PersonAsCharacterCollection(Parent parent)
        {
            this.SetParent(parent);
        }

        /// <summary>Gets the number of elements contained in this <see cref="PersonAsCharacterCollection"/>.</summary>
        public int Count => this.characters.Count;

        /// <summary>Returns an enumerator which iterates through this <see cref="PersonAsCharacterCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="PersonAsCharacterCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<PersonAsCharacter> GetEnumerator()
        {
            return this.characters.GetEnumerator();
        }

        /// <summary>Removes specified <paramref name="personAsCharacter"/> item to the list.</summary>
        /// <param name="personAsCharacter">The item to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="personAsCharacter"/> is <see langword="null" />.</exception>
        public void AddCharacter(PersonAsCharacter personAsCharacter)
        {
            if (personAsCharacter == null)
            {
                throw new ArgumentNullException(nameof(personAsCharacter));
            }

            if (this.characters.Exists(c => c.Character.Id == personAsCharacter.Character.Id))
            {
                // return;
            }

            this.characters.Add(personAsCharacter);

            // ToDo: this.AddPerson(new PersonInMovie(characterInMovie.Person, CastDepartment, ActorRole));
        }

        /// <summary>Removes specified <paramref name="personAsCharacter"/> item from the list and saves the change to the database.</summary>
        /// <param name="personAsCharacter">The item to remove.</param>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid parent.</exception>
        public void AddCharacterAndSave(PersonAsCharacter personAsCharacter)
        {
            this.ValidateParent();
            this.AddCharacter(personAsCharacter);
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"CharacterIn{this.parent.ParentType}Add", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue($"@{this.parent.VariableName}Id", this.parent.ParentId);
                command.Parameters.AddWithValue("@characterId", personAsCharacter.Character.Id);
                command.Parameters.AddWithValue("@personId", personAsCharacter.Person.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Removes specified <paramref name="personAsCharacter"/> item from the list.</summary>
        /// <param name="personAsCharacter">The item to remove.</param>
        /// <returns><see langword="true"/> if the <paramref name="personAsCharacter"/> was found and removed; <see langword="false"/> if it was not found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="personAsCharacter"/> is <see langword="null"/>.</exception>
        public bool RemoveCharacter(PersonAsCharacter personAsCharacter)
        {
            if (personAsCharacter == null)
            {
                throw new ArgumentNullException(nameof(personAsCharacter));
            }

            if (!this.characters.Contains(personAsCharacter))
            {
                return false;
            }

            this.characters.Remove(personAsCharacter);

            // ToDo: this.RemovePersonAsActor(person);
            return true;
        }

        /// <summary>Removes specified <paramref name="personAsCharacter"/> item from the list and saves the change to the database.</summary>
        /// <param name="personAsCharacter">The item to remove.</param>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid parent.</exception>
        public void RemoveCharacterAndSave(PersonAsCharacter personAsCharacter)
        {
            if (!this.RemoveCharacter(personAsCharacter))
            {
                return;
            }

            this.ValidateParent();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"CharacterIn{this.parent.ParentType}Remove", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue($"@{this.parent.VariableName}Id", this.parent.ParentId);
                command.Parameters.AddWithValue("@characterId", personAsCharacter.Character.Id);
                command.Parameters.AddWithValue("@personId", personAsCharacter.Person.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Saves all <see cref="PersonAsCharacter"/> to the database.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid parent.</exception>
        public void Save()
        {
            this.ValidateParent();
            using (var table = new DataTable())
            {
                table.Locale = CultureInfo.InvariantCulture;
                table.Columns.Add(new DataColumn("CharacterId"));
                table.Columns.Add(new DataColumn("PersonId"));
                foreach (var character in this.characters)
                {
                    table.Rows.Add(character.Character.Id, character.Person.Id);
                }

                using (var connection = new SqlConnection(Persistent.ConnectionString))
                using (var command = new SqlCommand($"CharactersIn{this.parent.ParentType}Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@{this.parent.VariableName}Id", this.parent.ParentId);
                    command.Parameters.AddWithValue("@characters", table);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Loads <see cref="Character"/>s for the current <see cref="Parent"/>.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid parent.</exception>
        public void LoadCharacters()
        {
            this.ValidateParent();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"CharactersIn{this.parent.ParentType}Get", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue($"@{this.parent.VariableName}Id", this.parent.ParentId);
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
                this.characters.AddRange(
                    loadData.Select(
                        c => new PersonAsCharacter(GlobalCache.GetPerson(c.PersonId), GlobalCache.GetCharacter(c.CharacterId), c.UserRating)));
            }
        }

        /// <summary>Sets the parent of this <see cref="PersonAsCharacterCollection"/>.</summary>
        /// <param name="newParent">The parent which this <see cref="PersonAsCharacterCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        internal void SetParent(Parent newParent)
        {
            if (this.parent != null)
            {
                throw new ValueLogicalReadOnlyException("The parent can't be changed once set.");
            }

            this.parent = newParent;
        }

        /// <summary>Validates that the <see cref="parent"/> is set.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid parent.</exception>
        private void ValidateParent()
        {
            if (this.parent == null)
            {
                throw new PersistentObjectRequiredException("The parent needs to be set.");
            }
        }

        /// <summary>Temporarily holds ids related to a <see cref="Character"/> in a <see cref="Parent"/> during loaded.</summary>
        private class CharacterLoadShell
        {
            /// <summary>Initializes a new instance of the <see cref="CharacterLoadShell" /> class.</summary>
            /// <param name="record">The record containing the data for the <see cref="CharacterLoadShell"/>.</param>
            /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
            /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
            public CharacterLoadShell(IDataRecord record)
            {
                this.ReadFromRecord(record);
            }

            /// <summary>Gets the id of the <see cref="Character"/>.</summary>
            public int CharacterId { get; private set; }

            /// <summary>Gets the id of the <see cref="Person"/> playing the <see cref="Character"/>.</summary>
            public int PersonId { get; private set; }

            /// <summary>Gets the current user's rating of the <see cref="Character"/> in the <see cref="Parent"/>.</summary>
            public int UserRating { get; private set; }

            /// <summary>Updates this <see cref="CharacterLoadShell"/> from the <paramref name="record"/>.</summary>
            /// <param name="record">The record containing the data for the <see cref="CharacterLoadShell"/>.</param>
            /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
            /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
            private void ReadFromRecord(IDataRecord record)
            {
                Helper.ValidateRecord(record, new[] { "CharacterId", "PersonId", "Rating" });
                this.CharacterId = (int)record["CharacterId"];
                this.PersonId = (int)record["PersonId"];
                this.UserRating = (int)record["Rating"];
            }
        }
    }
}