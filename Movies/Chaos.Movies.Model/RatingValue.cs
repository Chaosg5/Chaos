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
        public RatingValue(int value, double derived)
        {
            this.Value = value;
            this.Derived = derived;
        }

        /// <summary>Gets or sets the value of the rating.</summary>
        public int Value { get; set; }

        /// <summary>Gets or sets the derived value from sub ratings.</summary>
        public double Derived { get; set; }
    }
}