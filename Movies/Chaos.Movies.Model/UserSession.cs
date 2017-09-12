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
    using Chaos.Movies.Contract.Dto;

    /// <summary>A login session for a specific <see cref="User"/>.</summary>
    public class UserSession : IUserSession
    {
        /// <summary>Initializes a new instance of the <see cref="UserSession"/> class.</summary>
        /// <param name="record">The record.</param>
        public UserSession(IDataRecord record)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="UserSession"/> class.</summary>
        /// <param name="session">The session.</param>
        public UserSession(IUserSession session)
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
        
        /// <summary>The read from record.</summary>
        /// <param name="session">The session.</param>
        /// <param name="record">The record.</param>
        private static void ReadFromRecord(UserSession session, IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "SessionId", "ClientIp", "UserId", "ActiveFrom", "ActiveTo" });
            session.SessionId = (Guid)record["SessionId"];
            session.ClientIp = record["ClientIp"].ToString();
            session.UserId = (int)record["UserId"];
            session.ActiveForm = (DateTime)record["ActiveFrom"];
            session.ActiveTo = (DateTime)record["ActiveTo"];
        }
    }
}
