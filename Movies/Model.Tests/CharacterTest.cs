//-----------------------------------------------------------------------
// <copyright file="CharacterTest.cs">
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
    public static class CharacterTest
    {
        /// <summary>Tests the <see cref="CharacterDto"/>.</summary>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T,TDto}"/> has to be saved before added.</exception>
        [Test]
        public static void TestCharacterContract()
        {
            var contract = new CharacterDto { Name = "Character Name" };
            var character = Character.Static.FromContract(contract);
            Assert.IsNotNull(character);

            contract.Id = 7;
            contract.Name = "Character Name";
            contract.ExternalLookups = new ReadOnlyCollection<ExternalLookupDto>(new List<ExternalLookupDto>());
            contract.Images = new ReadOnlyCollection<IconDto>(new List<IconDto>());
            contract.UserRating = new TotalRatingDto
            {
                //UserId = 1,
                Value = 6
            };
            contract.TotalRating = new TotalRatingDto
            {
                Value = 7
            };

            character = Character.Static.FromContract(contract);
            Assert.AreEqual(contract.Id, character.Id);
            Assert.AreEqual(contract.Name, character.Name);
            Assert.AreEqual(contract.ExternalLookups.Count, character.ExternalLookups.Count);
            Assert.AreEqual(contract.Images.Count, character.Images.Count);
            Assert.AreEqual(contract.TotalRating.Value, character.TotalRating.Value);
            Assert.AreEqual(contract.UserRating.Value, character.UserRating.Value);
            //Assert.AreEqual(contract.UserRating.UserId, character.UserRating.UserId);
        }

        /// <summary>Tests the <see cref="Character.SaveAsync"/>.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Character"/> is not valid to be saved.</exception>
        [Test]
        public static async Task TestCharacterSaveAsync()
        {
            var session = await UserTest.GetSystemSessionAsync();
            var characterName = "Test Character H84H743";
            var characters = (await Character.Static.SearchAsync(
                new SearchParametersDto { SearchText = characterName, RequireExactMatch = true, SearchLimit = 1 },
                session)).ToList();
            Character character;
            if (!characters.Any())
            {
                character = new Character(characterName);

                await character.SaveAsync(session);
            }
            else
            {
                character = characters.First();
            }

            Assert.AreEqual(character.Name, characterName);
            Assert.Greater(character.Id, 0);
        }
    }
}
