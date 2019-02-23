//-----------------------------------------------------------------------
// <copyright file="UserDerivedRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Contract.Interface;
    using Chaos.Movies.Model.Base;

    using Exceptions;

    /// <inheritdoc cref="IDerivedRating" />
    /// <summary>A rating for a <see cref="Movie"/> set by a <see cref="User"/>.</summary>
    public class UserDerivedRating : DerivedRating<UserDerivedRating, UserDerivedRatingDto>, IDerivedRating
    {
        /// <summary>The list of sub ratings for this rating.</summary>
        private readonly List<UserDerivedRating> subRatings = new List<UserDerivedRating>();

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Chaos.Movies.Model.UserRating" /> class.</summary>
        /// <param name="ratingType">The type of the rating.</param>
        public UserDerivedRating(RatingType ratingType)
        {
            this.RatingType = ratingType;
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="UserDerivedRating" /> class.</summary>
        /// <param name="assignedValue">The value to set.</param>
        /// <param name="ratingType">The type of the rating.</param>
        public UserDerivedRating(int assignedValue, RatingType ratingType)
        {
            base.Value = assignedValue;
            this.ActualRating = assignedValue;
            this.RatingType = ratingType;
        }

        /// <inheritdoc />
        /// <summary>Prevents a default instance of the <see cref="UserDerivedRating"/> class from being created.</summary>
        private UserDerivedRating()
        {
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="UserDerivedRating"/> class.</summary>
        /// <param name="subRatings">The <see cref="subRatings"/> to set.</param>
        /// <param name="ratingValue">The <see cref="IRating.Value"/> to set.</param>
        /// <param name="derivedValue">The <see cref="IDerivedRating.Derived"/> to set.</param>
        private UserDerivedRating(List<UserDerivedRating> subRatings, int ratingValue, double derivedValue)
        {
            this.subRatings = subRatings;
            base.Value = ratingValue;
            this.ActualRating = ratingValue;
            this.Derived = derivedValue;
            this.ValueChanged = true;
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static UserDerivedRating Static { get; } = new UserDerivedRating();

        /// <summary>Gets the id of the <see cref="User"/> who owns the rating.</summary>
        public int UserId { get; private set; }

        /// <summary>Gets the type of this rating.</summary>
        public RatingType RatingType { get; private set; }

        /// <summary>Gets the created date.</summary>
        public DateTime CreatedDate { get; private set; } = DateTime.Now;

        /// <summary>Gets the child ratings of this rating.</summary>
        public ReadOnlyCollection<UserDerivedRating> SubRatings => this.subRatings.AsReadOnly();

        /// <inheritdoc />
        public new double Value
        {
            get
            {
                if (base.Value < 0 || this.Derived < 0)
                {
                    this.GetRatings(null);
                }

                return base.Value > 0 ? base.Value : this.Derived;
            }
        }

        /// <summary>Gets the actual rating value for this rating, not counting derived values.</summary>
        public int ActualRating { get; private set; }

        /// <summary>Gets the actual derived value for this rating.</summary>
        public double DerivedRating => this.Derived;

        /// <summary>Gets a value indicating whether value has changed since last saved.</summary>
        public bool ValueChanged { get; private set; }

        /// <summary>Sets the value of this rating.</summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(int value)
        {
            if (this.Value != value)
            {
                this.ValueChanged = true;
            }

            base.Value = value;
            this.ActualRating = value;
        }

        /// <summary>Sets the value of this rating.</summary>
        /// <param name="value">The value to set.</param>
        /// <param name="ratingType">The rating Type.</param>
        /// <returns>Always returns <see langword="true"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ratingType"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="ratingType"/> does not exist.</exception>
        public bool SetValue(int value, RatingType ratingType)
        {
            if (ratingType == null)
            {
                throw new ArgumentNullException(nameof(ratingType));
            }

            if (this.RatingType.Id == ratingType.Id)
            {
                if (this.Value != value)
                {
                    base.Value = value;
                    this.ActualRating = value;
                    this.ValueChanged = true;
                }

                return true;
            }

            foreach (var subRating in this.SubRatings)
            {
                if (subRating.SetValue(value, ratingType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Adds a sub rating to this rating.</summary>
        /// <param name="userRating">The sub rating to add.</param>
        public void AddSubRating(UserDerivedRating userRating)
        {
            // ToDo: Remove?
            this.subRatings.Add(userRating);
        }

        /// <summary>Gets the list of values for this rating.</summary>
        /// <param name="ratingSystem">The rating value system to calculate values based on.</param>
        /// <returns>The list of set rating values.</returns>
        public Dictionary<RatingType, UserDerivedRating> GetRatings(RatingSystem ratingSystem)
        {
            var allRatings = new Dictionary<RatingType, UserDerivedRating>();
            var derivedValues = new Dictionary<RatingType, double>();
            foreach (var childRating in this.subRatings)
            {
                foreach (var rating in childRating.GetRatings(ratingSystem))
                {
                    allRatings.Add(rating.Key, rating.Value);
                }

                derivedValues.Add(childRating.RatingType, childRating.Value);
            }

            this.CalculateValue(derivedValues, ratingSystem);
            allRatings.Add(this.RatingType, this);
            return allRatings;
        }
        
        /// <inheritdoc />
        public override UserDerivedRatingDto ToContract()
        {
            return new UserDerivedRatingDto
            {
                UserId = this.UserId,
                RatingType = this.RatingType.ToContract(),
                SubRatings = this.subRatings.Select(r => r.ToContract()).ToList().AsReadOnly(),
                Value = this.Value,
                Derived = this.Derived,
                HexColor = this.HexColor,
                Width = this.Width,
                DisplayValue = this.DisplayValue,
                CreatedDate = this.CreatedDate,
                ActualRating = this.ActualRating
            };
        }

        /// <inheritdoc />
        public override UserDerivedRatingDto ToContract(string languageName)
        {
            return new UserDerivedRatingDto
            {
                UserId = this.UserId,
                RatingType = this.RatingType.ToContract(languageName),
                SubRatings = this.subRatings.Select(r => r.ToContract(languageName)).ToList().AsReadOnly(),
                Value = this.Value,
                Derived = this.Derived,
                Width = this.Width,
                HexColor = this.HexColor,
                DisplayValue = this.DisplayValue,
                CreatedDate = this.CreatedDate,
                ActualRating = this.ActualRating
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override UserDerivedRating FromContract(UserDerivedRatingDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new UserDerivedRating(contract.SubRatings.Select(r => Static.FromContract(r)).ToList(), (int)contract.Value, contract.Derived)
            {
                UserId = contract.UserId,
                RatingType = RatingType.Static.FromContract(contract.RatingType),
                CreatedDate = contract.CreatedDate
            };
        }

        /// <summary>Creates new <see cref="UserDerivedRating"/>s from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing data sets and records the data for the <see cref="UserDerivedRating"/>s.</param>
        /// <returns>The list of <see cref="UserDerivedRating"/>s.</returns>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        internal async Task<IEnumerable<UserDerivedRating>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var userRatings = new List<UserDerivedRating>();
            if (!reader.HasRows)
            {
                return userRatings;
            }

            while (await reader.ReadAsync())
            {
                userRatings.Add(await this.NewFromRecordAsync(reader));
            }

            foreach (var userRating in userRatings)
            {
                foreach (var ratingType in userRating.RatingType.Subtypes)
                {
                    var subRating = userRatings.FirstOrDefault(r => r.RatingType.Id == ratingType.Id)
                        ?? new UserDerivedRating { UserId = userRating.UserId, RatingType = ratingType };
                    userRating.subRatings.Add(subRating);
                }
            }

            return userRatings.Where(r => r.RatingType.ParentRatingTypeId == 0);
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="UserDerivedRating"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (this.RatingType.Id == 0)
            {
                throw new InvalidSaveCandidateException("The id of the rating's type must be greater than zero.");
            }

            if (this.UserId == 0)
            {
                throw new InvalidSaveCandidateException("The id of the rating's user must be greater than zero.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<UserDerivedRating> NewFromRecordAsync(IDataRecord record)
        {
            var result = new UserDerivedRating();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { User.IdColumn, RatingType.IdColumn, RatingColumn, DerivedColumn, CreatedDateColumn });
            this.UserId = (int)record[User.IdColumn];
            this.RatingType = await GlobalCache.GetRatingTypeAsync((int)record[RatingType.IdColumn]);
            base.Value = (byte)record[RatingColumn];
            this.ActualRating = (byte)record[RatingColumn];
            this.Derived = (double)record[DerivedColumn];
            base.Value = this.Value;
            this.CreatedDate = (DateTime)record[CreatedDateColumn];
        }

        /// <summary>Calculates the derived value of this <see cref="UserDerivedRating"/>.</summary>
        /// <param name="derivedValues">The list of derived sub values.</param>
        /// <param name="ratingSystem">The rating value system to calculate values based on.</param>
        private void CalculateValue(Dictionary<RatingType, double> derivedValues, RatingSystem ratingSystem)
        {
            if (!derivedValues.Any())
            {
                this.Derived = 0;
                return;
            }

            if (ratingSystem == null)
            {
                this.Derived = derivedValues.Values.Average();
                return;
            }

            double ratingTotal = 0;
            double systemTotal = 0;
            foreach (var systemValue in ratingSystem.Values)
            {
                foreach (var childValue in derivedValues)
                {
                    if (childValue.Key.Id != systemValue.Key.Id || childValue.Value <= 0)
                    {
                        continue;
                    }

                    ratingTotal += systemValue.Value * childValue.Value;
                    systemTotal += systemValue.Value;
                }
            }

            if (!(ratingTotal > 0 && systemTotal > 0))
            {
                this.Derived = 0;
                return;
            }

            this.Derived = ratingTotal / systemTotal;
        }
    }
}
