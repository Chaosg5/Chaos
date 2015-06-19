//-----------------------------------------------------------------------
// <copyright file="RatingType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace Chaos.Movies.Model
{
    /// <summary>A sub rating with a defined type.</summary>
    public class RatingType
    {
        /// <summary>The id of this rating type.</summary>
        public int Id { get; private set; }

        /// <summary>The name of this rating type.</summary>
        public string Name { get; private set; }

        /// <summary>The description of this rating type.</summary>
        public string Decription { get; private set; }

        /// <summary>The <see cref="RatingType"/>s that makes up the derived children of this <see cref="RatingType"/>.</summary>
        public List<RatingType> SubTypes { get; private set; }
    }
}
