//-----------------------------------------------------------------------
// <copyright file="ISearchable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;

    /*
    ToDo: These should be searchable 
    * dc Character
    * d (Episode)
    * d Movie
    * d MovieSeries
    * dc Person
    * dc User
    */

    /// <summary>Represents an item that can be searched.</summary>
    /// <remarks>The <typeparamref name="T"/> needs to be a <see cref="Readable{T,TDto}"/>.</remarks>
    /// <typeparam name="T">The type of the inheriting class.</typeparam>
    public interface ISearchable<T>
    {
        /// <summary>The search async.</summary>
        /// <param name="parametersDto">The parameters.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<T>> SearchAsync(SearchParametersDto parametersDto, UserSession session);
    }
}
