//-----------------------------------------------------------------------
// <copyright file="Movie.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A movie or a series.</summary>
    public class Movie : Readable<Movie, MovieDto>
    {
        /// <summary>The database column for <see cref="Year"/>.</summary>
        private const string YearColumn = "Year";

        /// <summary>The database column for <see cref="EndYear"/>.</summary>
        private const string EndYearColumn = "EndYear";

        /// <summary>The database column for <see cref="RunTime"/>.</summary>
        private const string RunTimeColumn = "RunTime";

        /// <summary>Private part of the <see cref="Year"/> property.</summary>
        private int year;

        /// <summary>Private part of the <see cref="EndYear"/> property.</summary>
        private int endYear;

        /// <summary>Initializes a new instance of the <see cref="Movie" /> class.</summary>
        public Movie()
        {
            this.Characters = new PersonAsCharacterCollection<Movie, MovieDto>(this);
            this.People = new PersonInRoleCollection<Movie, MovieDto>(this);
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Movie Static { get; } = new Movie();

        /// <summary>Gets the id of the <see cref="Movie"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookups { get; private set; } = new ExternalLookupCollection();
    
        /// <summary>Gets the ratings of the movie in <see cref="ExternalSource"/>s.</summary>
        public ExternalRatingCollection ExternalRatings { get; private set; } = new ExternalRatingCollection();

        /// <summary>Gets the list of title of the movie in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <summary>Gets the list of genres that the movie belongs to.</summary>
        public GenreCollection Genres { get; private set; } = new GenreCollection();

        /// <summary>Gets the list of images for this <see cref="Movie"/> and their order.</summary>
        public IconCollection Images { get; private set; } = new IconCollection();

        /// <summary>Gets the total rating score from the current user.</summary>
        public UserRating UserRating { get; private set; } = new UserRating(new RatingType());

        /// <summary>Gets the total rating score from all users.</summary>
        public double TotalRating { get; private set; }

        /// <summary>Gets the list of <see cref="Character"/>s in this <see cref="Movie"/>.</summary>
        public PersonAsCharacterCollection<Movie, MovieDto> Characters { get; private set; }

        /// <summary>Gets the list of <see cref="Person"/>s in this <see cref="Movie"/>.</summary>
        public PersonInRoleCollection<Movie, MovieDto> People { get; private set; }

        /// <summary>Gets or sets the type of the movie.</summary>
        public MovieType MovieType { get; set; }

        /// <summary>Gets or sets the year when the movie was released.</summary>
        public int Year
        {
            get => this.year;
            set
            {
                var date = new SqlDateTime(value, 1, 1);
                this.year = date.Value.Year;
            }
        }
        
        /// <summary>Gets or sets the year when the movie was released.</summary>
        public int EndYear
        {
            get => this.endYear;
            set
            {
                var date = new SqlDateTime(value, 1, 1);
                this.endYear = date.Value.Year;
            }
        }

        /// <summary>Gets or sets the runtime of the movie.</summary>
        public int RunTime { get; private set; }

        /// <summary>The save all async.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAllAsync()
        {
        }

        /// <inheritdoc />
        public override MovieDto ToContract()
        {
            return new MovieDto
            {
                Id = this.Id,
                ExternalLookups = this.ExternalLookups.ToContract(),
                ExternalRatings = this.ExternalRatings.ToContract(),
                Titles = this.Titles.ToContract(),
                Genres = this.Genres.ToContract(),
                Images = this.Images.ToContract(),
                UserRating = this.UserRating.ToContract(),
                TotalRating = this.TotalRating,
                Characters = this.Characters.ToContract(),
                People = this.People.ToContract(),
                MovieType = this.MovieType.ToContract(),
                Year = this.Year,
                EndYear = this.EndYear,
                RunTime = this.RunTime
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override Movie FromContract(MovieDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new Movie
            {
                Id = contract.Id,
                ExternalLookups = this.ExternalLookups.FromContract(contract.ExternalLookups),
                ExternalRatings = this.ExternalRatings.FromContract(contract.ExternalRatings),
                Titles = this.Titles.FromContract(contract.Titles),
                Genres = this.Genres.FromContract(contract.Genres),
                Images = this.Images.FromContract(contract.Images),
                UserRating = this.UserRating.FromContract(contract.UserRating),
                TotalRating = this.TotalRating,
                Characters = this.Characters.FromContract(contract.Characters),
                People = this.People.FromContract(contract.People),
                MovieType = this.MovieType.FromContract(contract.MovieType),
                Year = contract.Year
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Movie> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Movie>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.MovieGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <inheritdoc />
        public override async Task SaveAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        internal override async Task<IEnumerable<Movie>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var movies = new List<Movie>();
            if (!reader.HasRows)
            {
                return movies;
            }

            while (await reader.ReadAsync())
            {
                movies.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, $"{nameof(Movie)}{LanguageTitleCollection.TitlesColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movie = (Movie)this.GetFromResultsByIdInRecord(movies, reader, IdColumn);
                movie.Titles.Add(await LanguageTitle.Static.NewFromRecordAsync(reader));
            }
            
            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(3, $"{nameof(Movie)}{ExternalLookupCollection.ExternalLookupColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movie = (Movie)this.GetFromResultsByIdInRecord(movies, reader, IdColumn);
                movie.ExternalLookups.Add(await ExternalLookup.Static.NewFromRecordAsync(reader));
            }
            
            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(4, $"{nameof(Movie)}{ExternalRatingCollection.ExternalRatingsColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movie = (Movie)this.GetFromResultsByIdInRecord(movies, reader, IdColumn);
                movie.ExternalRatings.Add(await ExternalRating.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(5, $"{nameof(Movie)}{GenreCollection.GenresColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movie = (Movie)this.GetFromResultsByIdInRecord(movies, reader, IdColumn);
                movie.Genres.Add(await Genre.Static.NewFromRecordAsync(reader));
            }
            
            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(6, $"{nameof(Movie)}{IconCollection.IconsColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movie = (Movie)this.GetFromResultsByIdInRecord(movies, reader, IdColumn);
                movie.Images.Add(await Icon.Static.NewFromRecordAsync(reader));
            }
            
            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(7, $"{nameof(Movie)}{PersonInRoleCollection<Movie, MovieDto>.PersonInRoleColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movie = (Movie)this.GetFromResultsByIdInRecord(movies, reader, IdColumn);
                movie.People.Add(await PersonInRole.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(8, $"{nameof(Movie)}{PersonAsCharacterCollection<Movie, MovieDto>.PersonAsCharacterColumn}");
            }

            while (await reader.ReadAsync())
            {
                var movie = (Movie)this.GetFromResultsByIdInRecord(movies, reader, IdColumn);
                movie.Characters.Add(await PersonAsCharacter.Static.NewFromRecordAsync(reader));
            }

            return movies;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<Movie> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Movie();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(
                record,
                new[] { IdColumn, MovieType.IdColumn, YearColumn, EndYearColumn, RunTimeColumn, UserSingleRating.TotalRatingColumn });
            this.Id = (int)record[IdColumn];
            this.MovieType = await GlobalCache.GetMovieTypeAsync((int)record[MovieType.IdColumn]);
            this.Year = (int)record[YearColumn];
            this.EndYear = (int)record[EndYearColumn];
            this.RunTime = (int)record[RunTimeColumn];
            this.TotalRating = (int)record[UserSingleRating.TotalRatingColumn];
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new System.NotImplementedException();
        }
    }
}