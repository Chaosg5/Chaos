//-----------------------------------------------------------------------
// <copyright file="IconDto.cs" company="Erik Bunnstad">
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
        /// <summary>Gets or sets the image of the icon.</summary>
        [DataMember]
        public Binary Image { get; set; }

        /// <summary>Gets or sets the URL of the image of the icon.</summary>
        [DataMember]
        public string Url { get; set; }
    }
}