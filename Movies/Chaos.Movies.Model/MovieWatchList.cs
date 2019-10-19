//-----------------------------------------------------------------------
// <copyright file="MovieWatchList.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a <see cref="Movie"/> in a list.</summary>
    public class MovieWatchList : Rateable<MovieWatchList, MovieListItemDto, IReadOnlyCollection<MovieListItemDto>>
    {
        /// <summary>Gets the <see cref="Movie"/>.</summary>
        public Movie Movie { get; private set; }

        /// <summary>Gets the list rating of the <see cref="Movie"/>.</summary>
        [DataMember]
        public int Rating { get; private set; }

        /// <summary>Gets the <see cref="WatchType"/> of the movie.</summary>
        [DataMember]
        public WatchType WatchType { get; private set; }

        /// <summary>Gets the list date of the <see cref="Movie"/>.</summary>
        [DataMember]
        public DateTime Date { get; private set; }

        /// <inheritdoc />
        public override MovieListItemDto ToContract()
        {
            return new MovieListItemDto
            {
                Id = this.Id,
                Movie = this.Movie.ToContract(),
                Rating = this.Rating,
                WatchType = this.WatchType.ToContract(),
                Date = this.Date
            };
        }

        /// <inheritdoc />
        public override MovieListItemDto ToContract(string languageName)
        {
            return new MovieListItemDto
            {
                Id = this.Id,
                Movie = this.Movie.ToContract(languageName),
                Rating = this.Rating,
                WatchType = this.WatchType.ToContract(languageName),
                Date = this.Date
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override MovieWatchList FromContract(MovieListItemDto contract)
        {
            return new MovieWatchList
            {
                Id = this.Id,
                Movie = Movie.FromContract(contract.Movie),
                Rating = this.Rating,
                WatchType = WatchType.FromContract(contract.WatchType),
                Date = this.Date
            };
        }

        /// <inheritdoc />
        public override Task SaveAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<MovieWatchList> GetAsync(UserSession session, int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<IEnumerable<MovieWatchList>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task GetUserRatingsAsync(ICollection<MovieListItemDto> items, UserSession session)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override async Task<IReadOnlyCollection<MovieListItemDto>> GetUserItemDetailsAsync(MovieListItemDto item, UserSession session, string languageName)
        {
            if (!Persistent.UseService)
            {
                return await this.GetUserDetailsFromDatabaseAsync(item, item.Id, this.ReadUserDetailsAsync, session, languageName);
            }

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override Task<IEnumerable<MovieWatchList>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override Task<MovieWatchList> NewFromRecordAsync(IDataRecord record)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task ReadUserRatingsAsync(IEnumerable<MovieListItemDto> items, int userId, DbDataReader reader)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task<IReadOnlyCollection<MovieListItemDto>> ReadUserDetailsAsync(MovieListItemDto item, int userId, DbDataReader reader, string languageName)
        {
            throw new NotImplementedException();
        }
    }
}