//-----------------------------------------------------------------------
// <copyright file="Role.cs">
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
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a role of a person in a movie.</summary>
    public sealed class Role : Typeable<Role, RoleDto>
    {
        /// <summary>The database column for <see cref="Id"/>.</summary>
        private const string RoleIdColumn = "RoleId";

        /// <summary>The database column for <see cref="Titles"/>.</summary>
        private const string TitlesColumn = "Titles";

        /// <inheritdoc />
        public Role()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private Role(IDataRecord record)
        {
            this.ReadFromRecordAsync(record);
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Role Static { get; } = new Role();

        /// <summary>Gets the id of the role.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the role in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();
        
        /// <inheritdoc />
        public override RoleDto ToContract()
        {
            return new RoleDto
            {
                Id = this.Id,
                Titles = this.Titles.ToContract()
            };
        }

        /// <summary>Validates that this <see cref="Role"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Role"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }
        
        /// <inheritdoc />
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.MissingColumnException">A required column is missing in the <paramref name="record" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="record" /> is <see langword="null" />.</exception>
        public override Task<Role> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "RoleId" });
            return Task.FromResult(new Role { Id = (int)record["RoleId"] });
        }

        /// <inheritdoc />
        public override Task SaveAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<Role> GetAsync(UserSession session, int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<IEnumerable<Role>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task SaveAllAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<IEnumerable<Role>> GetAllAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override Task<IEnumerable<Role>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(RoleIdColumn), this.Id },
                    { Persistent.ColumnToVariable(TitlesColumn), this.Titles.GetSaveTable }
                });
        }

        /// <summary>Creates a list of <see cref="Role"/>s from a reader.</summary>
        /// <param name="reader">The reader containing the data for the <see cref="Role"/>s.</param>
        /// <returns>The list of <see cref="Role"/>s.</returns>
        private static IEnumerable<Role> ReadFromReader(SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, "");
            }

            var result = new List<Role>();
            Role role = null;
            while (reader.Read())
            {
                var id = (int)reader["RoleId"];
                if (role == null || role.Id != id)
                {
                    role = result.Find(t => t.Id == id);
                    if (role == null)
                    {
                        role = new Role(reader);
                        result.Add(role);
                    }
                }

                role.Titles.SetTitle(new LanguageTitle(reader));
            }

            return result;
        }

        /// <summary>Saves this <see cref="Role"/> to the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>RoleSave</c>.
        /// Result 1 columns: RoleId
        /// Result 2 columns: Language, Title
        /// </remarks>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private void SaveToDatabase()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("RoleSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoleId", this.Id);
                command.Parameters.AddWithValue("@titles", this.Titles.GetSaveTable);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                      ////  await this.ReadFromRecordAsync(reader);
                    }

                    if (reader.NextResult())
                    {
                        this.Titles = new LanguageTitleCollection(reader);
                    }
                }
            }
        }
    }
}
