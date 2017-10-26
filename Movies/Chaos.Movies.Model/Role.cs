//-----------------------------------------------------------------------
// <copyright file="Role.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a role of a person in a movie.</summary>
    public class Role
    {
        /// <summary>Initializes a new instance of the <see cref="Role" /> class.</summary>
        /// <param name="record">The record containing the data for the role.</param>
        private Role(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of the role.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the role in different languages.</summary>
        public LanguageTitles Titles { get; private set; } = new LanguageTitles();

        /// <summary>Loads all roles from the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>RolesGetAll</c>.
        /// Result 1 columns: RoleId, Language, Title
        /// </remarks>
        /// <returns>All roles.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This method performs a time-consuming operation.")]
        public static IEnumerable<Role> GetAll()
        {
            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("RolesGetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    return ReadFromReader(reader);
                }
            }
        }

        /// <summary>Gets the specified <see cref="Role"/>s.</summary>
        /// <remarks>
        /// Uses stored procedure <c>RolesGet</c>.
        /// Result 1 columns: RoleId, Language, Title
        /// </remarks>
        /// <param name="idList">The list of ids of the <see cref="Role"/>s to get.</param>
        /// <returns>The specified <see cref="Role"/>s.</returns>
        public static IEnumerable<Role> Get(IEnumerable<int> idList)
        {
            if (idList == null || !idList.Any())
            {
                throw new ArgumentNullException("idList");
            }

            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("RolesGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idList", idList);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    return ReadFromReader(reader);
                }
            }
        }

        /// <summary>Saves this role to the database.</summary>
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Validates that the <paramref name="role"/> is valid to be saved.</summary>
        /// <param name="role">The role to validate.</param>
        private static void ValidateSaveCandidate(Role role)
        {
            if (role.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        /// <summary>Saves a role to the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>RoleSave</c>.
        /// Result 1 columns: RoleId
        /// Result 2 columns: Language, Title
        /// </remarks>
        /// <param name="role">The role to save.</param>
        private static void SaveToDatabase(Role role)
        {
            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("RoleSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoleId", role.Id);
                command.Parameters.AddWithValue("@titles", role.Titles.GetSaveTitles);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(role, reader);
                    }

                    if (reader.NextResult())
                    {
                        role.Titles = new LanguageTitles(reader);
                    }
                }
            }
        }

        /// <summary>Updates a role from a record.</summary>
        /// <param name="role">The role to update.</param>
        /// <param name="record">The record containing the data for the role.</param>
        private static void ReadFromRecord(Role role, IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "RoleId" });
            role.Id = (int)record["RoleId"];
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
    }
}
