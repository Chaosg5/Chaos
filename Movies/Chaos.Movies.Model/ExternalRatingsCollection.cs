//-----------------------------------------------------------------------
// <copyright file="ExternalRatingsCollection.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Contract;

    /// <summary>Represents a user.</summary>
    public class ExternalRatingsCollection
        : Listable<ExternalRating, ExternalRatingsDto>,
            IListable<ExternalRating>,
            ICommunicable<ExternalRatingsCollection, ReadOnlyCollection<ExternalRatingsDto>>
    {
        /// <inheritdoc />
        public DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(ExternalSource.ExternalSourceIdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(ExternalRating.ExternalRatingColumn, typeof(double)));
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
        public ReadOnlyCollection<ExternalRatingsDto> ToContract()
        {
            return new ReadOnlyCollection<ExternalRatingsDto>(this.Items.Select(item => item.ToContract()).ToList());
        }
    }
}