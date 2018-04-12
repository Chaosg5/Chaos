//-----------------------------------------------------------------------
// <copyright file="MovieSeriesDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class MovieSeriesDto
    {
        /// <summary>Gets or sets the id of the movie series.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the type of the movie series.</summary>
        [DataMember]
        public MovieSeriesTypeDto MovieSeriesType { get; set; }

        /// <summary>Gets or sets the list of title of the movie collection in different languages.</summary>
        [DataMember]
        public ReadOnlyCollection<LanguageTitleDto> Titles { get; set; }

        /// <summary>Gets or sets the movies which are a part of this collection with the keys representing their order.</summary>
        [DataMember]
        public ReadOnlyCollection<MovieDto> Movies { get; set; }

        /// <summary>Gets or sets the list of images for the <see cref="MovieSeriesDto"/> and their order as represented by the key.</summary>
        [DataMember]
        public ReadOnlyCollection<IconDto> Images { get; set; }
    }
}
