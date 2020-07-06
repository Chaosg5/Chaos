//-----------------------------------------------------------------------
// <copyright file="SystemData.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games
{
    using System.Collections.Generic;

    public class SystemData
    {
        public IEnumerable<Contract.ChallengeType> ChallengeTypes { get; set; }

        public IEnumerable<Contract.ChallengeSubject> ChallengeSubjects { get; set; }

        public IEnumerable<Contract.Difficulty> Difficulties { get; set; }

        public string UserLanguage { get; set; }

        public string GetLanguage(string key)
        {
            return string.Empty;
        }
    }
}