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
        /// <param name="record">The record containing the data for the <see cref="Role"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private Role(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the id of the role.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the role in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <summary>Loads all roles from the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>RolesGetAll</c>.
        /// Result 1 columns: RoleId, Language, Title
        /// </remarks>
        /// <returns>All roles.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This method performs a time-consuming operation.")]
        public static IEnumerable<Role> GetAll()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
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
        /// <exception cref="ArgumentNullException"><paramref name="idList"/> is <see langword="null"/></exception>
        public static IEnumerable<Role> Get(IEnumerable<int> idList)
        {
            if (idList == null || !idList.Any())
            {
                throw new ArgumentNullException(nameof(idList));
            }

            using (var connection = new SqlConnection(Persistent.ConnectionString))
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
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Role"/> is not valid to be saved.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public void Save()
        {
            this.ValidateSaveCandidate();
            this.SaveToDatabase();
        }

        /// <summary>Validates that this <see cref="Role"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Role"/> is not valid to be saved.</exception>
        private void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
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
                command.Parameters.AddWithValue("@titles", this.Titles.GetSaveTitles);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.ReadFromRecord(reader);
                    }

                    if (reader.NextResult())
                    {
                        this.Titles = new LanguageTitleCollection(reader);
                    }
                }
            }
        }
        
        /// <summary>Updates this <see cref="Role"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Role"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "RoleId" });
            this.Id = (int)record["RoleId"];
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
