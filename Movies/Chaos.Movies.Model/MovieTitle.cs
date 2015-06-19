//-----------------------------------------------------------------------
// <copyright file="MovieTitles.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Globalization;

    /// <summary>The title of a movie.</summary>
    public class MovieTitle
    {
        /// <summary>The title.</summary>
        public string Title { get; private set; }

        /// <summary>The language of the title.</summary>
        public CultureInfo Language { get; private set; }

    }
}