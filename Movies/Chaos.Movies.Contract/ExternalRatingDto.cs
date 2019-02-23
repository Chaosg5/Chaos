//-----------------------------------------------------------------------
// <copyright file="ExternalRatingDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract.Interface;

    /// <inheritdoc />
    /// <summary>Represents a an external rating for an item.</summary>
    [DataContract]
    public class ExternalRatingDto : IRating
    {
        /// <summary>Gets or sets the <see cref="ExternalSourceDto"/>.</summary>
        [DataMember]
        public ExternalSourceDto ExternalSource { get; set; }

        /// <inheritdoc />
        [DataMember]
        public double Value { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string DisplayValue { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string HexColor { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string Width { get; set; }

        /// <summary>Gets or sets the count of votes for the rating.</summary>
        [DataMember]
        public int RatingCount { get; set; }
    }
}