//-----------------------------------------------------------------------
// <copyright file="IUpdateable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models
{
    using System.Threading.Tasks;

    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Base;

    /// <summary>Extension contract for <see cref="Readable{T,TDto}"/>.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public interface IUpdateable<T, in TDto>
    {
        /// <summary>Updates the <typeparamref name="T"/> with data from the <paramref name="contract"/>.</summary>
        /// <param name="contract">The <typeparamref name="TDto"/>.</param>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateAsync(TDto contract, UserSession session);
    }
}