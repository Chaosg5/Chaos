//-----------------------------------------------------------------------
// <copyright file="SqlCharacter.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Sql
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>SQL logic and database communication for a <see cref="Character"/>.</summary>
    public class SqlCharacter : Character
    {
        /// <summary>Initializes a new instance of the <see cref="SqlCharacter" /> class.</summary>
        /// <param name="name">The name of the character.</param>
        public SqlCharacter(string name)
            : base(name)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SqlCharacter" /> class.</summary>
        /// <param name="character">The character to create.</param>
        public SqlCharacter(ICharacter character)
            : base(character)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SqlCharacter" /> class.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        public SqlCharacter(IDataRecord record)
            : base(record)
        {
        }

        /// <summary>Gets the specified <see cref="Character"/>s.</summary>
        /// <param name="idList">The list of ids of the <see cref="Character"/>s to get.</param>
        /// <remarks>
        /// Uses stored procedure <c>CharactersGet</c>.
        /// Result 1 columns: CharacterId, Name
        /// </remarks>
        /// <returns>The list of <see cref="Character"/>s.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        public static async Task<IEnumerable<Character>> GetAsync(IEnumerable<int> idList)
        {
            var characters = new List<Character>();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharactersGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idList", Persistent.CreateIntCollectionTable(idList));
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        throw new MissingResultException(1, "Characters");
                    }

                    while (await reader.ReadAsync())
                    {
                        characters.Add(new Character(reader));
                    }

                    if (!await reader.NextResultAsync())
                    {
                        throw new MissingResultException(2, "IconsInCharacters");
                    }

                    while (await reader.ReadAsync())
                    {
                    }
                }
            }

            return characters;
        }

        /// <summary>Saves this character to the database.</summary>
        /// <param name="session">The session.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Character"/> is not valid to be saved.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (Persistent.UseService)
            {
                await base.SaveAsync(session);
            }
            else
            {
                await this.SaveToDatabaseAsync();
            }
        }

        /// <summary>Saves this character to the database.</summary>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SaveToDatabaseAsync()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharacterSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@characterId", this.Id);
                command.Parameters.AddWithValue("@name", this.Name);
                command.Parameters.AddWithValue("@imdbId", this.ImdbId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        this.ReadFromRecord(reader);
                    }
                }
            }
        }
    }
}