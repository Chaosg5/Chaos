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
        [TestCase("2:25;3:50;4:25;5:75;6:25", "2:4;3:3-5:4-6:5;4:3", 3.25)]
        [TestCase("2:25;3:50;4:25;5:75;6:25", "2:4;3:0-5:4-6:5;4:3", 3.875)]
        public static void TestRatingCalculate(string systemValue, string ratingValue, double expectedResult)
        {
            var ratingSystem = GetRatingSystem(systemValue);
            var rating = GetRatings(ratingValue);
            rating.GetRatings(ratingSystem);
            Assert.AreEqual(expectedResult, rating.Value);
        }

        [TestCase(1, 255, 0, 0, "#FF0000")]
        [TestCase(2, 255, 51, 0, "#FF3300")]
        [TestCase(3, 255, 102, 0, "#FF6600")]
        [TestCase(4, 255, 153, 0, "#FF9900")]
        [TestCase(5, 255, 204, 0, "#FFCC00")]
        [TestCase(6, 204, 230, 0, "#CCE600")]
        [TestCase(7, 153, 204, 0, "#99CC00")]
        [TestCase(8, 102, 179, 0, "#66B300")]
        [TestCase(9, 51, 153, 0, "#339900")]
        [TestCase(10, 0, 128, 0, "#008000")]

        public static void TestGetColor(int value, byte red, byte green, byte blue, string hex)
        {
            var rating = new Rating(new RatingType(1));
            rating.SetValue(value);
            var color = rating.GetColor();
            Assert.AreEqual(red, color.R);
            Assert.AreEqual(green, color.G);
            Assert.AreEqual(blue, color.B);

            var hexColor = rating.GetHexColor();
            Assert.AreEqual(hex, hexColor);
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
