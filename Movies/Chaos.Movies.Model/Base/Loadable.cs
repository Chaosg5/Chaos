//-----------------------------------------------------------------------
// <copyright file="Loadable.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>Represents an object that can be loaded from the database, but not saved directly.</summary>
    /// <typeparam name="T">The base model logic type.</typeparam>
    /// <typeparam name="TDto">The data transfer type to use for communicating the <typeparamref name="T"/>.</typeparam>
    public abstract class Loadable<T, TDto> : Communicable<T, TDto>
    {
        /// <summary>Validates that the this <typeparamref name="T"/> is valid to be saved.</summary>
        internal abstract void ValidateSaveCandidate();

        /// <summary>Creates a new <typeparamref name="T"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <typeparamref name="T"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        internal abstract Task<T> NewFromRecordAsync(IDataRecord record);

        /// <summary>Updates the <typeparamref name="T"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <typeparamref name="T"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected abstract Task ReadFromRecordAsync(IDataRecord record);
    }
}
