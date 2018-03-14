//-----------------------------------------------------------------------
// <copyright file="UserRatingDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Windows.Media;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class UserRatingDto
    {
        /// <summary>Gets or sets the id of the <see cref="UserDto"/> who owns the rating.</summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>Gets or sets the type of this rating.</summary>
        [DataMember]
        public RatingTypeDto RatingType { get; set; }

        /// <summary>Gets or sets the child ratings of this rating.</summary>
        [DataMember]
        public ReadOnlyCollection<UserRatingDto> SubRatings { get; set; }

        /// <summary>Gets or sets the value of this rating.</summary>
        [DataMember]
        public int Value { get; set; }

        /// <summary>Gets or sets the derived value of this rating.</summary>
        [DataMember]
        public double Derived { get; set; }

        /// <summary>Gets or sets the display color in RBG hex for this <see cref="UserRatingDto"/>'s <see cref="Value"/>.</summary>
        [DataMember]
        public string HexColor { get; set; }

        /// <summary>Gets or sets the display color for this <see cref="UserRatingDto"/>'s <see cref="Value"/>.</summary>
        [DataMember]
        public Color Color { get; set; }

        /// <summary>Gets or sets the display value for this <see cref="UserRatingDto"/>'s <see cref="Value"/>.</summary>
        [DataMember]
        public string DisplayValue { get; set; }

        /// <summary>Gets or sets the created date.</summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}
