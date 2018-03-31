//-----------------------------------------------------------------------
// <copyright file="MovieType.cs">
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

    /// <summary>Represents a type of a movie.</summary>
    public class MovieType : Typeable<MovieType, MovieTypeDto>
    {
        /// <summary>Gets a reference to simulate static methods.</summary>
        public static MovieType Static { get; } = new MovieType();

        /// <summary>Gets the list of titles of the movie type in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <inheritdoc />
        public override MovieTypeDto ToContract()
        {
            return new MovieTypeDto
            {
                Id = this.Id,
                Titles = this.Titles.ToContract()
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override MovieType FromContract(MovieTypeDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new MovieType
            {
                Id = contract.Id,
                Titles = this.Titles.FromContract(contract.Titles)
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieType"/> is not valid to be saved.</exception>
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
        public override async Task<MovieType> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<MovieType>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAsync(session.ToContract(), idList.ToList())).Select(x => new ({T})(x));
                return new List<MovieType>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<MovieType>> GetAllAsync(UserSession session)
        {
            if (!Persistent.UseService)
            {
                return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAllAsync(session.ToContract())).Select(x => new ({T})(x));
                return new List<MovieType>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieType"/> is not valid to be saved.</exception>
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
        internal override async Task<IEnumerable<MovieType>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var movieTypes = new List<MovieType>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, $"{nameof(MovieType)}s");
            }

            while (await reader.ReadAsync())
            {
                movieTypes.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, $"{nameof(MovieType)}{LanguageTitleCollection.TitlesColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movieType = (MovieType)this.GetFromResultsByIdInRecord(movieTypes, reader, IdColumn);
                movieType.Titles.Add(await LanguageTitle.Static.NewFromRecordAsync(reader));
            }

            return movieTypes;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<MovieType> NewFromRecordAsync(IDataRecord record)
        {
            var result = new MovieType();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
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
