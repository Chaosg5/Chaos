//-----------------------------------------------------------------------
// <copyright file="RolesTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    using NUnit.Framework;

    [TestFixture]
    public static class RolesTest
    {
        [Test]
        public static void TestRoleCollection()
        {
            //ToDo: 
            var collection = new RoleCollection();
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

        /// <summary>Tests the <see cref="Role.SaveAsync"/>.</summary>
        /// <param name="englishName">The english Name.</param>
        /// <param name="swedishName">The swedish Name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Role"/> is not valid to be saved.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T,TDto}"/> has to be saved before added.</exception>
        [TestCase("Actor", "Skådespelare")]
        public static async Task TestEnsureRoleAsync(string englishName, string swedishName)
        {
            await GetRoleAsync(englishName, swedishName);
        }

        /// <summary>Tests the <see cref="Role.SaveAsync"/>.</summary>
        /// <param name="englishName">The english Name.</param>
        /// <param name="swedishName">The swedish Name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Role"/> is not valid to be saved.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T,TDto}"/> has to be saved before added.</exception>
        internal static async Task<int> GetRoleAsync(string englishName, string swedishName)
        {
            var session = await UserTest.GetSystemSessionAsync();
            var englishTitle = new LanguageTitle(englishName, LanguageTitleTest.English);
            var swedishTitle = new LanguageTitle(swedishName, LanguageTitleTest.Swedish);
            var roles = await Role.Static.GetAllAsync(session);
            var role = roles.FirstOrDefault(g => g.Titles.Any(t => t == englishTitle || t == swedishTitle));
            if (role == null)
            {
                role = new Role();
                role.Titles.Add(englishTitle);
                role.Titles.Add(swedishTitle);
                await role.SaveAsync(session);
            }

            Assert.Greater(role.Id, 0);
            Assert.AreEqual(2, role.Titles.Count);
            var title = role.Titles.First(t => t == englishTitle);
            Assert.IsNotNull(title);
            title = role.Titles.First(t => t == swedishTitle);
            Assert.IsNotNull(title);
            return role.Id;
        }

        /// <summary>The get actor role.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        internal static async Task<Role> GetActorRoleAsync()
        {
            var session = await UserTest.GetSystemSessionAsync();
            var englishTitle = new LanguageTitle("Actor", LanguageTitleTest.English);
            var roles = await Role.Static.GetAllAsync(session);
            return roles.FirstOrDefault(g => g.Titles.Any(t => t == englishTitle));
        }
    }
}
