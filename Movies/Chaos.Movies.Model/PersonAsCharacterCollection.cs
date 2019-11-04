//-----------------------------------------------------------------------
// <copyright file="PersonAsCharacterCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;
    using Chaos.Movies.Model.Properties;

    /// <summary>A <see cref="Person"/>s in a <typeparamref name="TParent"/>.</summary>
    /// <typeparam name="TParent">The parent type of the owner of the collection.</typeparam>
    /// <typeparam name="TParentDto">The data transfer type to use for communicating the <typeparamref name="TParent"/>.</typeparam>
    public class PersonAsCharacterCollection<TParent, TParentDto>
        : Collectable<PersonAsCharacter, PersonAsCharacterDto, PersonAsCharacterCollection<TParent, TParentDto>, ReadOnlyCollection<PersonAsCharacterDto>, TParent, TParentDto>
    {
        /// <summary>The database column for this <see cref="PersonAsCharacterCollection{TParent, TParentDto}"/>.</summary>
        internal const string PersonAsCharacterColumn = "PeopleAsCharacters";

        /// <inheritdoc />
        public PersonAsCharacterCollection(Persistable<TParent, TParentDto> parent)
            : base(parent)
        {
        }

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(Person.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(Character.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn($"Parent{typeof(TParent).Name}Id", typeof(int)));
                    table.Columns.Add(new DataColumn(UserSingleRating.RatingColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(User.IdColumn, typeof(int)));
                    foreach (var personAsCharacter in this.Items)
                    {
                        table.Rows.Add(
                            personAsCharacter.PersonInRole.Person.Id,
                            personAsCharacter.Character.Id,
                            this.ParentId,
                            personAsCharacter.UserRatings.Value,
                            personAsCharacter.UserRatings.UserId);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<PersonAsCharacterDto> ToContract()
        {
            return this.Items.Select(item => item.ToContract()).ToList().AsReadOnly();
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<PersonAsCharacterDto> ToContract(string languageName)
        {
            return this.Items.Select(item => item.ToContract(languageName)).ToList().AsReadOnly();
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This method is not supported.</exception>
        public override PersonAsCharacterCollection<TParent, TParentDto> FromContract(ReadOnlyCollection<PersonAsCharacterDto> contract)
        {
            throw new NotSupportedException(
                $"The method {nameof(FromContract)} is not supported for {nameof(PersonAsCharacterCollection<TParent, TParentDto>)}");
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override PersonAsCharacterCollection<TParent, TParentDto> FromContract(
            ReadOnlyCollection<PersonAsCharacterDto> contract,
            Persistable<TParent, TParentDto> parent)
        {
            if (contract == null)
            {
                return new PersonAsCharacterCollection<TParent, TParentDto>(parent);
            }

            var list = new PersonAsCharacterCollection<TParent, TParentDto>(parent);
            foreach (var item in contract)
            {
                list.Add(PersonAsCharacter.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), PersonAsCharacter.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    Resources.ErrorGenericNotSupportedInService,
                    nameof(PersonAsCharacterCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task AddAndSaveAsync(PersonAsCharacter item, UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.AddAndSaveToDatabaseAsync(item, this.GetSaveParameters(), PersonAsCharacter.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    Resources.ErrorGenericNotSupportedInService,
                    nameof(PersonAsCharacterCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task RemoveAndSaveAsync(PersonAsCharacter item, UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.RemoveAndSaveToDatabaseAsync(item, this.GetSaveParameters(), PersonAsCharacter.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    Resources.ErrorGenericNotSupportedInService,
                    nameof(PersonAsCharacterCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.ParentId <= 0)
            {
                throw new PersistentObjectRequiredException("The parent of the collection has to be saved before saving the collection.");
            }

            foreach (var userRating in this.Items)
            {
                userRating.ValidateSaveCandidate();
            }
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object> { { Persistent.ColumnToVariable(PersonAsCharacterColumn), this.GetSaveTable } });
        }
    }
}