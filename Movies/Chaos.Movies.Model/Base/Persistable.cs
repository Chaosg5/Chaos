﻿//-----------------------------------------------------------------------
// <copyright file="Persistable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    
    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public abstract class Persistable<T, TDto> : Loadable<T, TDto>
    {
        /// <summary>Initializes a new instance of the <see cref="Persistable{T,TDto}"/> class.</summary>
        /// <param name="dto">The <typeparamref name="TDto"/> to create the <typeparamref name="T"/> from.</param>
        protected Persistable(TDto dto)
            : base(dto)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Persistable{T,TDto}"/> class.</summary>
        protected Persistable()
        {
        }

        /// <summary>Saves this <typeparamref name="T"/> to the database.</summary>
        /// <param name="commandParameters">The list of key/values to add <see cref="SqlParameter"/>s to the <see cref="SqlCommand"/>.</param>
        /// <param name="readFromRecord">The callback method to use for reading the <typeparamref name="T"/> from data to object after being saved.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="commandParameters"/> or <parmref name="readFromRecord"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected async Task SaveToDatabaseAsync(IReadOnlyDictionary<string, object> commandParameters, Func<IDataRecord, Task> readFromRecord)
        {
            if (commandParameters == null)
            {
                throw new ArgumentNullException(nameof(commandParameters));
            }

            if (readFromRecord == null)
            {
                throw new ArgumentNullException(nameof(readFromRecord));
            }
            
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{typeof(T).Name}Save", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (var commandParameter in commandParameters)
                {
                    command.Parameters.AddWithValue(commandParameter.Key, commandParameter.Value);
                }

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        readFromRecord(reader);
                    }
                }
            }
        }

        /// <summary>Gets SQL parameters to use for <see cref="IPersistable{T,TDto}.SaveAsync"/>.</summary>
        /// <returns>The list of SQL parameters.</returns>
        protected abstract IReadOnlyDictionary<string, object> GetSaveParameters();
    }
}