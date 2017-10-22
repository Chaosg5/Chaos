//-----------------------------------------------------------------------
// <copyright file="PersonUserRatingDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class PersonUserRatingDto
    {
        /// <summary>Gets or sets or sets id of the <see cref="MovieDto"/>.</summary>
        [DataMember]
        public int MovieId { get; set; }

        /// <summary>Gets or sets the <see cref="UserDto"/>'s rating of the <see cref="PersonDto"/> in the <see cref="MovieDto"/>.</summary>
        [DataMember]
        public int Rating { get; set; }

        /// <summary>Gets or sets the number of times the <see cref="UserDto"/> has watched the <see cref="MovieDto"/>.</summary>
        [DataMember]
        public int Watches { get; set; }
    }
}
