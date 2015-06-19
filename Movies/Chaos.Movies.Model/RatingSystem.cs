//-----------------------------------------------------------------------
// <copyright file="RatingSystem.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace Chaos.Movies.Model
{
    /// <summary>A system for giving different <see cref="RatingType"/>s different values when calculating a rating for a <see cref="Movie"/>.</summary>
    public class RatingSystem
    {
        /// <summary>The id of this rating system.</summary>
        public int Id { get; private set; }

        /// <summary>The name of this rating system.</summary>
        public string Name { get; private set; }

        /// <summary>The description of this rating system.</summary>
        public string Decription { get; private set; }

        /// <summary>Contains the relative value for each <see cref="RatingType"/>.</summary>
        public Dictionary<RatingType, short> Values { get; private set; }
    }
}