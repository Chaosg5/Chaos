//-----------------------------------------------------------------------
// <copyright file="RatingTypeCollection.cs" company="Erik Bunnstad">
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
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a user.</summary>
    public class RatingTypeCollection : Collectable<RatingType, RatingTypeDto, RatingTypeCollection, RatingType, RatingTypeDto>
    {
        /// <summary>The database column for <see cref="RatingTypeCollection"/>.</summary>
        private const string RatingTypesColumn = "RatingTypes";

        /// <inheritdoc />
        public RatingTypeCollection(Persistable<RatingType, RatingTypeDto> parent)
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
                    table.Columns.Add(new DataColumn(RatingType.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn($"Parent{typeof(RatingType).Name}Id", typeof(int)));
                    foreach (var ratingType in this.Items)
                    {
                        table.Rows.Add(ratingType.Id, this.ParentId);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<RatingTypeDto> ToContract()
        {
            return new ReadOnlyCollection<RatingTypeDto>(this.Items.Select(item => item.ToContract()).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">This method is not supported.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override RatingTypeCollection FromContract(ReadOnlyCollection<RatingTypeDto> contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            throw new NotSupportedException($"The method {nameof(FromContract)} is not supported for {nameof(RatingTypeCollection)}");
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task AddAndSaveAsync(RatingType item)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.AddAndSaveToDatabaseAsync(item, this.GetSaveParameters(), RatingType.Static.ReadFromRecordsAsync);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.({T})SaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task RemoveAndSaveAsync(RatingType item)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.RemoveAndSaveToDatabaseAsync(item, this.GetSaveParameters(), RatingType.Static.ReadFromRecordsAsync);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.({T})SaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="parent" /> or <paramref name="contract"/> is <see langword="null" />.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override RatingTypeCollection FromContract(ReadOnlyCollection<RatingTypeDto> contract, Persistable<RatingType, RatingTypeDto> parent)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var list = new RatingTypeCollection(parent);
            foreach (var item in contract)
            {
                list.Add(RatingType.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">The parent of the collection has to be saved before saving the collection.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (this.ParentId <= 0)
            {
                throw new PersistentObjectRequiredException("The parent of the collection has to be saved before saving the collection.");
            }
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(RatingTypesColumn), this.GetSaveTable }
                });
        }
    }
}