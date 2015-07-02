//-----------------------------------------------------------------------
// <copyright file="RatingSystem.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>A system for giving different <see cref="RatingType"/>s different values when calculating a rating for a <see cref="Movie"/>.</summary>
    public class RatingSystem
    {
        /// <summary>Private part of the <see cref="Values"/> property.</summary>
        private readonly Dictionary<RatingType, short> values = new Dictionary<RatingType, short>();

        /// <summary>Initializes a new instance of the <see cref="RatingSystem" /> class.</summary>
        public RatingSystem()
        {

        }

        /// <summary>The id of this rating system.</summary>
        public int Id { get; private set; }

        /// <summary>The name of this rating system.</summary>
        public string Name { get; private set; }

        /// <summary>The description of this rating system.</summary>
        public string Description { get; private set; }

        /// <summary>Contains the relative value for each <see cref="RatingType"/>.</summary>
        public ReadOnlyDictionary<RatingType, short> Values
        {
            get { return new ReadOnlyDictionary<RatingType, short>(values); }
        }

        /// <summary>Sets the value for the specified type.</summary>
        /// <param name="ratingType">The type to set the value for.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(RatingType ratingType, short value)
        {
            if (this.values.ContainsKey(ratingType))
            {
                this.values[ratingType] = value;
            }
            else
            {
                this.values.Add(ratingType, value);
            }
        }
    }
}