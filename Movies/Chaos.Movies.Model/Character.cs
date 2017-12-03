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
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public sealed class Character : Readable<Character, CharacterDto>, IReadable<Character, CharacterDto>
    {
        /// <summary>The database column for <see cref="Id"/>.</summary>
        private const string CharacterIdColumn = "CharacterId";

        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name;

        /// <inheritdoc />
        public Character(string name)
        {
            this.Name = name;
        }

        /// <inheritdoc />
        public Character(CharacterDto character)
            : base(character)
        {
            this.Id = character.Id;
            this.Name = character.Name;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public Character(IDataRecord record)
            : base(record)
        {
            this.ReadFromRecord(record);
        }

        /// <inheritdoc />
        private Character()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Character Static { get; } = new Character();

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

        /// <summary>Gets the list of images for this <see cref="Character"/> and their order.</summary>
        public IconCollection Images { get; } = new IconCollection();

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task<Character> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task<IEnumerable<Character>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.CharacterGetAsync(session.ToContract(), idList.ToList())).Select(c => new Character(c));
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Character"/> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecord);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                await service.CharacterSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="T:Chaos.Movies.Model.Character" /> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task SaveAllAsync(UserSession session)
        {
            await this.SaveAsync(session);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task<IEnumerable<Character>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var characters = new List<Character>();
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
                var characterId = (int)reader[CharacterIdColumn];
                var character = characters.Find(c => c.Id == characterId);
                if (character == null)
                {
                    throw new MissingResultException($"The character id {characterId} in the icons was missing.");
                }

                character.Images.AddIcon(new Icon(reader));
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(3, "CharacterExternalLookup");
            }

            while (await reader.ReadAsync())
            {
                var characterId = (int)reader[CharacterIdColumn];
                var character = characters.Find(c => c.Id == characterId);
                if (character == null)
                {
                    throw new MissingResultException($"The character id {characterId} in the icons was missing.");
                }

                character.ExternalLookup.SetLookup(new ExternalLookup(reader));
            }

            return characters;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(CharacterIdColumn), this.Id },
                    { Persistent.ColumnToVariable(NameColumn), this.Name }
                });
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { CharacterIdColumn, NameColumn });
            this.Id = (int)record[CharacterIdColumn];
            this.Name = record[NameColumn].ToString();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">This <see cref="Character"/> is not valid to be saved.</exception>
        protected override void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException("The character's name can not be empty.");
            }
        }
    }
}
