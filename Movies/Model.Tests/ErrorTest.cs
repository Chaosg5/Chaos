//-----------------------------------------------------------------------
// <copyright file="ErrorTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Threading.Tasks;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class ErrorTest
    {
        /// <summary>Tests the <see cref="Extensions.SaveAsync"/>.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [Test]
        public static async Task TestErrorSaveAsync()
        {
            try
            {
                throw new InvalidOperationException("You can not do that!");
            }
            catch (Exception exception)
            {
                await exception.SaveAsync(await UserTest.GetUserSessionAsync());
            }
        }
    }
}
