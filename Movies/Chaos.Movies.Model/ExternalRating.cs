//-----------------------------------------------------------------------
// <copyright file="ExternalRatings.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    /// <summary>Represents a user.</summary>
    public class ExternalRating
    {
        /// <summary>Gets the <see cref="ExternalSource"/>.</summary>
        public ExternalSource ExternalSource { get; private set; }

        /// <summary>Gets the external rating.</summary>
        public double Rating { get; private set; }

        /// <summary>Gets the count of votes for the rating.</summary>
        public int RatingCount { get; private set; }
    }
}