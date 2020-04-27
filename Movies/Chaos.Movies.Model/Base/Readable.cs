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

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <inheritdoc />
    /// <summary>Represents a persitable object that can be saved to the database and then read up again as a self-containing entity.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T" />.</typeparam>
    public abstract class Readable<T, TDto> : Persistable<T, TDto>
    {
        /// <summary>The database column for <see cref="SearchParametersDto.SearchText"/>.</summary>
        private const string SearchTextColumn = "SearchText";

        /// <summary>The database column for <see cref="SearchParametersDto.SearchLimit"/>.</summary>
        private const string SearchLimitColumn = "SearchLimit";

        /// <summary>The database column for <see cref="SearchParametersDto.RequireExactMatch"/>.</summary>
        private const string RequireExactMatchColumn = "RequireExactMatch";

        /// <summary>Gets the specified <typeparamref name="T"/>.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="id">The id of the <typeparamref name="T"/> to get.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        public abstract Task<T> GetAsync(UserSession session, int id);

        /// <summary>Gets the specified <typeparamref name="T"/>s.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="idList">The list of ids of the <typeparamref name="T"/>s to get.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
        public abstract Task<IEnumerable<T>> GetAsync(UserSession session, IEnumerable<int> idList);

        /// <summary>Gets the specified <typeparamref name="T"/> and all it's details.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="id">The id of the <typeparamref name="T"/> to get.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        public virtual async Task<T> GetDetailsAsync(UserSession session, int id)
        {
            return await this.GetAsync(session, id);
        }

        /// <summary>Creates new <typeparamref name="T"/>s from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing data sets and records for the data for the <typeparamref name="T"/>s.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        public abstract Task<IEnumerable<T>> ReadFromRecordsAsync(DbDataReader reader);

        /// <summary>Gets a <typeparamref name="T"/> from the <paramref name="items"/> with the id specified in the <paramref name="record"/>.</summary>
        /// <param name="items">The list of <typeparamref name="T"/>s.</param>
        /// <param name="record">The record.</param>
        /// <param name="idColumnName">The name of the id column.</param>
        /// <returns>The <typeparamref name="T"/>.</returns>
        /// <exception cref="SqlResultSyncException">The specified <typeparamref name="T"/> was not found in the <paramref name="items"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
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
        /// <param name="session">The session.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="idList"/> or <paramref name="readFromRecords"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
        protected async Task<IEnumerable<T>> GetFromDatabaseAsync(IEnumerable<int> idList, Func<DbDataReader, Task<IEnumerable<T>>> readFromRecords, UserSession session)
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
            using (var command = new SqlCommand($"{SchemaName}.{typeof(T).Name}Get", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(Persistent.ColumnToVariable($"{typeof(T).Name}Ids"), Persistent.CreateIdCollectionTable(idList));
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return await readFromRecords(reader);
                }
            }
        }

        /// <summary>Gets the specified <typeparamref name="T"/>s.</summary>
        /// <param name="lookupIdList">The list of lookup ids of the <typeparamref name="T"/>s to get.</param>
        /// <param name="readFromRecords">The callback method to use for reading the <typeparamref name="T"/>s from data to object.</param>
        /// <param name="session">The session.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lookupIdList"/> or <paramref name="readFromRecords"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
        protected async Task<IEnumerable<T>> GetFromDatabaseAsync(IEnumerable<Guid> lookupIdList, Func<DbDataReader, Task<IEnumerable<T>>> readFromRecords, UserSession session)
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
            using (var command = new SqlCommand($"{SchemaName}.{typeof(T).Name}LookupGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(Persistent.ColumnToVariable($"{typeof(T).Name}LookupIds"), Persistent.CreateIdCollectionTable(lookupIdList));
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return await readFromRecords(reader);
                }
            }
        }

        /// <summary>Gets the specified <typeparamref name="T"/>s.</summary>
        /// <param name="parametersDto">The list of key/values to add <see cref="SqlParameter"/>s to the <see cref="SqlCommand"/>.</param>
        /// <param name="session">The session.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parametersDto"/> or <paramref name="session"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
        protected async Task<IEnumerable<int>> SearchDatabaseAsync(SearchParametersDto parametersDto, UserSession session)
        {
            if (string.IsNullOrWhiteSpace(parametersDto?.SearchText))
            {
                throw new ArgumentNullException(nameof(parametersDto));
            }
            
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            await session.ValidateSessionAsync();

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{SchemaName}.{typeof(T).Name}Search", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(SearchTextColumn), parametersDto.SearchText);
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(SearchLimitColumn), parametersDto.SearchLimit);
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(RequireExactMatchColumn), parametersDto.RequireExactMatch);
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(User.IdColumn), session.UserId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    Persistent.ValidateRecord(reader, new[] { IdColumn });
                    var itemsIds = new List<int>();
                    while (await reader.ReadAsync())
                    {
                        itemsIds.Add((int)reader[IdColumn]);
                    }

                    return itemsIds;
                }
            }
        }
    }
}
