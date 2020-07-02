//-----------------------------------------------------------------------
// <copyright file="LanguageDescriptionCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The title of a movie.</summary>
    public class LanguageDescriptionCollection : Listable<LanguageDescription, LanguageDescriptionDto, LanguageDescriptionCollection, LanguageDescriptionCollectionDto>
    {
        /// <summary>Initializes a new instance of the <see cref="LanguageDescriptionCollection"/> class.</summary>
        public LanguageDescriptionCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LanguageDescriptionCollection"/> class.</summary>
        /// <param name="titlesFromText">The <see cref="LanguageDescription"/> to set from <see cref="UpdateFromText"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="LanguageDescription"/> is not valid to be saved.</exception>
        public LanguageDescriptionCollection(string titlesFromText)
        {
            this.UpdateFromText(titlesFromText);
        }
        
        /// <summary>Gets the base title.</summary>
        // ReSharper disable once ExceptionNotDocumented
        public string GetBaseTitle => this.GetTitle(GlobalCache.BaseLanguage.Name).Title;

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(LanguageTitle.LanguageColumn, typeof(string)));
                    table.Columns.Add(new DataColumn(LanguageTitle.TitleColumn, typeof(string)));
                    table.Columns.Add(new DataColumn(LanguageDescription.DescriptionColumn, typeof(string)));
                    foreach (var languageDescription in this.Items)
                    {
                        table.Rows.Add(languageDescription.Language.Name, languageDescription.Title, languageDescription.Description);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override LanguageDescriptionCollectionDto ToContract()
        {
            return new LanguageDescriptionCollectionDto(this.Items.Select(t => t.ToContract()).ToList());
        }

        /// <inheritdoc />
        public override LanguageDescriptionCollectionDto ToContract(string language)
        {
            var contract = new LanguageDescriptionCollectionDto(this.Items.Select(t => t.ToContract(language)).ToList())
            {
                // ReSharper disable once ExceptionNotDocumented
                UserTitle = this.GetTitle(language).ToContract(language)
            };

            // ReSharper disable once ExceptionNotDocumented
            var originalTitle = this.GetTitle(LanguageType.Original.ToString())?.ToContract();
            if (originalTitle != null && originalTitle.Title != contract.UserTitle.Title)
            {
                contract.OriginalTitle = originalTitle;
            }

            return contract;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override LanguageDescriptionCollection FromContract(LanguageDescriptionCollectionDto contract)
        {
            if (contract == null)
            {
                return new LanguageDescriptionCollection();
            }

            var list = new LanguageDescriptionCollection();
            foreach (var item in contract)
            {
                list.Add(LanguageDescription.Static.FromContract(item));
            }

            return list;
        }

        /// <summary>Gets the title for the specified <paramref name="languageName"/>.</summary>
        /// <param name="languageName">The language to get the title for.</param>
        /// <returns>The title of the specified language; else the title of the default language</returns>
        public LanguageDescription GetTitle(string languageName)
        {
            if (this.Count == 0)
            {
                return LanguageDescription.EmptyTitle();
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
        public void SetTitle(LanguageDescription title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.SetTitle(title.Title, title.Description, title.Language);
        }

        /// <summary>Changes the title of this movie series type.</summary>
        /// <param name="title">The title to set for the specified <paramref name="language"/>.</param>
        /// <param name="description">The description to set for the specified <paramref name="language"/>.</param>
        /// <param name="language">The language of the specified <paramref name="title"/>.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="title"/> or <paramref name="language"/> is null.</exception>
        public void SetTitle(string title, string description, CultureInfo language)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            var languageDescription = this.Items.FirstOrDefault(t => t.Language?.Name == language.Name);
            if (languageDescription != null)
            {
                languageDescription.Title = title;
                languageDescription.Description = description;
            }
            else
            {
                this.Add(new LanguageDescription(title, description, language));
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="LanguageDescriptionCollection"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.Items.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }

            foreach (var item in this.Items)
            {
                item.ValidateSaveCandidate();
            }
        }

        /// <summary>Updates the <paramref name="titles"/>.</summary>
        /// <param name="titles">The <see cref="LanguageDescription"/>s to update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="titles"/> is <see langword="null"/></exception>
        public void Update(LanguageDescriptionCollection titles)
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
        /// <param name="title">The <see cref="LanguageDescription"/> to update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="title"/> is <see langword="null"/></exception>
        public void Update(LanguageDescription title)
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
                existing.Description = title.Description;
            }
        }

        /// <summary>Adds or updates all <see cref="LanguageDescription"/> in the <paramref name="text"/> to this <see cref="LanguageDescriptionCollection"/>.</summary>
        /// <param name="text">The text containing values for the <see cref="LanguageDescription"/>(s).</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="LanguageDescription"/> is not valid to be saved.</exception>
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

                if (parts.Length != 3)
                {
                    throw new InvalidSaveCandidateException($"Expected 3 parts of the title, found {parts.Length}.");
                }

                var newTitle = new LanguageDescription(parts[1], parts[2], new CultureInfo(parts[0]));
                var existing = this.FirstOrDefault(l => l.Language.Name == newTitle.Language.Name);
                if (existing == null)
                {
                    // ReSharper disable ExceptionNotDocumented
                    this.Add(newTitle);
                }
                else
                {
                    existing.Title = newTitle.Title;
                    existing.Description = newTitle.Description;
                }
            }
        }
    }
}