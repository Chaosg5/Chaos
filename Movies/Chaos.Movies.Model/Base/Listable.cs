//-----------------------------------------------------------------------
// <copyright file="Listable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;

    using Chaos.Movies.Model.Exceptions;
    
    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    /// <typeparam name="TList">The type of the list.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
    public abstract class Listable<T, TDto, TList, TListDto> : Communicable<TList, TListDto>, IReadOnlyCollection<T>
    {
        /// <summary>The list of <typeparamref name="T"/>s in this <see cref="Listable{T, TDto, TList, TListDto}"/>.</summary>
        private List<T> items = new List<T>();
        
        /// <summary>Gets the number of elements contained in this <see cref="RoleCollection"/>.</summary>
        public int Count => this.Items.Count;

        /// <summary>Gets properties from of each <typeparamref name="T"/> in a table which can be used to save them to the database.</summary>
        public abstract DataTable GetSaveTable { get; }

        /// <summary>The list of <typeparamref name="T"/>s in this <see cref="Listable{T, TDto, TList, TListDto}"/>.</summary>
        protected IReadOnlyList<T> Items => this.items.AsReadOnly();

        /// <summary>Returns an enumerator which iterates through this <see cref="RoleCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="RoleCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Adds all <typeparamref name="T"/> in the <paramref name="itemsToAdd"/> to this <see cref="Listable{T, TDto, TList, TListDto}"/>.</summary>
        /// <param name="itemsToAdd">The <typeparamref name="T"/>s to add.</param>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public void AddRange(IEnumerable<T> itemsToAdd)
        {
            if (itemsToAdd == null)
            {
                return;
            }

            foreach (var item in itemsToAdd)
            {
                this.Add(item);
            }
        }

        /// <summary>Removes all <typeparamref name="T"/> in the <paramref name="itemsToRemove"/> to this <see cref="Listable{T, TDto, TList, TListDto}"/>.</summary>
        /// <param name="itemsToRemove">The <typeparamref name="T"/>s to add.</param>
        public void RemoveRange(IEnumerable<T> itemsToRemove)
        {
            if (itemsToRemove == null)
            {
                return;
            }

            foreach (var item in itemsToRemove)
            {
                this.Remove(item);
            }
        }

        /// <summary>Adds the <paramref name="item"/> to this <see cref="Listable{T, TDto, TList, TListDto}"/>.</summary>
        /// <param name="item">The <typeparamref name="T"/> to add.</param>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <see langword="null"/></exception>
        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (this.items.Contains(item))
            {
                return;
            }

            if (item is Persistable<T, TDto> newPersistable)
            {
                if (newPersistable.Id <= 0)
                {
                    throw new PersistentObjectRequiredException($"The {typeof(T).Name} has to be saved before added to the {typeof(TList).Name}.");
                }

                for (var index = 0; index < this.items.Count; index++)
                {
                    var typeItem = this.items[index];
                    if (!(typeItem is Persistable<T, TDto> existingPersistable))
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

        /// <summary>Removes the  <paramref name="item"/> from the <see cref="Listable{T, TDto, TList, TListDto}"/>.</summary>
        /// <param name="item">The <typeparamref name="T"/> to remove.</param>
        public void Remove(T item)
        {
            if (item == null)
            {
                return;
            }

            if (this.items.Contains(item))
            {
                this.items.Remove(item);
                return;
            }

            if (!(item is Persistable<T, TDto> newPersistable))
            {
                return;
            }

            for (var index = 0; index < this.items.Count; index++)
            {
                var typeItem = this.items[index];
                if (!(typeItem is Persistable<T, TDto> existingPersistable))
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

        /// <summary>Validates that the this <typeparamref name="TList"/> is valid to be saved.</summary>
        internal abstract void ValidateSaveCandidate();

        /// <summary>Removes all <typeparamref name="T"/> from this <see cref="Listable{T, TDto, TList, TListDto}"/>.</summary>
        protected void Clear()
        {
            this.items.Clear();
        }

        /// <summary>Reorders the <see cref="Items"/>.</summary>
        /// <param name="newOrder">The order to set based on the indexes of the old order.</param>
        protected void SetReorededList(ICollection<int> newOrder)
        {
            this.items = Persistent.ReorderList(this.Items.ToList(), newOrder).ToList();
        }
    }
}