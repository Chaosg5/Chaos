//-----------------------------------------------------------------------
// <copyright file="GenreCollection.cs">
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

    /// <summary>A genre of <see cref="Movie"/>s.</summary>
    public class GenreCollection : Listable<Genre, GenreDto, GenreCollection>
    {
        /// <summary>The database column for <see cref="GenreCollection"/>.</summary>
        internal const string GenresColumn = "Genres";

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(Genre.IdColumn, typeof(int)));
                    foreach (var genre in this.Items)
                    {
                        table.Rows.Add(genre.Id);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<GenreDto> ToContract()
        {
            return new ReadOnlyCollection<GenreDto>(this.Items.Select(item => item.ToContract()).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override GenreCollection FromContract(ReadOnlyCollection<GenreDto> contract)
        {
            if (contract == null)
            {
                return new GenreCollection();
            }
            
            var list = new GenreCollection();
            foreach (var item in contract)
            {
                list.Add(Genre.Static.FromContract(item));
            }

            return list;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="GenreCollection"/> is not valid to be saved.</exception>
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