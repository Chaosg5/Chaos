//-----------------------------------------------------------------------
// <copyright file="Rating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Chaos.Movies.Model
{
    /// <summary>A rating for a <see cref="Movie"/> set by a <see cref="User"/>.</summary>
    public class Rating
    {
        private short value = 0;

        private decimal derivedValue = -1;

        /// <summary>The id of this rating.</summary>
        public int Id { get; private set; }

        public RatingType RatingType { get; private set; }

        public List<Rating> SubRatings { get; private set; }

        /// <summary>Gets the values of this rating.</summary>
        public decimal Value
        {
            get
            {
                if (this.derivedValue == -1)
                {
                    this.GetRatings(null);
                }

                return this.derivedValue;
            }
        }

        /// <summary>Gets the list of values for this rating.</summary>
        /// <returns>The list of set rating values.</returns>
        public Dictionary<RatingType, RatingValue> GetRatings(RatingSystem ratingSystem)
        {
            var allRatings = new Dictionary<RatingType, RatingValue>();
            var derivedValues = new Dictionary<RatingType, decimal>();
            foreach (var childRating in this.SubRatings)
            {
                allRatings.Add(childRating.GetRatings(ratingSystem));
                derivedValues.Add(childRating.RatingType, childRating.value);
            }


        }

        private RatingValue CalculateValue(Dictionary<RatingType, decimal> derivedValues, RatingSystem ratingSystem)
        {
            if (this.value > 0)
            {
                return new RatingValue(this.value, false);
            }

            if (derivedValues.Count() == 0)
            {
                return new RatingValue(0, true);
            }

            if (ratingSystem == null)
            {
                return new RatingValue(derivedValues.Values.Average(), true);
            }

            foreach (var ratingType in ratingSystem.Values)
            {
                foreach (var t in derivedValues)
                {
                    if (t.Key.Id != ratingType.Key.Id)
                    {
                        continue;
                    }
                }

                string s; 
                s = "string";

            }

        }
    }
}
