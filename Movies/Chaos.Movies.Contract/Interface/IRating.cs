//-----------------------------------------------------------------------
// <copyright file="IRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract.Interface
{
    /// <summary>Represents a rating value for an item.</summary>
    public interface IRating
    {
        /// <summary>Gets the value for this <see cref="IRating"/>.</summary>
        double Value { get; }

        /// <summary>Gets the display value for this <see cref="IRating"/>'s <see cref="Value"/>.</summary>
        string DisplayValue { get; }

        /// <summary>Gets the display color in RBG hex for this <see cref="IRating"/>'s <see cref="Value"/>.</summary>
        string HexColor { get; }
    }
}
