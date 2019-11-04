//-----------------------------------------------------------------------
// <copyright file="MovieSeries.cs">
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

    /// <summary>A series of movies.</summary>
    /// <remarks>A TV series is considered to be a <see cref="Movie"/> while a series of TV series would be considered to be a <see cref="MovieSeries"/>.
    /// For example "Star Trek: The Next Generation" is a <see cref="Movie"/> but is part of the "Star Trek" and "Star Trek TV series" <see cref="MovieSeries"/> but not the "Star Trek Movies" <see cref="MovieSeries"/>.</remarks>
    public class MovieSeries : Readable<MovieSeries, MovieSeriesDto>
    {
        /// <summary>Private part of the <see cref="Movies"/> property.</summary>
        private MovieSeriesType movieSeriesType;

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static MovieSeries Static { get; } = new MovieSeries();

        /// <summary>Gets the type of the movie series.</summary>
        public MovieSeriesType MovieSeriesType
        {
            get => this.movieSeriesType;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(MovieSeriesType)} has to be saved.");
                }

                this.movieSeriesType = value;
            }
        }

        /// <summary>Gets the list of title of the movie collection in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <summary>Gets the movies which are a part of this collection with the keys representing their order.</summary>
        public MovieCollection Movies { get; private set; } = new MovieCollection();

        /// <summary>Gets the list of images for the <see cref="Person"/> and their order.</summary>
        public IconCollection Images { get; private set; } = new IconCollection();

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieSeries"/> is not valid to be saved.</exception>
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
                await service.MovieSeriesSaveAsync(session.ToContract(), this.ToContract());
            }
        }
        
        /// <inheritdoc />
        public override MovieSeriesDto ToContract()
        {
            return new MovieSeriesDto
            {
                Id = this.Id,
                Movies = this.Movies.ToContract(),
                Titles = this.Titles.ToContract(),
                Images = this.Images.ToContract()
            };
        }

        /// <inheritdoc />
        public override MovieSeriesDto ToContract(string languageName)
        {
            return new MovieSeriesDto
            {
                Id = this.Id,
                Movies = this.Movies.ToContract(languageName),
                Titles = this.Titles.ToContract(languageName),
                Images = this.Images.ToContract(languageName)
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override MovieSeries FromContract(MovieSeriesDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new MovieSeries
            {
                Id = contract.Id,
                Movies = this.Movies.FromContract(contract.Movies),
                Titles = this.Titles.FromContract(contract.Titles),
                Images = this.Images.FromContract(contract.Images)
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<MovieSeries> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<MovieSeries>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.MovieSeriesGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <summary>Validates that this <see cref="MovieSeries"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieSeries"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }

            if (this.Movies.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }

            if (this.MovieSeriesType == null || this.MovieSeriesType.Id <= 0)
            {
                throw new InvalidSaveCandidateException("A valid type needs to be specified.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override async Task<IEnumerable<MovieSeries>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var movieSeriesList = new List<MovieSeries>();
            if (!reader.HasRows)
            {
                return movieSeriesList;
            }

            while (await reader.ReadAsync())
            {
                movieSeriesList.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, $"{nameof(MovieSeries)}{LanguageTitleCollection.TitlesColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movieSeries = (MovieSeries)this.GetFromResultsByIdInRecord(movieSeriesList, reader, IdColumn);
                movieSeries.Titles.Add(await LanguageTitle.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(3, $"{nameof(MovieSeries)}{MovieCollection.MoviesColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movieSeries = (MovieSeries)this.GetFromResultsByIdInRecord(movieSeriesList, reader, IdColumn);
                movieSeries.Movies.Add(await GlobalCache.GetMovieAsync((int)reader[Movie.IdColumn]));
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(4, $"{nameof(Person)}{IconCollection.IconsColumn}");
            }

            while (await reader.ReadAsync())
            {
                var person = (MovieSeries)this.GetFromResultsByIdInRecord(movieSeriesList, reader, IdColumn);
                person.Images.Add(await Icon.Static.NewFromRecordAsync(reader));
            }

            return movieSeriesList;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override async Task<MovieSeries> NewFromRecordAsync(IDataRecord record)
        {
            var result = new MovieSeries();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, MovieSeriesType.IdColumn });
            this.Id = (int)record[IdColumn];
            this.MovieSeriesType = await GlobalCache.GetMovieSeriesTypeAsync((int)record[MovieSeriesType.IdColumn]);
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(MovieCollection.MoviesColumn), this.Movies.GetSaveTable },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable },
                    { Persistent.ColumnToVariable(IconCollection.IconsColumn), this.Images.GetSaveTable }
                });
        }
    }
}
