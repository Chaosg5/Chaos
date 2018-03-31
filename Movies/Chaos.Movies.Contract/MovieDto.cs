//-----------------------------------------------------------------------
// <copyright file="MovieDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class MovieDto
    {
        /// <summary>Gets or sets id of the movie.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets id of the movie in <see cref="ExternalSourceDto"/>s.</summary>
        [DataMember]
        public ReadOnlyCollection<ExternalLookupDto> ExternalLookup { get; set; }

        /// <summary>Gets or sets ratings of the movie in <see cref="ExternalSourceDto"/>s.</summary>
        [DataMember]
        public ReadOnlyCollection<ExternalRatingDto> ExternalRatings { get; set; }

        /// <summary>Gets or sets list of title of the movie in different languages.</summary>
        [DataMember]
        public ReadOnlyCollection<LanguageTitleDto> Titles { get; set; }

        /// <summary>Gets or sets list of genres that the movie belongs to.</summary>
        [DataMember]
        public ReadOnlyCollection<GenreDto> Genres { get; set; }

        /// <summary>Gets or sets list of images for the movie and their order as represented by the key.</summary>
        [DataMember]
        public ReadOnlyCollection<IconDto> Images { get; set; }

        /// <summary>Gets or sets total rating score from the current user.</summary>
        [DataMember]
        public UserRatingDto UserUserRating { get; set; }

        /// <summary>Gets or sets total rating score from all users.</summary>
        [DataMember]
        public UserRatingDto TotalUserRating { get; set; }

        /// <summary>Gets or sets list of <see cref="CharacterDto"/>s in this <see cref="MovieDto"/>.</summary>
        [DataMember]
        public ReadOnlyCollection<PersonAsCharacterDto> Characters { get; set; }

        /// <summary>Gets or sets list of <see cref="PersonDto"/>s in this <see cref="MovieDto"/>.</summary>
        [DataMember]
        public ReadOnlyCollection<PersonInRoleDto> People { get; set; }

        /// <summary>Gets or sets the type of the movie.</summary>
        [DataMember]
        public MovieTypeDto MovieType { get; set; }
    }
}