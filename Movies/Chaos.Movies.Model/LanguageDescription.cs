//-----------------------------------------------------------------------
// <copyright file="LanguageDescription.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The title of a movie.</summary>
    public class LanguageDescription : Loadable<LanguageDescription, LanguageDescriptionDto>
    {
        /// <summary>The database column for <see cref="Description"/>.</summary>
        internal const string DescriptionColumn = "Description";

        /// <summary>Private part of the <see cref="Title"/> property.</summary>
        private string title = string.Empty;

        /// <summary>Private part of the <see cref="Description"/> property.</summary>
        private string description = string.Empty;

        /// <summary>Private part of the <see cref="Language"/> property.</summary>
        private CultureInfo language;

        /// <summary>Initializes a new instance of the <see cref="LanguageDescription"/> class.</summary>
        /// <param name="title">The <see cref="Title"/> to set.</param>
        /// <param name="description">The <see cref="Description"/> to set.</param>
        /// <param name="language">The <see cref="Language"/> to set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="title"/> or <paramref name="language"/> is <see langword="null"/></exception>
        public LanguageDescription(string title, string description, CultureInfo language)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.Title = title;
            this.Description = description ?? string.Empty;
            this.Language = language;
        }
        
        /// <summary>Prevents a default instance of the <see cref="LanguageDescription"/> class from being created.</summary>
        private LanguageDescription()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static LanguageDescription Static { get; } = new LanguageDescription();

        /// <summary>Gets or sets the title.</summary>
        public string Title
        {
            get => this.title;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                this.title = value;
            }
        }

        /// <summary>Gets or sets the title.</summary>
        public string Description
        {
            get => this.description;
            set => this.description = value ?? string.Empty;
        }

        /// <summary>Gets the language of the title.</summary>
        public CultureInfo Language
        {
            get => this.language;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (string.IsNullOrEmpty(value.Name))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentOutOfRangeException(nameof(value), "The language has to have a name.");
                }

                this.language = value;
            }
        }

        /// <summary>Gets the type of the <see cref="LanguageDescription"/>.</summary>
        public LanguageType LanguageType { get; private set; }

        /// <summary>Gets an empty <see cref="LanguageDescription"/>.</summary>
        /// <returns>The <see cref="LanguageDescription"/>.</returns>
        public static LanguageDescription EmptyTitle()
        {
            return new LanguageDescription { language = GlobalCache.BaseLanguage };
        }

        /// <inheritdoc />
        public override LanguageDescriptionDto ToContract()
        {
            return new LanguageDescriptionDto { Title = this.Title, Description = this.Description, Language = this.Language, LanguageType = this.LanguageType };
        }

        /// <inheritdoc />
        public override LanguageDescriptionDto ToContract(string languageName)
        {
            return new LanguageDescriptionDto { Title = this.Title, Description = this.Description, Language = this.Language, LanguageType = this.LanguageType };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override LanguageDescription FromContract(LanguageDescriptionDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new LanguageDescription { Title = contract.Title, Description = contract.Description, Language = contract.Language, LanguageType = contract.LanguageType };
        }

        /// <inheritdoc />
        public override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override async Task<LanguageDescription> NewFromRecordAsync(IDataRecord record)
        {
            var result = new LanguageDescription();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            // ToDo: Create custom CultureInfo for Default and Original
            Persistent.ValidateRecord(record, new[] { LanguageTitle.TitleColumn, DescriptionColumn, LanguageTitle.LanguageColumn });
            this.Title = (string)record[LanguageTitle.TitleColumn];
            this.Description = (string)record[DescriptionColumn];
            var languageType = (string)record[LanguageTitle.LanguageColumn];
            switch (languageType.ToUpperInvariant())
            {
                case "DEFAULT":
                    this.LanguageType = LanguageType.Default;
                    break;
                case "ORIGINAL":
                    this.LanguageType = LanguageType.Default;
                    break;
                default:
                    this.LanguageType = LanguageType.Language;
                    this.Language = new CultureInfo(languageType);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}