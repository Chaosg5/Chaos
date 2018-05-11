//-----------------------------------------------------------------------
// <copyright file="ExternalRatingCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a user.</summary>
    public class ExternalRatingCollection : Listable<ExternalRating, ExternalRatingDto, ExternalRatingCollection>
    {
        /// <summary>The database column for <see cref="ExternalRatingCollection"/>.</summary>
        internal const string ExternalRatingsColumn = "ExternalRatings";

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(ExternalSource.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(ExternalRating.RatingColumn, typeof(double)));
                    table.Columns.Add(new DataColumn(ExternalRating.RatingCountColumn, typeof(int)));
                    foreach (var rating in this.Items)
                    {
                        table.Rows.Add(rating.ExternalSource.Id, rating.Rating, rating.RatingCount);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<ExternalRatingDto> ToContract()
        {
            return new ReadOnlyCollection<ExternalRatingDto>(this.Items.Select(item => item.ToContract()).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override ExternalRatingCollection FromContract(ReadOnlyCollection<ExternalRatingDto> contract)
        {
            if (contract == null)
            {
                return new ExternalRatingCollection();
            }
            
            var list = new ExternalRatingCollection();
            foreach (var item in contract)
            {
                list.Add(ExternalRating.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
            foreach (var item in this.Items)
            {
                item.ValidateSaveCandidate();
            }
        }
    }
}