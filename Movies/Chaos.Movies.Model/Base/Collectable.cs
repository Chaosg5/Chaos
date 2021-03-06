﻿//-----------------------------------------------------------------------
// <copyright file="Collectable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents list of objects that requires a parent.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    /// <typeparam name="TList">The type of the list.</typeparam>
    /// <typeparam name="TParent">The parent type of the owner of the collection.</typeparam>
    /// <typeparam name="TParentDto">The data transfer type to use for communicating the <typeparamref name="TParent"/>.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1005:AvoidExcessiveParametersOnGenericTypes",
        Justification =
            "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
    public abstract class Collectable<T, TDto, TList, TListDto, TParent, TParentDto> : Listable<T, TDto, TList, TListDto>
    {
        /// <summary>Initializes a new instance of the <see cref="Collectable{T,TDto,TList,TListDto,TParent, TParentDto}"/> class.</summary>
        /// <param name="parent">The parent of the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent"/> is <see langword="null"/>.</exception>
        protected Collectable(Persistable<TParent, TParentDto> parent)
        {
            this.Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        /// <summary>Initializes a new instance of the <see cref="Collectable{T,TDto,TList,TListDto,TParent,TParentDto}"/> class.</summary>
        protected Collectable()
        {
        }

        /// <summary>Gets the id of the parent of this collection.</summary>
        public int ParentId => this.Parent.Id;

        /// <summary>Gets or sets the database schema name.</summary>
        protected string SchemaName { get; set; } = "dbo";

        /// <summary>Gets the parent of this collection.</summary>
        private Persistable<TParent, TParentDto> Parent { get; }

        /// <summary>Saves this <typeparamref name="T"/> to the database.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public abstract Task SaveAsync(UserSession session);

        /// <summary>Adds the <paramref name="item"/> to the collection.</summary>
        /// <param name="item">The <typeparamref name="T"/> to add.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public abstract Task AddAndSaveAsync(T item, UserSession session);

        /// <summary>Removes the <paramref name="item"/> from the collection.</summary>
        /// <param name="item">The <typeparamref name="T"/> to remove.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public abstract Task RemoveAndSaveAsync(T item, UserSession session);

        /// <summary>Converts the <paramref name="contract"/> to a <typeparamref name="T"/>.</summary>
        /// <param name="contract">The contract¨to convert.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>The <typeparamref name="T"/>.</returns>
        public abstract TList FromContract(ReadOnlyCollection<TDto> contract, Persistable<TParent, TParentDto> parent);

        /// <summary>Gets SQL parameters to use for <see cref="AddAndSaveToDatabaseAsync"/> and  <see cref="RemoveAndSaveToDatabaseAsync"/>.</summary>
        /// <returns>The list of SQL parameters.</returns>
        protected abstract IReadOnlyDictionary<string, object> GetSaveParameters();

        /// <summary>Adds the <paramref name="item"/> to the collection and saves all the content of this <see cref="Collectable{T,TDto,TList,TListDto,TParent, TParentDto}"/> to the database.</summary>
        /// <param name="item">The <typeparamref name="T"/> to add.</param>
        /// <param name="commandParameters">The list of key/values to add <see cref="SqlParameter"/>s to the <see cref="SqlCommand"/>.</param>
        /// <param name="readFromRecords">The callback method to use for reading the <typeparamref name="T"/>s from data to object.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandParameters"/> or <paramref name="readFromRecords"/> is <see langword="null"/></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification =
                "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
        protected async Task AddAndSaveToDatabaseAsync(
            T item,
            IReadOnlyDictionary<string, object> commandParameters,
            Func<DbDataReader, Task<IEnumerable<T>>> readFromRecords,
            UserSession session)
        {
            this.Add(item);
            await this.SaveToDatabaseAsync(commandParameters, readFromRecords, session);
        }

        /// <summary>Removes the <paramref name="item"/> from the collection and saves all the content of this <see cref="Collectable{T,TDto,TList,TListDto,TParent, TParentDto}"/> to the database.</summary>
        /// <param name="item">The <typeparamref name="T"/> to remove.</param>
        /// <param name="commandParameters">The list of key/values to add <see cref="SqlParameter"/>s to the <see cref="SqlCommand"/>.</param>
        /// <param name="readFromRecords">The callback method to use for reading the <typeparamref name="T"/>s from data to object.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandParameters"/> or <paramref name="readFromRecords"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        protected async Task RemoveAndSaveToDatabaseAsync(
            T item,
            IReadOnlyDictionary<string, object> commandParameters,
            Func<DbDataReader, Task<IEnumerable<T>>> readFromRecords,
            UserSession session)
        {
            this.Remove(item);
            await this.SaveToDatabaseAsync(commandParameters, readFromRecords, session);
        }

        /// <summary>Saves all the content of this <see cref="Collectable{T,TDto,TList,TListDto,TParent, TParentDto}"/> to the database.</summary>
        /// <param name="commandParameters">The list of key/values to add <see cref="SqlParameter"/>s to the <see cref="SqlCommand"/>.</param>
        /// <param name="readFromRecords">The callback method to use for reading the <typeparamref name="T"/>s from data to object.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="commandParameters"/> or <paramref name="readFromRecords"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification =
                "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
        protected async Task SaveToDatabaseAsync(
            IReadOnlyDictionary<string, object> commandParameters,
            Func<DbDataReader, Task<IEnumerable<T>>> readFromRecords,
            UserSession session)
        {
            if (commandParameters == null)
            {
                throw new ArgumentNullException(nameof(commandParameters));
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
            using (var command = new SqlCommand($"{SchemaName}.{typeof(TParent).Name}{typeof(T).Name}Save", connection))
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
                        var items = await readFromRecords(reader);
                        this.Clear();
                        this.AddRange(items);
                    }
                }
            }
        }
    }
}