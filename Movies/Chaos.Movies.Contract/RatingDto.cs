//-----------------------------------------------------------------------
// <copyright file="RatingDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Windows.Media;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class RatingDto
    {
        /// <summary>Gets the id of this rating.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets the id of the parent <see cref="RatingDto"/>.</summary>
        [DataMember]
        public int ParentRatingId { get; set; }

        /// <summary>Gets the id of the <see cref="UserDto"/> who owns the rating.</summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>Gets the type of this rating.</summary>
        [DataMember]
        public RatingTypeDto RatingType { get; set; }

        /// <summary>Gets the child ratings of this rating.</summary>
        [DataMember]
        public ReadOnlyCollection<RatingDto> SubRatings { get; set; }

        /// <summary>Gets the values of this rating.</summary>
        [DataMember]
        public double Value { get; set; }

        /// <summary>Gets the display color in RBG hex for this <see cref="RatingDto"/>'s <see cref="Value"/>.</summary>
        [DataMember]
        public string HexColor { get; }

        /// <summary>Gets the display color for this <see cref="RatingDto"/>'s <see cref="Value"/>.</summary>
        [DataMember]
        public Color Color { get; }

        /// <summary>Gets the display value for this <see cref="RatingDto"/>'s <see cref="Value"/>.</summary>
        [DataMember]
        public string DisplayValue { get; }
    }
}
