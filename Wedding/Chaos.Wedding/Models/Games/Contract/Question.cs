//-----------------------------------------------------------------------
// <copyright file="Question.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract;

    /// <summary>A question in a <see cref="Challenge" /> containing a set of <see cref="Alternative" />s.</summary>
    [DataContract]
    public class Question
    {
        /// <summary>Gets or sets the id of the <see cref="Question"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the <see cref="Challenge.Id"/> of the parent <see cref="Challenge"/>.</summary>
        [DataMember]
        public int ChallengeId { get; set; }

        /// <summary>Gets or sets the type of the <see cref="Question"/>.</summary>
        [DataMember]
        public ChallengeType Type { get; set; }

        /// <summary>Gets or sets the subject the <see cref="Question"/>.</summary>
        [DataMember]
        public ChallengeSubject Subject { get; set; }

        /// <summary>Gets or sets the difficulty the <see cref="Question"/>.</summary>
        [DataMember]
        public Difficulty Difficulty { get; set; }

        /// <summary>Gets or sets the image of the <see cref="Question"/>.</summary>
        [DataMember]
        public string ImageId { get; set; }

        /// <summary>Gets or sets the titles of the <see cref="Question"/>.</summary>
        [DataMember]
        public LanguageDescriptionCollectionDto Titles { get; set; }

        /// <summary>Gets or sets the children <see cref="Zone"/>s.</summary>
        [DataMember]
        public IEnumerable<Alternative> Alternatives { get; set; }
    }
}