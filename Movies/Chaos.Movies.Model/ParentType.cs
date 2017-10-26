//-----------------------------------------------------------------------
// <copyright file="ParentType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    /// <summary>A genre of <see cref="Movie"/>s.</summary>
    public enum ParentType
    {
        /// <summary>The parent is a <see cref="Model.Movie"/>.</summary>
        Movie,

        /// <summary>The parent is a <see cref="Model.MovieSeries"/>.</summary>
        MovieSeries,

        /// <summary>The parent is an episode.</summary>
        Episode
    }
}
