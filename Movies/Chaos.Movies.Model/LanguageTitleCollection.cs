//-----------------------------------------------------------------------
// <copyright file="LanguageTitleCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The title of a movie.</summary>
    public class LanguageTitleCollection : Listable<LanguageTitle, LanguageTitleDto, LanguageTitleCollection>
    {
        /// <summary>The database column for this <see cref="LanguageTitleCollection"/>.</summary>
        internal const string TitlesColumn = "Titles";

        /// <summary>Gets the base title.</summary>
        public string GetBaseTitle => this.GetTitle(null).Title;

        /// <summary>Gets all titles in a table which can be used to save them to the database.</summary>
        /// <returns>A table containing the title and language as columns for each title.</returns>
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(LanguageTitle.LanguageColumn, typeof(string)));
                    table.Columns.Add(new DataColumn(LanguageTitle.TitleColumn, typeof(string)));
                    foreach (var languageTitle in this.Items)
                    {
                        table.Rows.Add(languageTitle.Language.Name, languageTitle.Title);
                    }

                    return table;
                }
            }
        }

        /// <summary>Converts this <see cref="LanguageTitleCollection"/> to a <see cref="ReadOnlyCollection{LanguageTitleDto}"/>.</summary>
        /// <returns>The <see cref="ReadOnlyCollection{LanguageTitleDto}"/>.</returns>
        public override ReadOnlyCollection<LanguageTitleDto> ToContract()
        {
            return new ReadOnlyCollection<LanguageTitleDto>(this.Items.Select(t => t.ToContract()).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override LanguageTitleCollection FromContract(ReadOnlyCollection<LanguageTitleDto> contract)
        {
            if (contract == null)
            {
                return new LanguageTitleCollection();
            }

            var list = new LanguageTitleCollection();
            foreach (var item in contract)
            {
                list.Add(LanguageTitle.Static.FromContract(item));
            }

            return list;
        }

        /// <summary>Gets the title for the specified <paramref name="language"/>.</summary>
        /// <param name="language">The language to get the title for.</param>
        /// <returns>The title of the specified language; else the title of the default language</returns>
        public LanguageTitle GetTitle(CultureInfo language)
        {
            if (this.Count == 0)
            {
                return null;
            }

            var languageName = GlobalCache.DefaultLanguage.Name;
            if (!string.IsNullOrEmpty(language?.Name))
            {
                languageName = language.Name;
            }

            return this.Items.FirstOrDefault(t => t.Language.Name == languageName) ?? this.Items.FirstOrDefault(t => t.Language.Name == GlobalCache.DefaultLanguage.Name) ?? this.Items.First();
        }

        /// <summary>Changes the title of this movie series type.</summary>
        /// <param name="title">The title to set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="title"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
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
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
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

            var existingTitle = this.Items.FirstOrDefault(t => t.Language.Name == language.Name);
            if (existingTitle != null)
            {
                existingTitle.Title = title;
            }
            else
            {
                this.Add(new LanguageTitle(title, language));
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="LanguageTitleCollection"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (this.Items.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }

            foreach (var languageTitle in this.Items)
            {
                languageTitle.ValidateSaveCandidate();
            }
        }
    }
}