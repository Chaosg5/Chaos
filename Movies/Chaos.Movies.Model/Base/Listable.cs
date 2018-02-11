//-----------------------------------------------------------------------
// <copyright file="Listable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public abstract class Listable<T, TDto> : Communicable<T, TDto>, IReadOnlyCollection<T>
    {
        /// <summary>The list of <typeparamref name="T"/>s in this <see cref="Listable{T, TDto}"/>.</summary>
        private readonly List<T> items = new List<T>();

        /// <summary>Gets the number of elements contained in this <see cref="RolesCollection"/>.</summary>
        public int Count => this.Items.Count;

        /// <summary>The list of <typeparamref name="T"/>s in this <see cref="Listable{T, TDto}"/>.</summary>
        protected IReadOnlyList<T> Items => this.items.AsReadOnly();

        /// <summary>Returns an enumerator which iterates through this <see cref="RolesCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="RolesCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Adds the <paramref name="item"/> to this <see cref="Listable{T, TDto}"/>.</summary>
        /// <param name="item">The <typeparamref name="T"/> to add.</param>
        public void Add(T item)
        {
            if (this.items.Contains(item))
            {
                return;
            }

            if (item is IPersistable<T, TDto> newPersistable)
            {
                for (var index = 0; index < this.items.Count; index++)
                {
                    var typeItem = this.items[index];
                    if (!(typeItem is IPersistable<T, TDto> existingPersistable))
                    {
                        continue;
                    }

                    if (existingPersistable.Id != newPersistable.Id)
                    {
                        continue;
                    }

                    this.items[index] = item;
                    return;
                }
            }

            this.items.Add(item);
        }

        /// <summary>Removes the  <paramref name="item"/> from the <see cref="Listable{T, TDto}"/>.</summary>
        /// <param name="item">The <typeparamref name="T"/> to remove.</param>
        public void Remove(T item)
        {
            if (this.items.Contains(item))
            {
                this.items.Remove(item);
                return;
            }

            if (!(item is IPersistable<T, TDto> newPersistable))
            {
                return;
            }

            for (var index = 0; index < this.items.Count; index++)
            {
                var typeItem = this.items[index];
                if (!(typeItem is IPersistable<T, TDto> existingPersistable))
                {
                    continue;
                }

                if (existingPersistable.Id != newPersistable.Id)
                {
                    continue;
                }
                    
                this.items.RemoveAt(index);
                return;
            }
        }

        /// <summary>Removes all <typeparamref name="T"/> from this <see cref="Listable{T, TDto}"/>.</summary>
        protected void Clear()
        {
            this.items.Clear();
        }
    }
}