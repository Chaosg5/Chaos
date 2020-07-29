//-----------------------------------------------------------------------
// <copyright file="Zone.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;

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

        /// <summary>Gets or sets the <see cref="Height"/> for CSS.</summary>
        public string CssHeight => $"{this.Height}px";

        /// <summary>Gets or sets the <see cref="Width"/> for CSS.</summary>
        public string CssWidth => $"{this.Width}px";

        /// <summary>Gets or sets the X position of the <see cref="Zone"/>.</summary>
        [DataMember]
        public short PositionX { get; set; }

        /// <summary>Gets or sets the Y position of the <see cref="Zone"/>.</summary>
        [DataMember]
        public short PositionY { get; set; }

        /// <summary>Gets or sets the <see cref="PositionX"/> for CSS.</summary>
        public string CssPositionX => $"{this.PositionX}px";

        /// <summary>Gets or sets the <see cref="PositionY"/> for CSS.</summary>
        public string CssPositionY => $"{this.PositionY}px";

        /// <summary>Gets or sets the titles of the <see cref="Zone"/>.</summary>
        [DataMember]
        public LanguageDescriptionCollectionDto Titles { get; set; }

        /// <summary>Gets or sets the children <see cref="Challenge"/>s.</summary>
        [DataMember]
        public IReadOnlyCollection<Challenge> Challenges { get; set; }
        
        /// <summary>Gets a CSS color for the number of <see cref="Challenges"/> that are locked.</summary>
        public string CssCompletion
        {
            get
            {
                return TotalRating.GetHexColor(
                    TotalRating.NormalizeValue(this.Challenges.Count(c => c.TeamChallenge?.IsLocked == true), 0, this.Challenges.Count));
            }
        }
    }
}