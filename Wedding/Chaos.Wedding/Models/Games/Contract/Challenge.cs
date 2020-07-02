//-----------------------------------------------------------------------
// <copyright file="Challenge.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract;

    /// <summary>A challenge in a <see cref="Zone" /> containing a set of <see cref="Question" />s.</summary>
    [DataContract]
    public class Challenge
    {
        /// <summary>Gets or sets the id of the <see cref="Challenge"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the <see cref="Zone.Id"/> of the parent <see cref="Zone"/>.</summary>
        [DataMember]
        public int ZoneId { get; set; }

        /// <summary>Gets or sets the type of the <see cref="Challenge"/>.</summary>
        [DataMember]
        public ChallengeType Type { get; set; }

        /// <summary>Gets or sets the subject the <see cref="Challenge"/>.</summary>
        [DataMember]
        public ChallengeSubject Subject { get; set; }

        /// <summary>Gets or sets the difficulty the <see cref="Challenge"/>.</summary>
        [DataMember]
        public Difficulty Difficulty { get; set; }

        /// <summary>Gets or sets the titles of the <see cref="Challenge"/>.</summary>
        [DataMember]
        public LanguageDescriptionCollectionDto Titles { get; set; }

        /// <summary>Gets or sets the children <see cref="Question"/>s.</summary>
        [DataMember]
        public IEnumerable<Question> Questions { get; set; }
    }
}