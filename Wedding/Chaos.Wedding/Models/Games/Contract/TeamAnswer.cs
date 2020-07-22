//-----------------------------------------------------------------------
// <copyright file="TeamAnswer.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Runtime.Serialization;

    /// <summary>An answer to a <see cref="Alternative"/> on a <see cref="Question"/> done by a <see cref="Team"/>.</summary>
    [DataContract]
    public class TeamAnswer
    {
        /// <summary>Gets or sets the <see cref="Team.Id"/>.</summary>
        [DataMember]
        public int TeamId { get; set; }

        /// <summary>Gets or sets the <see cref="Challenge.Id"/>.</summary>
        [DataMember]
        public int ChallengeId { get; set; }

        /// <summary>Gets or sets the <see cref="Question.Id"/>.</summary>
        [DataMember]
        public int QuestionId { get; set; }

        /// <summary>Gets or sets the <see cref="Alternative.Id"/>.</summary>
        [DataMember]
        public int AlternativeId { get; set; }

        /// <summary>Gets or sets the answered row of the <see cref="AlternativeId"/>.</summary>
        [DataMember]
        public byte AnsweredRow { get; set; }

        /// <summary>Gets or sets the answered column of the <see cref="AlternativeId"/>.</summary>
        [DataMember]
        public byte AnsweredColumn { get; set; }

        /// <summary>Gets or sets a value indicating whether the <see cref="AlternativeId"/> is answered.</summary>
        [DataMember]
        public bool IsAnswered { get; set; }

        /// <summary>Gets or sets the answer of the <see cref="AlternativeId"/>.</summary>
        [DataMember]
        public string Answer { get; set; }
    }
}