﻿//-----------------------------------------------------------------------
// <copyright file="DepartmentDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class DepartmentDto
    {
        /// <summary>Gets or sets the id of the department.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the list of titles of the department in different languages.</summary>
        [DataMember]
        public LanguageTitleCollectionDto Titles { get; set; }

        /// <summary>Gets or sets all available person roles.</summary>
        [DataMember]
        public ReadOnlyCollection<RoleDto> Roles { get; set; }
    }
}
