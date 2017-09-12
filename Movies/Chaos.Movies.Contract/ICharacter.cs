//-----------------------------------------------------------------------
// <copyright file="ICharacter.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.Generic;

    /// <summary>Represents a character in a movie.</summary>
    public interface ICharacter
    {
        /// <summary>Gets the id of the character.</summary>
        int Id { get; }

        /// <summary>Gets the name of the character.</summary>
        string Name { get; }

        /// <summary>Gets the id of the character in IMDB.</summary>
        string ImdbId { get; }

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        IEnumerable<IIcon> Images { get; }
    }
}
