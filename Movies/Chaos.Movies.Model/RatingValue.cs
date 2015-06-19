//-----------------------------------------------------------------------
// <copyright file="RatingValue.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    /// <summary>The calculated value of a <see cref="Rating"/>.</summary>
    public class RatingValue
    {
        /// <summary>Initializes a new instance of the <see cref="RatingValue" /> class.</summary>
        /// <param name="value">The value to set for <see cref="Value"/>.</param>
        /// <param name="derived">The value to set for <see cref="Derived"/>.</param>
        public RatingValue(double value, bool derived)
        {
            this.Value = value;
            this.Derived = derived;
        }

        /// <summary>Gets the value of the rating.</summary>
        public double Value { get; private set; }

        /// <summary>Gets a value indicating whether the <see cref="Value"/> is set to a fixed value or derived from sub ratings.</summary>
        public bool Derived { get; private set; }
    }
}