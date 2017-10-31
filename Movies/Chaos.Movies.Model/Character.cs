//-----------------------------------------------------------------------
// <copyright file="Character.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public class Character
    {
        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name;

        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="name">The name of the character.</param>
        public Character(string name)
        {
            this.Name = name;
        }
        
        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="character">The character to create.</param>
        public Character(CharacterDto character)
        {
            this.Id = character.Id;
            this.Name = character.Name;
        }

        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        public Character(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the id of the <see cref="Character"/>.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the name of the character.</summary>
        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="value"/> is <see langword="null" />.</exception>
        public string Name
        {
            get => this.name;

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.name = value;
            }
        }

        /// <summary>Gets the id of the <see cref="Character"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookup { get; } = new ExternalLookupCollection();

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        public IEnumerable<Icon> Images => this.images;

        /// <summary>Gets the specified <see cref="Character"/>s.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="idList">The list of ids of the <see cref="Character"/>s to get.</param>
        /// <remarks>Uses stored procedure <c>CharactersGet</c>.
        /// Result 1 columns: CharacterId, Name</remarks>
        /// <returns>The list of <see cref="Character"/>s.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        public static async Task<IEnumerable<Character>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await GetFromDatabaseAsync(idList);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.CharacterGetAsync(session.ToContract(), idList.ToList())).Select(c => new Character(c));
            }
        }

        /// <summary>Saves this <see cref="Character"/> to the database.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Character"/> is not valid to be saved.</exception>
        /// <returns>No return.</returns>
        public async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync();
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                await service.CharacterSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <summary>Converts this <see cref="Character"/> to a <see cref="CharacterDto"/>.</summary>
        /// <returns>The <see cref="CharacterDto"/>.</returns>
        public CharacterDto ToContract()
        {
            return new CharacterDto
            {
                Id = this.Id,
                Name = this.Name,
                ExternalLookup = this.ExternalLookup.ToContract(),
                Images = this.Images.Select(s => s.ToContract())
            };
        }

        public void AddImage()
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
        private static async Task<IEnumerable<Character>> GetFromDatabaseAsync(IEnumerable<int> idList)
        {
            var characters = new List<Character>();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharactersGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idList", Persistent.CreateIdCollectionTable(idList));
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

        /// <summary>Updates this <see cref="Character"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Character"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "CharacterId", "Name" });
            this.Id = (int)record["CharacterId"];
            this.Name = record["Name"].ToString();
            // ToDo get ExternalLookup, Images
        }

        /// <summary>Validates that the this <see cref="Character"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">This <see cref="Character"/> is not valid to be saved.</exception>
        private void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException("The character's name can not be empty.");
            }
        }
    }
}
