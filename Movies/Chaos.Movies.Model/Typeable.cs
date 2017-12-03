//-----------------------------------------------------------------------
// <copyright file="Typeable.cs">
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
    
    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public abstract class Typeable<T, TDto> : Readable<T, TDto>
    {
        /// <summary>Initializes a new instance of the <see cref="Typeable{T,TDto}"/> class.</summary>
        /// <param name="record">The record containing the data for the <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null"/></exception>
        protected Typeable(IDataRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Typeable{T,TDto}"/> class.</summary>
        /// <param name="dto">The <typeparamref name="TDto"/> to create the <typeparamref name="T"/> from.</param>
        protected Typeable(TDto dto)
            : base(dto)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Typeable{T,TDto}"/> class.</summary>
        protected Typeable()
        {
        }

        /// <summary>Loads all <typeparamref name="T"/>s from the database.</summary>
        /// <param name="readFromRecords">The callback method to use for reading the <typeparamref name="T"/>s from data to object.</param>
        /// <returns>All <typeparamref name="T"/>s.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="readFromRecords"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected async Task<IEnumerable<T>> GetAllFromDatabaseAsync(Func<DbDataReader, Task<IEnumerable<T>>> readFromRecords)
        {
            if (readFromRecords == null)
            {
                throw new ArgumentNullException(nameof(readFromRecords));
            }

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{typeof(T).Name}GetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    return await readFromRecords(reader);
                }
            }
        }
    }
}
