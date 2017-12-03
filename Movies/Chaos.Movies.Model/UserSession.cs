//-----------------------------------------------------------------------
// <copyright file="UserSession.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A login session for a specific <see cref="User"/>.</summary>
    public class UserSession
    {
        /// <summary>Initializes a new instance of the <see cref="UserSession"/> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="UserSession"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public UserSession(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Initializes a new instance of the <see cref="UserSession"/> class.</summary>
        /// <param name="session">The session.</param>
        public UserSession(UserSessionDto session)
        {
            this.SessionId = session.SessionId;
            this.ClientIp = session.ClientIp;
            this.UserId = session.UserId;
            this.ActiveForm = session.ActiveForm;
            this.ActiveTo = session.ActiveTo;
        }

        /// <summary>Gets or sets the session id.</summary>
        public Guid SessionId { get; set; }

        /// <summary>Gets or sets the client IP.</summary>
        public string ClientIp { get; set; }

        /// <summary>Gets or sets the user id.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the active form.</summary>
        public DateTime ActiveForm { get; set; }

        /// <summary>Gets or sets the active to.</summary>
        public DateTime ActiveTo { get; set; }

        /// <summary>Converts this <see cref="UserSession"/> to a <see cref="UserSessionDto"/>.</summary>
        /// <returns>The <see cref="UserSessionDto"/>.</returns>
        public UserSessionDto ToContract()
        {
            return new UserSessionDto
            {
                SessionId = this.SessionId,
                ClientIp = this.ClientIp,
                UserId = this.UserId,
                ActiveForm = this.ActiveForm,
                ActiveTo = this.ActiveTo
            };
        }

        /// <summary>Refreshes this <see cref="UserSession"/> by adding time to it.</summary>
        public void RefreshSession(int minutes)
        {
            // ToDo: Save & get value from Configuration instead?
            this.ActiveTo = DateTime.Now.AddMinutes(minutes);
        }

        /// <summary>Updates this <see cref="UserSession"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="UserSession"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "SessionId", "ClientIp", "UserId", "ActiveFrom", "ActiveTo" });
            this.SessionId = (Guid)record["SessionId"];
            this.ClientIp = record["ClientIp"].ToString();
            this.UserId = (int)record["UserId"];
            this.ActiveForm = (DateTime)record["ActiveFrom"];
            this.ActiveTo = (DateTime)record["ActiveTo"];
        }
    }
}
