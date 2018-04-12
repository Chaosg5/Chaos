//-----------------------------------------------------------------------
// <copyright file="ExternalLookupCollection.cs">
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
    /// <remarks>This does only support a single entry for each <see cref="ExternalSource"/>.</remarks>
    public class ExternalLookupCollection : Listable<ExternalLookup, ExternalLookupDto, ExternalLookupCollection>
    {
        /// <summary>The database column for <see cref="ExternalLookupCollection"/>.</summary>
        internal const string ExternalLookupColumn = "ExternalLookups";

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(ExternalSource.IdColumn, typeof(int)));
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

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override ExternalLookupCollection FromContract(ReadOnlyCollection<ExternalLookupDto> contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var list = new ExternalLookupCollection();
            foreach (var item in contract)
            {
                list.Add(ExternalLookup.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">This <see cref="ExternalLookupCollection"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            foreach (var item in this.Items)
            {
                item.ValidateSaveCandidate();
            }
        }
    }
}