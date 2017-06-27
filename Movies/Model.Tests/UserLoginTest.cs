//-----------------------------------------------------------------------
// <copyright file="UserLoginTest.cs">
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
    public static class UserLoginTest
    {
        [Test]
        public static void TestUserLogin()
        {
            var firstLogin = new UserLogin("TestName", "Hj7:_3f(", "192.1.0.1");
            var secondLogin = new UserLogin("TestName", "Hj7:_3f(", "192.1.0.1");

            Assert.That(!string.IsNullOrEmpty(firstLogin.Username));
            Assert.AreEqual(firstLogin.Username, secondLogin.Username);

            Assert.That(!string.IsNullOrEmpty(firstLogin.Password));
            Assert.AreEqual(firstLogin.Password, secondLogin.Password);
            Assert.AreNotEqual(firstLogin.Password, "Hj7:_3f(");

            Assert.That(!string.IsNullOrEmpty(firstLogin.ClientIp));
            Assert.AreEqual(firstLogin.ClientIp, secondLogin.ClientIp);
        }
    }
}
