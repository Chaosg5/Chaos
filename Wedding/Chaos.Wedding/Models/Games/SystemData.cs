//-----------------------------------------------------------------------
// <copyright file="SystemData.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games
{
    using System.Collections.Generic;
    using System.Globalization;

    using Chaos.Movies.Contract;

    /// <summary>The system data.</summary>
    public class SystemData
    {
        /// <summary>Gets or sets the <see cref="Contract.SystemText"/>s.</summary>
        public Dictionary<string, Contract.SystemText> SystemTexts { get; set; }

        /// <summary>Gets or sets the <see cref="Contract.ChallengeType"/>s.</summary>
        public IEnumerable<Contract.ChallengeType> ChallengeTypes { get; set; }

        /// <summary>Gets or sets the <see cref="Contract.ChallengeSubject"/>s.</summary>
        public IEnumerable<Contract.ChallengeSubject> ChallengeSubjects { get; set; }

        /// <summary>Gets or sets the <see cref="Contract.Difficulty"/>s.</summary>
        public IEnumerable<Contract.Difficulty> Difficulties { get; set; }

        /// <summary>Gets or sets the user's <see cref="CultureInfo.Name"/>.</summary>
        public string UserLanguage { get; set; }

        /// <summary>Gets or sets a value indicating whether the current user is admin.</summary>
        public bool IsAdmin { get; set; }

        /// <summary>Gets the <see cref="LanguageDescriptionDto.Title"/> of the <paramref name="key"/>.</summary>
        /// <param name="key">The key of the value to get from the <see cref="SystemTexts"/>.</param>
        /// <returns>The title.</returns>
        public string GetTitle(string key)
        {
            return this.SystemTexts.TryGetValue(key, out var systemText) ? systemText.Titles.UserTitle.Title : string.Empty;
        }

        /// <summary>Gets the <see cref="LanguageDescriptionDto.Description"/> of the <paramref name="key"/>.</summary>
        /// <param name="key">The key of the value to get from the <see cref="SystemTexts"/>.</param>
        /// <returns>The description.</returns>
        public string GetDescription(string key)
        {
            return this.SystemTexts.TryGetValue(key, out var systemText) ? systemText.Titles.UserTitle.Description : string.Empty;
        }
    }
}