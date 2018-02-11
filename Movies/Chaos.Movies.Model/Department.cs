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
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a production department in a <see cref="Movie"/>.</summary>
    public sealed class Department : Typeable<Department, DepartmentDto>, ITypeable<Department, DepartmentDto>
    {
        /// <summary>The database column for <see cref="Id"/>.</summary>
        private const string DepartmentIdColumn = "DepartmentId";

        /// <summary>The database column for <see cref="Titles"/>.</summary>
        private const string TitlesColumn = "Titles";

        /// <summary>The database column for <see cref="Roles"/>.</summary>
        private const string RolesColumn = "Roles";
        
        /// <inheritdoc />
        private Department()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Department Static { get; } = new Department();

        /// <summary>Gets the id of the department.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the department in different languages.</summary>
        public LanguageTitleCollection Titles { get; } = new LanguageTitleCollection();

        /// <summary>Gets all available person roles.</summary>
        public RolesCollection Roles { get; } = new RolesCollection();

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task<IEnumerable<Department>> GetAllAsync(UserSession session)
        {
            if (!Persistent.UseService)
            {
                return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.<T>GetAsync(session.ToContract(), idList.ToList())).Select(d => new <T>(d));
                return new List<Department>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<Department> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<IEnumerable<Department>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAsync(session.ToContract(), idList.ToList())).Select(x => new ({T})(x));
                return new List<Department>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Department"/> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task SaveAsync(UserSession session)
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

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Department"/> is not valid to be saved.</exception>
        public async Task SaveAllAsync(UserSession session)
        {
            await this.SaveAsync(session);
        }
        
        /// <inheritdoc />
        public DepartmentDto ToContract()
        {
            return new DepartmentDto
            {
                Id = this.Id,
                Titles = this.Titles.ToContract(),
                Roles = new ReadOnlyCollection<RoleDto>(this.Roles.Select(r => r.ToContract()).ToList())
            };
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        protected override async Task<IEnumerable<Department>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var result = new List<Department>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, "Departments");
            }

            Department department = null;
            while (await reader.ReadAsync())
            {
                var id = (int)reader["DepartmentId"];
                if (department == null || department.Id != id)
                {
                    department = result.Find(t => t.Id == id);
                    if (department == null)
                    {
                        department = await this.ReadFromRecordAsync(reader);
                        result.Add(department);
                    }
                }

                department.Titles.SetTitle(new LanguageTitle(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, "DepartmentTitles");
            }

            // ToDo: DepartmentTitles

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(3, "DepartmentRoles");
            }

            while (await reader.ReadAsync())
            {
                var departmentId = (int)reader["DepartmentId"];
                var roleId = (int)reader["RoleId"];
                department = result.Find(t => t.Id == departmentId);
                if (department == null)
                {
                    throw new SqlResultSyncException(departmentId);
                }

                department.Roles.Add(await GlobalCache.GetRoleAsync(roleId));
            }

            return result;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Department"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(DepartmentIdColumn), this.Id },
                    { Persistent.ColumnToVariable(TitlesColumn), this.Titles.GetSaveTable },
                    { Persistent.ColumnToVariable(RolesColumn), this.Roles.GetSaveTable }
                });
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override Task<Department> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "DepartmentId" });
            return Task.FromResult(new Department { Id = (int)record["DepartmentId"] });
        }
    }
}
