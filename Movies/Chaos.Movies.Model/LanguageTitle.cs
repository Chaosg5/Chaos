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
    public class LanguageTitle : Loadable<LanguageTitle, LanguageTitleDto>, IEquatable<LanguageTitle>
    {
        // ToDo: Replace CultureInfo with an own class with Name, UserTitle, Titles and CultureInfo inside

        /// <summary>The database column for <see cref="Title"/>.</summary>
        internal const string TitleColumn = "Title";

        /// <summary>The database column for <see cref="Language"/>.</summary>
        internal const string LanguageColumn = "Language";

        /// <summary>Private part of the <see cref="Title"/> property.</summary>
        private string title = string.Empty;

        /// <summary>Private part of the <see cref="Language"/> property.</summary>
        private CultureInfo language;

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
            this.Language = language;
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

        /// <summary>Gets the type of the <see cref="LanguageTitle"/>.</summary>
        public LanguageType LanguageType { get; private set; }
        
        /// <summary>Returns a value indicating whether the <paramref name="titleB"/> is equal to the specified <paramref name="titleB"/>.</summary>
        /// <param name="titleA">The first title to compare to the <paramref name="titleB"/>.</param>
        /// <param name="titleB">The second title to compare to the <paramref name="titleA"/>.</param>
        /// <returns><see langword="true"/> if the <paramref name="titleA"/> has the same value the <paramref name="titleB"/>; otherwise <see langword="false"/>.</returns>
        public static bool operator ==(LanguageTitle titleA, LanguageTitle titleB)
        {
            return titleA?.Equals(titleB) ?? object.ReferenceEquals(titleB, null);
        }

        /// <summary>Returns a value indicating whether the <paramref name="titleB"/> is not equal to the specified <paramref name="titleB"/>.</summary>
        /// <param name="titleA">The first title to compare to the <paramref name="titleB"/>.</param>
        /// <param name="titleB">The second title to compare to the <paramref name="titleA"/>.</param>
        /// <returns><see langword="true"/> if the <paramref name="titleA"/> doesn't have the same value the <paramref name="titleB"/>; otherwise <see langword="false"/>.</returns>
        public static bool operator !=(LanguageTitle titleA, LanguageTitle titleB)
        {

            return !(titleA == titleB);
        }

        /// <summary>Returns a value indicating whether this instance is equal to the specified <paramref name="otherTitle"/>.</summary>
        /// <param name="otherTitle">The other title to compare to this instance.</param>
        /// <returns><see langword="true"/> if the <paramref name="otherTitle"/> has the same value as this instance; otherwise <see langword="false"/>.</returns>
        public bool Equals(LanguageTitle otherTitle)
        {
            if (object.ReferenceEquals(null, otherTitle))
            {
                return false;
            }

            if (object.ReferenceEquals(this, otherTitle))
            {
                return true;
            }

            return string.Equals(this.title, otherTitle.title)
                && string.Equals(
                    this.language?.Name ?? this.LanguageType.ToString(),
                    otherTitle.language?.Name ?? otherTitle.LanguageType.ToString());
        }

        /// <summary>Returns a value indicating whether this instance is equal to the specified <paramref name="obj"/>.</summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns><see langword="true"/> if the <paramref name="obj"/> is an <see cref="LanguageTitle"/> and has the same value as this instance; otherwise <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((LanguageTitle)obj);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A hash code for the current <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode
                return ((this.title != null ? this.title.GetHashCode() : 0) * 397) ^ ((this.language != null ? this.language.GetHashCode() : 0) * 397);
                // ReSharper restore NonReadonlyMemberInGetHashCode
            }
        }

        /// <inheritdoc />
        public override LanguageTitleDto ToContract()
        {
            return new LanguageTitleDto { Title = this.Title, Language = this.Language, LanguageType = this.LanguageType };
        }

        /// <inheritdoc />
        public override LanguageTitleDto ToContract(string languageName)
        {
            return new LanguageTitleDto { Title = this.Title, Language = this.Language, LanguageType = this.LanguageType };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override LanguageTitle FromContract(LanguageTitleDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new LanguageTitle { Title = contract.Title, Language = contract.Language, LanguageType = contract.LanguageType };
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<LanguageTitle> NewFromRecordAsync(IDataRecord record)
        {
            var result = new LanguageTitle();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { TitleColumn, LanguageColumn });
            this.Title = (string)record[TitleColumn];
            var languageType = (string)record[LanguageColumn];
            switch (languageType.ToUpperInvariant())
            {
                case "DEFAULT":
                    this.LanguageType = LanguageType.Default;
                    break;
                case "ORIGINAL":
                    this.LanguageType = LanguageType.Original;
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