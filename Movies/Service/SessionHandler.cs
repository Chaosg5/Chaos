//-----------------------------------------------------------------------
// <copyright file="SessionHandler.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Service
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Chaos.Movies.Model;

    /// <summary>Handles logic for <see cref="UserSession"/>s</summary>
    public static class SessionHandler
    {
        private static readonly AsyncCache<Guid, UserSession> SessionCache = new AsyncCache<Guid, UserSession>(GetSessionAsync);

        private static Task<UserSession> GetSessionAsync(Guid guid)
        {
            // ToDo: Get from SqlUserSession
            throw new NotImplementedException();
        }

        internal static async Task ValidateSessionAsync(UserSession userSession)
        {
            if (userSession == null)
            {
                throw new ArgumentNullException(nameof(userSession));
            }

            if (userSession.SessionId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userSession.SessionId));
            }

            var session = await SessionCache.GetValue(userSession.SessionId);
            if (session?.SessionId != userSession.SessionId)
            {
                
            }

        }
    }
}