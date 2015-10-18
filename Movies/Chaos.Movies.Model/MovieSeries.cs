//-----------------------------------------------------------------------
// <copyright file="MovieSeries.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>A serie of movies.</summary>
    public class MovieSeries
    {
        /// <summary>Private part of the <see cref="Movies"/> property.</summary>
        private List<Movie> movies = new List<Movie>();

        /// <summary>Initializes a new instance of the <see cref="MovieSeries" /> class.</summary>
        public MovieSeries()
        {
            this.Titles = new LanguageTitles();
        }

        /// <summary>The id of the movie series.</summary>
        public int Id { get; private set; }

        /// <summary>The type of the movie series.</summary>
        public MovieSeriesType MovieSeriesType { get; private set; }

        /// <summary>The list of title of the movie collection in different languages.</summary>
        public LanguageTitles Titles { get; private set; }

        /// <summary>The movies which are a part of this collection with the keys representing their order.</summary>
        public ReadOnlyCollection<Movie> Movies
        {
            get { return this.movies.AsReadOnly(); }
        }

        #region Methods

        #region Public

        /// <summary>Sets the order of the movies in this collection.</summary>
        /// <param name="newOrder">The order to set based on the indexes of the old order.</param>
        public void ReorderMovies(ICollection<int> newOrder)
        {
             this.movies = Helper.ReorderList(this.movies, newOrder).ToList();
        }

        #endregion

        #region Private

        #endregion

        #endregion
    }
}
