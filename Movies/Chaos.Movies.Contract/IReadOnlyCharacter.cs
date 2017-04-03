//-----------------------------------------------------------------------
// <copyright file="IReadOnlyCharacter.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;

    /// <summary>Represents a character in a movie.</summary>
    public interface IReadOnlyCharacter
    {
        /// <summary>Gets the name of the character.</summary>
        string Name { get; }

        /// <summary>Gets the id of the character in IMDB.</summary>
        string ImdbId { get; }

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        ///ReadOnlyCollection<Icon> Images { get; }
    }
}
