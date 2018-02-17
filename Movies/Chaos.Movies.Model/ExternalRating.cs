//-----------------------------------------------------------------------
// <copyright file="ExternalRating.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a rating of an item in an <see cref="ExternalSource"/>.</summary>
    public class ExternalRating : Loadable<ExternalRating, ExternalRatingsDto>
    {
        /// <summary>The database column for <see cref="Rating"/>.</summary>
        public const string ExternalRatingColumn = "ExternalRating";

        /// <summary>The database column for <see cref="RatingCount"/>.</summary>
        public const string RatingCountColumn = "RatingCount";

        /// <summary>Private part of the <see cref="ExternalSource"/> property.</summary>
        private ExternalSource externalSource;

        /// <summary>Private part of the <see cref="Rating"/> property.</summary>
        private double rating;

        /// <summary>Private part of the <see cref="RatingCount"/> property.</summary>
        private int ratingCount;

        /// <summary>Initializes a new instance of the <see cref="ExternalRating"/> class.</summary>
        /// <param name="externalSource">The <see cref="ExternalSource"/>.</param>
        /// <param name="rating">The value to set for <see cref="Rating"/>.</param>
        /// <param name="ratingCount">The value to set for <see cref="RatingCount"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="externalSource"/> is <see langword="null"/></exception>
        public ExternalRating(ExternalSource externalSource, double rating, int ratingCount)
        {
            this.ExternalSource = externalSource ?? throw new ArgumentNullException(nameof(externalSource));
            this.Rating = rating;
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
                    throw new ArgumentNullException(nameof(this.ExternalSource));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(this.ExternalSource)} has to be saved.");
                }

                this.externalSource = value;
            }
        }

        /// <summary>Gets the external rating.</summary>
        /// <exception cref="ArgumentOutOfRangeException" accessor="set">The value is not valid.</exception>
        public double Rating
        {
            get => this.rating;

            private set
            {
                if (value < 0 || value > 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.rating = value;
            }
        }

        /// <summary>Gets the count of votes for the rating.</summary>
        /// <exception cref="ArgumentOutOfRangeException" accessor="set">The value is not valid.</exception>
        public int RatingCount
        {
            get => this.ratingCount;

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.ratingCount = value;
            }
        }

        /// <inheritdoc />
        public override ExternalRatingsDto ToContract()
        {
            return new ExternalRatingsDto
            {
                ExternalSource = this.ExternalSource.ToContract(),
                Rating = this.Rating,
                RatingCount = this.RatingCount
            };
        }
        
        /// <inheritdoc />
        public override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override async Task<ExternalRating> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { ExternalSource.ExternalSourceIdColumn, ExternalRatingColumn, RatingCountColumn });
            return new ExternalRating(
                await GlobalCache.GetExternalSourceAsync((int)record[ExternalSource.ExternalSourceIdColumn]),
                (double)record[ExternalRatingColumn],
                (int)record[RatingCountColumn]);
        }
    }
}