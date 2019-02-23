//-----------------------------------------------------------------------
// <copyright file="Movie.cs">
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
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A movie or a series.</summary>
    public class Movie : Rateable<Movie, MovieDto, bool>, ISearchable<Movie>
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
            this.Watches = new WatchCollection<Movie, MovieDto>(this);
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
        public UserDerivedRating UserRatings { get; private set; }

        /// <summary>Gets the user ratings.</summary>
        public UserSingleRating UserRating { get; private set; } = new UserSingleRating();

        /// <summary>Gets the total rating.</summary>
        public TotalRating TotalRating { get; private set; } = new TotalRating(typeof(Movie));

        /// <summary>Gets the list of <see cref="Character"/>s in this <see cref="Movie"/>.</summary>
        public PersonAsCharacterCollection<Movie, MovieDto> Characters { get; private set; }

        /// <summary>Gets the list of <see cref="Person"/>s in this <see cref="Movie"/>.</summary>
        public PersonInRoleCollection<Movie, MovieDto> People { get; private set; }

        /// <summary>Gets the list of <see cref="Watch"/>es in this <see cref="Movie"/>.</summary>
        public WatchCollection<Movie, MovieDto> Watches { get; private set; }

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

        /// <summary>Gets the runtime of the movie.</summary>
        public int RunTime { get; private set; }

        /// <summary>Saves a <see cref="UserSingleRating"/> for the <see cref="User"/> of the <paramref name="session"/> for the specified <paramref name="movieId"/> and <paramref name="ratingTypeId"/>.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> to rate the <see cref="Person"/> in.</param>
        /// <param name="ratingTypeId">The rating Type Id.</param>
        /// <param name="rating">The value of the <see cref="User"/>'s rating.</param>
        /// <param name="session">The <see cref="User"/>'s session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="PersistentObjectRequiredException">All items to save needs to be persisted.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="session"/> is <see langword="null"/></exception>
        public static async Task SaveUserRatingAsync(int movieId, int ratingTypeId, int rating, UserSession session)
        {
            if (movieId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Movie)} has to be saved.");
            }

            if (ratingTypeId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Department)} has to be saved.");
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (!Persistent.UseService)
            {
                // ToDo: Get Ratings Parents, not all
                var movie = await Static.GetAsync(session, movieId);
                await Static.GetUserDetailsFromDatabaseAsync(movie, movie.Id, Static.ReadUserMovieRatingsDetailsAsync, session, string.Empty);
                var ratingType = await GlobalCache.GetRatingTypeAsync(ratingTypeId);
                movie.UserRatings.SetValue(rating, ratingType);
                var ratings = movie.UserRatings.GetRatings(null);
                foreach (var userRating in ratings.Values)
                {
                    if (true)
                    {
                        var parameters = new ReadOnlyDictionary<string, object>(
                            new Dictionary<string, object>
                            {
                                { Persistent.ColumnToVariable(IdColumn), movieId },
                                { Persistent.ColumnToVariable(RatingType.IdColumn), userRating.RatingType.Id },
                                { Persistent.ColumnToVariable(User.IdColumn), session.UserId },
                                { Persistent.ColumnToVariable(UserSingleRating.RatingColumn), userRating.ActualRating },
                                { Persistent.ColumnToVariable(UserDerivedRating.DerivedColumn), userRating.DerivedRating }
                            });
                        await Movie.CustomDatabaseActionAsync(parameters, UserSingleRating.UserRatingSaveProcedure, session);
                    }
                }

                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.PersonSearchAsync(session.ToContract(), this.ToContract());
            }
        }

        public static async Task SaveWatchMovieAsync(int movieId, DateTime watchDate, bool dateUncertain, int watchTypeId, UserSession session)
        {
            if (movieId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Movie)} has to be saved.");
            }

            if (watchTypeId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Department)} has to be saved.");
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            
            if (!Persistent.UseService)
            {
                var parameters = new ReadOnlyDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { Persistent.ColumnToVariable(IdColumn), movieId },
                        { Persistent.ColumnToVariable(User.IdColumn), session.UserId },
                        { Persistent.ColumnToVariable(WatchType.IdColumn), watchTypeId },
                        { Persistent.ColumnToVariable(Watch.WatchDateColumn), watchDate },
                        { Persistent.ColumnToVariable(Watch.DateUncertainColumn), dateUncertain }
                    });
                await Movie.CustomDatabaseActionAsync(parameters, Watch.UserWatchSaveProcedure, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.PersonSearchAsync(session.ToContract(), this.ToContract());
            }
        }

        public static async Task DeleteWatchMovieAsync(int watchId, int movieId, int watchTypeId, UserSession session)
        {
            if (watchId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Watch)} has to be saved.");
            }

            if (movieId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Movie)} has to be saved.");
            }

            if (watchTypeId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(WatchType)} has to be saved.");
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (!Persistent.UseService)
            {
                var parameters = new ReadOnlyDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { Persistent.ColumnToVariable(Watch.IdColumn), watchId },
                        { Persistent.ColumnToVariable(IdColumn), movieId },
                        { Persistent.ColumnToVariable(User.IdColumn), session.UserId },
                        { Persistent.ColumnToVariable(WatchType.IdColumn), watchTypeId }
                    });
                await Movie.CustomDatabaseActionAsync(parameters, Watch.UserWatchDeleteProcedure, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.PersonSearchAsync(session.ToContract(), this.ToContract());
            }
        }

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
                UserRatings = this.UserRatings?.ToContract(),
                UserRating = this.UserRating.ToContract(),
                TotalRating = this.TotalRating.ToContract(),
                Characters = this.Characters.ToContract(),
                People = this.People.ToContract(),
                Watches = this.Watches.ToContract(),
                MovieType = this.MovieType.ToContract(),
                Year = this.Year,
                EndYear = this.EndYear,
                RunTime = this.RunTime
            };
        }

        /// <inheritdoc />
        public override MovieDto ToContract(string languageName)
        {
            return new MovieDto
            {
                Id = this.Id,
                ExternalLookups = this.ExternalLookups.ToContract(languageName),
                ExternalRatings = this.ExternalRatings.ToContract(languageName),
                Titles = this.Titles.ToContract(languageName),
                Genres = this.Genres.ToContract(languageName),
                Images = this.Images.ToContract(languageName),
                UserRatings = this.UserRatings?.ToContract(languageName),
                UserRating = this.UserRating.ToContract(languageName),
                TotalRating = this.TotalRating.ToContract(languageName),
                Characters = this.Characters.ToContract(languageName),
                People = this.People.ToContract(languageName),
                Watches = this.Watches.ToContract(languageName),
                MovieType = this.MovieType.ToContract(languageName),
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
                UserRatings = this.UserRatings?.FromContract(contract.UserRatings),
                UserRating = this.UserRating.FromContract(contract.UserRating),
                TotalRating = this.TotalRating.FromContract(contract.TotalRating),
                Characters = this.Characters.FromContract(contract.Characters),
                People = this.People.FromContract(contract.People),
                Watches = this.Watches.FromContract(contract.Watches),
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
                var movies = (await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session)).ToList();
                return movies;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.MovieGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/></exception>
        public override async Task GetUserRatingsAsync(ICollection<MovieDto> items, UserSession session)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            if (!Persistent.UseService)
            {
                await this.GetUserRatingsFromDatabaseAsync(items, items.Select(i => i.Id), this.ReadUserRatingsAsync, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                //return (await service.MovieGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <inheritdoc />
        public override async Task<bool> GetUserItemDetailsAsync(MovieDto item, UserSession session, string languageName)
        {
            if (!Persistent.UseService)
            {
                await this.GetUserDetailsFromDatabaseAsync(item, item.Id, this.ReadUserDetailsAsync, session, languageName);
                return true;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                //return (await service.MovieGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
                return false;
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<IEnumerable<Movie>> SearchAsync(SearchParametersDto parametersDto, UserSession session)
        {
            if (!Persistent.UseService)
            {
                var results = (await this.SearchDatabaseAsync(parametersDto, session)).ToList();
                if (results.Any())
                {
                    return await this.GetAsync(session, results);
                }

                return new List<Movie>();
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return new List<Movie>();
                ////await service.MovieSearchAsync(session.ToContract(), this.ToContract());
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
                var genre = await Genre.Static.NewFromRecordAsync(reader);
                movie.Genres.Add(await GlobalCache.GetGenreAsync(genre.Id));
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
                if (DBNull.Value.Equals(reader[Character.IdColumn]))
                {
                    movie.People.Add(await PersonInRole.Static.NewFromRecordAsync(reader));
                }
                else
                {
                    var character = await PersonAsCharacter.Static.NewFromRecordAsync(reader);
                    movie.Characters.Add(character);
                    movie.People.Add(character.PersonInRole);
                }
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
                new[] { IdColumn, MovieType.IdColumn, YearColumn, EndYearColumn, RunTimeColumn });
            this.Id = (int)record[IdColumn];
            this.MovieType = await GlobalCache.GetMovieTypeAsync((int)record[MovieType.IdColumn]);
            this.Year = (int)record[YearColumn];
            this.EndYear = (int)record[EndYearColumn];
            this.RunTime = (int)record[RunTimeColumn];
            this.TotalRating = await this.TotalRating.NewFromRecordAsync(record);
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override async Task ReadUserRatingsAsync(IEnumerable<MovieDto> items, int userId, DbDataReader reader)
        {
            var ratings = new Dictionary<int, double>();
            while (await reader.ReadAsync())
            {
                ratings.Add((int)reader[IdColumn], (double)reader[UserSingleRating.RatingColumn]);
            }

            foreach (var movie in items)
            {
                double rating;
                if (!ratings.TryGetValue(movie.Id, out rating))
                {
                    rating = 0;
                }

                UserSingleRating.SetUserRating(movie.UserRating, userId, rating);
            }
        }

        /// <inheritdoc cref="ReadUserDetailsAsync" />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        protected async Task ReadUserMovieRatingsDetailsAsync(Movie item, int userId, DbDataReader reader, string languageName)
        {
            item.UserRatings = (await Model.UserDerivedRating.Static.ReadFromRecordsAsync(reader)).First();
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        protected override async Task<bool> ReadUserDetailsAsync(MovieDto item, int userId, DbDataReader reader, string languageName)
        {
            item.UserRatings = (await UserDerivedRating.Static.ReadFromRecordsAsync(reader)).First().ToContract(languageName);
            UserSingleRating.SetUserRating(item.UserRating, userId, item.UserRatings.Value);

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(2, $"{nameof(Movie)}{LanguageTitleCollection.TitlesColumn}");
            }

            while (await reader.ReadAsync())
            {
                Persistent.ValidateRecord(
                    reader,
                    new[] { Character.IdColumn, Person.IdColumn, Role.IdColumn, Department.IdColumn, UserSingleRating.RatingColumn });
                var characterId = (int)reader[Character.IdColumn];
                var personId = (int)reader[Person.IdColumn];
                var roleId = (int)reader[Role.IdColumn];
                var departmentId = (int)reader[Department.IdColumn];
                var character = item.Characters.FirstOrDefault(
                    c => c.Character.Id == characterId
                        && c.PersonInRole.Person.Id == personId
                        && c.PersonInRole.Department.Id == departmentId
                        && c.PersonInRole.Role.Id == roleId);
                if (character != null)
                {
                    character.UserRating = (await this.UserRating.NewFromRecordAsync(reader)).ToContract(languageName);
                    character.Character.UserRating = (await Character.Static.TotalRating.NewFromRecordAsync(reader)).ToContract(languageName);
                }
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(3, $"{nameof(Movie)}{LanguageTitleCollection.TitlesColumn}");
            }

            while (await reader.ReadAsync())
            {
                Persistent.ValidateRecord(
                    reader,
                    new[] { Person.IdColumn, Role.IdColumn, Department.IdColumn });
                var personId = (int)reader[Person.IdColumn];
                var roleId = (int)reader[Role.IdColumn];
                var departmentId = (int)reader[Department.IdColumn];
                var person = item.People.FirstOrDefault(
                    p => p.Person.Id == personId
                        && p.Department.Id == departmentId
                        && p.Role.Id == roleId);
                if (person != null)
                {
                    person.UserRating = (await this.UserRating.NewFromRecordAsync(reader)).ToContract(languageName);
                    person.Person.UserRating = (await Person.Static.TotalRating.NewFromRecordAsync(reader)).ToContract(languageName);
                    var character = item.Characters.FirstOrDefault(
                        c => c.PersonInRole.Person.Id == personId
                            && c.PersonInRole.Department.Id == departmentId
                            && c.PersonInRole.Role.Id == roleId);
                    if (character != null)
                    {
                        character.PersonInRole = person;
                    }
                }
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(4, $"{nameof(Movie)}{WatchCollection<Movie, MovieDto>.WatchesColumn}");
            }

            var watches = new List<WatchDto>();
            while (await reader.ReadAsync())
            {
                watches.Add((await Watch.Static.NewFromRecordAsync(reader)).ToContract(languageName));
            }

            item.Watches = new ReadOnlyCollection<WatchDto>(watches);
            return true;
        }
    }
}