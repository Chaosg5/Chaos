﻿//-----------------------------------------------------------------------
// <copyright file="IPersistable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Threading.Tasks;

    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public interface IPersistable<T, TDto> : ILoadable<T, TDto>
    {
        /// <summary>Gets the id of this <typeparamref name="T"/>.</summary>
        int Id { get; }

        /// <summary>Saves this <typeparamref name="T"/> to the database.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SaveAsync(UserSession session);

        /// <summary>Saves this <typeparamref name="T"/> to the database.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SaveAllAsync(UserSession session);
    }
}