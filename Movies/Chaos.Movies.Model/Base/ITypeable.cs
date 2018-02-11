//-----------------------------------------------------------------------
// <copyright file="ITypeable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public interface ITypeable<T, TDto> : IReadable<T, TDto>
    {
        /// <summary>Gets the all <typeparamref name="T"/>.</summary>
        /// <param name="session">The session.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        Task<IEnumerable<T>> GetAllAsync(UserSession session);
    }
}