//-----------------------------------------------------------------------
// <copyright file="IUserLogin.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    /// <summary>A login request for a specific <see cref="IUser"/>.</summary>
    public interface IUserLogin
    {
        /// <summary>Gets the username of the user logging in.</summary>
        string Username { get; }

        /// <summary>Gets the IP-address of the client device the user is using.</summary>
        string ClientIp { get; }

        /// <summary>Gets the encrypted </summary>
        string Password { get; }
    }
}
