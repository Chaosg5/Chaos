﻿//-----------------------------------------------------------------------
// <copyright file="ExternalLookupTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;

    using Chaos.Movies.Contract;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class ExternalLookupTest
    {
        /// <summary>Tests the <see cref="ExternalLookupDto"/>.</summary>
        [Test]
        public static void TestExternalLookupContract()
        {
            var contract = GetExternalLookupContract();
            var externalId = contract.ExternalId;
            var externalLookup = ExternalLookup.Static.FromContract(contract);
            Assert.AreEqual(contract.ExternalId, externalLookup.ExternalId);
            ExternalSourceTest.AssertExternalSource(contract.ExternalSource, externalLookup.ExternalSource);

            contract.ExternalId = " ";
            Assert.Throws<ArgumentNullException>(() => ExternalLookup.Static.FromContract(contract));
            contract.ExternalId = externalId;
            contract.ExternalSource = null;
            Assert.Throws<ArgumentNullException>(() => ExternalLookup.Static.FromContract(contract));
        }

        /// <summary>The get external lookup contract.</summary>
        /// <returns>The <see cref="ExternalLookupDto"/>.</returns>
        internal static ExternalLookupDto GetExternalLookupContract()
        {
            return new ExternalLookupDto
            {
                ExternalId = "externalId9",
                ExternalSource = ExternalSourceTest.GetExternalSourceContract()
            };
        }
    }
}
