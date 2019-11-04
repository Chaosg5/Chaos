//-----------------------------------------------------------------------
// <copyright file="IReadableExtension.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models
{
    using System;
    using System.Threading.Tasks;

    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Base;

    /// <summary>Extension contract for <see cref="Readable{T,TDto}"/>.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public interface IReadableExtension<T, TDto>
    {
        /// <summary>Gets the specified <typeparamref name="T"/>.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="lookupId">The lookup id of the <typeparamref name="T"/> to get.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        Task<T> GetAsync(UserSession session, Guid lookupId);
    }
}