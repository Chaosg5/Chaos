//-----------------------------------------------------------------------
// <copyright file="UserTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class UserTest
    {
        /// <summary>Tests that the system user exists.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [Test]
        public static async Task TestEnsureSystemUser()
        {
            var login = new UserLogin("erik@skillingaryd.nu", "712", (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList[0].ToString());
            var session = await UserSession.Static.CreateSessionAsync(login);
            Assert.IsNotNull(session);
            Assert.AreNotEqual(Guid.Empty, session.SessionId);
            Assert.Greater(session.UserId, 0);
            Assert.Less(session.ActiveFrom, DateTime.Now);
            Assert.Greater(session.ActiveTo, DateTime.Now);
            Assert.IsTrue(!string.IsNullOrEmpty(session.ClientIp));
        }

        /// <summary>The get user session async.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        internal static async Task<UserSession> GetUserSessionAsync()
        {
            var login = new UserLogin("erik@skillingaryd.nu", "712", (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList[0].ToString());
            var session = await UserSession.Static.CreateSessionAsync(login);
            Assert.IsNotNull(session);
            Assert.AreNotEqual(Guid.Empty, session.SessionId);
            Assert.Greater(session.UserId, 0);
            Assert.Less(session.ActiveFrom, DateTime.Now);
            Assert.Greater(session.ActiveTo, DateTime.Now);
            Assert.IsTrue(!string.IsNullOrEmpty(session.ClientIp));
            return session;
        }
    }
}
