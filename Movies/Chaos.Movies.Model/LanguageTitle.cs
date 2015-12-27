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

            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            this.Title = title;
            this.Language = language;
        }

        /// <summary>Initializes a new instance of the <see cref="LanguageTitle"/> class.</summary>
        /// <param name="record">The data record containing the data for the language title.</param>
        public LanguageTitle(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets or sets the title.</summary>
        public string Title { get; set; }

        /// <summary>Gets the language of the title.</summary>
        public CultureInfo Language { get; private set; }

        /// <summary>Updates a language title from a record.</summary>
        /// <param name="title">The language title to update.</param>
        /// <param name="record">The record containing the data for the language title.</param>
        private static void ReadFromRecord(LanguageTitle title, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "Title", "Language" });
            title.Title = record["Title"].ToString();
            title.Language = new CultureInfo(record["Language"].ToString());
        }
    }
}