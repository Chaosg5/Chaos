//-----------------------------------------------------------------------
// <copyright file="LanguageTitles.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The title of a movie.</summary>
    public class LanguageTitleCollection : IReadOnlyCollection<LanguageTitle>
    {
        /// <summary>The list of <see cref="LanguageTitle"/>s in this <see cref="LanguageTitleCollection"/>.</summary>
        private readonly List<LanguageTitle> titles = new List<LanguageTitle>();

        /// <summary>Initializes a new instance of the <see cref="LanguageTitleCollection" /> class.</summary>
        public LanguageTitleCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LanguageTitleCollection" /> class.</summary>
        /// <param name="reader">The record containing the data for the <see cref="LanguageTitleCollection"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="reader"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is <see langword="null" />.</exception>
        public LanguageTitleCollection(IDataReader reader)
        {
            this.ReadFromRecord(reader);
        }
        
        /// <summary>Gets the number if existing titles.</summary>
        public int Count => this.titles.Count;

        /// <summary>Gets the base title.</summary>
        public string GetBaseTitle => this.GetTitle(null).Title;

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

        /// <summary>Returns an enumerator which iterates through this <see cref="LanguageTitleCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="LanguageTitleCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<LanguageTitle> GetEnumerator()
        {
            return this.titles.GetEnumerator();
        }

        /// <summary>Converts this <see cref="LanguageTitleCollection"/> to a <see cref="ReadOnlyCollection{LanguageTitleDto}"/>.</summary>
        /// <returns>The <see cref="ReadOnlyCollection{LanguageTitleDto}"/>.</returns>
        public ReadOnlyCollection<LanguageTitleDto> ToContract()
        {
            return new ReadOnlyCollection<LanguageTitleDto>(this.titles.Select(t => t.ToContract()).ToList());
        }

        /// <summary>Gets the title for the specified <paramref name="language"/>.</summary>
        /// <param name="language">The language to get the title for.</param>
        /// <returns>The title of the specified language; else the title of the default language</returns>
        public LanguageTitle GetTitle(CultureInfo language)
        {
            if (this.Count == 0)
            {
                return new LanguageTitle(string.Empty, language ?? CultureInfo.InvariantCulture);
            }
            
            var languageName = "en-US";
            if (!string.IsNullOrEmpty(language?.Name))
            {
                languageName = language.Name;
            }

            return this.titles.Find(t => t.Language.Name == languageName) ?? this.titles.Find(t => t.Language.Name == "en-US") ?? this.titles.First();
        }

        /// <summary>Changes the title of this movie series type.</summary>
        /// <param name="title">The title to set.</param>
        public void SetTitle(LanguageTitle title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
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
                throw new ArgumentNullException(nameof(title));
            }

            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
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

        /// <summary>Updates this <see cref="LanguageTitleCollection"/> from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The record containing the data for the <see cref="LanguageTitleCollection"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="reader"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.titles.Clear();
            while (reader.Read())
            {
                this.titles.Add(new LanguageTitle(reader));
            }
        }
    }
}