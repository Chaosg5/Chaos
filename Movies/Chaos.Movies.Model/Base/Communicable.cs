//-----------------------------------------------------------------------
// <copyright file="Communicable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    /// <summary>Represents an object that can be communicated over a service.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public abstract class Communicable<T, TDto>
    {
        /// <summary>Converts this <typeparamref name="T"/> to a <typeparamref name="TDto"/>.</summary>
        /// <returns>The <typeparamref name="TDto"/>.</returns>
        public abstract TDto ToContract();

        /// <summary>Converts this <typeparamref name="T"/> to a <typeparamref name="TDto"/>.</summary>
        /// <param name="languageName">The name of the language to convert underlying objects to.</param>
        /// <returns>The <typeparamref name="TDto"/>.</returns>
        public abstract TDto ToContract(string languageName);

        /// <summary>Converts the <paramref name="contract"/> to a <typeparamref name="T"/>.</summary>
        /// <param name="contract">The contract¨to convert.</param>
        /// <returns>The <typeparamref name="T"/>.</returns>
        public abstract T FromContract(TDto contract);
    }
}
