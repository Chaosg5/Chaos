//-----------------------------------------------------------------------
// <copyright file="IUserSession.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;

    /// <summary>A login session for a specific <see cref="IUser"/>.</summary>
    public interface IUserSession
    {
        /// <summary>Gets the id of the session.</summary>
        Guid SessionId { get; }

        /// <summary>Gets the client IP.</summary>
        string ClientIp { get; }

        /// <summary>Gets the user id.</summary>
        int UserId { get; }

        /// <summary>Gets the active form.</summary>
        DateTime ActiveForm { get; }

        /// <summary>Gets the active to.</summary>
        DateTime ActiveTo { get; }
    }
}
