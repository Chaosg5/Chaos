//-----------------------------------------------------------------------
// <copyright file="MovieListItemDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Represents a <see cref="Movie"/> in a list.</summary>
    [DataContract]
    public class MovieListItemDto
    {
        /// <summary>Gets or sets the id of the list item.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the <see cref="Movie"/>.</summary>
        [DataMember]
        public MovieDto Movie { get; set; }

        /// <summary>Gets or sets the list rating of the <see cref="Movie"/>.</summary>
        [DataMember]
        public int Rating { get; set; }

        /// <summary>Gets or sets the <see cref="WatchTypeDto"/> of the movie.</summary>
        [DataMember]
        public WatchTypeDto WatchType { get; set; }

        /// <summary>Gets or sets the list date of the <see cref="Movie"/>.</summary>
        [DataMember]
        public DateTime Date { get; set; }
    }
}