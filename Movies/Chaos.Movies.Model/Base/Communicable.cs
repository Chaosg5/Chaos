//-----------------------------------------------------------------------
// <copyright file="Communicable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;

    /// <summary>Represents an object that can be communicated over a service.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public abstract class Communicable<T, TDto>
    {
        /// <summary>Initializes a new instance of the <see cref="Communicable{T,TDto}"/> class.</summary>
        /// <param name="dto">The <typeparamref name="TDto"/> to create the <typeparamref name="T"/> from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dto"/> is <see langword="null"/></exception>
        protected Communicable(TDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Communicable{T,TDto}"/> class.</summary>
        protected Communicable()
        {
        }
    }
}
