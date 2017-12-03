//-----------------------------------------------------------------------
// <copyright file="Rating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media;

    using Exceptions;

    /// <summary>A rating for a <see cref="Movie"/> set by a <see cref="User"/>.</summary>
    public class Rating
    {
        #region Fields

        /// <summary>The set and derived value of the rating.</summary>
        private readonly RatingValue ratingValue = new RatingValue(-1, -1);

        /// <summary>The list of sub ratings for this rating.</summary>
        private readonly List<Rating> subRatings = new List<Rating>();

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Rating" /> class.</summary>
        /// <param name="ratingType">The type of the rating.</param>
        public Rating(RatingType ratingType)
        {
            this.RatingType = ratingType;
        }

        /// <summary>Initializes a new instance of the <see cref="Rating" /> class.</summary>
        /// <param name="assignedValue">The value to set.</param>
        /// <param name="ratingType">The type of the rating.</param>
        public Rating(int assignedValue, RatingType ratingType)
        {
            this.ratingValue.Value = assignedValue;
            this.RatingType = ratingType;
        }

        /// <summary>Initializes a new instance of the <see cref="Rating"/> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="Rating"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public Rating(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        #endregion

        /// <summary>Gets the id of this rating.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the id of the parent <see cref="Rating"/>.</summary>
        public int ParentRatingId { get; private set; }

        /// <summary>Gets the id of the <see cref="User"/> who owns the rating.</summary>
        public int UserId { get; private set; }

        /// <summary>Gets the type of this rating.</summary>
        public RatingType RatingType { get; private set; }

        /// <summary>Gets the child ratings of this rating.</summary>
        public ReadOnlyCollection<Rating> SubRatings => this.subRatings.AsReadOnly();

        /// <summary>Gets the values of this rating.</summary>
        public double Value
        {
            get
            {
                if (this.ratingValue.Value < 0 || this.ratingValue.Derived < 0)
                {
                    this.GetRatings(null);
                }

                return this.ratingValue.Value > 0 ? this.ratingValue.Value : this.ratingValue.Derived;
            }
        }
        
        /// <summary>Gets the display color in RBG hex for this <see cref="Rating"/>'s <see cref="Value"/>.</summary>
        public string HexColor
        {
            get
            {
                var color = this.Color;
                return string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
            }
        }

        /// <summary>Gets the display color for this <see cref="Rating"/>'s <see cref="Value"/>.</summary>
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

        /// <summary>Gets the display value for this <see cref="Rating"/>'s <see cref="Value"/>.</summary>
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

        /// <summary>Sets the value of this rating.</summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(int value)
        {
            this.ratingValue.Value = value;
        }

        /// <summary>Adds a sub rating to this rating.</summary>
        /// <param name="rating">The sub rating to add.</param>
        public void AddSubRating(Rating rating)
        {
            this.subRatings.Add(rating);
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

        /// <summary>Saves this rating to the database.</summary>
        public async void Save()
        {
            if (Persistent.UseService)
            {
                throw new ServiceRequiredException();
            }

            this.ValidateSaveCandidate();
            await SaveToDatabaseAsync(this);
        }

        /// <summary>Saves this rating to the database.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAllAsync()
        {
            if (Persistent.UseService)
            {
                throw new ServiceRequiredException();
            }

            this.ValidateAllSaveCandidates();
            await SaveAllToDatabaseAsync(this);
        }

        #region Private
        
        /// <summary>Validates that this <see cref="Rating"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Rating"/> is not valid to be saved.</exception>
        private void ValidateAllSaveCandidates()
        {
            this.ValidateSaveCandidate();
            foreach (var subtype in this.subRatings)
            {
                subtype.ValidateAllSaveCandidates();
            }
        }

        /// <summary>Validates that this <see cref="Rating"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Rating"/> is not valid to be saved.</exception>
        private void ValidateSaveCandidate()
        {
            if (this.RatingType.Id == 0)
            {
                throw new InvalidSaveCandidateException("The id of the rating's type must be greater than zero.");
            }

            if (this.UserId == 0)
            {
                throw new InvalidSaveCandidateException("The id of the rating's user must be greater than zero.");
            }

            if (this.ratingValue.Value < 0)
            {
                this.ratingValue.Value = 0;
            }
        }

        /// <summary>Updates this <see cref="Rating"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Rating"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "RatingId", "RatingTypeId", "Value", "UserId" });
            this.Id = (int)record["RatingId"];
            this.ratingValue.Value = (int)record["Value"];

            var ratingTypeId = (int)record["RatingTypeId"];
            if (this.RatingType.Id != ratingTypeId)
            {
                this.RatingType = new RatingType(ratingTypeId);
            }

            this.UserId = (int)record["UserId"];
        }

        /// <summary>Saves this rating to the database.</summary>
        /// <param name="rating">The type to save.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task SaveToDatabaseAsync(Rating rating)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("RatingSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RatingId", rating.Id);
                command.Parameters.AddWithValue("@RatingTypeId", rating.RatingType.Id);
                command.Parameters.AddWithValue("@Value", rating.ratingValue.Value);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        rating.ReadFromRecord(reader);
                    }
                }
            }
        }

        /// <summary>Saves this rating to the database.</summary>
        /// <param name="rating">The type to save.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task SaveAllToDatabaseAsync(Rating rating)
        {
            await SaveToDatabaseAsync(rating);
            foreach (var subRating in rating.subRatings)
            {
                subRating.ParentRatingId = rating.Id;
                await SaveAllToDatabaseAsync(subRating);
            }
        }

        /// <summary>Calculates the derived value of this <see cref="Rating"/>.</summary>
        /// <param name="derivedValues">The list of derived sub values.</param>
        /// <param name="ratingSystem">The rating value system to calculate values based on.</param>
        private void CalculateValue(Dictionary<RatingType, double> derivedValues, RatingSystem ratingSystem)
        {
            if (this.ratingValue.Value < 0)
            {
                this.ratingValue.Value = 0;
            }

            if (!derivedValues.Any())
            {
                this.ratingValue.Derived = 0;
                return;
            }

            if (ratingSystem == null)
            {
                this.ratingValue.Derived = derivedValues.Values.Average();
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
                this.ratingValue.Derived = 0;
                return;
            }

            this.ratingValue.Derived = ratingTotal / systemTotal;
        }

        #endregion
    }
}
