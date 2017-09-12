//-----------------------------------------------------------------------
// <copyright file="UserSessionDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract.Dto
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>A login session for a specific <see cref="UserDto"/>.</summary>
    [DataContract]
    public class UserSessionDto : IUserSession
    {
        /// <summary>Gets or sets the id of the session.</summary>
        [DataMember]
        public Guid SessionId { get; set; }

        /// <summary>Gets or sets the client IP.</summary>
        [DataMember]
        public string ClientIp { get; set; }

        /// <summary>Gets or sets the user id.</summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>Gets or sets the active form.</summary>
        [DataMember]
        public DateTime ActiveForm { get; set; }

        /// <summary>Gets or sets the active to.</summary>
        [DataMember]
        public DateTime ActiveTo { get; set; }
    }
}
