//-----------------------------------------------------------------------
// <copyright file="Department.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a production department in a movie.</summary>
    public class Department
    {
        /// <summary>Private part of the <see cref="Roles"/> property.</summary>
        private readonly List<Role> roles = new List<Role>();

        /// <summary>Initializes a new instance of the <see cref="Department" /> class.</summary>
        public Department()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Department" /> class.</summary>
        /// <param name="record">The record containing the data for the department.</param>
        public Department(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of the department.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the department in different languages.</summary>
        public LanguageTitles Titles { get; private set; } = new LanguageTitles();

        /// <summary>Gets all available person roles.</summary>
        public ReadOnlyCollection<Role> Roles => this.roles.AsReadOnly();

        /// <summary>Loads all departments from the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>DepartmentsGetAll</c>.
        /// Result 1 columns: DepartmentId, Language, Title
        /// Result 2 columns: DepartmentId, RoleId
        /// </remarks>
        /// <returns>All departments.</returns>
        public static IEnumerable<Department> GetAll()
        {
            var result = new List<Department>();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("DepartmentsGetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        throw new MissingResultException(1);
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
                        throw new MissingResultException(2);
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
                }
            }

            return result;
        }

        /// <summary>Saves this department to the database.</summary>
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Validates that the <paramref name="department"/> is valid to be saved.</summary>
        /// <param name="department">The department to validate.</param>
        private static void ValidateSaveCandidate(Department department)
        {
            if (department.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        /// <summary>Saves a department to the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>DepartmentSave</c>.
        /// Result 1 columns: DepartmentId
        /// Result 2 columns: Language, Title
        /// </remarks>
        /// <param name="department">The department to save.</param>
        private static void SaveToDatabase(Department department)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("DepartmentSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@DepartmentId", department.Id));
                command.Parameters.Add(new SqlParameter("@titles", department.Titles.GetSaveTitles));
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(department, reader);
                    }

                    if (reader.NextResult())
                    {
                        department.Titles = new LanguageTitles(reader);
                    }
                }
            }
        }

        /// <summary>Updates a department from a record.</summary>
        /// <param name="department">The department to update.</param>
        /// <param name="record">The record containing the data for the department.</param>
        private static void ReadFromRecord(Department department, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "DepartmentId" });
            department.Id = (int)record["DepartmentId"];
        }
    }
}
