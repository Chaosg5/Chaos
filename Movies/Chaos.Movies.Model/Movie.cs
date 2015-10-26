//-----------------------------------------------------------------------
// <copyright file="Movie.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>A movie or a series.</summary>
    public class Movie
    {
        #region Fields

        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<Genre> genres = new List<Genre>();

        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="UserRating"/> property.</summary>
        private readonly Rating userRating = new Rating(new RatingType(1));

        /// <summary>Private part of the <see cref="TotalRating"/> property.</summary>
        private readonly Rating totalRating = new Rating(new RatingType(1));

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Movie" /> class.</summary>
        public Movie()
        {
            this.Titles = new LanguageTitles();
        }

        #endregion

        #region Properties

        /// <summary>The id of the movie.</summary>
        public int Id { get; private set; }

        /// <summary>The id of the movie in IMDB.</summary>
        public string ImdbId { get; private set; }

        /// <summary>The id of the movie in TMDB.</summary>
        public int TmdbId { get; private set; }

        /// <summary>The list of title of the movie in different languages.</summary>
        public LanguageTitles Titles { get; private set; }

        /// <summary>The list of genres that the movie belongs to.</summary>
        public ReadOnlyCollection<Genre> Genres
        {
            get { return this.genres.AsReadOnly(); }
        }

        /// <summary>The list of images for the movie and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images
        {
            get { return this.images.AsReadOnly(); }
        }

        /// <summary>The total rating score from the current user.</summary>
        public Rating UserRating
        {
            get { return this.userRating; }
        }

        /// <summary>The total rating score from all users.</summary>
        public Rating TotalRating
        {
            get { return this.totalRating; }
        }

        #endregion

        #region Methods

        #region Private


        #endregion

        #endregion
    }
}
