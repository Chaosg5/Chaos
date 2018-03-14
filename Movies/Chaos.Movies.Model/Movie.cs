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
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A movie or a series.</summary>
    public class Movie : Readable<Movie, MovieDto>
    {
        /// <summary>The department for cast members.</summary>
        ////private static Department castDepartment = GlobalCache.GetDepartment("Cast", GlobalCache.DefaultLanguage);

        /// <summary>The role for cast actors.</summary>
        ////private static Role actorRole = GlobalCache.GetRole("Actor", "Cast", GlobalCache.DefaultLanguage);

        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<Genre> genres = new List<Genre>();

        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="Characters"/> property.</summary>
        private PersonAsCharacterCollection<Movie> characters;

        /// <summary>Initializes a new instance of the <see cref="Movie" /> class.</summary>
        public Movie()
        {
            this.characters = new PersonAsCharacterCollection<Movie>();
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
        public ReadOnlyCollection<Genre> Genres => this.genres.AsReadOnly();

        /// <summary>Gets the list of images for this <see cref="Movie"/> and their order.</summary>
        public IconCollection Images { get; } = new IconCollection();

        /// <summary>Gets the total rating score from the current user.</summary>
        public UserRating UserRating { get; } = new UserRating(new RatingType());

        /// <summary>Gets the total rating score from all users.</summary>
        public UserRating TotalUserRating { get; } = new UserRating(new RatingType());

        /// <summary>Gets the list of <see cref="Character"/>s in this <see cref="Movie"/>.</summary>
        public PersonAsCharacterCollection<Movie> Characters
        {
            get
            {
                if (this.characters == null)
                {
                    // ToDo: Obsolete needs a session...
                    ////this.characters = new PersonAsCharacterCollection(new Parent(this));
                    ////this.characters.LoadCharacters();
                }

                return this.characters;
            }
        }

        /// <summary>Gets the list of <see cref="Person"/>s in this <see cref="Movie"/>.</summary>
        public PersonInRoleCollection<Movie> People { get; } = new PersonInRoleCollection<Movie>();

        /// <summary>Gets or sets the type of the movie.</summary>
        public MovieType MovieType { get; set; }
        
        /// <summary>The save all async.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="Id"/> is not a valid id of a <see cref="Movie"/>.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAllAsync()
        {
            await this.People.SaveAsync();
            await this.Characters.SaveAsync();
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
        internal override Task<Movie> ReadFromRecordAsync(IDataRecord record)
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
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Sets the id of the <see cref="Movie"/> this <see cref="PersonAsCharacterCollection{Movie}"/> belongs to.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="PersonAsCharacterCollection{Movie}"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent{Movie}"/> can't be changed once set.</exception>
        /// <exception cref="PersistentObjectRequiredException">The <paramref name="movieId"/> is not valid.</exception>
        private void SetMovieId(int movieId)
        {
            if (movieId <= 0)
            {
                throw new PersistentObjectRequiredException($"The id '{nameof(movieId)}' of the movie has to be greater than zero.");
            }

            if (this.Id != 0)
            {
                throw new ValueLogicalReadOnlyException("The id of the movie can't be changed once set.");
            }

            this.Id = movieId;
            var parent = new Parent<Movie>(this.Id);
            this.Characters.SetParent(parent);
            this.People.SetParent(parent);
        }
    }
}