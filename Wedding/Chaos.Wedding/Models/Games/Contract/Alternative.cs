//-----------------------------------------------------------------------
// <copyright file="Alternative.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract;

    /// <summary>An alternative in a <see cref="Question"/>.</summary>
    [DataContract]
    public class Alternative
    {
        /// <summary>The base class.</summary>
        public const string BaseClass = "game-alternative-choice";

        /// <summary>The selected class.</summary>
        public const string SelectedClass = BaseClass + " selected";

        /// <summary>The missed class.</summary>
        public const string MissedClass = BaseClass + " missed";

        /// <summary>The incorrect class.</summary>
        public const string IncorrectClass = BaseClass + " incorrect";

        /// <summary>The correct.</summary>
        public const string CorrectClass = BaseClass + " correct";

        /// <summary>Gets or sets the id of the <see cref="Alternative"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the <see cref="Question.Id"/> of the parent <see cref="Question"/>.</summary>
        [DataMember]
        public int QuestionId { get; set; }

        /// <summary>Gets or sets the correct row of the <see cref="Alternative"/>.</summary>
        [DataMember]
        public byte CorrectRow { get; set; }

        /// <summary>Gets or sets the correct column of the <see cref="Alternative"/>.</summary>
        [DataMember]
        public byte CorrectColumn { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="Alternative"/> is correct for the parent <see cref="Question"/>.</summary>
        [DataMember]
        public bool IsCorrect { get; set; }

        /// <summary>Gets or sets the score value of the <see cref="Alternative"/>.</summary>
        [DataMember]
        public byte ScoreValue { get; set; }

        /// <summary>Gets or sets the correct answer of the <see cref="Alternative"/>.</summary>
        [DataMember]
        public string CorrectAnswer { get; set; }

        /// <summary>Gets or sets the image of the <see cref="Alternative"/>.</summary>
        [DataMember]
        public string ImageId { get; set; }

        /// <summary>Gets or sets the titles of the <see cref="Alternative"/>.</summary>
        [DataMember]
        public LanguageDescriptionCollectionDto Titles { get; set; }

        /// <summary>Gets or sets the <see cref="TeamAnswer"/>.</summary>
        [DataMember]
        public TeamAnswer TeamAnswer { get; set; }
    }
}