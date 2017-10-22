//-----------------------------------------------------------------------
// <copyright file="User.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using Chaos.Movies.Contract;

    /// <summary>A user.</summary>
    public class User
    {
        /// <summary>Gets the id of the <see cref="User"/>.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the username of the <see cref="User"/>.</summary>
        public string UserName { get; private set; }

        /// <summary>Gets the name of the <see cref="User"/>.</summary>
        public string Name { get; private set; }
    }
}
