//-----------------------------------------------------------------------
// <copyright file="IconCollection.cs">
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

    /// <summary>The title of a movie.</summary>
    public class IconCollection : Orderable<Icon, IconDto, IconCollection, ReadOnlyCollection<IconDto>>
    {
        /// <summary>The database column for <see cref="IconCollection"/>.</summary>
        internal const string IconsColumn = "Icons";

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(Icon.IdColumn, typeof(int)));
                    table.Columns.Add(new DataColumn(OrderColumn, typeof(int)));
                    for (var i = 0; i < this.Items.Count; i++)
                    {
                        table.Rows.Add(this.Items[i].Id, i + 1);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<IconDto> ToContract()
        {
            return new ReadOnlyCollection<IconDto>(this.Items.Select(i => i.ToContract()).ToList());
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<IconDto> ToContract(string languageName)
        {
            return new ReadOnlyCollection<IconDto>(this.Items.Select(i => i.ToContract(languageName)).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override IconCollection FromContract(ReadOnlyCollection<IconDto> contract)
        {
            if (contract == null)
            {
                return new IconCollection();
            }

            var list = new IconCollection();
            foreach (var item in contract)
            {
                list.Add(Icon.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        public override void ValidateSaveCandidate()
        {
            foreach (var item in this.Items)
            {
                item.ValidateSaveCandidate();
            }
        }
    }
}