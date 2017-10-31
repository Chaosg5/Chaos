//-----------------------------------------------------------------------
// <copyright file="DepartmentDto.cs" company="Erik Bunnstad">
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
        /// <summary>Gets the id of the department.</summary>
        [DataMember]
        public int Id { get; private set; }

        /// <summary>Gets or sets the list of titles of the department in different languages.</summary>
        [DataMember]
        public ReadOnlyCollection<LanguageTitleDto> Titles { get; set; }

        /// <summary>Gets or sets all available person roles.</summary>
        [DataMember]
        public ReadOnlyCollection<RoleDto> Roles { get; set; }
    }
}
