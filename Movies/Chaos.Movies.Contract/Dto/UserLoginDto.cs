//-----------------------------------------------------------------------
// <copyright file="UserLoginDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract.Dto
{
    using System.Runtime.Serialization;

    /// <summary>A login request for a specific <see cref="UserDto"/>.</summary>
    [DataContract]
    public class UserLoginDto
    {
        /// <summary>Gets or sets the username of the user logging in.</summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>Gets or sets the IP-address of the client device the user is using.</summary>
        [DataMember]
        public string ClientIp { get; set; }

        /// <summary>Gets or sets the encrypted </summary>
        [DataMember]
        public string Password { get; set; }
    }
}
