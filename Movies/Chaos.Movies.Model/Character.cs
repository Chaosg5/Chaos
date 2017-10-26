//-----------------------------------------------------------------------
// <copyright file="Character.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public class Character
    {
        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name;

        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="name">The name of the character.</param>
        public Character(string name)
        {
            this.Name = name;
        }
        
        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="character">The character to create.</param>
        public Character(CharacterDto character)
        {
            this.Id = character.Id;
            this.Name = character.Name;
        }

        /// <summary>Initializes a new instance of the <see cref="Character" /> class.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        public Character(IDataRecord record)
        {
            this.ReadFromRecord(record);
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

        /// <summary>Gets the id of the <see cref="Character"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookup { get; } = new ExternalLookupCollection();

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        public IEnumerable<Icon> Images => this.images;

        /// <summary>The save async.</summary>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual async Task SaveAsync(UserSession session)
        {
            using (var service = new ChaosMoviesServiceClient())
            {
                await service.CharacterSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <summary>Converts this <see cref="Character"/> to a <see cref="CharacterDto"/>.</summary>
        /// <returns>The <see cref="CharacterDto"/>.</returns>
        public CharacterDto ToContract()
        {
            return new CharacterDto { Id = this.Id, Name = this.Name, ExternalLookup = this.ExternalLookup.ToContract(), Images = this.Images.Select(s => s.ToContract()) };
        }

        /// <summary>Updates a character from a record.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected void ReadFromRecord(IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "CharacterId", "Name" });
            this.Id = (int)record["CharacterId"];
            this.Name = record["Name"].ToString();
            // ToDo get ExternalLookup, Images
        }

        /// <summary>Validates that the this <see cref="Character"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">This <see cref="Character"/> is not valid to be saved.</exception>
        protected void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException("The character's name can not be empty.");
            }
        }
    }
}
