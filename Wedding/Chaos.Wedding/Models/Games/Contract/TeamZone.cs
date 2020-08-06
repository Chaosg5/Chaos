//-----------------------------------------------------------------------
// <copyright file="TeamZone.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Runtime.Serialization;

    /// <summary>A <see cref="Zone"/> unlocked by a <see cref="Team"/>.</summary>
    [DataContract]
    public class TeamZone
    {
        /// <summary>Gets or sets the <see cref="Team.Id"/>.</summary>
        [DataMember]
        public int TeamId { get; set; }

        /// <summary>Gets or sets the <see cref="Zone.Id"/>.</summary>
        [DataMember]
        public int ZoneId { get; set; }

        /// <summary>Gets or sets a value indicating whether the <see cref="Zone"/> is unlocked.</summary>
        [DataMember]
        public bool Unlocked { get; set; }
    }
}