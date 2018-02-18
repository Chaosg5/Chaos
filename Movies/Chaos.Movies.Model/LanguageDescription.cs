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
        public const string DescriptionColumn = "Description";

        /// <summary>Private part of the <see cref="Title"/> property.</summary>
        private string title;

        /// <summary>Private part of the <see cref="Description"/> property.</summary>
        private string description;

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
            this.Description = description;
            this.Language = language ?? throw new ArgumentNullException(nameof(language));
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
        public CultureInfo Language { get; private set; }

        /// <inheritdoc />
        public override LanguageDescriptionDto ToContract()
        {
            return new LanguageDescriptionDto { Title = this.Title, Description = this.Description, Language = this.Language };
        }

        /// <inheritdoc />
        public override LanguageDescription FromContract(LanguageDescriptionDto contract)
        {
            return new LanguageDescription { Title = contract.Title, Description = contract.Description, Language = contract.Language };
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override Task<LanguageDescription> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { LanguageTitle.TitleColumn, DescriptionColumn, LanguageTitle.LanguageColumn });
            return Task.FromResult(new LanguageDescription(record[LanguageTitle.TitleColumn].ToString(), record[DescriptionColumn].ToString(), new CultureInfo(record[LanguageTitle.LanguageColumn].ToString())));
        }

        /// <inheritdoc />
        public override void ValidateSaveCandidate()
        {
        }
    }
}