//-----------------------------------------------------------------------
// <copyright file="CharacterTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
            contract.Ratings = new UserSingleRatingDto
            {
                TotalRating = 7,
                UserId = 1,
                UserRating = 6
            };

            character = Character.Static.FromContract(contract);
            Assert.AreEqual(contract.Id, character.Id);
            Assert.AreEqual(contract.Name, character.Name);
            Assert.AreEqual(contract.ExternalLookups.Count, character.ExternalLookups.Count);
            Assert.AreEqual(contract.Images.Count, character.Images.Count);
            Assert.AreEqual(contract.Ratings.TotalRating, character.Ratings.TotalRating);
            Assert.AreEqual(contract.Ratings.UserRating, character.Ratings.UserRating);
            Assert.AreEqual(contract.Ratings.UserId, character.Ratings.UserId);
        }

        /// <summary>Tests the <see cref="Character.SaveAsync"/>.</summary>
        [Test]
        public static async Task TestCharacterSaveAsync()
        {
            
        }
    }
}
