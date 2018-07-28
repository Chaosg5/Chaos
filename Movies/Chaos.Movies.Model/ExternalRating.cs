//-----------------------------------------------------------------------
// <copyright file="ExternalRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Contract.Interface;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a rating of an item in an <see cref="ExternalSource"/>.</summary>
    public class ExternalRating : Rating<ExternalRating, ExternalRatingDto>, IRating
    {
        /// <summary>The database column for <see cref="RatingCount"/>.</summary>
        internal const string RatingCountColumn = "RatingCount";

        /// <summary>Private part of the <see cref="ExternalSource"/> property.</summary>
        private ExternalSource externalSource;

        /// <summary>Private part of the <see cref="RatingCount"/> property.</summary>
        private int ratingCount;

        /// <summary>Initializes a new instance of the <see cref="ExternalRating"/> class.</summary>
        /// <param name="externalSource">The <see cref="ExternalSource"/>.</param>
        /// <param name="rating">The value to set for <see cref="IRating.Value"/>.</param>
        /// <param name="ratingCount">The value to set for <see cref="RatingCount"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="externalSource"/> is <see langword="null"/></exception>
        public ExternalRating(ExternalSource externalSource, double rating, int ratingCount)
        {
            this.ExternalSource = externalSource ?? throw new ArgumentNullException(nameof(externalSource));
            this.Value = rating;
            this.RatingCount = ratingCount;
        }

        /// <summary>Prevents a default instance of the <see cref="ExternalRating"/> class from being created.</summary>
        private ExternalRating()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static ExternalRating Static { get; } = new ExternalRating();

        /// <summary>Gets the <see cref="ExternalSource"/>.</summary>
        public ExternalSource ExternalSource
        {
            get => this.externalSource;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(this.ExternalSource)} has to be saved.");
                }

                this.externalSource = value;
            }
        }
        
        /// <summary>Gets the count of votes for the rating.</summary>
        public int RatingCount
        {
            get => this.ratingCount;

            private set
            {
                if (value < 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.ratingCount = value;
            }
        }

        /// <inheritdoc />
        public override ExternalRatingDto ToContract()
        {
            return new ExternalRatingDto
            {
                ExternalSource = this.ExternalSource.ToContract(),
                Value = this.Value,
                DisplayValue = this.DisplayValue,
                HexColor = this.HexColor,
                RatingCount = this.RatingCount
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override ExternalRating FromContract(ExternalRatingDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new ExternalRating
            {
                ExternalSource = this.ExternalSource.FromContract(contract.ExternalSource),
                Value = contract.Value,
                RatingCount = contract.RatingCount
            };
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<ExternalRating> NewFromRecordAsync(IDataRecord record)
        {
            var result = new ExternalRating();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { ExternalSource.IdColumn, RatingColumn, RatingCountColumn });
            this.ExternalSource = await GlobalCache.GetExternalSourceAsync((int)record[ExternalSource.IdColumn]);
            this.Value = (double)record[RatingColumn];
            this.RatingCount = (int)record[RatingCountColumn];
        }
    }
}