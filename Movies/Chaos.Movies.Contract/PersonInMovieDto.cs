//-----------------------------------------------------------------------
// <copyright file="PersonInMovieDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class PersonInMovieDto
    {
        /// <summary>Gets the person.</summary>
        [DataMember]
        public PersonDto Person { get; private set; }

        /// <summary>Gets the role of the <see cref="Person"/>.</summary>
        [DataMember]
        public RoleDto Role { get; private set; }

        /// <summary>Gets the department of the <see cref="Person"/>.</summary>
        [DataMember]
        public DepartmentDto Department { get; private set; }

        /// <summary>Gets the current user's rating of the <see cref="CharacterDto"/> in the <see cref="MovieDto"/>.</summary>
        [DataMember]
        public int UserRating { get; private set; }
    }
}
