//-----------------------------------------------------------------------
// <copyright file="PersonInRoleCollection.cs">
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
    public class PersonInRoleCollection<TParent, TParentDto> : Collectable<PersonInRole, PersonInRoleDto, PersonInRoleCollection<TParent, TParentDto>, TParent, TParentDto>
    {
        /// <summary>The database column for this <see cref="UserRatingCollection{TParent, TParentDto}"/>.</summary>
        private const string UserRatingsColumn = "UserRatings";

        /// <inheritdoc />
        public PersonInRoleCollection(Persistable<TParent, TParentDto> parent)
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
                    table.Columns.Add(new DataColumn(Role.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(Department.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn($"Parent{typeof(TParent).Name}Id", typeof(int)));
                    table.Columns.Add(new DataColumn(UserSingleRating.UserRatingColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(UserSingleRating.UserIdColumn, typeof(int)));
                    foreach (var personInRole in this.Items)
                    {
                        table.Rows.Add(personInRole.Person.Id, personInRole.Role.Id, personInRole.Department.Id, this.ParentId, personInRole.Ratings.UserRating, personInRole.Ratings.UserId);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<PersonInRoleDto> ToContract()
        {
            return this.Items.Select(item => item.ToContract()).ToList().AsReadOnly();
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This method is not supported.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override PersonInRoleCollection<TParent, TParentDto> FromContract(ReadOnlyCollection<PersonInRoleDto> contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            throw new NotSupportedException($"The method {nameof(FromContract)} is not supported for {nameof(UserRatingCollection<TParent, TParentDto>)}");
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override PersonInRoleCollection<TParent, TParentDto> FromContract(ReadOnlyCollection<PersonInRoleDto> contract, Persistable<TParent, TParentDto> parent)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var list = new PersonInRoleCollection<TParent, TParentDto>(parent);
            foreach (var item in contract)
            {
                list.Add(PersonInRole.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="PersonInRoleCollection{TParent, TParentDto}"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), PersonInRole.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(UserRatingCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="PersonInRoleCollection{TParent, TParentDto}"/> is not valid to be saved.</exception>
        public override async Task AddAndSaveAsync(PersonInRole item, UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.AddAndSaveToDatabaseAsync(item, this.GetSaveParameters(), PersonInRole.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(UserRatingCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="PersonInRoleCollection{TParent, TParentDto}"/> is not valid to be saved.</exception>
        public override async Task RemoveAndSaveAsync(PersonInRole item, UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.RemoveAndSaveToDatabaseAsync(item, this.GetSaveParameters(), PersonInRole.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(UserRatingCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="PersonInRoleCollection{TParent, TParentDto}"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
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
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(UserRatingsColumn), this.GetSaveTable }
                });
        }
    }
}
