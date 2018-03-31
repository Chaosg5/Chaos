//-----------------------------------------------------------------------
// <copyright file="PersonAsCharacterDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class PersonAsCharacterDto
    {
        /// <summary>Gets or sets the character.</summary>
        [DataMember]
        public CharacterDto Character { get; set; }

        /// <summary>Gets or sets the person playing the <see cref="Character"/>.</summary>
        [DataMember]
        public PersonDto Person { get; set; }

        /// <summary>Gets or sets the user rating.</summary>
        [DataMember]
        public UserSingleRatingDto Ratings { get; set; }
    }
}
