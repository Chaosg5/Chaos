//-----------------------------------------------------------------------
// <copyright file="RatingTypeDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class RatingTypeDto
    {
        /// <summary>Gets or sets the id of this rating type.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the list of titles of this <see cref="RatingTypeDto"/> in different languages.</summary>
        [DataMember]
        public ReadOnlyCollection<LanguageDescriptionDto> Titles { get; set; }

        /// <summary>Gets or sets the <see cref="RatingTypeDto"/>s that makes up the derived children of this <see cref="RatingTypeDto"/>.</summary>
        [DataMember]
        public ReadOnlyCollection<RatingTypeDto> Subtypes { get; set; }
    }
}