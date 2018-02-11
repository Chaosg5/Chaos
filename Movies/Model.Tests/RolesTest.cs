//-----------------------------------------------------------------------
// <copyright file="RolesTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System.Globalization;
    using NUnit.Framework;

    [TestFixture]
    public static class RolesTest
    {
        [Test]
        public static void TestRoleCollection()
        {
            var collection = new RolesCollection();
            Assert.IsTrue(collection.Count == 0);
            collection.Add(new Role());
            Assert.IsTrue(collection.Count == 1);
            collection.Add(new Role());
            Assert.IsTrue(collection.Count == 1);
            collection.Remove(new Role());
            Assert.IsTrue(collection.Count == 0);
            var role = new Role();
            collection.Add(role);
            Assert.IsTrue(collection.Count == 1);
            collection.Remove(role);
            Assert.IsTrue(collection.Count == 0);
        }
    }
}
