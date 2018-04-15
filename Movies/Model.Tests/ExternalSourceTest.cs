//-----------------------------------------------------------------------
// <copyright file="ExternalSourceTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class ExternalSourceTest
    {
        /// <summary>Tests the <see cref="ExternalSourceDto"/>.</summary>
        [Test]
        public static void TestExternalSourceContract()
        {
            var contract = new ExternalSourceDto { Name = "External Source" };
            var externalSource = ExternalSource.Static.FromContract(contract);
            Assert.IsNotNull(externalSource);

            contract.Id = 8;
            contract.BaseAddress = "https://www.externalsource.com";
            contract.PeopleAddress = "/Person";
            contract.CharacterAddress = "/Character";
            contract.GenreAddress = "/Genre";
            contract.EpisodeAddress = "/Episode";
            externalSource = ExternalSource.Static.FromContract(contract);
            Assert.AreEqual(contract.Id, externalSource.Id);
            Assert.AreEqual(contract.BaseAddress, externalSource.BaseAddress);
            Assert.AreEqual(contract.PeopleAddress, externalSource.PeopleAddress);
            Assert.AreEqual(contract.CharacterAddress, externalSource.CharacterAddress);
            Assert.AreEqual(contract.GenreAddress, externalSource.GenreAddress);
            Assert.AreEqual(contract.EpisodeAddress, externalSource.EpisodeAddress);
        }

        /// <summary>Tests the <see cref="ExternalSource.SaveAsync"/>.</summary>
        /// <param name="name">The <see cref="ExternalSource.Name"/>.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="ExternalSource"/> is not valid to be saved.</exception>
        [TestCase("IMDB")]
        [TestCase("TMDB")]
        [TestCase("Metacritic")]
        [TestCase("Rotten Tomatoes")]
        public static async Task TestEnsureExternalSourceAsync(string name)
        {
            var session = await UserTest.GetSystemSessionAsync();
            var allSources = await ExternalSource.Static.GetAllAsync(session);
            var source = allSources.FirstOrDefault(s => s.Name == name);
            if (source == null)
            {
                source = new ExternalSource(name, "https://www.externalsource.com", string.Empty, string.Empty, string.Empty, string.Empty);
                await source.SaveAsync(session);
                Assert.Greater(source.Id, 0);
                Assert.AreEqual(name, source.Name);
            }
        }

        /// <summary>Asserts that the <paramref name="contract"/> and <paramref name="externalSource"/> are equal.</summary>
        /// <param name="contract">The contract.</param>
        /// <param name="externalSource">The external source.</param>
        internal static void AssertExternalSource(ExternalSourceDto contract, ExternalSource externalSource)
        {
            Assert.AreEqual(contract.Id, externalSource.Id);
            Assert.AreEqual(contract.BaseAddress, externalSource.BaseAddress);
            Assert.AreEqual(contract.PeopleAddress, externalSource.PeopleAddress);
            Assert.AreEqual(contract.CharacterAddress, externalSource.CharacterAddress);
            Assert.AreEqual(contract.GenreAddress, externalSource.GenreAddress);
            Assert.AreEqual(contract.EpisodeAddress, externalSource.EpisodeAddress);
        }

        /// <summary>The get external source contract.</summary>
        /// <returns>The <see cref="ExternalSourceDto"/>.</returns>
        internal static ExternalSourceDto GetExternalSourceContract()
        {
            return new ExternalSourceDto
            {
                Name = "External Source",
                Id = 8,
                BaseAddress = "https://www.externalsource.com",
                PeopleAddress = "/Person",
                CharacterAddress = "/Character",
                GenreAddress = "/Genre",
                EpisodeAddress = "/Episode"
            };
        }
    }
}
