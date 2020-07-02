//-----------------------------------------------------------------------
// <copyright file="Game.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract;

    /// <summary>A game containing a set of <see cref="Zone" />s.</summary>
    [DataContract]
    public class Game
    {
        /// <summary>Gets or sets the id of the <see cref="Game"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the image of the <see cref="Game"/>.</summary>
        [DataMember]
        public string ImageId { get; set; }

        /// <summary>Gets or sets the height of the <see cref="Game"/>.</summary>
        [DataMember]
        public short Height { get; set; }

        /// <summary>Gets or sets the width of the <see cref="Game"/>.</summary>
        [DataMember]
        public short Width { get; set; }

        /// <summary>Gets or sets the titles of the <see cref="Game"/>.</summary>
        [DataMember]
        public LanguageDescriptionCollectionDto Titles { get; set; }

        /// <summary>Gets or sets the children <see cref="Zone"/>s.</summary>
        [DataMember]
        public IEnumerable<Zone> Zones { get; set; }
    }
}