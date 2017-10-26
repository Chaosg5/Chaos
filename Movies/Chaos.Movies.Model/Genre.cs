//-----------------------------------------------------------------------
// <copyright file="Genre.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    /// <summary>A genre of <see cref="Movie"/>s.</summary>
    public class Genre
    {
        /// <summary>Gets the id of the genre.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the id of the <see cref="Genre"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookup { get; } = new ExternalLookupCollection();

        /// <summary>Gets the title of the genre.</summary>
        public string Title { get; private set; }
    }
}
