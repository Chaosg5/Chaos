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
        /// <summary>The id of the movie.</summary>
        public int Id { get; private set; }

        /// <summary>The id of the movie in IMDB.</summary>
        public string ImdbId { get; private set; }

        /// <summary>The id of the movie in TMDB.</summary>
        public int TmdbId { get; private set; }

        /// <summary>The list of title of the movie in different languages.</summary>
        public ReadOnlyCollection<MovieTitle> Titles { get; private set; }

        /// <summary>The list of genres that the movie belongs to.</summary>
        public ReadOnlyCollection<Genre> Genres { get; private set; } 
    }
}
