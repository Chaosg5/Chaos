//-----------------------------------------------------------------------
// <copyright file="UserSingleRatingDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract.Interface;

    /// <inheritdoc />
    [DataContract]
    public class UserSingleRatingDto : IUserSingleRating
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

        /// <inheritdoc />
        [DataMember]
        public string Width { get; set; }

        /// <inheritdoc />
        [DataMember]
        public int UserId { get; set; }

        /// <inheritdoc />
        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}
