//-----------------------------------------------------------------------
// <copyright file="IIcon.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Data.Linq;

    /// <summary>Represents a character in a movie.</summary>
    public interface IIcon
    {
        Binary Image { get; set; }
    }
}
