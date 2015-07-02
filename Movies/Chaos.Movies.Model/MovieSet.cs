//-----------------------------------------------------------------------
// <copyright file="Collection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>A collection of movies.</summary>
    public class MovieSet
    {
        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<MovieTitle> titles = new List<MovieTitle>();

        /// <summary>Private part of the <see cref="Movies"/> property.</summary>
        private List<Movie> movies = new List<Movie>();

        /// <summary>The list of title of the movie collection in different languages.</summary>
        public ReadOnlyCollection<MovieTitle> Titles
        {
            get { return this.titles.AsReadOnly(); }
        }

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
