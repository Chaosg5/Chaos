//-----------------------------------------------------------------------
// <copyright file="LanguageTitleCollectionDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
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

        /// <summary>Gets or sets the title.</summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>Gets or sets the language of the title.</summary>
        [DataMember]
        public CultureInfo Language { get; set; }

        /// <summary>Gets or sets the type of the language of the title.</summary>
        [DataMember]
        public LanguageType LanguageType { get; set; }
    }
}
