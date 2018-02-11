//-----------------------------------------------------------------------
// <copyright file="IListable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Data;

    /// <summary>Represents a persitable object that can be saved to the database.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IListable<T> : IReadOnlyCollection<T>
    {
        /// <summary>Gets properties from of each <typeparamref name="T"/> in a table which can be used to save them to the database.</summary>
        DataTable GetSaveTable { get; }
    }
}