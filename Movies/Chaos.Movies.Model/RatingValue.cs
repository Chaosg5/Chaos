//-----------------------------------------------------------------------
// <copyright file="RatingValue.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;

    /// <summary>The calculated value of a <see cref="UserRating"/>.</summary>
    public class RatingValue : Communicable<RatingValue, RatingValueDto>
    {
        /// <summary>Initializes a new instance of the <see cref="RatingValue" /> class.</summary>
        /// <param name="value">The value to set for <see cref="Value"/>.</param>
        /// <param name="derived">The value to set for <see cref="Derived"/>.</param>
        public RatingValue(int value, double derived)
        {
            this.Value = value;
            this.Derived = derived;
        }

        /// <summary>Prevents a default instance of the <see cref="RatingValue"/> class from being created.</summary>
        private RatingValue()
        {
        }

        /// <summary>Gets or sets the value of the rating.</summary>
        public int Value { get; set; }

        /// <summary>Gets or sets the derived value from sub ratings.</summary>
        public double Derived { get; set; }

        /// <inheritdoc />
        public override RatingValueDto ToContract()
        {
            return new RatingValueDto { Value = this.Value, Derived = this.Derived };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override RatingValue FromContract(RatingValueDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new RatingValue { Value = contract.Value, Derived = contract.Derived };
        }
    }
}