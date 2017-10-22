//-----------------------------------------------------------------------
// <copyright file="RatingValueDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class RatingValueDto
    {
        /// <summary>Gets or sets the value of the rating.</summary>
        [DataMember]
        public int Value { get; set; }

        /// <summary>Gets or sets the derived value from sub ratings.</summary>
        [DataMember]
        public double Derived { get; set; }
    }
}
