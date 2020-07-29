//-----------------------------------------------------------------------
// <copyright file="Question.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        /// <summary>Gets the maximum score value.</summary>
        public int MaxScore
        {
            get
            {
                return this.Alternatives.Where(a => a.IsCorrect).Sum(a => a.ScoreValue);
            }
        }

        /// <summary>Gets the maximum number of choices.</summary>
        public int MaxChoices
        {
            get
            {
                switch (this.QuestionType)
                {
                    case QuestionType.SingleChoice:
                        return 1;
                    case QuestionType.MultiChoice:
                        return this.Alternatives.Count(a => a.IsCorrect);
                    default:
                        return 0;
                }
            }
        }

        /// <summary>Gets the number of columns.</summary>
        public int ColumnCount
        {
            get
            {
                return this.Alternatives.Max(a => a.CorrectColumn);
            }
        }

        /// <summary>Gets the number of columns.</summary>
        public int RowCount
        {
            get
            {
                return this.Alternatives.Max(a => a.CorrectRow);
            }
        }

        /// <summary>Gets the CSS class for the <paramref name="alternative"/> based on it's <see cref="Alternative.TeamAnswer"/>.</summary>
        /// <param name="alternative">The <see cref="Alternative"/>.</param>
        /// <param name="isLocked">The <see cref="TeamChallenge.IsLocked"/>.</param>
        /// <returns>The CSS class.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="alternative"/> is <see langword="null"/></exception>
        //// ReSharper disable once CyclomaticComplexity
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
                case QuestionType.MultiText:
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

                case QuestionType.Match:
                case QuestionType.Sort:
                case QuestionType.SortAndMatch:
                    if (!isLocked)
                    {
                        return alternative.TeamAnswer?.IsAnswered == true ? Alternative.SelectedClass : Alternative.BaseClass;
                    }
                    else if (alternative.TeamAnswer?.IsAnswered == true)
                    {
                        if (alternative.TeamAnswer.AnsweredRow == alternative.CorrectRow)
                        {
                            return Alternative.CorrectClass;
                        }
                        else
                        {
                            return Alternative.IncorrectClass;
                        }
                    }
                    else
                    {
                        return Alternative.MissedClass;
                    }

                    return Alternative.BaseClass;
                case QuestionType.Unknown:
                default:
                    return Alternative.BaseClass;
            }
        }

        /// <summary>Gets the score for the <see cref="Question"/> for all of the current <see cref="Team"/>s <see cref="TeamAnswer"/>s.</summary>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetScore()
        {
            int score;
            IEnumerable<Alternative> correctAlternatives;
            switch (this.QuestionType)
            {
                case QuestionType.SingleChoice:
                case QuestionType.MultiChoice:
                    correctAlternatives = this.Alternatives.Where(a => a.TeamAnswer?.IsAnswered == a.IsCorrect);
                    score = correctAlternatives.Sum(a => a.ScoreValue);
                    return score > 0 ? score : 0;
                case QuestionType.Text:
                case QuestionType.MultiText:
                    correctAlternatives = this.Alternatives.Where(a => a.TeamAnswer?.Answer == a.CorrectAnswer);
                    score = correctAlternatives.Sum(a => a.ScoreValue);
                    return score > 0 ? score : 0;
                case QuestionType.Match:
                    correctAlternatives = this.Alternatives.Where(a => a.CorrectColumn > 1 && a.TeamAnswer?.AnsweredRow == a.CorrectRow);
                    score = correctAlternatives.Sum(a => a.ScoreValue);
                    return score > 0 ? score : 0;
                case QuestionType.Sort:
                case QuestionType.SortAndMatch:
                    correctAlternatives = this.Alternatives.Where(a => a.CorrectColumn == 1 && a.TeamAnswer?.AnsweredRow == a.CorrectRow);
                    score = correctAlternatives.Sum(a => a.ScoreValue);
                    for (var column = 2; column <= this.ColumnCount; column++)
                    {
                        correctAlternatives = this.Alternatives.Where(a => a.CorrectColumn == 1 && a.TeamAnswer?.AnsweredRow == a.CorrectRow);
                        score += correctAlternatives.Sum(a => a.ScoreValue);
                    }

                    return score > 0 ? score : 0;
                default:
                    return 0;
            }
        }
    }
}