//-----------------------------------------------------------------------
// <copyright file="WatchDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class WatchDto
    {
        /// <summary>Gets or sets the id of the watch.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the id of the <see cref="UserDto"/> who watched.</summary>
        [DataMember]
        public int UserId { get; set; }
        
        /// <summary>Gets or sets or sets the date when the watch happened.</summary>
        [DataMember]
        public DateTime WatchDate { get; set; }

        /// <summary>Gets or sets a value indicating whether the <see cref="WatchDate"/> is uncertain or not.</summary>
        [DataMember]
        public bool DateUncertain { get; set; }

        /// <summary>Gets or sets the <see cref="WatchTypeDto"/>.</summary>
        [DataMember]
        public WatchTypeDto WatchType { get; set; }
    }
}
