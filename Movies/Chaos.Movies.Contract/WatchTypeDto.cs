//-----------------------------------------------------------------------
// <copyright file="WatchTypeDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class WatchTypeDto
    {
        /// <summary>Gets or sets the id of the location.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the list of titles of this <see cref="WatchTypeDto"/> in different languages.</summary>
        [DataMember]
        public LanguageTitleCollectionDto Titles { get; set; }
    }
}
