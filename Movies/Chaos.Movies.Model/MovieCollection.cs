//-----------------------------------------------------------------------
// <copyright file="MovieCollection.cs">
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
    public class MovieCollection : Orderable<Movie, MovieDto, MovieCollection, ReadOnlyCollection<MovieDto>>
    {
        /// <summary>The database column for <see cref="MovieCollection"/>.</summary>
        internal const string MoviesColumn = "Movies";

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(Movie.IdColumn, typeof(int)));
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
        public override ReadOnlyCollection<MovieDto> ToContract()
        {
            return new ReadOnlyCollection<MovieDto>(this.Items.Select(item => item.ToContract()).ToList());
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<MovieDto> ToContract(string languageName)
        {
            return new ReadOnlyCollection<MovieDto>(this.Items.Select(item => item.ToContract(languageName)).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override MovieCollection FromContract(ReadOnlyCollection<MovieDto> contract)
        {
            if (contract == null)
            {
                return new MovieCollection();
            }

            var list = new MovieCollection();
            foreach (var item in contract)
            {
                list.Add(Movie.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieCollection"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (this.Items.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }

            foreach (var item in this.Items)
            {
                item.ValidateSaveCandidate();
            }
        }
    }
}