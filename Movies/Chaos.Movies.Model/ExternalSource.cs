//-----------------------------------------------------------------------
// <copyright file="ExternalSource.cs">
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

    /// <summary>Represents a user.</summary>
    public sealed class ExternalSource : Typeable<ExternalSource, ExternalSourceDto>
    {
        // ToDo: MovieTypeAddress

        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>The database column for <see cref="BaseAddress"/>.</summary>
        private const string BaseAddressColumn = "BaseAddress";

        /// <summary>The database column for <see cref="LogoIcon"/>.</summary>
        private const string LogoIconColumn = "LogoIcon";

        /// <summary>The database column for <see cref="PeopleAddress"/>.</summary>
        private const string PeopleAddressColumn = "PeopleAddress";

        /// <summary>The database column for <see cref="CharacterAddress"/>.</summary>
        private const string CharacterAddressColumn = "CharacterAddress";

        /// <summary>The database column for <see cref="GenreAddress"/>.</summary>
        private const string GenreAddressColumn = "GenreAddress";

        /// <summary>The database column for <see cref="EpisodeAddress"/>.</summary>
        private const string EpisodeAddressColumn = "EpisodeAddress";

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name = string.Empty;

        /// <summary>Initializes a new instance of the <see cref="ExternalSource"/> class.</summary>
        /// <param name="name">The <see cref="Name"/> to set.</param>
        /// <param name="logoIcon">The <see cref="LogoIcon"/> to set.</param>
        /// <param name="baseAddress">The <see cref="BaseAddress"/> to set.</param>
        /// <param name="peopleAddress">The <see cref="PeopleAddress"/> to set.</param>
        /// <param name="characterAddress">The <see cref="CharacterAddress"/> to set.</param>
        /// <param name="genreAddress">The <see cref="GenreAddress"/> to set.</param>
        /// <param name="episodeAddress">The <see cref="EpisodeAddress"/> to set.</param>
        public ExternalSource(
            string name,
            string logoIcon,
            string baseAddress,
            string peopleAddress,
            string characterAddress,
            string genreAddress,
            string episodeAddress)
        {
            this.Name = name;
            this.LogoIcon = logoIcon ?? string.Empty;
            this.BaseAddress = baseAddress ?? string.Empty;
            this.PeopleAddress = peopleAddress ?? string.Empty;
            this.CharacterAddress = characterAddress ?? string.Empty;
            this.GenreAddress = genreAddress ?? string.Empty;
            this.EpisodeAddress = episodeAddress ?? string.Empty;
        }

        /// <inheritdoc />
        private ExternalSource()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static ExternalSource Static { get; } = new ExternalSource();

        /// <summary>Gets the name.</summary>
        public string Name
        {
            get => this.name;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                this.name = value;
            }
        }

        /// <summary>Gets the base address.</summary>
        public string LogoIcon { get; private set; } = string.Empty;

        /// <summary>Gets the base address.</summary>
        public string BaseAddress { get; private set; } = string.Empty;

        /// <summary>Gets the people address.</summary>
        public string PeopleAddress { get; private set; } = string.Empty;

        /// <summary>Gets the character address.</summary>
        public string CharacterAddress { get; private set; } = string.Empty;

        /// <summary>Gets the genre address.</summary>
        public string GenreAddress { get; private set; } = string.Empty;

        /// <summary>Gets the episode address.</summary>
        public string EpisodeAddress { get; private set; } = string.Empty;

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="ExternalSource"/> is not valid to be saved.</exception>
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
                await service.ExternalSourceSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        public override ExternalSourceDto ToContract()
        {
            return new ExternalSourceDto
            {
                Id = this.Id,
                Name = this.Name,
                LogoIcon = this.LogoIcon,
                BaseAddress = this.BaseAddress,
                PeopleAddress = this.PeopleAddress,
                CharacterAddress = this.CharacterAddress,
                GenreAddress = this.GenreAddress,
                EpisodeAddress = this.EpisodeAddress
            };
        }

        /// <inheritdoc />
        public override ExternalSourceDto ToContract(string languageName)
        {
            return new ExternalSourceDto
            {
                Id = this.Id,
                Name = this.Name,
                LogoIcon = this.LogoIcon,
                BaseAddress = this.BaseAddress,
                PeopleAddress = this.PeopleAddress,
                CharacterAddress = this.CharacterAddress,
                GenreAddress = this.GenreAddress,
                EpisodeAddress = this.EpisodeAddress
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override ExternalSource FromContract(ExternalSourceDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }
            
            return new ExternalSource
            {
                Id = contract.Id,
                Name = contract.Name,
                LogoIcon = contract.LogoIcon,
                BaseAddress = contract.BaseAddress,
                PeopleAddress = contract.PeopleAddress,
                CharacterAddress = contract.CharacterAddress,
                GenreAddress = contract.GenreAddress,
                EpisodeAddress = contract.EpisodeAddress
            };
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<ExternalSource> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<IEnumerable<ExternalSource>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.ExternalSourceGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<ExternalSource>> GetAllAsync(UserSession session)
        {
            if (!Persistent.UseService)
            {
                return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return new List<ExternalSource>();
                ////return (await service.ExternalSourceGetAllAsync(session.ToContract())).Select(this.FromContract);
            }
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
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<IEnumerable<ExternalSource>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var externalSources = new List<ExternalSource>();
            if (!reader.HasRows)
            {
                return externalSources;
            }
            
            while (await reader.ReadAsync())
            {
                externalSources.Add(await this.NewFromRecordAsync(reader));
            }

            return externalSources;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override async Task<ExternalSource> NewFromRecordAsync(IDataRecord record)
        {
            var result = new ExternalSource();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            var requiredColumns = new[]
            {
                IdColumn,
                NameColumn,
                LogoIconColumn,
                BaseAddressColumn,
                PeopleAddressColumn,
                CharacterAddressColumn,
                GenreAddressColumn,
                EpisodeAddressColumn
            };
            Persistent.ValidateRecord(
                record,
                requiredColumns);
            this.Id = (int)record[IdColumn];
            this.Name = (string)record[NameColumn];
            this.LogoIcon = (string)record[LogoIconColumn];
            this.BaseAddress = (string)record[BaseAddressColumn];
            this.PeopleAddress = (string)record[PeopleAddressColumn];
            this.CharacterAddress = (string)record[CharacterAddressColumn];
            this.GenreAddress = (string)record[GenreAddressColumn];
            this.EpisodeAddress = (string)record[EpisodeAddressColumn];
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
                    { Persistent.ColumnToVariable(LogoIconColumn), this.LogoIcon },
                    { Persistent.ColumnToVariable(BaseAddressColumn), this.BaseAddress },
                    { Persistent.ColumnToVariable(PeopleAddressColumn), this.PeopleAddress },
                    { Persistent.ColumnToVariable(CharacterAddressColumn), this.CharacterAddress },
                    { Persistent.ColumnToVariable(GenreAddressColumn), this.GenreAddress },
                    { Persistent.ColumnToVariable(EpisodeAddressColumn), this.EpisodeAddress }
                });
        }
    }
}