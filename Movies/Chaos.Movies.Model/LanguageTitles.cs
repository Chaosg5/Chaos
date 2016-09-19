//-----------------------------------------------------------------------
// <copyright file="LanguageTitles.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    /// <summary>The title of a movie.</summary>
    public class LanguageTitles
    {
        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<LanguageTitle> titles = new List<LanguageTitle>();

        /// <summary>Initializes a new instance of the <see cref="LanguageTitles" /> class.</summary>
        public LanguageTitles()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LanguageTitles" /> class.</summary>
        /// <param name="reader">The reader containing the data for the movie series type.</param>
        public LanguageTitles(IDataReader reader)
        {
            ReadFromRecord(this, reader);
        }

        /// <summary>Gets the list of title of the movie series type in different languages.</summary>
        public ReadOnlyCollection<LanguageTitle> Titles
        {
            get { return this.titles.AsReadOnly(); }
        }

        /// <summary>Gets the number if existing titles.</summary>
        public int Count
        {
            get { return this.titles.Count; }
        }

        /// <summary>Gets the base title.</summary>
        public string GetBaseTitle
        {
            get { return this.GetTitle(null); }
        }

        /// <summary>Gets all titles in a table which can be used to save them to the database.</summary>
        /// <returns>A table containing the title and language as columns for each title.</returns>
        public DataTable GetSaveTitles
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn("Language"));
                    table.Columns.Add(new DataColumn("Title"));
                    foreach (var languageTitle in this.titles)
                    {
                        table.Rows.Add(languageTitle.Title, languageTitle.Language.Name);
                    }

                    return table;
                }
            }
        }

        /// <summary>Gets the title for the specified <paramref name="language"/>.</summary>
        /// <param name="language">The language to get the title for.</param>
        /// <returns>The title of the specified language; else the title of the default language</returns>
        public string GetTitle(CultureInfo language)
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }
            
            var languageName = "en-US";
            if (language != null)
            {
                languageName = language.Name;
            }

            return (this.titles.Find(t => t.Language.Name == languageName) ?? this.titles.First()).Title;
        }

        /// <summary>Changes the title of this movie series type.</summary>
        /// <param name="title">The title to set.</param>
        public void SetTitle(LanguageTitle title)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            this.SetTitle(title.Title, title.Language);
        }

        /// <summary>Changes the title of this movie series type.</summary>
        /// <param name="title">The title to set for the specified <paramref name="language"/>.</param>
        /// <param name="language">The language of the specified <paramref name="title"/>.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="title"/> or <paramref name="language"/> is null.</exception>
        public void SetTitle(string title, CultureInfo language)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException("title");
            }

            if (language == null)
            {
                throw new ArgumentNullException("language");
            }

            var existingTitle = this.titles.Find(t => t.Language.Name == language.Name);
            if (existingTitle != null)
            {
                existingTitle.Title = title;
            }
            else
            {
                this.titles.Add(new LanguageTitle(title, language));
            }
        }

        /// <summary>Updates language titles from a reader.</summary>
        /// <param name="titles">The language titles to update.</param>
        /// <param name="reader">The record containing the data for the language titles.</param>
        private static void ReadFromRecord(LanguageTitles titles, IDataReader reader)
        {
            titles.titles.Clear();
            while (reader.Read())
            {
                titles.titles.Add(new LanguageTitle(reader));
            }
        }
    }
}