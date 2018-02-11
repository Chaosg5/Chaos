//-----------------------------------------------------------------------
// <copyright file="GenreCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;

    using Chaos.Movies.Contract;

    /// <summary>A genre of <see cref="Movie"/>s.</summary>
    public class GenreCollection : Listable<Genre, GenreDto>, IListable<Genre>, ICommunicable<GenreCollection, ReadOnlyCollection<GenreDto>>
    {
        public DataTable GetSaveTable { get; }
        
        /// <inheritdoc />
        public ReadOnlyCollection<GenreDto> ToContract()
        {
            return new ReadOnlyCollection<GenreDto>(this.Items.Select(item => item.ToContract()).ToList());
        }
    }
}