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
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a production department in a <see cref="Movie"/>.</summary>
    public sealed class Department : Typeable<Department, DepartmentDto>
    {
        /// <inheritdoc />
        public Department(DepartmentDto department)
            : base(department)
        {
        }

        /// <inheritdoc />
        public Department()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Department Static { get; } = new Department();
        
        /// <summary>Gets the list of titles of the department in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <summary>Gets all available person roles.</summary>
        public RoleCollection Roles { get; private set; } = new RoleCollection();

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Department>> GetAllAsync(UserSession session)
        {
            if (!Persistent.UseService)
            {
                return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAllAsync(session.ToContract())).Select(x => new ({T})(x));
                return new List<Department>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<Department> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<IEnumerable<Department>> GetAsync(UserSession session, IEnumerable<int> idList)
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

        /// <inheritdoc />
        public override DepartmentDto ToContract()
        {
            return new DepartmentDto
            {
                Id = this.Id,
                Titles = this.Titles.ToContract(),
                Roles = this.Roles.ToContract()
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override Department FromContract(DepartmentDto contract)
        {
            return new Department
            {
                Id = contract.Id,
                Titles = this.Titles.FromContract(contract.Titles),
                Roles = this.Roles.FromContract(contract.Roles)
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Department"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }

            if (this.Roles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one role needs to be specified.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override Task<Department> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            return Task.FromResult(new Department { Id = (int)record[IdColumn] });
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        protected override async Task<IEnumerable<Department>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var departments = new List<Department>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, $"{nameof(Department)}s");
            }
            
            while (await reader.ReadAsync())
            {
                departments.Add(await this.ReadFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, $"{nameof(Department)}{LanguageTitleCollection.TitlesColumn}");
            }
            
            while (await reader.ReadAsync())
            {
                var department = (Department)this.GetFromResultsByIdInRecord(departments, reader, IdColumn);
                department.Titles.Add(await LanguageTitle.Static.ReadFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(3, $"{nameof(Department)}{RoleCollection.RolesColumn}");
            }

            while (await reader.ReadAsync())
            {
                var department = (Department)this.GetFromResultsByIdInRecord(departments, reader, IdColumn);
                var roleId = (int)reader[Role.IdColumn];
                department.Roles.Add(await GlobalCache.GetRoleAsync(roleId));
            }

            return departments;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable },
                    { Persistent.ColumnToVariable(RoleCollection.RolesColumn), this.Roles.GetSaveTable }
                });
        }
    }
}
