//-----------------------------------------------------------------------
// <copyright file="IGenre.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>A genre of <see cref="MovieDto"/>s.</summary>
    [DataContract]
    public class GenreDto
    {
        /// <summary>Gets or sets the id of the genre.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the id of the genre in IMDB.</summary>
        [DataMember]
        public string ImdbId { get; set; }

        /// <summary>Gets or sets the id of the genre in TMDB.</summary>
        [DataMember]
        public int TmdbId { get; set; }

        /// <summary>Gets or sets the title of the genre.</summary>
        [DataMember]
        public string Title { get; set; }
    }
}
