//-----------------------------------------------------------------------
// <copyright file="RoleDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class RoleDto
    {
        /// <summary>Gets or sets the id of the role.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the list of titles of the role in different languages.</summary>
        [DataMember]
        public LanguageTitlesDto Titles { get; set; }
    }
}
