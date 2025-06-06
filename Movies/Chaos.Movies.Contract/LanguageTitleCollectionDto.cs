﻿//-----------------------------------------------------------------------
// <copyright file="LanguageTitleCollectionDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class LanguageTitleCollectionDto : ReadOnlyCollection<LanguageTitleDto>
    {
        /// <summary>Initializes a new instance of the <see cref="LanguageTitleCollectionDto"/> class.</summary>
        /// <param name="items">The items.</param>
        public LanguageTitleCollectionDto(IList<LanguageTitleDto> items)
            : base(items)
        {
        }

        /// <summary>Gets or sets the user title.</summary>
        [DataMember]
        public LanguageTitleDto UserTitle { get; set; }

        /// <summary>Gets or sets the original title.</summary>
        [DataMember]
        public LanguageTitleDto OriginalTitle { get; set; }
    }
}
