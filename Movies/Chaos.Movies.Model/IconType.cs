//-----------------------------------------------------------------------
// <copyright file="LanguageTitle.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>An image icon.</summary>
    public class IconType
    {
        /// <summary>Initializes a new instance of the <see cref="IconType"/> class.</summary>
        public IconType()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="IconType"/> class.</summary>
        /// <param name="record">The data record containing the data for the language title.</param>
        public IconType(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the id of the <see cref="IconType"/>.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of title of this <see cref="IconType"/> in different languages.</summary>
        public LanguageTitleCollection Titles { get; } = new LanguageTitleCollection();

        /// <summary>Converts this <see cref="IconType"/> to a <see cref="IconTypeDto"/>.</summary>
        /// <returns>The <see cref="IconTypeDto"/>.</returns>
        public IconTypeDto ToContract()
        {
            return new IconTypeDto { Id = this.Id, Titles = this.Titles.ToContract() };
        }

        /// <summary>Updates this <see cref="IconType"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="IconType"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "IconTypeId" });
            this.Id = (int)record["IconTypeId"];
        }
    }
}