//-----------------------------------------------------------------------
// <copyright file="RatingValue.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The calculated value of a <see cref="UserRating"/>.</summary>
    public class RatingValue : Loadable<RatingValue, RatingValueDto>
    {
        /// <summary>The database column for <see cref="Value"/>.</summary>
        internal const string RatingColumn = "Rating";

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
        public int Value { get; set; } = -1;

        /// <summary>Gets or sets the derived value from sub ratings.</summary>
        public double Derived { get; set; } = -1;

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

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<RatingValue> NewFromRecordAsync(IDataRecord record)
        {
            var result = new RatingValue();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { RatingColumn });
            this.Value = (int)record[RatingColumn];
            return Task.CompletedTask;
        }
    }
}