//-----------------------------------------------------------------------
// <copyright file="ExternalLookupCollection.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class ExternalLookupCollection : IReadOnlyCollection<ExternalLookup>
    {
        /// <summary>The list of <see cref="ExternalLookup"/>s in this <see cref="ExternalLookupCollection"/>.</summary>
        private readonly List<ExternalLookup> externalLookup = new List<ExternalLookup>();

        /// <summary>Gets the number of elements contained in this <see cref="ExternalLookupCollection"/>.</summary>
        public int Count => this.externalLookup.Count;

        /// <summary>Returns an enumerator which iterates through this <see cref="ExternalLookupCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<ExternalLookup> GetEnumerator()
        {
            return this.externalLookup.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="ExternalLookupCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}