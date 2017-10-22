//-----------------------------------------------------------------------
// <copyright file="ExternalLookupDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class ExternalLookupDto
    {
        /// <summary>Gets or sets the <see cref="ExternalSourceDto"/>.</summary>
        [DataMember]
        public ExternalSourceDto ExternalSource { get; set; }

        /// <summary>Gets or sets the id of the item in the <see cref="ExternalSourceDto"/>.</summary>
        [DataMember]
        public string ExternalId { get; set; }
    }
}