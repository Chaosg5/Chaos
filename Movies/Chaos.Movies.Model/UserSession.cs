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
    using System.Data.SqlClient;
    using System.Threading;
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

        /// <summary>Available sessions.</summary>
        private static readonly AsyncCache<Guid, UserSession> UserSessions = new AsyncCache<Guid, UserSession>(GetFromDatabaseAsync);

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
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        public async Task<UserSession> CreateSessionAsync(UserLogin login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (!Persistent.UseService)
            {
                return await this.CreateSessionToDatabaseAsync(login);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return this.FromContract(await service.CreateUserSessionAsync(login));
            }
        }

        /// <summary>Validates that this <see cref="UserSession"/> is valid.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ValidateSessionAsync()
        {
            var session = await UserSessions.GetValue(this.SessionId);
            if (session.ActiveTo > DateTime.Now)
            {
                return;
            }

            if (session.SessionId == Guid.Empty)
            {
                // ReSharper disable once ExceptionNotDocumented
                throw new InvalidSessionException("The session does not exist and can't be used.");
            }
            
            // Updates ActiveTo from the database, since the cache may not be up to date
            session = await GetFromDatabaseAsync(this.SessionId);
            if (session.ActiveTo > DateTime.Now)
            {
                UserSessions.SetValue(session.SessionId, session);
                return;
            }

            // ReSharper disable once ExceptionNotDocumented
            throw new InvalidSessionException("The session has expired and can't be used.");
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
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override UserSession FromContract(UserSessionDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

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
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                await service.UserSessionSaveAsync(session.ToContract(), this.ToContract());
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

        /// <summary>The get from database async.</summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task<UserSession> GetFromDatabaseAsync(Guid sessionId)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{typeof(UserSession).Name}Get", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                // ReSharper disable once ExceptionNotDocumented
                command.Parameters.AddWithValue(
                    Persistent.ColumnToVariable($"{IdColumn}s"),
                    Persistent.CreateIdCollectionTable(new List<Guid> { sessionId }));
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    // ReSharper disable once ExceptionNotDocumented
                    if (await reader.ReadAsync())
                    {
                        return await Static.NewFromRecordAsync(reader);
                    }
                }
            }

            return new UserSession();
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

        /// <summary>The create session to database async.</summary>
        /// <param name="login">The login.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">Failed to create a new session.</exception>
        private async Task<UserSession> CreateSessionToDatabaseAsync(UserLogin login)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{nameof(UserSession)}Save", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (var commandParameter in this.GetSaveParameters(login))
                {
                    command.Parameters.AddWithValue(commandParameter.Key, commandParameter.Value);
                }

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var session = new UserSession();
                        await session.ReadFromRecordAsync(reader);
                        return session;
                    }

                    throw new MissingResultException("Failed to create a new session.");
                }
            }
        }
    }
}
