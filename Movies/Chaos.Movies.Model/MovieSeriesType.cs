//-----------------------------------------------------------------------
// <copyright file="MovieSeriesType.cs">
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

    /// <summary>Represents a type of a movie series.</summary>
    public class MovieSeriesType : Typeable<MovieSeriesType, MovieSeriesTypeDto>
    {
        /// <summary>Gets a reference to simulate static methods.</summary>
        public static MovieSeriesType Static { get; } = new MovieSeriesType();
        
        /// <summary>Gets the list of titles of the movie series type in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <inheritdoc />
        public override MovieSeriesTypeDto ToContract()
        {
            return new MovieSeriesTypeDto
            {
                Id = this.Id,
                Titles = this.Titles.ToContract()
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override MovieSeriesType FromContract(MovieSeriesTypeDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new MovieSeriesType
            {
                Id = contract.Id,
                Titles = this.Titles.FromContract(contract.Titles)
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieSeriesType"/> is not valid to be saved.</exception>
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
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<MovieSeriesType> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<MovieSeriesType>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAsync(session.ToContract(), idList.ToList())).Select(x => new ({T})(x));
                return new List<MovieSeriesType>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<MovieSeriesType>> GetAllAsync(UserSession session)
        {
            if (!Persistent.UseService)
            {
                return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAllAsync(session.ToContract())).Select(x => new ({T})(x));
                return new List<MovieSeriesType>();
            }
        }

        /// <summary>Validates that this <see cref="MovieSeriesType"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieSeriesType"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        internal override async Task<IEnumerable<MovieSeriesType>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var movieSeriesTypes = new List<MovieSeriesType>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, $"{nameof(MovieSeriesType)}s");
            }

            while (await reader.ReadAsync())
            {
                movieSeriesTypes.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, $"{nameof(MovieSeriesType)}{LanguageTitleCollection.TitlesColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movieSeriesType = (MovieSeriesType)this.GetFromResultsByIdInRecord(movieSeriesTypes, reader, IdColumn);
                movieSeriesType.Titles.Add(await LanguageTitle.Static.NewFromRecordAsync(reader));
            }

            return movieSeriesTypes;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<MovieSeriesType> NewFromRecordAsync(IDataRecord record)
        {
            var result = new MovieSeriesType();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.MissingColumnException">A required column is missing in the <paramref name="record" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="record" /> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "MovieSeriesTypeId" });
            this.Id = (int)record["MovieSeriesTypeId"];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}
