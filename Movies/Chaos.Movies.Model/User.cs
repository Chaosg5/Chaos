//-----------------------------------------------------------------------
// <copyright file="User.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A user.</summary>
    public class User : Readable<User, UserDto>, ISearchable<User>
    {
        /// <summary>The database column for <see cref="Username"/>.</summary>
        internal const string UsernameColumn = "Username";

        /// <summary>The database column for password.</summary>
        internal const string PasswordColumn = "Password";

        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>The database column for <see cref="Email"/>.</summary>
        private const string EmailColumn = "Email";

        /// <summary>The database column for <see cref="Username"/>.</summary>
        private const string OldUsernameColumn = "OldUsername";

        /// <summary>The database column for password.</summary>
        private const string OldPasswordColumn = "OldPassword";

        /// <summary>Private part of the <see cref="Username"/> property.</summary>
        private string username = string.Empty;

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name = string.Empty;

        /// <summary>Private part of the <see cref="Email"/> property.</summary>
        private string email = string.Empty;

        /// <inheritdoc />
        /// <param name="name">The <see cref="Name"/> to set.</param>
        /// <param name="email">The <see cref="Email"/> to set.</param>
        /// <param name="userLogin">The <see cref="Username"/> to set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="userLogin"/> is <see langword="null"/></exception>
        public User(string name, string email, UserLogin userLogin)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException(nameof(userLogin));
            }

            this.Username = userLogin.Username;
            this.Name = name;
            this.Email = email;
        }

        /// <summary>Prevents a default instance of the <see cref="User"/> class from being created.</summary>
        private User()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static User Static { get; } = new User();

        /// <summary>Gets or sets the username of the <see cref="User"/>.</summary>
        public string Username
        {
            get => this.username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(value);
                }

                this.username = value;
            }
        }

        /// <summary>Gets or sets the name of the <see cref="User"/>.</summary>
        public string Name
        {
            get => this.name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(value);
                }

                this.name = value;
            }
        }

        /// <summary>Gets or sets the e-mail of the <see cref="User"/>.</summary>
        public string Email
        {
            get => this.email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(value);
                }

                if (new MailAddress(value).Address != value)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentException($"The value {value} is not a valid e-mail.", nameof(value));
                }

                this.email = value;
            }
        }

        /// <inheritdoc />
        public override UserDto ToContract()
        {
            return new UserDto
            {
                Id = this.Id,
                Name = this.Name,
                UserName = this.Username,
                Email = this.Email
            };
        }

        /// <inheritdoc />
        public override UserDto ToContract(string languageName)
        {
            return new UserDto
            {
                Id = this.Id,
                Name = this.Name,
                UserName = this.Username,
                Email = this.Email
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override User FromContract(UserDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new User
            {
                Id = contract.Id,
                Name = contract.Name,
                Username = contract.UserName,
                Email = contract.Email
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<User> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<User>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.UserGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
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
                await service.UserSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <summary>Updates the password and/or <see cref="Username"/> of the <see cref="User"/> and saves.</summary>
        /// <param name="session">The session of the user making the change.</param>
        /// <param name="newLogin">The new login credentials.</param>
        /// <param name="oldLogin">The old login credentials.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public async Task SetPasswordAsync(UserSession session, UserLogin newLogin, UserLogin oldLogin = null)
        {
            if (!Persistent.UseService)
            {
                await this.SetPasswordToDatabaseAsync(oldLogin ?? newLogin, newLogin, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.UserSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <remarks>Searching with <see cref="SearchParametersDto.RequireExactMatch"/> = <see langword="true"/> and <see cref="SearchParametersDto.SearchLimit"/> = 1 will force the search to only <see cref="Username"/> for an exact match.</remarks>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<IEnumerable<User>> SearchAsync(SearchParametersDto parametersDto, UserSession session)
        {
            if (!Persistent.UseService)
            {
                var items = new List<User>();
                foreach (var id in await this.SearchDatabaseAsync(parametersDto, session))
                {
                    items.Add(await this.GetAsync(session, id));
                }

                return items;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return new List<User>();
                ////await service.CharacterSearchAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        public override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<IEnumerable<User>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var users = new List<User>();
            if (!reader.HasRows)
            {
                return users;
            }

            while (await reader.ReadAsync())
            {
                users.Add(await this.NewFromRecordAsync(reader));
            }

            return users;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override async Task<User> NewFromRecordAsync(IDataRecord record)
        {
            var result = new User();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, UsernameColumn, NameColumn, EmailColumn });
            this.Id = (int)record[IdColumn];
            this.Username = (string)record[UsernameColumn];
            this.Name = (string)record[NameColumn];
            this.Email = (string)record[EmailColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(UsernameColumn), this.Username },
                    { Persistent.ColumnToVariable(NameColumn), this.Name },
                    { Persistent.ColumnToVariable(EmailColumn), this.Email }
                });
        }

        /// <summary>Sets the password to the database.</summary>
        /// <param name="oldLogin">The old Login.</param>
        /// <param name="newLogin">The new Login.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="oldLogin"/> or <parmref name="newLogin"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private async Task SetPasswordToDatabaseAsync(UserLogin oldLogin, UserLogin newLogin, UserSession session)
        {
            if (oldLogin == null)
            {
                throw new ArgumentNullException(nameof(oldLogin));
            }

            if (newLogin == null)
            {
                throw new ArgumentNullException(nameof(newLogin));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            await session.ValidateSessionAsync();

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand($"{SchemaName}.{typeof(User).Name}SetPassword", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(User.IdColumn), this.Id);
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(OldUsernameColumn), oldLogin.Username);
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(OldPasswordColumn), oldLogin.Password);
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(UsernameColumn), newLogin.Username);
                command.Parameters.AddWithValue(Persistent.ColumnToVariable(PasswordColumn), newLogin.Password);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        await this.ReadFromRecordAsync(reader);
                    }
                }
            }
        }
    }
}
