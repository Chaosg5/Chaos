//-----------------------------------------------------------------------
// <copyright file="Orderable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    using System.Collections.Generic;

    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    /// <typeparam name="TList">The type of the list.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "The design is made to minimize the amount of code in the inheriting classes and to ensure they implement all required methods.")]
    public abstract class Orderable<T, TDto, TList, TListDto> : Listable<T, TDto, TList, TListDto>
    {
        /// <summary>The database column for the order.</summary>
        protected const string OrderColumn = "Order";

        /// <summary>Sets the order of the movies in this collection.</summary>
        /// <param name="newOrder">The order to set based on the indexes of the old order.</param>
        public void ReorderMovies(ICollection<int> newOrder)
        {
             this.SetReorededList(newOrder);
        }
    }
}