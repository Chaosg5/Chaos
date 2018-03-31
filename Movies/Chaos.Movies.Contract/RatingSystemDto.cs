//-----------------------------------------------------------------------
// <copyright file="RatingSystemDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class RatingSystemDto
    {
        /// <summary>Gets or sets the id of this rating system.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the list of titles of this <see cref="RatingTypeDto"/> in different languages.</summary>
        [DataMember]
        public ReadOnlyCollection<LanguageDescriptionDto> Titles { get; set; }

        /// <summary>Gets or sets the the relative value for each <see cref="RatingTypeDto"/>.</summary>
        //[DataMember]
        //public ReadOnlyDictionary<RatingTypeDto, short> Values { get; set; }
    }
}