//-----------------------------------------------------------------------
// <copyright file="UserSession.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A login session for a specific <see cref="User"/>.</summary>
    public class UserSession : Persistable<UserSession, UserSessionDto>
    {
        /// <summary>The database column for <see cref="ClientIp"/>.</summary>
        private const string ClientIpColumn = "ClientIp";

        /// <summary>The database column for <see cref="ActiveFrom"/>.</summary>
        private const string ActiveFromColumn = "ActiveFrom";

        /// <summary>The database column for <see cref="ActiveTo"/>.</summary>
        private const string ActiveToColumn = "ActiveTo";

        /// <summary>Prevents a default instance of the <see cref="UserSession"/> class from being created.</summary>
        private UserSession()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static UserSession Static { get; } = new UserSession();

        /// <summary>Gets the session id.</summary>
        public Guid SessionId { get; private set; }

        /// <summary>Gets the client IP.</summary>
        public string ClientIp { get; private set; }

        /// <summary>Gets the user id.</summary>
        public int UserId { get; private set; }

        /// <summary>Gets the active form.</summary>
        public DateTime ActiveFrom { get; private set; } = DateTime.Now;

        /// <summary>Gets the active to.</summary>
        public DateTime ActiveTo { get; private set; } = DateTime.Now.AddMinutes(30);

        /// <summary>Creates a new <see cref="UserSession"/>.</summary>
        /// <param name="login">The login.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="login"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static async Task<UserSession> CreateSessionAsync(UserLogin login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var session = new UserSession();
            await session.CreateUserSessionAsync(login);
            return session;
        }

        /// <inheritdoc />
        public override UserSessionDto ToContract()
        {
            return new UserSessionDto
            {
                SessionId = this.SessionId,
                ClientIp = this.ClientIp,
                UserId = this.UserId,
                ActiveFrom = this.ActiveFrom,
                ActiveTo = this.ActiveTo
            };
        }

        /// <inheritdoc />
        public override UserSession FromContract(UserSessionDto contract)
        {
            return new UserSession
            {
                SessionId = contract.SessionId,
                ClientIp = contract.ClientIp,
                UserId = contract.UserId,
                ActiveFrom = contract.ActiveFrom,
                ActiveTo = contract.ActiveTo
            };
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.({T})SaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <summary>Refreshes this <see cref="UserSession"/> by adding time to it.</summary>
        /// <param name="minutes">The minutes to refresh with.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task RefreshSessionAsync(int minutes)
        {
            // ToDo: Save & get value from Configuration instead?
            this.ActiveTo = DateTime.Now.AddMinutes(minutes);
            await this.SaveAsync(this);
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<UserSession> NewFromRecordAsync(IDataRecord record)
        {
            var result = new UserSession();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, ClientIpColumn, User.IdColumn, ActiveFromColumn, ActiveToColumn });
            this.SessionId = (Guid)record[IdColumn];
            this.ClientIp = record[ClientIpColumn].ToString();
            this.UserId = (int)record[User.IdColumn];
            this.ActiveFrom = (DateTime)record[ActiveFromColumn];
            this.ActiveTo = (DateTime)record[ActiveToColumn]; 
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.SessionId },
                    { Persistent.ColumnToVariable(ClientIpColumn), this.ClientIp },
                    { Persistent.ColumnToVariable(User.IdColumn), this.UserId },
                    { Persistent.ColumnToVariable(ActiveFromColumn), this.ActiveFrom },
                    { Persistent.ColumnToVariable(ActiveToColumn), this.ActiveTo },
                });
        }


        /// <summary>Gets SQL parameters to use for <see cref="Persistable{T,TDto}.SaveAsync"/>.</summary>
        /// <param name="userLogin">The user login credentials.</param>
        /// <returns>The list of SQL parameters.</returns>
        private IReadOnlyDictionary<string, object> GetSaveParameters(UserLogin userLogin)
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(ClientIpColumn), userLogin.ClientIp },
                    { Persistent.ColumnToVariable(ActiveFromColumn), this.ActiveFrom },
                    { Persistent.ColumnToVariable(ActiveToColumn), this.ActiveTo },
                    { Persistent.ColumnToVariable(User.UsernameColumn), userLogin.Username },
                    { Persistent.ColumnToVariable(User.PasswordColumn), userLogin.Password }
                });
        }

        /// <summary>Creates a new <see cref="UserSession"/>.</summary>
        /// <param name="login">The login.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task CreateUserSessionAsync(UserLogin login)
        {
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(login), this.ReadFromRecordAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.({T})SaveAsync(session.ToContract(), this.ToContract());
            }
        }
    }
}
