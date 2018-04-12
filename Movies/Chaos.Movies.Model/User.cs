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
    using System.Linq;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A user.</summary>
    public class User : Readable<User, UserDto>
    {
        /// <summary>The database column for <see cref="Username"/>.</summary>
        internal const string UsernameColumn = "Username";

        /// <summary>The database column for <see cref="Password"/>.</summary>
        internal const string PasswordColumn = "Password";

        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>The database column for <see cref="Email"/>.</summary>
        private const string EmailColumn = "Email";

        /// <summary>Private part of the <see cref="Username"/> property.</summary>
        private string username;

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name;

        /// <summary>Private part of the <see cref="Email"/> property.</summary>
        private string email;

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
            this.Password = userLogin.Password;
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

        /// <summary>Gets or sets the password.</summary>
        private string Password { get; set; }

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

        /// <summary>Updates the <see cref="Password"/> and/or <see cref="Username"/> of the <see cref="User"/> and saves.</summary>
        /// <param name="oldLogin">The old login credentials.</param>
        /// <param name="newLogin">The new login credentials.</param>
        /// <param name="session">The session of the user making the change.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentException">The <paramref name="oldLogin"/> does not match.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task UpdatePasswordAndSaveAsync(UserLogin oldLogin, UserLogin newLogin, UserSession session)
        {
            if (this.Username != oldLogin.Username || this.Password != oldLogin.Password)
            {
                throw new ArgumentException("The login credentials does not match.", nameof(oldLogin));
            }

            this.Username = newLogin.Username;
            this.Password = newLogin.Password;
            await this.SaveAsync(session);
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        internal override async Task<IEnumerable<User>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var departments = new List<User>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, $"{nameof(User)}s");
            }

            while (await reader.ReadAsync())
            {
                departments.Add(await this.NewFromRecordAsync(reader));
            }

            return departments;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<User> NewFromRecordAsync(IDataRecord record)
        {
            var result = new User();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
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
    }
}
