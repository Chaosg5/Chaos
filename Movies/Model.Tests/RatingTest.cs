//-----------------------------------------------------------------------
// <copyright file="RatingTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public static class RatingTest
    {
        [TestCase("2:25;3:50;4:25;5:75;6:25", "2:4;3:3-5:4-6:5;4:3", 3.5)]
        public static void TestRatingCalculate(string systemValue, string ratingValue, double expectedResult)
        {
            var ratingSystem = GetRatingSystem(systemValue);
            var rating = GetRatings(ratingValue);
            rating.GetRatings(ratingSystem);
            Assert.AreEqual(expectedResult, rating.Value);
        }

        private static Rating GetRatings(string ratingString)
        {
            var rootRating = new Rating(new RatingType(1));
            var ratings = ratingString.Split(new[] { ";" }, StringSplitOptions.None);
            foreach (var groupValues in ratings)
            {
                var subRatings = groupValues.Split(new[] { "-" }, StringSplitOptions.None);
                var parentValues = subRatings[0].Split(new[] { ":" }, StringSplitOptions.None);
                var parentRating = new Rating(int.Parse(parentValues[1], CultureInfo.InvariantCulture), new RatingType(int.Parse(parentValues[0], CultureInfo.InvariantCulture)));
                for (var i = 1; i < subRatings.Length; i++)
                {
                    var values = subRatings[i].Split(new[] {":"}, StringSplitOptions.None);
                    parentRating.AddSubRating(new Rating(int.Parse(values[1], CultureInfo.InvariantCulture), new RatingType(int.Parse(values[0], CultureInfo.InvariantCulture))));
                }

                rootRating.AddSubRating(parentRating);
            }

            return rootRating;
        }

        private static RatingSystem GetRatingSystem(string systemString)
        {
            var system = new RatingSystem();
            var types = systemString.Split(new[] { ";" }, StringSplitOptions.None);
            foreach (var values in types.Select(type => type.Split(new[] { ":" }, StringSplitOptions.None)))
            {
                system.SetValue(new RatingType(int.Parse(values[0], CultureInfo.InvariantCulture)), short.Parse(values[1], CultureInfo.InvariantCulture));
            }

            return system;
        }
    }
}
