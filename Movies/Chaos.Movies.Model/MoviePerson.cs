//-----------------------------------------------------------------------
// <copyright file="MoviePerson.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    /// <summary>Represents a person in a movie.</summary>
    public class MoviePerson
    {
        /// <summary>Gets or sets the person.</summary>
        public Person Person { get; set; }

        /// <summary>Gets or sets the role of the <see cref="Person"/>.</summary>
        public Role Role { get; set; }

        /// <summary>Gets or sets the department of the <see cref="Person"/>.</summary>
        public Department Department { get; set; }
    }
}
