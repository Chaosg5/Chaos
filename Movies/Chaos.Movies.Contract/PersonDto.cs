//-----------------------------------------------------------------------
// <copyright file="PersonDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
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

        /// <summary>Gets or sets the id of the <see cref="PersonDto"/> in <see cref="ExternalSourceDto"/>s.</summary>
        [DataMember]
        public ReadOnlyCollection<ExternalLookupDto> ExternalLookups { get; set; }

        /// <summary>Gets or sets the list of images for the <see cref="PersonDto"/> and their order as represented by the key.</summary>
        [DataMember]
        public ReadOnlyCollection<IconDto> Images { get; set; }

        /// <summary>Gets or sets the user ratings.</summary>
        [DataMember]
        public UserSingleRatingDto Ratings { get; set; }
    }
}