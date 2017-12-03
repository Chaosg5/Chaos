//-----------------------------------------------------------------------
// <copyright file="Readable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public abstract class Readable<T, TDto> : Persistable<T, TDto>
    {
        /// <summary>Initializes a new instance of the <see cref="Readable{T,TDto}"/> class.</summary>
        /// <param name="record">The record containing the data for the <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null"/></exception>
        protected Readable(IDataRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Readable{T,TDto}"/> class.</summary>
        /// <param name="dto">The <typeparamref name="TDto"/> to create the <typeparamref name="T"/> from.</param>
        protected Readable(TDto dto)
            : base(dto)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Readable{T,TDto}"/> class.</summary>
        protected Readable()
        {
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
        
        /// <summary>Creates new <typeparamref name="T"/>s from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing data sets and records the data for the <typeparamref name="T"/>s.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        protected abstract Task<IEnumerable<T>> ReadFromRecordsAsync(DbDataReader reader);
    }
}
