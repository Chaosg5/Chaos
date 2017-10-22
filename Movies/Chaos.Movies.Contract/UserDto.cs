//-----------------------------------------------------------------------
// <copyright file="UserDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class UserDto
    {
        /// <summary>Gets or sets the id of the user.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the username of the user.</summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>Gets or sets the name of the user.</summary>
        [DataMember]
        public string Name { get; set; }
    }
}
