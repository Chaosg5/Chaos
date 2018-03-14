//-----------------------------------------------------------------------
// <copyright file="Readable.cs">
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

    /// <summary>Represents a persitable object that can be saved to the database and then read up again as a self-containing entity.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public abstract class Readable<T, TDto> : Persistable<T, TDto>
    {
        /// <summary>Gets the specified <typeparamref name="T"/>.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="id">The id of the <typeparamref name="T"/> to get.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        public abstract Task<T> GetAsync(UserSession session, int id);

        /// <summary>Gets the specified <typeparamref name="T"/>s.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="idList">The list of ids of the <typeparamref name="T"/>s to get.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        public abstract Task<IEnumerable<T>> GetAsync(UserSession session, IEnumerable<int> idList);

        /// <summary>Creates new <typeparamref name="T"/>s from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing data sets and records the data for the <typeparamref name="T"/>s.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        internal abstract Task<IEnumerable<T>> ReadFromRecordsAsync(DbDataReader reader);

        /// <summary>Gets a <typeparamref name="T"/> from the <paramref name="items"/> with the id specified in the <paramref name="record"/>.</summary>
        /// <param name="items">The list of <typeparamref name="T"/>s.</param>
        /// <param name="record">The record.</param>
        /// <param name="idColumnName">The name of the id column.</param>
        /// <returns>The <typeparamref name="T"/>.</returns>
        /// <exception cref="SqlResultSyncException">The specified <typeparamref name="T"/> was not found in the <paramref name="items"/>.</exception>
        protected Persistable<T, TDto> GetFromResultsByIdInRecord(IEnumerable<Persistable<T, TDto>> items, IDataRecord record, string idColumnName)
        {
            var id = (int)record[idColumnName];
            var item = items.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                throw new SqlResultSyncException(id, typeof(T));
            }

            return item;
        }

        /// <summary>Gets the specified <typeparamref name="T"/>s.</summary>
        /// <param name="idList">The list of ids of the <typeparamref name="T"/>s to get.</param>
        /// <param name="readFromRecords">The callback method to use for reading the <typeparamref name="T"/>s from data to object.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="idList"/> or <paramref name="readFromRecords"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        protected async Task<IEnumerable<T>> GetFromDatabaseAsync(IEnumerable<int> idList, Func<DbDataReader, Task<IEnumerable<T>>> readFromRecords)
        {
            if (readFromRecords == null)
            {
                throw new ArgumentNullException(nameof(readFromRecords));
            }

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{typeof(T).Name}Get", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idList", Persistent.CreateIdCollectionTable(idList));
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return await readFromRecords(reader);
                }
            }
        }
    }
}
