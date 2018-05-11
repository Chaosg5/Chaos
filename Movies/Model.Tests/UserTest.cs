//-----------------------------------------------------------------------
// <copyright file="UserTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class UserTest
    {
        /// <summary>Tests that the system user exists.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        [Test]
        public static async Task TestCreateUserAsync()
        {
            var firstName = Helper.GetRandomFirstName();
            var lastName = Helper.GetRandomLastName();
            var username = $"{firstName}{lastName}";
            var password = $"{lastName}{firstName}";
            var login = new UserLogin(username, password, await GetClientIpAsync());
            var session = await GetSystemSessionAsync();

            var user = (await User.Static.SearchAsync(GetUsernameSearch(username), session)).FirstOrDefault();
            if (user == null)
            {
                user = new User($"{firstName} {lastName}", $"{firstName}.{lastName}@cmdb.hopto.org", login);
                await user.SaveAsync(session);
                await user.SetPasswordAsync(session, login);
            }

            session = await UserSession.Static.CreateSessionAsync(login);
            AssertSession(session);
        }

        /// <summary>The test user change password async.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        [Test]
        public static async Task TestUserChangePasswordAsync()
        {
            var username1 = "UsernameChange1";
            var username2 = "UsernameChange2";
            var password1 = "PasswordChange1";
            var password2 = "PasswordChange2";
            var firstName = Helper.GetRandomFirstName();
            var lastName = Helper.GetRandomLastName();
            var session = await GetSystemSessionAsync();
            var login1 = new UserLogin(username1, password1, await GetClientIpAsync());
            var login2 = new UserLogin(username2, password2, await GetClientIpAsync());
            var user = (await User.Static.SearchAsync(GetUsernameSearch(username1), session)).FirstOrDefault()
                ?? (await User.Static.SearchAsync(GetUsernameSearch(username2), session)).FirstOrDefault();
            if (user == null)
            {
                var name = $"{firstName} {lastName}";
                var email = $"{firstName}.{lastName}@cmdb.hopto.org";
                user = new User(name, email, login1);
                await user.SaveAsync(session);
                await user.SetPasswordAsync(session, login1);
                Assert.Greater(user.Id, 0);
                Assert.AreEqual(name, user.Name);
                Assert.AreEqual(email, user.Email);
            }

            if (user.Username == username1)
            {
                session = await UserSession.Static.CreateSessionAsync(login1);
                AssertSession(session);
                await user.SetPasswordAsync(session, login2, login1);
                Assert.AreEqual(user.Username, username2);
                session = await UserSession.Static.CreateSessionAsync(login2);
                AssertSession(session);
            }
            else if (user.Username == username2)
            {
                session = await UserSession.Static.CreateSessionAsync(login2);
                AssertSession(session);
                await user.SetPasswordAsync(session, login1, login2);
                Assert.AreEqual(user.Username, username1);
                session = await UserSession.Static.CreateSessionAsync(login1);
                AssertSession(session);
            }
            else
            {
                throw new MissingResultException($"The username {user.Username} is not valid.");
            }
        }

        /// <summary>Gets a <see cref="UserSession"/> for the system user.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        internal static async Task<UserSession> GetSystemSessionAsync()
        {
            var login = new UserLogin(
                Properties.Settings.Default.SystemUserName,
                Properties.Settings.Default.SystemUserName,
                await GlobalCache.GetServerIpAsync());
            // ReSharper disable ExceptionNotDocumented
            var session = await UserSession.Static.CreateSessionAsync(login);
            // ReSharper restore ExceptionNotDocumented
            AssertSession(session);
            return session;
        }

        /// <summary>Gets the current client IP.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        internal static async Task<string> GetClientIpAsync()
        {
            return (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList[0].ToString();
        }

        /// <summary>Gets search parameters for getting a user by the <paramref name="username"/>.</summary>
        /// <param name="username">The username of the <see cref="User"/> to search for.</param>
        /// <returns>The <see cref="SearchParametersDto"/>.</returns>
        private static SearchParametersDto GetUsernameSearch(string username)
        {
            return new SearchParametersDto { SearchText = username, SearchLimit = 1, RequireExactMatch = true };
        }

        /// <summary>Assert that the <paramref name="session"/> is valid.</summary>
        /// <param name="session">The session to assert.</param>
        private static void AssertSession(UserSession session)
        {
            Assert.IsNotNull(session);
            Assert.AreNotEqual(Guid.Empty, session.SessionId);
            Assert.Greater(session.UserId, 0);
            Assert.Less(session.ActiveFrom, DateTime.Now);
            Assert.Greater(session.ActiveTo, DateTime.Now);
            Assert.IsTrue(!string.IsNullOrEmpty(session.ClientIp));
        }
    }
}
