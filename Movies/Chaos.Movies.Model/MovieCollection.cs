//-----------------------------------------------------------------------
// <copyright file="MovieCollection.cs" company="Erik Bunnstad">
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
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a user.</summary>
    public class MovieCollection : Orderable<Movie, MovieDto, MovieCollection>
    {
        /// <summary>The database column for <see cref="MovieCollection"/>.</summary>
        public const string MoviesColumn = "Movies";

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(Movie.IdColumn));
                    table.Columns.Add(new DataColumn(OrderColumn));
                    for (var i = 0; i < this.Items.Count; i++)
                    {
                        table.Rows.Add(this.Items[i].Id, i + 1);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<MovieDto> ToContract()
        {
            return new ReadOnlyCollection<MovieDto>(this.Items.Select(item => item.ToContract()).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override MovieCollection FromContract(ReadOnlyCollection<MovieDto> contract)
        {
            var list = new MovieCollection();
            foreach (var item in contract)
            {
                list.Add(Movie.Static.FromContract(item));
            }

            return list;
        }
    }
}