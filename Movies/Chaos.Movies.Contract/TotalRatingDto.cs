//-----------------------------------------------------------------------
// <copyright file="TotalRatingDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract.Interface;

    /// <inheritdoc />
    /// <summary>The total rating from all users.</summary>
    [DataContract]
    public class TotalRatingDto : IRating
    {
        /// <inheritdoc />
        [DataMember]
        public double Value { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string DisplayValue { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string HexColor { get; set; }
    }
}
