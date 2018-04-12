//-----------------------------------------------------------------------
// <copyright file="UserSingleRatingDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class UserSingleRatingDto
    {
        /// <summary>Gets or sets total rating from all user's ratings.</summary>
        [DataMember]
        public double TotalRating { get; set; }

        /// <summary>Gets or sets the current user's rating.</summary>
        [DataMember]
        public int UserRating { get; set; }

        /// <summary>Gets or sets the id <see cref="UserDto"/> which the <see cref="UserRating"/> belongs to.</summary>
        [DataMember]
        public int UserId { get; set; }
    }
}
