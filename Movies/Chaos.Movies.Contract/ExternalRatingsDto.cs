//-----------------------------------------------------------------------
// <copyright file="ExternalRatingsDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class ExternalRatingsDto
    {
        /// <summary>Gets or sets the <see cref="ExternalSourceDto"/>.</summary>
        [DataMember]
        public ExternalSourceDto ExternalSource { get; set; }

        /// <summary>Gets or sets the external rating.</summary>
        [DataMember]
        public double Rating { get; set; }

        /// <summary>Gets or sets the count of votes for the rating.</summary>
        [DataMember]
        public int RatingCount { get; set; }
    }
}