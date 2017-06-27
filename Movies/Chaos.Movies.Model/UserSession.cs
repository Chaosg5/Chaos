//-----------------------------------------------------------------------
// <copyright file="UserSession.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    /// <summary>A login session for a specific <see cref="User"/>.</summary>
    public class UserSession
    {
        public UserSession(IDataRecord record)
        {
        }

        /// <summary>Gets the id of the session.</summary>
        public Guid SessionId { get; private set; }

        public string ClientIp { get; private set; }

        public int UserId { get; private set; }

        public DateTime ActiveForm { get; private set; }

        public DateTime ActiveTo { get; private set; }

        /// <summary>Refreshes this <see cref="UserSession"/> by adding time to it.</summary>
        public void RefreshSession(int minutes)
        {
            this.ActiveTo = DateTime.Now.AddMinutes(minutes);
            // ToDo: Save
        }

        /// <summary>The read from record.</summary>
        /// <param name="session">The session.</param>
        /// <param name="record">The record.</param>
        private static void ReadFromRecord(UserSession session, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "SessionId", "ClientIp", "UserId", "ActiveFrom", "ActiveTo" });
            session.SessionId = (Guid)record["SessionId"];
            session.ClientIp = record["ClientIp"].ToString();
            session.UserId = (int)record["UserId"];
            session.ActiveForm = (DateTime)record["ActiveFrom"];
            session.ActiveTo = (DateTime)record["ActiveTo"];
        }
    }
}
