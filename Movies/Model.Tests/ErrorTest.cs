//-----------------------------------------------------------------------
// <copyright file="ErrorTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Threading.Tasks;

    using Chaos.Movies.Model.Exceptions;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class ErrorTest
    {
        /// <summary>Tests the <see cref="Extensions.SaveAsync"/>.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        [Test]
        public static async Task TestErrorSaveAsync()
        {
            try
            {
                throw new InvalidOperationException("You can not do that!");
            }
            catch (Exception exception)
            {
                await exception.SaveAsync(await UserTest.GetSystemSessionAsync());
            }
        }
    }
}
