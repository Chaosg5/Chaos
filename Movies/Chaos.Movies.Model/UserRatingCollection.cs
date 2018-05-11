//-----------------------------------------------------------------------
// <copyright file="UserRatingCollection.cs">
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

    /// <summary>A rating for a <typeparamref name="TParent"/> set by a <see cref="User"/>.</summary>
    /// <typeparam name="TParent">The parent type of the owner of the collection.</typeparam>
    /// <typeparam name="TParentDto">The data transfer type to use for communicating the <typeparamref name="TParent"/>.</typeparam>
    public class UserRatingCollection<TParent, TParentDto> : Collectable<UserRating, UserRatingDto, UserRatingCollection<TParent, TParentDto>, TParent, TParentDto>
    {
        /// <summary>The database column for this <see cref="UserRatingCollection{TParent, TParentDto}"/>.</summary>
        private const string UserRatingsColumn = "UserRatings";

        /// <inheritdoc />
        public UserRatingCollection(Persistable<TParent, TParentDto> parent)
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
                    table.Columns.Add(new DataColumn(User.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(RatingType.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn($"Parent{typeof(TParent).Name}Id", typeof(int)));
                    table.Columns.Add(new DataColumn(RatingValue.RatingColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(UserRating.CreatedDateColumn, typeof(DateTime)));
                    foreach (var rating in this.Items)
                    {
                        table.Rows.Add(rating.UserId, rating.RatingType.Id, this.ParentId, rating.ActualRating, rating.CreatedDate);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<UserRatingDto> ToContract()
        {
            return this.Items.Select(item => item.ToContract()).ToList().AsReadOnly();
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This method is not supported.</exception>
        public override UserRatingCollection<TParent, TParentDto> FromContract(ReadOnlyCollection<UserRatingDto> contract)
        {
            throw new NotSupportedException($"The method {nameof(FromContract)} is not supported for {nameof(UserRatingCollection<TParent, TParentDto>)}");
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override UserRatingCollection<TParent, TParentDto> FromContract(ReadOnlyCollection<UserRatingDto> contract, Persistable<TParent, TParentDto> parent)
        {
            if (contract == null)
            {
                return new UserRatingCollection<TParent, TParentDto>(parent);
            }

            var list = new UserRatingCollection<TParent, TParentDto>(parent);
            foreach (var item in contract)
            {
                list.Add(UserRating.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="UserRating"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), UserRating.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(UserRatingCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="UserRating"/> is not valid to be saved.</exception>
        public override async Task AddAndSaveAsync(UserRating item, UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.AddAndSaveToDatabaseAsync(item, this.GetSaveParameters(), UserRating.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(UserRatingCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="UserRating"/> is not valid to be saved.</exception>
        public override async Task RemoveAndSaveAsync(UserRating item, UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.RemoveAndSaveToDatabaseAsync(item, this.GetSaveParameters(), UserRating.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(UserRatingCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="UserRatingCollection{TParent, TParentDto}"/> is not valid to be saved.</exception>
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
