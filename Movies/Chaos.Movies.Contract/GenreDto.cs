//-----------------------------------------------------------------------
// <copyright file="GenreDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>A genre of <see cref="MovieDto"/>s.</summary>
    [DataContract]
    public class GenreDto
    {
        /// <summary>Gets or sets the id of the genre.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the id of the <see cref="GenreDto"/> in <see cref="ExternalSourceDto"/>s.</summary>
        [DataMember]
        public ReadOnlyCollection<ExternalLookupDto> ExternalLookups { get; set; }

        /// <summary>Gets or sets the list of titles of this <see cref="GenreDto"/> in different languages.</summary>
        [DataMember]
        public ReadOnlyCollection<LanguageTitleDto> Titles { get; set; }
    }
}
