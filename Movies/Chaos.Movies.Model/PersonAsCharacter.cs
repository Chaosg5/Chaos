//-----------------------------------------------------------------------
// <copyright file="PersonAsCharacter.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public class PersonAsCharacter : Loadable<PersonAsCharacter, PersonAsCharacterDto>
    {
        /// <summary>Private part of the <see cref="Character"/> property.</summary>
        private Character character;

        /// <summary>Private part of the <see cref="PersonInRole"/> property.</summary>
        private PersonInRole personInRoleInRole;

        /// <summary>Initializes a new instance of the <see cref="PersonAsCharacter"/> class.</summary>
        /// <param name="personInRole">The <see cref="Model.PersonInRole"/> to set.</param>
        /// <param name="character">The <see cref="Character"/> to set.</param>
        public PersonAsCharacter(PersonInRole personInRole, Character character)
        {
            this.PersonInRole = personInRole;
            this.Character = character;
        }

        /// <summary>Prevents a default instance of the <see cref="PersonAsCharacter"/> class from being created.</summary>
        private PersonAsCharacter()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static PersonAsCharacter Static { get; } = new PersonAsCharacter();

        /// <summary>Gets the character.</summary>
        public Character Character
        {
            get => this.character;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(this.Character)} has to be saved.");
                }

                this.character = value;
            }
        }

        /// <summary>Gets the person playing the <see cref="Character"/>.</summary>
        public PersonInRole PersonInRole
        {
            get => this.personInRoleInRole;
            private set
            {
                if (value?.Person == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Person.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(this.PersonInRole)} has to be saved.");
                }

                this.personInRoleInRole = value;
            }
        }

        /// <summary>Gets the user ratings.</summary>
        public UserSingleRating UserRatings { get; private set; } = new UserSingleRating();

        /// <summary>Gets the total rating.</summary>
        public TotalRating TotalRating { get; private set; } = new TotalRating(typeof(PersonAsCharacter));

        /// <inheritdoc />
        public override PersonAsCharacterDto ToContract()
        {
            return new PersonAsCharacterDto
            {
                Character = this.Character.ToContract(),
                PersonInRole = this.PersonInRole.ToContract(),
                UserRating = this.UserRatings.ToContract(),
                TotalRating = this.TotalRating.ToContract()
            };
        }

        /// <inheritdoc />
        public override PersonAsCharacterDto ToContract(string languageName)
        {
            return new PersonAsCharacterDto
            {
                Character = this.Character.ToContract(languageName),
                PersonInRole = this.PersonInRole.ToContract(languageName),
                UserRating = this.UserRatings.ToContract(languageName),
                TotalRating = this.TotalRating.ToContract(languageName)
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override PersonAsCharacter FromContract(PersonAsCharacterDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }
            
            return new PersonAsCharacter
            {
                Character = Character.Static.FromContract(contract.Character),
                PersonInRole = PersonInRole.Static.FromContract(contract.PersonInRole),
                UserRatings = this.UserRatings.FromContract(contract.UserRating),
                TotalRating = this.TotalRating.FromContract(contract.TotalRating)
            };
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }

        /// <summary>Creates new <see cref="PersonAsCharacter"/>s from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing data sets and records the data for the <see cref="PersonAsCharacter"/>s.</param>
        /// <returns>The list of <see cref="PersonAsCharacter"/>s.</returns>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        internal async Task<IEnumerable<PersonAsCharacter>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var personAsCharacters = new List<PersonAsCharacter>();
            if (!reader.HasRows)
            {
                return personAsCharacters;
            }

            while (await reader.ReadAsync())
            {
                personAsCharacters.Add(await this.NewFromRecordAsync(reader));
            }

            return personAsCharacters;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<PersonAsCharacter> NewFromRecordAsync(IDataRecord record)
        {
            var result = new PersonAsCharacter();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { Character.IdColumn });
            this.Character = await GlobalCache.GetCharacterAsync((int)record[Character.IdColumn]);
            this.PersonInRole = await PersonInRole.Static.NewFromRecordAsync(record);
            this.TotalRating = await this.TotalRating.NewFromRecordAsync(record);
        }
    }
}