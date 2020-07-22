//-----------------------------------------------------------------------
// <copyright file="Question.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System;
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

        /// <summary>Gets or sets the type of the <see cref="Question"/> as defined by the <see cref="Type"/> <see cref="ChallengeType"/>.</summary>
        [DataMember]
        public QuestionType QuestionType { get; set; }

        /// <summary>Gets or sets the titles of the <see cref="Question"/>.</summary>
        [DataMember]
        public LanguageDescriptionCollectionDto Titles { get; set; }

        /// <summary>Gets or sets the children <see cref="Zone"/>s.</summary>
        [DataMember]
        public IReadOnlyCollection<Alternative> Alternatives { get; set; }

        public string GetAlternativeCssClass(Alternative alternative, bool isLocked)
        {
            if (alternative == null)
            {
                throw new ArgumentNullException(nameof(alternative));
            }

            switch (this.QuestionType)
            {
                case QuestionType.SingleChoice:
                case QuestionType.MultiChoice:
                    if (!isLocked)
                    {
                        return alternative.TeamAnswer?.IsAnswered == true ? Alternative.SelectedClass : Alternative.BaseClass;
                    }
                    else if (alternative.TeamAnswer?.IsAnswered == true)
                    {
                        return alternative.IsCorrect ? Alternative.CorrectClass : Alternative.IncorrectClass;
                    }
                    else
                    {
                        return alternative.IsCorrect ? Alternative.MissedClass : Alternative.BaseClass;
                    }

                case QuestionType.Text:
                    if (!isLocked)
                    {
                        return string.IsNullOrWhiteSpace(alternative.TeamAnswer?.Answer) ? Alternative.BaseClass : Alternative.SelectedClass;
                    }
                    else if (string.IsNullOrWhiteSpace(alternative.TeamAnswer?.Answer))
                    {
                        return Alternative.MissedClass;
                    }
                    else if (alternative.TeamAnswer?.Answer == alternative.CorrectAnswer)
                    {
                        return Alternative.CorrectClass;
                    }
                    else
                    {
                        return Alternative.IncorrectClass;
                    }

                case QuestionType.Unknown:
                default:
                    return Alternative.BaseClass;
            }
        }

        public int GetScore(Alternative alternative)
        {
            if (alternative == null)
            {
                throw new ArgumentNullException(nameof(alternative));
            }

            switch (this.QuestionType)
            {
                case QuestionType.MultiChoice:
                case QuestionType.SingleChoice:
                    if (!alternative.IsCorrect)
                    {
                        return 0;
                    }

                    if (alternative.TeamAnswer?.IsAnswered != true)
                    {
                        return 0;
                    }

                    return alternative.ScoreValue;
                case QuestionType.Text:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}