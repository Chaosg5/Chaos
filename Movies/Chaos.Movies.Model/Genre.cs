//-----------------------------------------------------------------------
// <copyright file="Genre.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;

    /// <summary>A genre of <see cref="Movie"/>s.</summary>
    public class Genre : Typeable<Genre, GenreDto>
    {
        /// <summary>Gets the id of the <see cref="Genre"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookup { get; } = new ExternalLookupCollection();

        /// <summary>Gets the title of the genre.</summary>
        public string Title { get; private set; }

        /// <inheritdoc />
        public override Task SaveAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<Genre> ReadFromRecordAsync(IDataRecord record)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override void ValidateSaveCandidate()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override GenreDto ToContract()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<Genre> GetAsync(UserSession session, int id)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<IEnumerable<Genre>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Task SaveAllAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<IEnumerable<Genre>> GetAllAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task<IEnumerable<Genre>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new System.NotImplementedException();
        }
    }
}
