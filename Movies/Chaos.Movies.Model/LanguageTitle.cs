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
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The title of a movie.</summary>
    public class LanguageTitle : Loadable<LanguageTitle, LanguageTitleDto>
    {
        /// <summary>The database column for <see cref="Title"/>.</summary>
        internal const string TitleColumn = "Title";

        /// <summary>The database column for <see cref="Language"/>.</summary>
        internal const string LanguageColumn = "Language";

        /// <summary>Private part of the <see cref="Title"/> property.</summary>
        private string title;

        /// <summary>Initializes a new instance of the <see cref="LanguageTitle"/> class.</summary>
        /// <param name="title">The title to set.</param>
        /// <param name="language">The language to set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="title"/> or <paramref name="language"/> is <see langword="null"/></exception>
        public LanguageTitle(string title, CultureInfo language)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.Title = title;
            this.Language = language ?? throw new ArgumentNullException(nameof(language));
        }

        /// <summary>Prevents a default instance of the <see cref="LanguageTitle"/> class from being created.</summary>
        private LanguageTitle()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static LanguageTitle Static { get; } = new LanguageTitle();

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

        /// <summary>Gets the language of the title.</summary>
        public CultureInfo Language { get; private set; }

        /// <inheritdoc />
        public override LanguageTitleDto ToContract()
        {
            return new LanguageTitleDto { Title = this.Title, Language = this.Language };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override LanguageTitle FromContract(LanguageTitleDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new LanguageTitle { Title = contract.Title, Language = contract.Language };
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override Task<LanguageTitle> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { TitleColumn, LanguageColumn });
            return Task.FromResult(new LanguageTitle(record[TitleColumn].ToString(), new CultureInfo(record[LanguageColumn].ToString())));
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }
    }
}