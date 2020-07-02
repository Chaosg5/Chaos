//-----------------------------------------------------------------------
// <copyright file="LanguageDescriptionCollectionDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <inheritdoc />
    /// <summary>Represents a user.</summary>
    [DataContract]
    public class LanguageDescriptionCollectionDto : ReadOnlyCollection<LanguageDescriptionDto>
    {
        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Chaos.Movies.Contract.LanguageDescriptionCollectionDto" /> class.</summary>
        /// <param name="items">The items.</param>
        public LanguageDescriptionCollectionDto(IList<LanguageDescriptionDto> items)
            : base(items)
        {
        }

        /// <summary>Gets or sets the user title.</summary>
        [DataMember]
        public LanguageDescriptionDto UserTitle { get; set; }

        /// <summary>Gets or sets the original title.</summary>
        [DataMember]
        public LanguageDescriptionDto OriginalTitle { get; set; }
    }
}
