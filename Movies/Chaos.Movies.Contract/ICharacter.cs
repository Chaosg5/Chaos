//-----------------------------------------------------------------------
// <copyright file="ICharacter.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;

    /// <summary>Represents a character in a movie.</summary>
    public interface ICharacter : IReadOnlyCharacter
    {
        /// <summary>Gets or sets name of the character.</summary>
        new string Name { get; set; }

        /// <summary>Gets or sets the id of the character in IMDB.</summary>
        new string ImdbId { get; set; }

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        ///ReadOnlyCollection<Icon> Images { get; set; }
    }
}
