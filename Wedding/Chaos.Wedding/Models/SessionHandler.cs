//-----------------------------------------------------------------------
// <copyright file="SessionHandler.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models
{
    using System;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>The session handler.</summary>
    public static class SessionHandler
    {
        /// <summary>The current session.</summary>
        private static UserSession currentSession;

        /// <summary>Gets the session.</summary>
        /// <returns>The <see cref="UserSession"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        public static async Task<UserSession> GetSessionAsync()
        {
            if (currentSession == null)
            {
                currentSession = await GetSystemSessionAsync();
            }

            try
            {
                await currentSession.ValidateSessionAsync();
            }
            // ReSharper disable once CatchAllClause
            catch
            {
                currentSession = await GetSystemSessionAsync();
            }

            return currentSession;
        }

        /// <summary>Gets a <see cref="UserSession"/> for the system user.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        private static async Task<UserSession> GetSystemSessionAsync()
        {
            var login = new UserLogin(
                 Movies.Model.Properties.Settings.Default.SystemUserName,
                 Movies.Model.Properties.Settings.Default.SystemPassword,
                await GameCache.GetServerIpAsync());
            var session = await UserSession.Static.CreateSessionAsync(login);
            return session;
        }
    }
}