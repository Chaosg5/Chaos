//-----------------------------------------------------------------------
// <copyright file="TeamChallenge.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Runtime.Serialization;

    /// <summary>A <see cref="Challenge"/> played by a <see cref="Team"/>.</summary>
    [DataContract]
    public class TeamChallenge
    {
        /// <summary>Gets or sets the <see cref="Team.Id"/>.</summary>
        [DataMember]
        public int TeamId { get; set; }

        /// <summary>Gets or sets the <see cref="Challenge.Id"/>.</summary>
        [DataMember]
        public int ChallengeId { get; set; }

        /// <summary>Gets or sets a value indicating whether the <see cref="Challenge"/> is locked.</summary>
        [DataMember]
        public bool IsLocked { get; set; }

        /// <summary>Gets or sets the <see cref="Team"/>'s score on the <see cref="Challenge"/>.</summary>
        [DataMember]
        public int Score { get; set; }
    }
}