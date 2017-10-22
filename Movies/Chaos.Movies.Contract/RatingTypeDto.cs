//-----------------------------------------------------------------------
// <copyright file="RatingTypeDto.cs" company="Erik Bunnstad">
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

        /// <summary>Gets or sets the name of this rating type.</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>Gets or sets the description of this rating type.</summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>Gets or sets the <see cref="RatingTypeDto"/>s that makes up the derived children of this <see cref="RatingTypeDto"/>.</summary>
        [DataMember]
        public ReadOnlyCollection<RatingTypeDto> Subtypes { get; set; }
    }
}