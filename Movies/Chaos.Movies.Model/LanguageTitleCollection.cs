//-----------------------------------------------------------------------
// <copyright file="LanguageTitleCollection.cs">
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

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The title of a movie.</summary>
    public class LanguageTitleCollection : Listable<LanguageTitle, LanguageTitleDto, LanguageTitleCollection, LanguageTitleCollectionDto>
    {
        /// <summary>The database column for this collection of titles.</summary>
        public const string TitlesColumn = "Titles";

        /// <summary>Initializes a new instance of the <see cref="LanguageTitleCollection"/> class.</summary>
        public LanguageTitleCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LanguageTitleCollection"/> class.</summary>
        /// <param name="titlesFromText">The <see cref="LanguageDescription"/> to set from <see cref="UpdateFromText"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="LanguageDescription"/> is not valid to be saved.</exception>
        public LanguageTitleCollection(string titlesFromText)
        {
            this.UpdateFromText(titlesFromText);
        }

        /// <summary>Gets the base title.</summary>
        // ReSharper disable once ExceptionNotDocumented
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

        /// <inheritdoc />
        public override LanguageTitleCollectionDto ToContract()
        {
            return new LanguageTitleCollectionDto(this.Items.Select(t => t.ToContract()).ToList());
        }

        /// <inheritdoc />
        public override LanguageTitleCollectionDto ToContract(string language)
        {
            var contract = new LanguageTitleCollectionDto(this.Items.Select(t => t.ToContract(language)).ToList())
            {
                UserTitle = this.GetTitle(language).ToContract(language)
            };
            
            var originalTitle = this.GetTitle(LanguageType.Original.ToString())?.ToContract();
            if (originalTitle != null && originalTitle.Title != contract.UserTitle.Title)
            {
                contract.OriginalTitle = originalTitle;
            }

            return contract;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override LanguageTitleCollection FromContract(LanguageTitleCollectionDto contract)
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

        /// <summary>Gets the title for the specified <paramref name="languageName"/>.</summary>
        /// <param name="languageName">The language to get the title for.</param>
        /// <returns>The title of the specified language; else the title of the default language</returns>
        public LanguageTitle GetTitle(string languageName)
        {
            if (this.Count == 0)
            {
                return LanguageTitle.EmptyTitle();
            }
            
            if (string.Equals(languageName, LanguageType.Original.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return this.FirstOrDefault(t => t.LanguageType == LanguageType.Original);
            }

            if (string.Equals(languageName, LanguageType.Default.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                var title = this.FirstOrDefault(t => t.LanguageType == LanguageType.Default);
                if (title != null)
                {
                    return title;
                }

                languageName = GlobalCache.BaseLanguage.Name;
            }

            return this.Items.FirstOrDefault(t => t.Language?.Name == languageName) ?? this.Items.FirstOrDefault(t => t.Language?.Name == GlobalCache.BaseLanguage.Name) ?? this.Items.First();
        }

        /// <summary>Changes the title of this movie series type.</summary>
        /// <param name="title">The title to set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="title"/> is <see langword="null"/></exception>
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
        public override void ValidateSaveCandidate()
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

        /// <summary>Updates the <paramref name="titles"/>.</summary>
        /// <param name="titles">The <see cref="LanguageTitle"/>s to update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="titles"/> is <see langword="null"/></exception>
        public void UpdateRange(IEnumerable<LanguageTitle> titles)
        {
            if (titles == null)
            {
                throw new ArgumentNullException(nameof(titles));
            }

            foreach (var title in titles)
            {
                this.Update(title);
            }
        }

        /// <summary>Updates the <paramref name="title"/>.</summary>
        /// <param name="title">The <see cref="LanguageTitle"/> to update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="title"/> is <see langword="null"/></exception>
        public void Update(LanguageTitle title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            var existing = this.FirstOrDefault(l => l.Language.Name == title.Language.Name);
            if (existing == null)
            {
                // ReSharper disable ExceptionNotDocumented
                this.Add(title);
            }
            else
            {
                existing.Title = title.Title;
            }
        }

        /// <summary>Adds or updates all <see cref="LanguageTitle"/> in the <paramref name="text"/> to this <see cref="LanguageDescriptionCollection"/>.</summary>
        /// <param name="text">The text containing values for the <see cref="LanguageTitle"/>(s).</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="LanguageTitle"/> is not valid to be saved.</exception>
        public void UpdateFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var sets = text.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var set in sets)
            {
                var parts = set.Split(new[] { "¤" }, StringSplitOptions.None);
                if (parts.All(string.IsNullOrWhiteSpace))
                {
                    continue;
                }

                if (parts.Length != 2)
                {
                    throw new InvalidSaveCandidateException($"Expected 2 parts of the title, found {parts.Length}.");
                }

                var newTitle = new LanguageTitle(parts[1], new CultureInfo(parts[0].Trim()));
                var existing = this.FirstOrDefault(l => l.Language.Name == newTitle.Language.Name);
                if (existing == null)
                {
                    // ReSharper disable ExceptionNotDocumented
                    this.Add(newTitle);
                }
                else
                {
                    existing.Title = newTitle.Title;
                }
            }
        }
    }
}