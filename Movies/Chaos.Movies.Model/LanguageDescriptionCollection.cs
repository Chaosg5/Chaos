//-----------------------------------------------------------------------
// <copyright file="LanguageDescriptionCollection.cs">
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
    public class LanguageDescriptionCollection : Listable<LanguageDescription, LanguageDescriptionDto, LanguageDescriptionCollection, ReadOnlyCollection<LanguageDescriptionDto>>
    {
        /// <summary>The database column for this <see cref="LanguageDescriptionCollection"/>.</summary>
        internal const string TitlesColumn = "Titles";

        /// <summary>Gets the base title.</summary>
        public string GetBaseTitle => this.GetTitle(null).Title;

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
        public override ReadOnlyCollection<LanguageDescriptionDto> ToContract()
        {
            return new ReadOnlyCollection<LanguageDescriptionDto>(this.Items.Select(t => t.ToContract()).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override LanguageDescriptionCollection FromContract(ReadOnlyCollection<LanguageDescriptionDto> contract)
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

        /// <summary>Gets the title for the specified <paramref name="language"/>.</summary>
        /// <param name="language">The language to get the title for.</param>
        /// <returns>The title of the specified language; else the title of the default language</returns>
        public LanguageDescription GetTitle(CultureInfo language)
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
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
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

            var languageDescription = this.Items.FirstOrDefault(t => t.Language.Name == language.Name);
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
        internal override void ValidateSaveCandidate()
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
    }
}