//-----------------------------------------------------------------------
// <copyright file="WatchCollection.cs">
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
    public class WatchCollection<TParent, TParentDto> : Collectable<Watch, WatchDto, WatchCollection<TParent, TParentDto>, TParent, TParentDto>
    {
        /// <summary>The database column for this <see cref="WatchCollection{TParent, TParentDto}"/>.</summary>
        private const string WatchesColumn = "Watches";

        /// <inheritdoc />
        public WatchCollection(Persistable<TParent, TParentDto> parent)
            : base(parent)
        {
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException" accessor="get">A valid type needs to be specified.</exception>
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(Watch.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(User.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn($"Parent{typeof(TParent).Name}Id", typeof(int)));
                    table.Columns.Add(new DataColumn(Watch.WatchDateColumn, typeof(DateTime)));
                    table.Columns.Add(new DataColumn(Watch.DateUncertainColumn, typeof(bool)));
                    table.Columns.Add(new DataColumn(WatchType.IdColumn, typeof(int)));
                    foreach (var watch in this.Items)
                    {
                        watch.ValidateSaveCandidate();
                        table.Rows.Add(watch.Id, watch.UserId, this.ParentId, watch.WatchDate, watch.DateUncertain, watch.WatchType.Id);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<WatchDto> ToContract()
        {
            return this.Items.Select(item => item.ToContract()).ToList().AsReadOnly();
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This method is not supported.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override WatchCollection<TParent, TParentDto> FromContract(ReadOnlyCollection<WatchDto> contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            throw new NotSupportedException($"The method {nameof(FromContract)} is not supported for {nameof(WatchCollection<TParent, TParentDto>)}");
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override WatchCollection<TParent, TParentDto> FromContract(ReadOnlyCollection<WatchDto> contract, Persistable<TParent, TParentDto> parent)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var list = new WatchCollection<TParent, TParentDto>(parent);
            foreach (var item in contract)
            {
                list.Add(Watch.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Watch"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), Watch.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(WatchCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Watch"/> is not valid to be saved.</exception>
        public override async Task AddAndSaveAsync(Watch item, UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.AddAndSaveToDatabaseAsync(item, this.GetSaveParameters(), Watch.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(WatchCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Watch"/> is not valid to be saved.</exception>
        public override async Task RemoveAndSaveAsync(Watch item, UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.RemoveAndSaveToDatabaseAsync(item, this.GetSaveParameters(), Watch.Static.ReadFromRecordsAsync, session);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.ErrorGenericNotSupportedInService, nameof(WatchCollection<TParent, TParentDto>)));
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="WatchCollection{TParent, TParentDto}"/> is not valid to be saved.</exception>
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
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Watch"/> is not valid to be saved.</exception>
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(WatchesColumn), this.GetSaveTable }
                });
        }
    }
}
