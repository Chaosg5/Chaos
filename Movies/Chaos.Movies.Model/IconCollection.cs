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
    public class IconCollection : Listable<Icon, IconDto, IconCollection>
    {
        /// <summary>The database column for <see cref="IconCollection"/>.</summary>
        internal const string IconsColumn = "Icons";

        /// <summary>Gets all titles in a table which can be used to save them to the database.</summary>
        /// <returns>A table containing the title and language as columns for each title.</returns>
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(Icon.IdColumn, typeof(int)));
                    foreach (var icon in this.Items)
                    {
                        table.Rows.Add(icon.Id);
                    }

                    return table;
                }
            }
        }
        
        /// <summary>Converts this <see cref="IconCollection"/> to a <see cref="ReadOnlyCollection{IconDto}"/>.</summary>
        /// <returns>The <see cref="ReadOnlyCollection{IconDto}"/>.</returns>
        public override ReadOnlyCollection<IconDto> ToContract()
        {
            return new ReadOnlyCollection<IconDto>(this.Items.Select(i => i.ToContract()).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override IconCollection FromContract(ReadOnlyCollection<IconDto> contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var list = new IconCollection();
            foreach (var item in contract)
            {
                list.Add(Icon.Static.FromContract(item));
            }

            return list;
        }
    }
}