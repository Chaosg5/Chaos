//-----------------------------------------------------------------------
// <copyright file="Typeable.cs">
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
    using System.Threading.Tasks;
    
    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public abstract class Typeable<T, TDto> : Readable<T, TDto>
    {
        /// <summary>Gets the all <typeparamref name="T"/>.</summary>
        /// <param name="session">The session.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
        public abstract Task<IEnumerable<T>> GetAllAsync(UserSession session);

        /// <summary>Loads all <typeparamref name="T"/>s from the database.</summary>
        /// <param name="readFromRecords">The callback method to use for reading the <typeparamref name="T"/>s from data to object.</param>
        /// <param name="session">The session.</param>
        /// <returns>All <typeparamref name="T"/>s.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="readFromRecords"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
        protected async Task<IEnumerable<T>> GetAllFromDatabaseAsync(Func<DbDataReader, Task<IEnumerable<T>>> readFromRecords, UserSession session)
        {
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
