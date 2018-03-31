//-----------------------------------------------------------------------
// <copyright file="UserRating.cs">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;

    using Exceptions;

    /// <summary>A rating for a <see cref="Movie"/> set by a <see cref="User"/>.</summary>
    public class UserRating : Loadable<UserRating, UserRatingDto>
    {
        /// <summary>The database column for <see cref="CreatedDate"/>.r</summary>
        internal const string CreatedDateColumn = "CreatedDate";

        /// <summary>The list of sub ratings for this rating.</summary>
        private readonly List<UserRating> subRatings = new List<UserRating>();

        /// <summary>Initializes a new instance of the <see cref="UserRating" /> class.</summary>
        /// <param name="ratingType">The type of the rating.</param>
        public UserRating(RatingType ratingType)
        {
            this.RatingType = ratingType;
        }

        /// <summary>Initializes a new instance of the <see cref="UserRating" /> class.</summary>
        /// <param name="assignedValue">The value to set.</param>
        /// <param name="ratingType">The type of the rating.</param>
        public UserRating(int assignedValue, RatingType ratingType)
        {
            this.RatingValue.Value = assignedValue;
            this.RatingType = ratingType;
        }

        /// <summary>Prevents a default instance of the <see cref="UserRating"/> class from being created.</summary>
        private UserRating()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="UserRating"/> class.</summary>
        /// <param name="subRatings">The <see cref="subRatings"/> to set.</param>
        /// <param name="ratingValue">The <see cref="RatingValue"/> to set.</param>
        private UserRating(List<UserRating> subRatings, RatingValue ratingValue)
        {
            this.subRatings = subRatings;
            this.RatingValue = ratingValue;
            this.ValueChanged = true;
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static UserRating Static { get; } = new UserRating();

        /// <summary>Gets the id of the <see cref="User"/> who owns the rating.</summary>
        public int UserId { get; private set; }

        /// <summary>Gets the type of this rating.</summary>
        public RatingType RatingType { get; private set; }

        /// <summary>Gets the created date.</summary>
        public DateTime CreatedDate { get; private set; } = DateTime.Now;

        /// <summary>Gets the child ratings of this rating.</summary>
        public ReadOnlyCollection<UserRating> SubRatings => this.subRatings.AsReadOnly();

        /// <summary>Gets the values of this rating.</summary>
        public double Value
        {
            get
            {
                if (this.RatingValue.Value < 0 || this.RatingValue.Derived < 0)
                {
                    this.GetRatings(null);
                }

                return this.RatingValue.Value > 0 ? this.RatingValue.Value : this.RatingValue.Derived;
            }
        }

        /// <summary>Gets the actual rating value for this rating, not counting derived values.</summary>
        public int ActualRating => this.RatingValue.Value;

        /// <summary>Gets the display color in RBG hex for this <see cref="UserRating"/>'s <see cref="Value"/>.</summary>
        public string HexColor
        {
            get
            {
                var color = this.Color;
                return string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
            }
        }

        /// <summary>Gets the display color for this <see cref="UserRating"/>'s <see cref="Value"/>.</summary>
        public Color Color
        {
            get
            {
                byte redValue = 255;
                byte greenValue = 0;
                if (this.Value > 1 && this.Value <= 5)
                {
                    greenValue = (byte)((this.Value - 1) * 51);
                }

                if (this.Value > 5 && this.Value < 6)
                {
                    greenValue = (byte)(204 + ((this.Value - 5) * 26));
                }

                if (this.Value >= 6)
                {
                    greenValue = (byte)(230 - ((this.Value - 6) * 25.5));
                }

                if (this.Value > 5)
                {
                    redValue = (byte)(255 - ((this.Value - 5) * 51));
                }

                return Color.FromRgb(redValue, greenValue, 0);
            }
        }

        /// <summary>Gets the display value for this <see cref="UserRating"/>'s <see cref="Value"/>.</summary>
        public string DisplayValue
        {
            get
            {
                var value = Math.Round(this.Value, 1, MidpointRounding.AwayFromZero);
                if (value >= 10)
                {
                    return ((int)value).ToString(CultureInfo.InvariantCulture);
                }

                return value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>Gets or sets the value of the rating.</summary>
        private RatingValue RatingValue { get; set; } = new RatingValue(-1, -1);

        /// <summary>Gets or sets a value indicating whether value has changed since last saved.</summary>
        private bool ValueChanged { get; set; }
        
        /// <summary>Sets the value of this rating.</summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(int value)
        {
            if (this.RatingValue.Value != value)
            {
                this.ValueChanged = true;
            }

            this.RatingValue.Value = value;
        }
        
        /// <summary>Adds a sub rating to this rating.</summary>
        /// <param name="userRating">The sub rating to add.</param>
        public void AddSubRating(UserRating userRating)
        {
            // ToDo: Remove?
            this.subRatings.Add(userRating);
        }

        /// <summary>Gets the list of values for this rating.</summary>
        /// <param name="ratingSystem">The rating value system to calculate values based on.</param>
        /// <returns>The list of set rating values.</returns>
        public Dictionary<RatingType, RatingValue> GetRatings(RatingSystem ratingSystem)
        {
            var allRatings = new Dictionary<RatingType, RatingValue>();
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
            return allRatings;
        }
        
        /// <inheritdoc />
        public override UserRatingDto ToContract()
        {
            return new UserRatingDto
            {
                UserId = this.UserId,
                RatingType = this.RatingType.ToContract(),
                SubRatings = this.subRatings.Select(r => r.ToContract()).ToList().AsReadOnly(),
                Value = this.RatingValue.Value,
                Derived = this.RatingValue.Derived,
                HexColor = this.HexColor,
                Color = this.Color,
                DisplayValue = this.DisplayValue,
                CreatedDate = this.CreatedDate
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override UserRating FromContract(UserRatingDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new UserRating(contract.SubRatings.Select(r => Static.FromContract(r)).ToList(), new RatingValue(contract.Value, -1))
            {
                UserId = contract.UserId,
                RatingType = RatingType.Static.FromContract(contract.RatingType),
                CreatedDate = contract.CreatedDate
            };
        }

        /// <summary>Creates new <see cref="UserRating"/>s from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing data sets and records the data for the <see cref="UserRating"/>s.</param>
        /// <returns>The list of <see cref="UserRating"/>s.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        internal async Task<IEnumerable<UserRating>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var userRatings = new List<UserRating>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, $"{nameof(UserRating)}s");
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
                        ?? new UserRating { UserId = userRating.UserId, RatingType = ratingType, RatingValue = { Value = -1, Derived = -1 } };
                    userRating.subRatings.Add(subRating);
                }
            }

            return userRatings;
        }

        /// <summary>Validates that this <see cref="UserRating"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="UserRating"/> is not valid to be saved.</exception>
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

            if (this.RatingValue.Value < 0)
            {
                this.RatingValue.Value = 0;
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<UserRating> NewFromRecordAsync(IDataRecord record)
        {
            var result = new UserRating();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { User.IdColumn, RatingType.IdColumn, CreatedDateColumn });
            this.UserId = (int)record[User.IdColumn];
            this.RatingType = await GlobalCache.GetRatingTypeAsync((int)record[RatingType.IdColumn]);
            this.RatingValue = await this.RatingValue.NewFromRecordAsync(record);
            this.CreatedDate = (DateTime)record[CreatedDateColumn];
        }

        /// <summary>Calculates the derived value of this <see cref="UserRating"/>.</summary>
        /// <param name="derivedValues">The list of derived sub values.</param>
        /// <param name="ratingSystem">The rating value system to calculate values based on.</param>
        private void CalculateValue(Dictionary<RatingType, double> derivedValues, RatingSystem ratingSystem)
        {
            if (this.RatingValue.Value < 0)
            {
                this.RatingValue.Value = 0;
            }

            if (!derivedValues.Any())
            {
                this.RatingValue.Derived = 0;
                return;
            }

            if (ratingSystem == null)
            {
                this.RatingValue.Derived = derivedValues.Values.Average();
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
                this.RatingValue.Derived = 0;
                return;
            }

            this.RatingValue.Derived = ratingTotal / systemTotal;
        }
    }
}
