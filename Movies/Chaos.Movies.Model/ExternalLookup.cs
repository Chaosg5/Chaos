//-----------------------------------------------------------------------
// <copyright file="ExternalLookup.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using Chaos.Movies.Contract;

    /// <summary>Represents a user.</summary>
    public class ExternalLookup
    {
        /// <summary>Gets the <see cref="ExternalSource"/>.</summary>
        public ExternalSource ExternalSource { get; private set; }

        /// <summary>Gets the id of the item in the <see cref="ExternalSource"/>.</summary>
        public string ExternalId { get; private set; }

        /// <summary>Converts this <see cref="ExternalLookup"/> to a <see cref="ExternalLookupDto"/>.</summary>
        /// <returns>The <see cref="ExternalLookupDto"/>.</returns>
        public ExternalLookupDto ToContract()
        {
            return new ExternalLookupDto { ExternalSource = this.ExternalSource.ToContract(), ExternalId = this.ExternalId };
        }
    }
}