//-----------------------------------------------------------------------
// <copyright file="QuestionType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games
{
    /// <summary>Enumeration for <see cref="ChallengeType"/>, since they need to be specified.</summary>
    public enum QuestionType
    {
        Unknown = 0,
        SingleChoice = 1,
        MultiChoice = 2,
        Text = 3,
        MultiText = 4,
        TrueOrFalse = 5,
        Sort = 6,
        SortAndMatch = 7,
        Puzzle = 8,
        WordScramble = 9,
        OddOneOut = 10,
        Rebus = 11,
        ImageRebus = 12,
        SpellCheck = 13,
        ClozeTest = 14,
    }
}