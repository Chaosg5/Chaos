//-----------------------------------------------------------------------
// <copyright file="Icons.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Linq;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The title of a movie.</summary>
    public class IconCollection : IReadOnlyCollection<Icon>
    {
        /// <summary>The list of <see cref="Icon"/>s in this <see cref="IconCollection"/>.</summary>
        private readonly List<Icon> icons = new List<Icon>();

        /// <summary>Initializes a new instance of the <see cref="IconCollection" /> class.</summary>
        public IconCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="IconCollection" /> class.</summary>
        /// <param name="reader">The reader containing the data for the movie series type.</param>
        public IconCollection(IDataReader reader)
        {
            this.ReadFromRecord(reader);
        }
        
        /// <summary>Gets the number if existing titles.</summary>
        public int Count => this.icons.Count;
        
        /// <summary>Gets all titles in a table which can be used to save them to the database.</summary>
        /// <returns>A table containing the title and language as columns for each title.</returns>
        public DataTable GetSaveIcons
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn("IconId", typeof(int)));
                    table.Columns.Add(new DataColumn("IconTypeId", typeof(int)));
                    table.Columns.Add(new DataColumn("IconUrl", typeof(string)));
                    table.Columns.Add(new DataColumn("Data", typeof(Binary)));
                    table.Columns.Add(new DataColumn("DataSize", typeof(int)));
                    foreach (var icon in this.icons)
                    {
                        table.Rows.Add(icon.Id, icon.IconType.Id, icon.Url, icon.Data, icon.Size);
                    }

                    return table;
                }
            }
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="IconCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="IconCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<Icon> GetEnumerator()
        {
            return this.icons.GetEnumerator();
        }

        /// <summary>Converts this <see cref="IconCollection"/> to a <see cref="ReadOnlyCollection{IconDto}"/>.</summary>
        /// <returns>The <see cref="ReadOnlyCollection{IconDto}"/>.</returns>
        public ReadOnlyCollection<IconDto> ToContract()
        {
            return new ReadOnlyCollection<IconDto>(this.icons.Select(i => i.ToContract()).ToList());
        }

        /// <summary>Updates this <see cref="IconCollection"/> from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing the data for the <see cref="IconCollection"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="reader"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.icons.Clear();
            while (reader.Read())
            {
                this.icons.Add(new Icon(reader));
            }
        }
    }
}