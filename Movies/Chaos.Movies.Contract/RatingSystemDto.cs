//-----------------------------------------------------------------------
// <copyright file="RatingSystemDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class RatingSystemDto
    {
        /// <summary>Gets or sets the id of this rating system.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the name of this rating system.</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>Gets or sets the description of this rating system.</summary>
        [DataMember]
        public string Description { get; set; }
    }
}