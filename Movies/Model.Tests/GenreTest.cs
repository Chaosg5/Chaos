//-----------------------------------------------------------------------
// <copyright file="GenreTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class GenreTest
    {
        /// <summary>Tests the <see cref="ExternalLookupDto"/>.</summary>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T,TDto}"/> has to be saved before added.</exception>
        [Test]
        public static void TestGenreContract()
        {
            var contract = new GenreDto();
            var genre = Genre.Static.FromContract(contract);
            Assert.IsNotNull(genre);

            var externalLookup = ExternalLookupTest.GetExternalLookupContract();
            var title = LanguageTitleTest.GetLanguageTitleContract();
            contract.Id = 11;
            contract.ExternalLookups = new ReadOnlyCollection<ExternalLookupDto>(new List<ExternalLookupDto> { externalLookup });
            contract.Titles = new LanguageTitleCollectionDto(new List<LanguageTitleDto> { title });
            genre = Genre.Static.FromContract(contract);

            Assert.AreEqual(contract.Id, genre.Id);
            Assert.IsTrue(LanguageTitle.Static.FromContract(contract.Titles.First()).Equals(genre.Titles.First()));
            Assert.AreEqual(contract.ExternalLookups.Count, genre.ExternalLookups.Count);
            ////Assert.IsTrue(ExternalLookup.Static.FromContract(contract.ExternalLookups.First()).Equals(genre.ExternalLookups.First()));
        }

        /// <summary>The test ensure genre.</summary>
        /// <param name="englishName">The english name.</param>
        /// <param name="swedishName">The swedish name.</param>
        /// <param name="externalSources">The external sources.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">At least one title needs to be specified.</exception>
        [TestCase("Action", "Action", null)]
        [TestCase("Adventure", "Äventyr", null)]
        [TestCase("Comedy", "Komedi", null)]
        [TestCase("Crime", "Kriminal", null)]
        [TestCase("Drama", "Drama", null)]
        [TestCase("Family", "Familj", null)]
        [TestCase("Fantasy", "Fantasy", null)]
        [TestCase("History", "Historisk", null)]
        [TestCase("Horror", "Skräck", null)]
        [TestCase("Mystery", "Mystik", null)]
        [TestCase("Musical", "Musikal", null)]
        [TestCase("Romance", "Romans", null)]
        [TestCase("Science fiction", "Science fiction", null)]
        [TestCase("Thriller", "Thriller", null)]
        [TestCase("War", "Krig", null)]
        [TestCase("Western", "Västern", null)]
        [TestCase("Animation", "Animation", null)]
        [TestCase("Superhero", "Superhjälte", null)]
        [TestCase("Documentary", "Dokumentär", null)]
        [TestCase("Biography", "Biografi", null)]
        [TestCase("Sport", "Sport", null)]
        [TestCase("Short", "Kortfilm", null)]
        [TestCase("TV-movie", "TV-film", null)]
        [TestCase("Video Game", "TV-spel", null)]
        [TestCase("Music", "Musik", null)]
        public static async Task TestEnsureGenre(string englishName, string swedishName, string externalSources)
        {
            var session = await UserTest.GetSystemSessionAsync();
            var englishTitle = new LanguageTitle(englishName, LanguageTitleTest.English);
            var swedishTitle = new LanguageTitle(swedishName, LanguageTitleTest.Swedish);
            var genres = await Genre.Static.GetAllAsync(session);
            var genre = genres.FirstOrDefault(g => g.Titles.Any(t => t == englishTitle || t == swedishTitle));
            if (genre == null)
            {
                genre = new Genre();
                genre.Titles.Add(englishTitle);
                genre.Titles.Add(swedishTitle);
                await genre.SaveAsync(session);
            }

            Assert.Greater(genre.Id, 0);
            Assert.AreEqual(2, genre.Titles.Count);
            var title = genre.Titles.First(t => t == englishTitle);
            Assert.IsNotNull(title);
            title = genre.Titles.First(t => t == swedishTitle);
            Assert.IsNotNull(title);
        }
    }
}
