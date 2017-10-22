//-----------------------------------------------------------------------
// <copyright file="CharacterInMovieDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class CharacterInMovieDto
    {
        /// <summary>Gets or sets the character.</summary>
        [DataMember]
        public CharacterDto Character { get; set; }

        /// <summary>Gets or sets the person playing the <see cref="Character"/>.</summary>
        [DataMember]
        public PersonDto Person { get; set; }

        /// <summary>Gets or sets the current user's rating of the <see cref="Character"/> in the <see cref="MovieDto"/>.</summary>
        [DataMember]
        public int UserRating { get; set; }

    }
}
