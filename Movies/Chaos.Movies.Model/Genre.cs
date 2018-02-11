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
    public class Genre : Typeable<Genre, GenreDto>, ITypeable<Genre, GenreDto>
    {
        /// <summary>Gets the id of the genre.</summary>
        public int Id { get; private set; }

        public Task SaveAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Gets the id of the <see cref="Genre"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookup { get; } = new ExternalLookupCollection();

        /// <summary>Gets the title of the genre.</summary>
        public string Title { get; private set; }

        public override Task<Genre> ReadFromRecordAsync(IDataRecord record)
        {
            throw new System.NotImplementedException();
        }

        public override void ValidateSaveCandidate()
        {
            throw new System.NotImplementedException();
        }

        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new System.NotImplementedException();
        }
        

        protected override Task<IEnumerable<Genre>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new System.NotImplementedException();
        }

        public GenreDto ToContract()
        {
            throw new System.NotImplementedException();
        }

        public Task<Genre> GetAsync(UserSession session, int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Genre>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveAllAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Genre>> GetAllAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }
    }
}
