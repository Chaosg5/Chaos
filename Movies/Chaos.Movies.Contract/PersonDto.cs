﻿//-----------------------------------------------------------------------
// <copyright file="PersonDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class PersonDto
    {
        /// <summary>Gets or sets the id of the person.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets name of the person.</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>Gets or sets the year when the person was born.</summary>
        [DataMember]
        public DateTime BirthDate { get; set; }

        /// <summary>Gets or sets the year when the person died.</summary>
        [DataMember]
        public DateTime DeathDate { get; set; }

        /// <summary>Gets or sets the id of the <see cref="PersonDto"/> in <see cref="ExternalSourceDto"/>s.</summary>
        [DataMember]
        public ReadOnlyCollection<ExternalLookupDto> ExternalLookups { get; set; }

        /// <summary>Gets or sets the list of images for the <see cref="PersonDto"/> and their order as represented by the key.</summary>
        [DataMember]
        public ReadOnlyCollection<IconDto> Images { get; set; }

        /// <summary>Gets or sets the user ratings.</summary>
        [DataMember]
        public TotalRatingDto UserRating { get; set; }

        /// <summary>Gets or sets the total rating from all users.</summary>
        [DataMember]
        public TotalRatingDto TotalRating { get; set; }
    }
}