//-----------------------------------------------------------------------
// <copyright file="LanguageTitles.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace Chaos.Movies.Model
{
    using System.Globalization;

    /// <summary>The title of a movie.</summary>
    public class LanguageTitles
    {
        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<LanguageTitle> titles = new List<LanguageTitle>();

        /// <summary>The list of title of the movie series type in different languages.</summary>
        public ReadOnlyCollection<LanguageTitle> Titles
        {
            get { return this.titles.AsReadOnly(); }
        }

        /// <summary>Gets the number if existing titles.</summary>
        public int Count
        {
            get { return this.titles.Count; }
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
        /// <param name="title">The title to set for the speciefied <paramref name="language"/>.</param>
        /// <param name="language">The language of the specified <paramref name="title"/>.</param>
        public void SetTitle(string title, CultureInfo language)
        {
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

        /// <summary>Gets all titles in a table which can be used to save them to the database.</summary>
        /// <returns>A table containing the title and language as columns for each title.</returns>
        public DataTable GetSaveTitles
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn("Title"));
                    table.Columns.Add(new DataColumn("Language"));
                    foreach (var languageTitle in this.titles)
                    {
                        table.Rows.Add(languageTitle.Title, languageTitle.Language.Name);
                    }

                    return table;
                }
            }
        }
    }
}