//-----------------------------------------------------------------------
// <copyright file="SystemText.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games.Contract
{
    using System.Runtime.Serialization;

    using Chaos.Movies.Contract;

    /// <summary>A subject of a <see cref="Challenge"/> or <see cref="Question"/>.</summary>
    [DataContract]
    public class SystemText
    {
        /// <summary>Gets or sets the id of the <see cref="SystemText"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the image of the <see cref="Game"/>.</summary>
        [DataMember]
        public string TextKey { get; set; }

        /// <summary>Gets or sets the titles of the <see cref="SystemText"/>.</summary>
        [DataMember]
        public LanguageDescriptionCollectionDto Titles { get; set; }
    }
}