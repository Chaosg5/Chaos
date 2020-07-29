//-----------------------------------------------------------------------
// <copyright file="Team.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>A team.</summary>
    [DataContract]
    public class Team
    {
        /// <summary>Gets or sets the id of the <see cref="Team"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the lookup id of the <see cref="Team"/>.</summary>
        [DataMember]
        public Guid LookupId { get; set; }

        /// <summary>Gets or sets the name of the <see cref="Team"/>.</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>Gets or sets total score of the <see cref="Team"/> for the current <see cref="Game"/>.</summary>
        [DataMember]
        public int TeamScore { get; set; }
    }
}