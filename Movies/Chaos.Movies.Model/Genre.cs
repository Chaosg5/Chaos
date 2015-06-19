//-----------------------------------------------------------------------
// <copyright file="Genre.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    /// <summary>A genre of movies.</summary>
    public class Genre
    {
        /// <summary>The id of the genre.</summary>
        public int Id { get; private set; }

        /// <summary>The id of the genre in IMDB.</summary>
        public string ImdbId { get; private set; }

        /// <summary>The id of the genre in TMDB.</summary>
        public int TmdbId { get; private set; }

        /// <summary>The title of the genre.</summary>
        public string Title { get; private set; }
    }
}
