//-----------------------------------------------------------------------
// <copyright file="ExternalSource.cs" company="Erik Bunnstad">
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
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a user.</summary>
    public sealed class ExternalSource : Readable<ExternalSource, ExternalSourceDto>
    {
        /// <summary>The database column for <see cref="Id"/>.</summary>
        public const string ExternalSourceIdColumn = "ExternalSourceId";

        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>The database column for <see cref="BaseAddress"/>.</summary>
        private const string BaseAddressColumn = "BaseAddress";

        /// <summary>The database column for <see cref="PeopleAddress"/>.</summary>
        private const string PeopleAddressColumn = "PeopleAddress";

        /// <summary>The database column for <see cref="CharacterAddress"/>.</summary>
        private const string CharacterAddressColumn = "CharacterAddress";

        /// <summary>The database column for <see cref="GenreAddress"/>.</summary>
        private const string GenreAddressColumn = "GenreAddress";

        /// <summary>The database column for <see cref="EpisodeAddress"/>.</summary>
        private const string EpisodeAddressColumn = "EpisodeAddress";

        /// <inheritdoc />
        private ExternalSource()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static ExternalSource Static { get; } = new ExternalSource();
        
        /// <summary>Gets the name.</summary>
        public string Name { get; private set; }

        /// <summary>Gets the base address.</summary>
        public string BaseAddress { get; private set; }

        /// <summary>Gets the people address.</summary>
        public string PeopleAddress { get; private set; }

        /// <summary>Gets the character address.</summary>
        public string CharacterAddress { get; private set; }

        /// <summary>Gets the genre address.</summary>
        public string GenreAddress { get; private set; }

        /// <summary>Gets the episode address.</summary>
        public string EpisodeAddress { get; private set; }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="ExternalSource"/> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.({T})SaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="ExternalSource"/> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAllAsync(UserSession session)
        {
            await this.SaveAsync(session);
        }

        /// <summary>Converts this <see cref="ExternalSource"/> to a <see cref="ExternalSourceDto"/>.</summary>
        /// <returns>The <see cref="ExternalSourceDto"/>.</returns>
        public override ExternalSourceDto ToContract()
        {
            return new ExternalSourceDto
            {
                Id = this.Id,
                Name = this.Name,
                BaseAddress = this.BaseAddress,
                PeopleAddress = this.PeopleAddress,
                CharacterAddress = this.CharacterAddress,
                GenreAddress = this.GenreAddress,
                EpisodeAddress = this.EpisodeAddress
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">This <see cref="ExternalSource"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException($"The {nameof(this.Name)} can't be empty.");
            }

            if (string.IsNullOrEmpty(this.BaseAddress))
            {
                throw new InvalidSaveCandidateException($"The {nameof(this.BaseAddress)} can't be empty.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override Task<ExternalSource> ReadFromRecordAsync(IDataRecord record)
        {
            var requiredColumns = new[]
            {
                ExternalSourceIdColumn,
                NameColumn,
                BaseAddressColumn,
                PeopleAddressColumn,
                CharacterAddressColumn,
                GenreAddressColumn,
                EpisodeAddressColumn
            };
            Persistent.ValidateRecord(
                record,
                requiredColumns);
            var externalSource = new ExternalSource
            {
                Id = (int)record[ExternalSourceIdColumn],
                Name = record[NameColumn].ToString(),
                BaseAddress = record[BaseAddressColumn].ToString(),
                PeopleAddress = record[PeopleAddressColumn].ToString(),
                CharacterAddress = record[CharacterAddressColumn].ToString(),
                GenreAddress = record[GenreAddressColumn].ToString(),
                EpisodeAddress = record[EpisodeAddressColumn].ToString()
            };
            return Task.FromResult(externalSource);
        }

        /// <inheritdoc />
        public override Task<ExternalSource> GetAsync(UserSession session, int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<IEnumerable<ExternalSource>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(ExternalSourceIdColumn), this.Id },
                    { Persistent.ColumnToVariable(NameColumn), this.Name },
                    { Persistent.ColumnToVariable(BaseAddressColumn), this.BaseAddress },
                    { Persistent.ColumnToVariable(PeopleAddressColumn), this.PeopleAddress },
                    { Persistent.ColumnToVariable(CharacterAddressColumn), this.CharacterAddress },
                    { Persistent.ColumnToVariable(GenreAddressColumn), this.GenreAddress },
                    { Persistent.ColumnToVariable(EpisodeAddressColumn), this.EpisodeAddress }
                });
        }

        /// <inheritdoc />
        protected override Task<IEnumerable<ExternalSource>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}