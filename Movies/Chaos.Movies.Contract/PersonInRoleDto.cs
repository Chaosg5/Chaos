//-----------------------------------------------------------------------
// <copyright file="PersonInRoleDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class PersonInRoleDto
    {
        /// <summary>Gets or sets the person.</summary>
        [DataMember]
        public PersonDto Person { get; set; }

        /// <summary>Gets or sets the role of the <see cref="Person"/>.</summary>
        [DataMember]
        public RoleDto Role { get; set; }

        /// <summary>Gets or sets the department of the <see cref="Person"/>.</summary>
        [DataMember]
        public DepartmentDto Department { get; set; }

        /// <summary>Gets or sets the user rating.</summary>
        [DataMember]
        public UserSingleRatingDto UserRatings { get; set; }

        /// <summary>Gets or sets total rating score from all users.</summary>
        [DataMember]
        public double TotalRating { get; set; }
    }
}
