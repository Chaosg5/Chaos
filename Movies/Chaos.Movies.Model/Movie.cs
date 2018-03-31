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
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A movie or a series.</summary>
    public class Movie : Readable<Movie, MovieDto>
    {
        /// <summary>Private part of the <see cref="Year"/> property.</summary>
        private int year;

        /// <summary>Initializes a new instance of the <see cref="Movie" /> class.</summary>
        public Movie()
        {
            this.Characters = new PersonAsCharacterCollection<Movie, MovieDto>(this);
            this.People = new PersonInRoleCollection<Movie, MovieDto>(this);
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Movie Static { get; } = new Movie();

        /// <summary>Gets the id of the <see cref="Movie"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookups { get; } = new ExternalLookupCollection();
    
        /// <summary>Gets the ratings of the movie in <see cref="ExternalSource"/>s.</summary>
        public ExternalRatingCollection ExternalRating { get; } = new ExternalRatingCollection();

        /// <summary>Gets the list of title of the movie in different languages.</summary>
        public LanguageTitleCollection Titles { get; } = new LanguageTitleCollection();

        /// <summary>Gets the list of genres that the movie belongs to.</summary>
        public GenreCollection Genres { get; } = new GenreCollection();

        /// <summary>Gets the list of images for this <see cref="Movie"/> and their order.</summary>
        public IconCollection Images { get; } = new IconCollection();

        /// <summary>Gets the total rating score from the current user.</summary>
        public UserRating UserRating { get; } = new UserRating(new RatingType());

        /// <summary>Gets the total rating score from all users.</summary>
        public UserRating TotalUserRating { get; } = new UserRating(new RatingType());

        /// <summary>Gets the list of <see cref="Character"/>s in this <see cref="Movie"/>.</summary>
        public PersonAsCharacterCollection<Movie, MovieDto> Characters { get; }

        /// <summary>Gets the list of <see cref="Person"/>s in this <see cref="Movie"/>.</summary>
        public PersonInRoleCollection<Movie, MovieDto> People { get; }

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

        /// <summary>The save all async.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAllAsync()
        {
        }

        /// <inheritdoc />
        public override MovieDto ToContract()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override Movie FromContract(MovieDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<Movie> GetAsync(UserSession session, int id)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<IEnumerable<Movie>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Task SaveAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        internal override Task<IEnumerable<Movie>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new System.NotImplementedException();
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
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new System.NotImplementedException();
        }
    }
}