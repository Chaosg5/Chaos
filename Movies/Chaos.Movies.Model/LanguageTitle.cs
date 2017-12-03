//-----------------------------------------------------------------------
// <copyright file="LanguageTitle.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using System.Globalization;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The title of a movie.</summary>
    public class LanguageTitle
    {
        /// <summary>Initializes a new instance of the <see cref="LanguageTitle"/> class.</summary>
        /// <param name="title">The title to set.</param>
        /// <param name="language">The language to set.</param>
        public LanguageTitle(string title, CultureInfo language)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            // ReSharper disable once JoinNullCheckWithUsage
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            this.Title = title;
            this.Language = language;
        }

        /// <summary>Initializes a new instance of the <see cref="LanguageTitle"/> class.</summary>
        /// <param name="record">The data record containing the data for the language title.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public LanguageTitle(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets or sets the title.</summary>
        public string Title { get; set; }

        /// <summary>Gets the language of the title.</summary>
        public CultureInfo Language { get; private set; }

        /// <summary>Converts this <see cref="LanguageTitle"/> to a <see cref="LanguageTitleDto"/>.</summary>
        /// <returns>The <see cref="LanguageTitleDto"/>.</returns>
        public LanguageTitleDto ToContract()
        {
            return new LanguageTitleDto { Title = this.Title, Language = this.Language };
        }
        
        /// <summary>Updates this <see cref="LanguageTitle"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="LanguageTitle"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "Title", "Language" });
            this.Title = record["Title"].ToString();
            this.Language = new CultureInfo(record["Language"].ToString());
        }
    }
}