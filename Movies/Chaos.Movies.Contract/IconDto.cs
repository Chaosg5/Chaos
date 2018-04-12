//-----------------------------------------------------------------------
// <copyright file="IconDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Data.Linq;
    using System.Runtime.Serialization;

    /// <summary>Represents an icon.</summary>
    [DataContract]
    public class IconDto
    {
        /// <summary>Gets or sets the id of this <see cref="IconDto"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the image of the icon.</summary>
        [DataMember]
        public IconTypeDto IconType { get; set; }

        /// <summary>Gets or sets the image of this <see cref="IconDto"/>.</summary>
        [DataMember]
        public Binary Image { get; set; }

        /// <summary>Gets or sets the URL of the image of this <see cref="IconDto"/>.</summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>Gets or sets the binary data of the image of this <see cref="IconDto"/>.</summary>
        [DataMember]
        public Binary Data { get; set; }
    }
}