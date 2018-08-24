//-----------------------------------------------------------------------
// <copyright file="Rateable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Model.Exceptions;

    public abstract class Rateable<T, TDto> : Readable<T, TDto> where T : Rateable<T, TDto>
    {
        public abstract Task GetUserRatingsAsync(IEnumerable<TDto> items, UserSession session);

        public abstract Task GetUserItemDetailsAsync(TDto item, UserSession session, string languageName);

        protected abstract Task ReadUserRatingsAsync(IEnumerable<TDto> items, int userId, DbDataReader reader);

        protected abstract Task ReadUserDetailsAsync(TDto item, int userId, DbDataReader reader, string languageName);

        protected async Task GetUserDetailsFromDatabaseAsync(
            TDto item,
            int id,
            Func<TDto, int, DbDataReader, string, Task> readFromRecords,
            UserSession session,
            string languageName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (readFromRecords == null)
            {
                throw new ArgumentNullException(nameof(readFromRecords));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            await session.ValidateSessionAsync();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{typeof(T).Name}GetUserDetails", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(Persistent.ColumnToVariable($"{typeof(T).Name}Id"), id);
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(User.IdColumn), session.UserId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    await readFromRecords(item, session.UserId, reader, languageName);
                }
            }
        }

        /// <summary>Gets <see cref="UserSingleRating"/>s for the <paramref name="items"/>.</summary>
        /// <param name="items">The list of <see cref="Rateable{T,TDto}"/> to get <see cref="UserSingleRating"/>s for.</param>
        /// <param name="ids">The <see cref="Persistable{T,TDto}.Id"/> of each item in <paramref name="items"/>.</param>
        /// <param name="readFromRecords">The callback method to use for reading the <see cref="UserSingleRating"/>s for into the <paramref name="items"/>.</param>
        /// <param name="session">The session containing the <see cref="User"/> for whom to get <see cref="UserSingleRating"/>s for.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> or <paramref name="readFromRecords"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        protected async Task GetUserRatingsFromDatabaseAsync(IEnumerable<TDto> items, IEnumerable<int> ids, Func<IEnumerable<TDto>, int, DbDataReader, Task> readFromRecords, UserSession session)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var itemIds = ids.ToList();
            var itemList = items.ToList();
            if (itemIds == null || !itemIds.Any() || itemIds.Count != itemList.Count)
            {
                throw new ArgumentNullException(nameof(items), Properties.Resources.ErrorRatableItemsNeedsToBePersistable);
            }

            if (readFromRecords == null)
            {
                throw new ArgumentNullException(nameof(readFromRecords));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            
            await session.ValidateSessionAsync();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{typeof(T).Name}GetUserRatings", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(Persistent.ColumnToVariable($"{typeof(T).Name}Ids"), Persistent.CreateIdCollectionTable(itemIds));
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(User.IdColumn), session.UserId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    await readFromRecords(itemList, session.UserId, reader);
                }
            }
        }
    }
}
