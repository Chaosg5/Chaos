//-----------------------------------------------------------------------
// <copyright file="IUserSingleRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract.Interface
{
    using System;

    /// <inheritdoc />
    /// <summary>Represents a user's rating for an item.</summary>
    public interface IUserSingleRating : IRating
    {
        /// <summary>Gets the id <see cref="UserDto"/> which the <see cref="IUserSingleRating"/> belongs to.</summary>
        int UserId { get; }

        /// <summary>Gets or sets the created date.</summary>
        DateTime CreatedDate { get; set; }
    }
}
