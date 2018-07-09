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
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public sealed class Character : Readable<Character, CharacterDto>, ISearchable<Character>
    {
        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name = string.Empty;

        /// <inheritdoc />
        /// <param name="name">The value to set for <see cref="Name"/>.</param>
        public Character(string name)
        {
            this.Name = name;
        }
        
        /// <inheritdoc />
        private Character()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Character Static { get; } = new Character();
        
        /// <summary>Gets the name of the character.</summary>
        public string Name
        {
            get => this.name;

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value), "The name of the character has to be set.");
                }

                this.name = value;
            }
        }

        /// <summary>Gets the id of the <see cref="Character"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookups { get; private set; } = new ExternalLookupCollection();

        /// <summary>Gets the list of images for this <see cref="Character"/> and their order.</summary>
        public IconCollection Images { get; private set; } = new IconCollection();

        /// <summary>Gets the user ratings.</summary>
        public UserSingleRating UserRatings { get; private set; } = new UserSingleRating();

        /// <summary>Gets the total rating score from all users.</summary>
        public double TotalRating { get; private set; }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Character> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Character>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.CharacterGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public async Task<IEnumerable<Character>> SearchAsync(SearchParametersDto parametersDto, UserSession session)
        {
            if (!Persistent.UseService)
            {
                var items = new List<Character>();
                foreach (var id in await this.SearchDatabaseAsync(parametersDto, session))
                {
                    items.Add(await GlobalCache.GetCharacterAsync(id));
                }

                return items;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return new List<Character>();
                ////await service.CharacterSearchAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Character"/> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                await service.CharacterSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        public override CharacterDto ToContract()
        {
            return new CharacterDto
            {
                Id = this.Id,
                Name = this.Name,
                ExternalLookups = this.ExternalLookups.ToContract(),
                Images = this.Images.ToContract(),
                UserRatings = this.UserRatings.ToContract(),
                TotalRating = this.TotalRating
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override Character FromContract(CharacterDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new Character
            {
                Id = contract.Id,
                Name = contract.Name,
                ExternalLookups = this.ExternalLookups.FromContract(contract.ExternalLookups),
                Images = this.Images.FromContract(contract.Images),
                UserRatings = this.UserRatings.FromContract(contract.UserRatings),
                TotalRating = contract.TotalRating
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">This <see cref="Character"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException($"The {nameof(this.Name)} can't be empty.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        internal override async Task<IEnumerable<Character>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var characters = new List<Character>();
            if (!reader.HasRows)
            {
                return characters;
            }

            while (await reader.ReadAsync())
            {
                characters.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(2, $"{nameof(Character)}{IconCollection.IconsColumn}");
            }

            while (await reader.ReadAsync())
            {
                var character = (Character)this.GetFromResultsByIdInRecord(characters, reader, IdColumn);
                character.Images.Add(await Icon.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(3, $"{nameof(Character)}{ExternalLookupCollection.ExternalLookupColumn}");
            }

            while (await reader.ReadAsync())
            {
                var character = (Character)this.GetFromResultsByIdInRecord(characters, reader, IdColumn);
                character.ExternalLookups.Add(await ExternalLookup.Static.NewFromRecordAsync(reader));
            }

            return characters;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<Character> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Character();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, NameColumn, UserSingleRating.TotalRatingColumn });
            this.Id = (int)record[IdColumn];
            this.Name = (string)record[NameColumn];
            this.TotalRating = (int)record[UserSingleRating.TotalRatingColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(NameColumn), this.Name },
                    { Persistent.ColumnToVariable(ExternalLookupCollection.ExternalLookupColumn), this.ExternalLookups.GetSaveTable },
                    { Persistent.ColumnToVariable(IconCollection.IconsColumn), this.Images.GetSaveTable }
                });
        }
    }
}
