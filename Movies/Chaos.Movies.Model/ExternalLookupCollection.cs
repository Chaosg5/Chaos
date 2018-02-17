//-----------------------------------------------------------------------
// <copyright file="ExternalLookupCollection.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Contract;

    /// <summary>Represents a user.</summary>
    /// <remarks>This does only support a single entry for each <see cref="ExternalSource"/>.</remarks>
    public class ExternalLookupCollection : Listable<ExternalLookup, ExternalLookupDto, ExternalLookupCollection>
    {
        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(ExternalSource.ExternalSourceIdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(ExternalLookup.ExternalIdColumn, typeof(string)));
                    foreach (var lookup in this.Items)
                    {
                        table.Rows.Add(lookup.ExternalSource.Id, lookup.ExternalId);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<ExternalLookupDto> ToContract()
        {
            return new ReadOnlyCollection<ExternalLookupDto>(this.Items.Select(item => item.ToContract()).ToList());
        }
    }
}