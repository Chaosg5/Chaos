//-----------------------------------------------------------------------
// <copyright file="ChallengeSubject.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract;

    /// <summary>A subject of a <see cref="Challenge"/> or <see cref="Question"/>.</summary>
    [DataContract]
    public class ChallengeSubject
    {
        /// <summary>Gets or sets the id of the <see cref="ChallengeSubject"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the image of the <see cref="Game"/>.</summary>
        [DataMember]
        public string ImageId { get; set; }

        /// <summary>Gets or sets the titles of the <see cref="ChallengeSubject"/>.</summary>
        [DataMember]
        public LanguageTitleCollectionDto Titles { get; set; }
    }
}