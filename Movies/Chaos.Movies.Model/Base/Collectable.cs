//-----------------------------------------------------------------------
// <copyright file="Collectable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    using System.Threading.Tasks;

    /// <summary>Represents list of objects that requires a parent.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    /// <typeparam name="TList">The type of the list.</typeparam>
    /// <typeparam name="TParent">The parent type of the owner of the collection.</typeparam>
    public abstract class Collectable<T, TDto, TList, TParent> : Listable<T, TDto, TList>
    {
        /// <summary>Adds the <paramref name="item"/> to the collection.</summary>
        /// <param name="item">The <typeparamref name="T"/> to add.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public abstract Task AddAndSaveAsync(T item);

        /// <summary>Removes the <paramref name="item"/> from the collection.</summary>
        /// <param name="item">The <typeparamref name="T"/> to remove.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public abstract Task RemoveAndSaveAsync(T item);

        /// <summary>Adds the <paramref name="item"/> to the collection and saves the change to the database.</summary>
        /// <param name="item">The <typeparamref name="T"/> to add.</param>
        protected async Task AddAndSaveToDatabaseAsync(T item)
        {
        }

        /// <summary>Removes the <paramref name="item"/> from the collection and saves the change to the database.</summary>
        /// <param name="item">The <typeparamref name="T"/> to remove.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected async Task RemoveAndSaveToDatabaseAsync(T item)
        {
        }
    }
}