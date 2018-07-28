//-----------------------------------------------------------------------
// <copyright file="Persistable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
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
        /// <summary>Gets or sets the id of this <typeparamref name="T"/>.</summary>
        public int Id { get; protected set; }

        /// <summary>The database column for the id of the <typeparamref name="T"/>.</summary>
        internal static string IdColumn => $"{typeof(T).Name}Id";
        
        /// <summary>Saves this <typeparamref name="T"/> to the database.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public abstract Task SaveAsync(UserSession session);

        /// <summary>Performs a custom database action.</summary>
        /// <param name="commandParameters">The list of key/values to add <see cref="SqlParameter"/>s to the <see cref="SqlCommand"/>.</param>
        /// <param name="procedureSuffix">The name of the stored procedure to call.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="commandParameters"/> or <parmref name="commandParameters"/> is <see langword="null"/></exception>
        protected static async Task CustomDatabaseActionAsync(IReadOnlyDictionary<string, object> commandParameters, string procedureSuffix, UserSession session)
        {
            if (commandParameters == null)
            {
                throw new ArgumentNullException(nameof(commandParameters));
            }

            if (string.IsNullOrWhiteSpace(procedureSuffix))
            {
                throw new ArgumentNullException(nameof(procedureSuffix));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            await session.ValidateSessionAsync();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{typeof(T).Name}{procedureSuffix}", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (var commandParameter in commandParameters)
                {
                    command.Parameters.AddWithValue(commandParameter.Key, commandParameter.Value);
                }

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>Saves this <typeparamref name="T"/> to the database.</summary>
        /// <param name="commandParameters">The list of key/values to add <see cref="SqlParameter"/>s to the <see cref="SqlCommand"/>.</param>
        /// <param name="readFromRecord">The callback method to use for reading the <typeparamref name="T"/> from data to object after being saved.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="commandParameters"/> or <parmref name="readFromRecord"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected async Task SaveToDatabaseAsync(IReadOnlyDictionary<string, object> commandParameters, Func<IDataRecord, Task> readFromRecord, UserSession session)
        {
            if (commandParameters == null)
            {
                throw new ArgumentNullException(nameof(commandParameters));
            }

            if (readFromRecord == null)
            {
                throw new ArgumentNullException(nameof(readFromRecord));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            
            await session.ValidateSessionAsync();
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
                        await readFromRecord(reader);
                    }
                }
            }
        }

        /// <summary>Gets SQL parameters to use for <see cref="Persistable{T,TDto}.SaveAsync"/>.</summary>
        /// <returns>The list of SQL parameters.</returns>
        protected abstract IReadOnlyDictionary<string, object> GetSaveParameters();
    }
}
