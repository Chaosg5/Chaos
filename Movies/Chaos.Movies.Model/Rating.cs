//-----------------------------------------------------------------------
// <copyright file="Rating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>A rating for a <see cref="Movie"/> set by a <see cref="User"/>.</summary>
    public class Rating
    {
        #region Fields

        /// <summary>The set value of the rating.</summary>
        private double assignedValue = -1;

        /// <summary>The set and derived value of the rating.</summary>
        private RatingValue ratingValue;

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
            this.assignedValue = assignedValue;
            this.RatingType = ratingType;
        }

        #endregion

        #region Properties

        /// <summary>Gets the id of this rating.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the type of this rating.</summary>
        public RatingType RatingType { get; private set; }

        /// <summary>Gets the child ratings of this rating.</summary>
        public ReadOnlyCollection<Rating> SubRatings
        {
            get { return this.subRatings.AsReadOnly(); }
        }

        /// <summary>Gets the values of this rating.</summary>
        public double Value
        {
            get
            {
                if (this.ratingValue == null)
                {
                    this.GetRatings(null);
                }

                return this.ratingValue.Value > 0 ? this.ratingValue.Value : this.ratingValue.Derived;
            }
        }

        #endregion

        #region Methods

        #region Public

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

            this.ratingValue = this.CalculateValue(derivedValues, ratingSystem);
            return allRatings;
        }

        #endregion

        /// <summary>Calculates the derived value of this <see cref="Rating"/>.</summary>
        /// <param name="derivedValues">The list of derived sub values.</param>
        /// <param name="ratingSystem">The rating value system to calculate values based on.</param>
        /// <returns>The set value and derived value.</returns>
        private RatingValue CalculateValue(Dictionary<RatingType, double> derivedValues, RatingSystem ratingSystem)
        {
            if (this.assignedValue < 0)
            {
                this.assignedValue = 0;
            }

            if (!derivedValues.Any())
            {
                return new RatingValue(this.assignedValue, 0);
            }

            if (ratingSystem == null)
            {
                return new RatingValue(this.assignedValue, derivedValues.Values.Average());
            }

            double ratingTotal = 0;
            double systemTotal = 0;
            foreach (var systemValue in ratingSystem.Values)
            {
                // ReSharper disable once LoopCanBePartlyConvertedToQuery - http://stackoverflow.com/questions/15837313/foreach-variable-in-closure-why-results-differ-for-these-snippets
                foreach (var t in derivedValues)
                {
                    if (t.Key.Id != systemValue.Key.Id)
                    {
                        continue;
                    }

                    ratingTotal += systemValue.Value * t.Value;
                    systemTotal += systemValue.Value;
                }
            }

            if (!(ratingTotal > 0 && systemTotal > 0))
            {
                return new RatingValue(this.assignedValue, 0);
            }

            return new RatingValue(this.assignedValue, ratingTotal / systemTotal);
        }

        #endregion
    }
}
