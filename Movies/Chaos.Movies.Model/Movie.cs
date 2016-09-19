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
    using System.Data.SqlClient;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A movie or a series.</summary>
    public class Movie
    {
        /// <summary>The department for cast members.</summary>
        private static Department castDepartment = GlobalCache.GetDepartment("Cast", GlobalCache.DefaultLanguage);

        /// <summary>The role for cast actors.</summary>
        private static Role actorRole = GlobalCache.GetRole("Actor", "Cast", GlobalCache.DefaultLanguage);

        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<Genre> genres = new List<Genre>();

        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="UserRating"/> property.</summary>
        private readonly Rating userRating = new Rating(new RatingType(1));

        /// <summary>Private part of the <see cref="TotalRating"/> property.</summary>
        private readonly Rating totalRating = new Rating(new RatingType(1));

        /// <summary>Private part of the <see cref="Characters"/> property.</summary>
        private readonly CharactersInMovieCollection characters = new CharactersInMovieCollection();

        /// <summary>Private part of the <see cref="People"/> property.</summary>
        private readonly PeopleInMovieCollection people = new PeopleInMovieCollection();

        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private LanguageTitles titles = new LanguageTitles();

        /// <summary>Initializes a new instance of the <see cref="Movie" /> class.</summary>
        public Movie()
        {
        }

        /// <summary>Gets the id of the movie.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the id of the movie in IMDB.</summary>
        public string ImdbId { get; private set; }

        /// <summary>Gets the id of the movie in TMDB.</summary>
        public int TmdbId { get; private set; }

        /// <summary>Gets the list of title of the movie in different languages.</summary>
        public LanguageTitles Titles
        {
            get { return this.titles; }
            private set { this.titles = value; }
        }

        /// <summary>Gets the list of genres that the movie belongs to.</summary>
        public ReadOnlyCollection<Genre> Genres
        {
            get { return this.genres.AsReadOnly(); }
        }

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images
        {
            get { return this.images.AsReadOnly(); }
        }

        /// <summary>Gets the total rating score from the current user.</summary>
        public Rating UserRating
        {
            get { return this.userRating; }
        }

        /// <summary>Gets the total rating score from all users.</summary>
        public Rating TotalRating
        {
            get { return this.totalRating; }
        }

        /// <summary>Gets the list of <see cref="Character"/>s in this <see cref="Movie"/>.</summary>
        public CharactersInMovieCollection Characters
        {
            get
            {
                if (this.characters.Count == 0)
                {
                    this.characters.LoadCharacters();
                }

                return this.characters;
            }
        }

        /// <summary>Gets the list of <see cref="Person"/>s in this <see cref="Movie"/>.</summary>
        public PeopleInMovieCollection People
        {
            get
            {
                return this.people;
            }
        }
        
        /// <summary>Gets or sets the type of the movie.</summary>
        public MovieType MovieType { get; set; }

        /// <exception cref="PersistentObjectRequiredException">If the <see cref="Id"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void SaveAll()
        {
            this.People.Save();
            this.Characters.Save();
        }
        
        /// <summary>Sets the id of the <see cref="Movie"/> this <see cref="CharactersInMovieCollection"/> belongs to.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="CharactersInMovieCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The id of the <see cref="Movie"/> can't be changed once set.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="movieId"/> is not valid.</exception>
        private void SetMovieId(int movieId)
        {
            if (movieId <= 0)
            {
                throw new ArgumentOutOfRangeException("movieId");
            }

            if (this.Id != 0)
            {
                throw new ValueLogicalReadOnlyException("The id of the movie can't be changed once set.");
            }

            this.Id = movieId;
            this.Characters.SetMovieId(this.Id);
            this.People.SetMovieId(this.Id);
        }
    }
}