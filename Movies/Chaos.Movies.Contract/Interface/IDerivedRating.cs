//-----------------------------------------------------------------------
// <copyright file="IDerivedRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract.Interface
{
    /// <inheritdoc />
    /// <summary>Represents a rating value for an item.</summary>
    public interface IDerivedRating : IRating
    {
        /// <summary>Gets the derived value for this <see cref="IRating"/>.</summary>
        double Derived { get; }
    }
}
