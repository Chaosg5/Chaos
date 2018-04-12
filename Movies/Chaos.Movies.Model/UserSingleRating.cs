//-----------------------------------------------------------------------
// <copyright file="UserSingleRating.cs">
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

    /// <summary>A simple user rating, similar to <see cref="UserRating"/> but not using the <see cref="RatingType"/>.</summary>
    public class UserSingleRating : Loadable<UserSingleRating, UserSingleRatingDto>
    {
        /// <summary>The database column for <see cref="UserRating"/>.</summary>
        internal const string UserRatingColumn = "UserRating";

        /// <summary>The database column for <see cref="UserId"/>.</summary>
        internal const string UserIdColumn = "UserId";

        /// <summary>The database column for <see cref="TotalRating"/>.</summary>
        private const string TotalRatingColumn = "TotalRating";

        /// <summary>Private part of the <see cref="UserRating"/> property.</summary>
        private int userRating;
        
        /// <summary>Gets a reference to simulate static methods.</summary>
        public static UserSingleRating Static { get; } = new UserSingleRating();

        /// <summary>Gets the total rating from all user's ratings.</summary>
        public double TotalRating { get; private set; }

        /// <summary>Gets the current user's rating of the <see cref="Character"/>.</summary>
        public int UserRating
        {
            get => this.userRating;
            private set
            {
                if (value < 0 || value > 10)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.userRating = value;
            }
        }

        /// <summary>Gets the id <see cref="User"/> which the <see cref="UserRating"/> belongs to.</summary>
        public int UserId { get; private set; }

        /// <inheritdoc />
        public override UserSingleRatingDto ToContract()
        {
            return new UserSingleRatingDto
            {
                TotalRating = this.TotalRating,
                UserId = this.UserId,
                UserRating = this.UserRating
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override UserSingleRating FromContract(UserSingleRatingDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new UserSingleRating
            {
                TotalRating = contract.TotalRating,
                UserId = contract.UserId,
                UserRating = contract.UserRating
            };
        }

        /// <summary>Sets the <see cref="UserRating"/> and <see cref="UserId"/>.</summary>
        /// <param name="userId">The <see cref="UserId"/> to set.</param>
        /// <param name="rating">The <see cref="UserRating"/> to set.</param>
        public void SetUserRating(int userId, int rating)
        {
            this.UserId = userId;
            this.UserRating = rating;
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<UserSingleRating> NewFromRecordAsync(IDataRecord record)
        {
            var result = new UserSingleRating();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { TotalRatingColumn, UserRatingColumn, UserIdColumn });
            this.TotalRating = (double)record[TotalRatingColumn];
            this.UserRating = (int)record[UserRatingColumn];
            this.UserId = (int)record[UserIdColumn];
            return Task.CompletedTask;
        }
    }
}
