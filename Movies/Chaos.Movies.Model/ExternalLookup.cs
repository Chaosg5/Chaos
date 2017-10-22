//-----------------------------------------------------------------------
// <copyright file="ExternalLookup.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class ExternalLookup
    {
        /// <summary>Gets the <see cref="ExternalSource"/>.</summary>
        [DataMember]
        public ExternalSource ExternalSource { get; private set; }

        /// <summary>Gets the id of the item in the <see cref="ExternalSource"/>.</summary>
        [DataMember]
        public string ExternalId { get; private set; }
    }
}