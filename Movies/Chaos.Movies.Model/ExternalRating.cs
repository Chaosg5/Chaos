//-----------------------------------------------------------------------
// <copyright file="ExternalRatings.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class ExternalRating
    {
        /// <summary>Gets the <see cref="ExternalSource"/>.</summary>
        [DataMember]
        public ExternalSource ExternalSource { get; private set; }

        /// <summary>Gets the external rating.</summary>
        [DataMember]
        public double Rating { get; private set; }

        /// <summary>Gets the count of votes for the rating.</summary>
        [DataMember]
        public int RatingCount { get; private set; }
    }
}