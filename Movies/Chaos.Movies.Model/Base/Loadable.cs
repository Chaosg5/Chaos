//-----------------------------------------------------------------------
// <copyright file="Loadable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>Represents an object that can be loaded from the database, but not saved directly.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public abstract class Loadable<T, TDto> : Communicable<T, TDto>
    {
        /// <summary>Initializes a new instance of the <see cref="Loadable{T,TDto}"/> class.</summary>
        /// <param name="dto">The <typeparamref name="TDto"/> to create the <typeparamref name="T"/> from.</param>
        protected Loadable(TDto dto)
            : base(dto)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Loadable{T,TDto}"/> class.</summary>
        protected Loadable()
        {
        }
        
        /// <summary>Updates this <typeparamref name="T"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <typeparamref name="T"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public abstract Task<T> ReadFromRecordAsync(IDataRecord record);
        
        /// <summary>Validates that the this <typeparamref name="T"/> is valid to be saved.</summary>
        public abstract void ValidateSaveCandidate();
    }
}
