//-----------------------------------------------------------------------
// <copyright file="QuestionType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    /// <summary>Enumeration for <see cref="ChallengeType"/>, since they need to be specified.</summary>
    public enum QuestionType
    {
        /// <summary>The type is not set.</summary>
        Unknown = 0,

        /// <summary>A single choice.</summary>
        SingleChoice = 1,

        /// <summary>Multiple choice.</summary>
        MultiChoice = 2,

        /// <summary>Single text.</summary>
        Text = 3,

        /// <summary>Multiple text.</summary>
        MultiText = 4,

        /// <summary>True or false.</summary>
        TrueOrFalse = 5,

        /// <summary>Sort the alternatives.</summary>
        Sort = 6,

        /// <summary>Search and match.</summary>
        SortAndMatch = 7,

        /// <summary>Make a puzzle.</summary>
        Puzzle = 8,

        /// <summary>Word scramble.</summary>
        WordScramble = 9,

        /// <summary>Odd one out.</summary>
        OddOneOut = 10,

        /// <summary>Rebus of text.</summary>
        Rebus = 11,

        /// <summary>Rebus of images.</summary>
        ImageRebus = 12,

        /// <summary>Spell check.</summary>
        SpellCheck = 13,

        /// <summary>Close text.</summary>
        CloseTest = 14,

        /// <summary>Match choices.</summary>
        Match = 15
    }
}