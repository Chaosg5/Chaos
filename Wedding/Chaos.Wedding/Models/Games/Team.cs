//-----------------------------------------------------------------------
// <copyright file="Team.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <inheritdoc cref="Readable{T, TDto}" />
    /// <summary>A team.</summary>
    public sealed class Team : Readable<Team, Contract.Team>, IReadableLookup<Team, Contract.Team>
    {
        /// <summary>The database column for <see cref="LookupId"/>.</summary>
        private const string LookupIdColumn = "LookupId";

        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>Prevents a default instance of the <see cref="Team"/> class from being created.</summary>
        private Team()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Team Static { get; } = new Team();

        /// <summary>Gets the lookup id of the <see cref="Team"/>.</summary>
        public Guid LookupId { get; private set; }

        /// <summary>Gets the name of the <see cref="Team"/>.</summary>
        public string Name { get; private set; }

        /// <inheritdoc />
        public override Contract.Team ToContract()
        {
            return new Contract.Team
            {
                Id = this.Id,
                LookupId = this.LookupId,
                Name = this.Name
            };
        }

        /// <inheritdoc />
        public override Contract.Team ToContract(string languageName)
        {
            return new Contract.Team
            {
                Id = this.Id,
                LookupId = this.LookupId,
                Name = this.Name
            };
        }

        /// <inheritdoc />
        public override Team FromContract(Contract.Team contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Team"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<Team> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Team();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.LookupId = (Guid)record[LookupIdColumn];
            this.Name = (string)record[NameColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Team> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Team>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task<Team> GetAsync(UserSession session, Guid lookupId)
        {
            return (await this.GetFromDatabaseAsync(new List<Guid> { lookupId }, this.ReadFromRecordsAsync, session)).First();
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<IEnumerable<Team>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var teams = new List<Team>();
            if (!reader.HasRows)
            {
                return teams;
            }

            while (await reader.ReadAsync())
            {
                teams.Add(await this.NewFromRecordAsync(reader));
            }
            
            return teams;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(LookupIdColumn), this.LookupId },
                    { Persistent.ColumnToVariable(NameColumn), this.Name }
                });
        }
    }
}