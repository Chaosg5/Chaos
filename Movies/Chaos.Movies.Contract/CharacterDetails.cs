//-----------------------------------------------------------------------
// <copyright file="CharacterDetails.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>Item details for a <see cref="PersonDto"/>.</summary>
    [DataContract]
    public class CharacterDetails
    {
        /// <summary>Gets or sets the movie.</summary>
        [DataMember]
        public MovieDto Movie { get; set; }

        ///// <summary>Gets or sets the movie.</summary>
        //[DataMember]
        //public IEnumerable<> Movie { get; set; }
    }
}
