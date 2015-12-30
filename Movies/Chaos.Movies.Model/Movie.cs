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
        /// <summary>Private part of the <see cref="Id"/> property.</summary>
        private int id;

        /// <summary>The department for cast members.</summary>
        private static Department CastDepartment = GlobalCache.GetDepartment("Cast", GlobalCache.DefaultLanguage);

        /// <summary>The role for cast actors.</summary>
        private static Role ActorRole = GlobalCache.GetRole("Actor", "Cast", GlobalCache.DefaultLanguage);

        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<Genre> genres = new List<Genre>();

        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="UserRating"/> property.</summary>
        private readonly Rating userRating = new Rating(new RatingType(1));

        /// <summary>Private part of the <see cref="TotalRating"/> property.</summary>
        private readonly Rating totalRating = new Rating(new RatingType(1));

        /// <summary>Initializes a new instance of the <see cref="Movie" /> class.</summary>
        public Movie()
        {
        }

        /// <summary>Gets the id of the movie.</summary>
        public int Id
        {
            get { return this.id; }

            private set
            {
                this.id = value;
                this.Characters = new CharactersInMovie(this.Id);
            }
        }

        /// <summary>The id of the movie in IMDB.</summary>
        public string ImdbId { get; private set; }

        /// <summary>The id of the movie in TMDB.</summary>
        public int TmdbId { get; private set; }

        /// <summary>The list of title of the movie in different languages.</summary>
        public LanguageTitles Titles { get; private set; } = new LanguageTitles();

        /// <summary>The list of genres that the movie belongs to.</summary>
        public ReadOnlyCollection<Genre> Genres => this.genres.AsReadOnly();

        /// <summary>The list of images for the movie and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images => this.images.AsReadOnly();

        /// <summary>The total rating score from the current user.</summary>
        public Rating UserRating => this.userRating;

        /// <summary>The total rating score from all users.</summary>
        public Rating TotalRating => this.totalRating;
        
        public CharactersInMovie Characters { get; private set; }

        public void SavePeople()
        {
            if (this.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The movie needs to be saved before saving people.");
            }
        }

        public void SaveCharacters()
        {
            if (this.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The movie needs to be saved before saving characters.");
            }
        }
    }
}