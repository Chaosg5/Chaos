//-----------------------------------------------------------------------
// <copyright file="Zone.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract;

    /// <summary>A zone in a <see cref="Game" /> containing a set of <see cref="Challenge" />s.</summary>
    [DataContract]
    public class Zone
    {
        /// <summary>Gets or sets the id of the <see cref="Zone"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the <see cref="Game.Id"/> of the parent <see cref="Game"/>.</summary>
        [DataMember]
        public int GameId { get; set; }

        /// <summary>Gets or sets the image <see cref="Zone"/>.</summary>
        [DataMember]
        public string ImageId { get; set; }

        /// <summary>Gets or sets the height of the <see cref="Zone"/>.</summary>
        [DataMember]
        public short Height { get; set; }

        /// <summary>Gets or sets the width of the <see cref="Zone"/>.</summary>
        [DataMember]
        public short Width { get; set; }

        /// <summary>Gets or sets the X position of the <see cref="Zone"/>.</summary>
        [DataMember]
        public short PositionX { get; set; }

        /// <summary>Gets or sets the Y position of the <see cref="Zone"/>.</summary>
        [DataMember]
        public short PositionY { get; set; }

        /// <summary>Gets or sets the titles of the <see cref="Zone"/>.</summary>
        [DataMember]
        public LanguageDescriptionCollectionDto Titles { get; set; }

        /// <summary>Gets or sets the children <see cref="Challenge"/>s.</summary>
        [DataMember]
        public IEnumerable<Challenge> Challenges { get; set; }
    }
}