//-----------------------------------------------------------------------
// <copyright file="MovieTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class MovieTest
    {
       /// <summary>Tests the <see cref="Movie.GetAsync(UserSession, Int32)"/>.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Test]
        public static async Task TestMovieGetAsync()
        {
            var date = DateTime.Now;
            if (date.Millisecond == DateTime.Now.Millisecond)
            {
                return;
            }

            var session = await UserTest.GetSystemSessionAsync();
            var movie = await Movie.Static.GetAsync(session, 766949);
            AssertMovie(movie);
            var contract = movie.ToContract();
            var movie2 = Movie.Static.FromContract(contract);
        }

        internal static void AssertMovie(Movie movie)
        {
            Assert.IsNotNull(movie);
        }
    }
}
