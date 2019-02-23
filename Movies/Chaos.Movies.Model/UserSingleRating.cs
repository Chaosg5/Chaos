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
    using Chaos.Movies.Contract.Interface;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <inheritdoc cref="IUserSingleRating" />
    /// <summary>A simple user rating, similar to <see cref="T:Chaos.Movies.Model.UserRating" /> but not using the <see cref="T:Chaos.Movies.Model.RatingType" />.</summary>
    public class UserSingleRating : SingleRating<UserSingleRating, UserSingleRatingDto>, IUserSingleRating
    {
        /// <summary>The database procedure for saving a <see cref="User"/> rating for an item in a <see cref="Movie"/>.</summary>
        internal const string UserRatingSaveProcedure = "UserRatingSave";

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static UserSingleRating Static { get; } = new UserSingleRating();

        /// <inheritdoc />
        public int UserId { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedDate { get; set; }

        /// <summary>Sets the <see cref="IRating.Value"/> and <see cref="UserId"/>.</summary>
        /// <param name="userRating">The <see cref="UserSingleRating"/> to set ratings for.</param>
        /// <param name="userId">The <see cref="UserId"/> to set.</param>
        /// <param name="rating">The <see cref="IRating.Value"/> to set.</param>
        public static void SetUserRating(UserSingleRatingDto userRating, int userId, double rating)
        {
            var validated = Static.FromContract(userRating);
            validated.SetUserRating(userId, rating);
            userRating.UserId = validated.UserId;
            userRating.Value = validated.Value;
            userRating.DisplayValue = validated.DisplayValue;
            userRating.HexColor = validated.HexColor;
        }

        /// <inheritdoc />
        public override UserSingleRatingDto ToContract()
        {
            return new UserSingleRatingDto
            {
                UserId = this.UserId,
                Value = this.Value,
                DisplayValue = this.DisplayValue,
                HexColor = this.HexColor,
                Width = this.Width
            };
        }

        /// <inheritdoc />
        public override UserSingleRatingDto ToContract(string languageName)
        {
            return new UserSingleRatingDto
            {
                UserId = this.UserId,
                Value = this.Value,
                DisplayValue = this.DisplayValue,
                HexColor = this.HexColor,
                Width = this.Width
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
                UserId = contract.UserId,
                Value = contract.Value
            };
        }

        /// <summary>Sets the <see cref="IRating.Value"/> and <see cref="UserId"/>.</summary>
        /// <param name="userId">The <see cref="UserId"/> to set.</param>
        /// <param name="rating">The <see cref="IRating.Value"/> to set.</param>
        public void SetUserRating(int userId, double rating)
        {
            this.UserId = userId;
            this.Value = rating;
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

        /// <inheritdoc cref="NewFromRecordAsync(IDataRecord)" />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal Task<UserSingleRating> NewFromRecordAsync(IDataRecord record, string ratingColumn)
        {
            var result = new UserSingleRating();
            Persistent.ValidateRecord(record, new[] { ratingColumn, User.IdColumn });
            this.Value = (int)record[ratingColumn];
            this.UserId = (int)record[User.IdColumn];
            return Task.FromResult(result);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { RatingColumn, User.IdColumn });
            this.Value = (byte)record[RatingColumn];
            this.UserId = (int)record[User.IdColumn];
            return Task.CompletedTask;
        }
    }
}
