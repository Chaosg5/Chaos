//-----------------------------------------------------------------------
// <copyright file="SearchParametersDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents search parameters.</summary>
    [DataContract]
    public class SearchParametersDto
    {
        /// <summary>Gets or sets the search text.</summary>
        [DataMember]
        public string SearchText { get; set; }

        /// <summary>Gets or sets a value indicating whether require exact match.</summary>
        [DataMember]
        public bool RequireExactMatch { get; set; }

        /// <summary>Gets or sets the search limit.</summary>
        [DataMember]
        public int SearchLimit { get; set; }
    }
}