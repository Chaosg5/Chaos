//-----------------------------------------------------------------------
// <copyright file="IconTypeDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents an icon.</summary>
    [DataContract]
    public class IconTypeDto
    {
        /// <summary>Gets or sets the id of this <see cref="IconTypeDto"/>.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the list of titles of this <see cref="IconTypeDto"/> in different languages.</summary>
        [DataMember]
        public ReadOnlyCollection<LanguageTitleDto> Titles { get; set; }
    }
}