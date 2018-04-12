//-----------------------------------------------------------------------
// <copyright file="LanguageTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System.Globalization;
    using NUnit.Framework;

    [TestFixture]
    public static class LanguageTest
    {
        [Test]
        public static void LanguageTitleTest()
        {
            var lang = new LanguageTitle("Test", CultureInfo.CurrentCulture);
            //lang = new LanguageTitle("Test", CultureInfo.GetCultureInfo());
        }
    }
}
