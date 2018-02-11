//-----------------------------------------------------------------------
// <copyright file="ICommunicable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    /// <summary>Represents an object that can be communicated over a service.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    // ReSharper disable once TypeParameterCanBeVariant
    public interface ICommunicable<T, TDto>
    {
        /// <summary>Converts this <typeparamref name="T"/> to a <typeparamref name="TDto"/>.</summary>
        /// <returns>The <typeparamref name="TDto"/>.</returns>
        TDto ToContract();
    }
}
