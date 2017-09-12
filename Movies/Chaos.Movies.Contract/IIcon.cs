//-----------------------------------------------------------------------
// <copyright file="IIcon.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Data.Linq;

    /// <summary>Represents xxx.</summary>
    public interface IIcon
    {
        /// <summary>Gets the image of the icon.</summary>
        Binary Image { get; }

        /// <summary>Gets the URL of the image of the icon.</summary>
        string Url { get; }
    }
}
