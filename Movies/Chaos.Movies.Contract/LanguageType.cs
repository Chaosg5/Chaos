//-----------------------------------------------------------------------
// <copyright file="LanguageType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Globalization;

    /// <summary>Enumeration of language types.</summary>
    public enum LanguageType
    {
        /// <summary>A <see cref="CultureInfo"/> language.</summary>
        Language,

        /// <summary>A default name.</summary>
        Default,

        /// <summary>An original name.</summary>
        Original
    }
}
