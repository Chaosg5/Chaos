//-----------------------------------------------------------------------
// <copyright file="LanguageTitleTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System.Globalization;

    using Chaos.Movies.Contract;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="LanguageTitle"/>.</summary>
    [TestFixture]
    public static class LanguageTitleTest
    {
        /// <summary>Gets the english.</summary>
        public static CultureInfo English => new CultureInfo("en-US");

        /// <summary>The swedish.</summary>
        public static CultureInfo Swedish => new CultureInfo("sv-SE");

        /// <summary>Test the equal methods for <see cref="LanguageTitle"/>.</summary>
        [Test]
        public static void TestLanguageTitleEquals()
        {
            var title1 = new LanguageTitle("Test", new CultureInfo("en-US"));
            var title2 = new LanguageTitle("Test", new CultureInfo("en-US"));
            var title3 = new LanguageTitle("Test", new CultureInfo("sv-SE"));
            var title4 = new LanguageTitle("Test", new CultureInfo("sv-SE"));
            Assert.IsTrue(title2 == title1);
            Assert.IsTrue(title1.Equals(title2));
            Assert.IsTrue(title3 == title4);
            Assert.IsTrue(title4.Equals(title3));
            Assert.IsFalse(title2 == title4);
            Assert.IsFalse(title1.Equals(title3));
            Assert.IsTrue(title1 != title3);
            Assert.IsTrue(!title2.Equals(title4));
        }

        /// <summary>The get language title contract.</summary>
        /// <returns>The <see cref="LanguageTitleDto"/>.</returns>
        internal static LanguageTitleDto GetLanguageTitleContract()
        {
            return new LanguageTitleDto
            {
                Language = new CultureInfo("en-US"),
                Title = "New Title for Test"
            };
        }
    }
}
