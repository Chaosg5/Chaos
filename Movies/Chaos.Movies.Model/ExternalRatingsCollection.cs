//-----------------------------------------------------------------------
// <copyright file="ExternalRatingsCollection.cs" company="Erik Bunnstad">
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
    public class ExternalRatingsCollection : IReadOnlyCollection<ExternalRating>
    {
        /// <summary>The list of <see cref="ExternalRating"/>s in this <see cref="ExternalRatingsCollection"/>.</summary>
        private readonly List<ExternalRating> externalRatings = new List<ExternalRating>();

        /// <summary>Gets the number of elements contained in this <see cref="ExternalRatingsCollection"/>.</summary>
        public int Count => this.externalRatings.Count;

        /// <summary>Returns an enumerator which iterates through this <see cref="ExternalRatingsCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<ExternalRating> GetEnumerator()
        {
            return this.externalRatings.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="ExternalRatingsCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}