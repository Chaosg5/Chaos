//-----------------------------------------------------------------------
// <copyright file="ILoadable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    /// <summary>Represents an object that can be loaded from the database, but not saved directly.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public interface ILoadable<T, TDto> : ICommunicable<T, TDto>
    {
    }
}
