//-----------------------------------------------------------------------
// <copyright file="ExternalLookupCollection.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Chaos.Movies.Contract;

    /// <summary>Represents a user.</summary>
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

        /// <summary>Converts this <see cref="ExternalLookupCollection"/> to a <see cref="ReadOnlyCollection{ExternalLookupDto}"/>.</summary>
        /// <returns>The <see cref="ReadOnlyCollection{ExternalLookupDto}"/>.</returns>
        public ReadOnlyCollection<ExternalLookupDto> ToContract()
        {
            return new ReadOnlyCollection<ExternalLookupDto>(this.externalLookup.Select(l => l.ToContract()).ToList());
        }

        public void SetLookup(ExternalLookup lookup)
        {
            
        }

        public void RemoveLookup(ExternalLookup lookup)
        {
            
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="ExternalLookupCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}