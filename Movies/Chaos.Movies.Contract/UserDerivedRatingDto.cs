﻿//-----------------------------------------------------------------------
// <copyright file="UserDerivedRatingDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract.Interface;

    /// <inheritdoc />
    /// <summary>Represents a user's rating for a specific <see cref="RatingTypeDto"/>.</summary>
    [DataContract]
    public class UserDerivedRatingDto : IRating
    {
        /// <summary>Gets or sets the id of the <see cref="UserDto"/> who owns the rating.</summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>Gets or sets the type of this rating.</summary>
        [DataMember]
        public RatingTypeDto RatingType { get; set; }

        /// <summary>Gets or sets the child ratings of this rating.</summary>
        [DataMember]
        public ReadOnlyCollection<UserDerivedRatingDto> SubRatings { get; set; }

        /// <inheritdoc />
        [DataMember]
        public double Value { get; set; }
        
        /// <summary>Gets or sets the derived value of this rating.</summary>
        [DataMember]
        public double Derived { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string Width { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string HexColor { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string DisplayValue { get; set; }

        /// <summary>Gets or sets the created date.</summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>Gets or sets the actual rating value for this rating, not counting derived values.</summary>
        [DataMember]
        public int ActualRating { get; set; }
    }
}
