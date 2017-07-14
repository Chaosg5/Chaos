//-----------------------------------------------------------------------
// <copyright file="Character.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public class Character : ICharacter
    {
        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name;

        /// <summary>Private part of the <see cref="ImdbId"/> property.</summary>
        private string imdbId = string.Empty;

        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="name">The name of the character.</param>
        public Character(string name)
        {
            this.Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        public Character(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of the character.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the name of the character.</summary>
        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="value"/> is <see langword="null" />.</exception>
        public string Name
        {
            get => this.name;

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.name = value;
            }
        }

        /// <summary>Gets the id of the character in IMDB.</summary>
        public string ImdbId
        {
            get => this.imdbId;
            private set => this.imdbId = value ?? string.Empty;
        }

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images => this.images.AsReadOnly();
        
        /// <summary>Updates a character from a record.</summary>
        /// <param name="character">The character to update.</param>
        /// <param name="record">The record containing the data for the character.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected static void ReadFromRecord(Character character, IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "CharacterId", "Name", "ImdbId" });
            character.Id = (int)record["CharacterId"];
            character.Name = record["Name"].ToString();
            character.ImdbId = record["ImdbId"].ToString();
        }
    }
}
