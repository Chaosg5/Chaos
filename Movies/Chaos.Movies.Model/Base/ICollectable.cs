//-----------------------------------------------------------------------
// <copyright file="ICollectable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Threading.Tasks;

    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    public interface ICollectable<T> : IListable<T>
    {
        /// <summary>Adds the <paramref name="item"/> to the collection.</summary>
        /// <param name="item">The <typeparamref name="T"/> to add.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task AddAndSaveAsync(T item);

        /// <summary>Removes the <paramref name="item"/> from the collection.</summary>
        /// <param name="item">The <typeparamref name="T"/> to remove.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RemoveAndSaveAsync(T item);
    }
}