//-----------------------------------------------------------------------
// <copyright file="WatchDto.cs" company="Erik Bunnstad">
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

        /// <summary>Gets or sets the id of the <see cref="MovieDto"/> watched.</summary>
        [DataMember]
        public int MovieId { get; set; }

        /// <summary>Gets or sets the id of the <see cref="User"/> who watched <see cref="MovieDto"/>.</summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>Gets or sets the user who watched the <see cref="MovieDto"/>.</summary>
        [DataMember]
        public UserDto User { get; set; }

        /// <summary>Gets or sets or sets the date when the <see cref="MovieDto"/> was watched.</summary>
        [DataMember]
        public DateTime WatchDate { get; set; }

        /// <summary>Gets or sets a value indicating whether the <see cref="WatchDate"/> is uncertain or not.</summary>
        [DataMember]
        public bool DateUncertain { get; set; }

        /// <summary>Gets or sets the id of the <see cref="WatchLocation"/> where the <see cref="MovieDto"/> was watched.</summary>
        [DataMember]
        public int WatchLocationId { get; set; }

        /// <summary>Gets or sets the  <see cref="WatchLocation"/> where the <see cref="MovieDto"/> was watched.</summary>
        [DataMember]
        public WatchLocationDto WatchLocation { get; set; }

        /// <summary>Gets or sets the id of the <see cref="WatchType"/> of how the <see cref="MovieDto"/> was watched.</summary>
        [DataMember]
        public int WatchTypeId { get; set; }

        /// <summary>Gets or sets the <see cref="WatchType"/> how the <see cref="MovieDto"/> was watched.</summary>
        [DataMember]
        public WatchTypeDto WatchType { get; set; }
    }
}
