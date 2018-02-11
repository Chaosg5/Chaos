//-----------------------------------------------------------------------
// <copyright file="IReadable.cs">
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
    public interface IReadable<T, TDto> : IPersistable<T, TDto>
    {
        /// <summary>Gets the specified <typeparamref name="T"/>.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="id">The id of the <typeparamref name="T"/> to get.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        Task<T> GetAsync(UserSession session, int id);

        /// <summary>Gets the specified <typeparamref name="T"/>s.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <param name="idList">The list of ids of the <typeparamref name="T"/>s to get.</param>
        /// <returns>The list of <typeparamref name="T"/>s.</returns>
        Task<IEnumerable<T>> GetAsync(UserSession session, IEnumerable<int> idList);
    }
}