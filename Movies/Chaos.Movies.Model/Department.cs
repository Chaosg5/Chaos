//-----------------------------------------------------------------------
// <copyright file="Department.cs">
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
    using System.IO;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a production department in a <see cref="Movie"/>.</summary>
    public class Department
    {
        /// <summary>Private part of the <see cref="Roles"/> property.</summary>
        private readonly List<Role> roles = new List<Role>();

        /// <summary>Initializes a new instance of the <see cref="Department" /> class.</summary>
        /// <param name="record">The record containing the data for the department.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
        private Department(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the id of the department.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the department in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <summary>Gets all available person roles.</summary>
        public ReadOnlyCollection<Role> Roles => this.roles.AsReadOnly();

        /// <summary>Loads all departments from the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>DepartmentsGetAll</c>.
        /// Result 1 columns: DepartmentId, Language, Title
        /// Result 2 columns: DepartmentId, RoleId
        /// </remarks>
        /// <returns>All departments.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This method performs a time-consuming operation.")]
        public static IEnumerable<Department> GetAll()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("DepartmentsGetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    return ReadFromReader(reader);
                }
            }
        }

        /// <summary>Loads all <see cref="Department"/>s from the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>DepartmentsGet</c>.
        /// Result 1 columns: DepartmentId, Language, Title
        /// Result 2 columns: DepartmentId, RoleId
        /// </remarks>
        /// <param name="idList">The list of ids of the <see cref="Department"/>s to get.</param>
        /// <returns>The specified <see cref="Department"/>s.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="idList"/> is <see langword="null" />.</exception>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public static IEnumerable<Department> Get(IEnumerable<int> idList)
        {
            if (idList == null || !idList.Any())
            {
                throw new ArgumentNullException(nameof(idList));
            }

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("DepartmentsGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idList", idList);
                command.Parameters.AddWithValue("@idList", idList);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    return ReadFromReader(reader);
                }
            }
        }

        /// <summary>Saves this department to the database.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Department"/> is not valid to be saved.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        public void Save()
        {
            this.ValidateSaveCandidate();
            this.SaveToDatabase();
        }

        /// <summary>Validates that this <see cref="Department"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Department"/> is not valid to be saved.</exception>
        private void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        /// <summary>Saves this <see cref="Department"/> to the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>DepartmentSave</c>.
        /// Result 1 columns: DepartmentId
        /// Result 2 columns: Language, Title
        /// </remarks>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        private void SaveToDatabase()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("DepartmentSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DepartmentId", this.Id);
                command.Parameters.AddWithValue("@titles", this.Titles.GetSaveTitles);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        throw new MissingResultException(1, "Departments");
                    }

                    if (reader.Read())
                    {
                        this.ReadFromRecord(reader);
                    }

                    if (!reader.NextResult() || !reader.HasRows)
                    {
                        throw new MissingResultException(2, "DepartmentTitles");
                    }

                    this.Titles = new LanguageTitleCollection(reader);
                }
            }
        }
        
        /// <summary>Updates this <see cref="Department"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Department"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "DepartmentId" });
            this.Id = (int)record["DepartmentId"];
        }

        /// <summary>Creates a list of <see cref="Department"/>s from a reader.</summary>
        /// <param name="reader">The reader containing the data for the <see cref="Department"/>s.</param>
        /// <returns>The list of <see cref="Department"/>s.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        private static IEnumerable<Department> ReadFromReader(SqlDataReader reader)
        {
            var result = new List<Department>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, "Departments");
            }

            Department department = null;
            while (reader.Read())
            {
                var id = (int)reader["DepartmentId"];
                if (department == null || department.Id != id)
                {
                    department = result.Find(t => t.Id == id);
                    if (department == null)
                    {
                        department = new Department(reader);
                        result.Add(department);
                    }
                }

                department.Titles.SetTitle(new LanguageTitle(reader));
            }

            if (!reader.NextResult() || !reader.HasRows)
            {
                throw new MissingResultException(2, "DepartmentTitles");
            }

            while (reader.Read())
            {
                var departmentId = (int)reader["DepartmentId"];
                var roleId = (int)reader["RoleId"];
                department = result.Find(t => t.Id == departmentId);
                if (department == null)
                {
                    throw new SqlResultSyncException(departmentId);
                }

                department.roles.Add(GlobalCache.GetRole(roleId));
            }

            return result;
        }
    }
}
