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

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <inheritdoc cref="Readable{T, TDto}" />
    /// <summary>A team.</summary>
    public sealed class Team : Readable<Team, Contract.Team>, IReadableLookup<Team, Contract.Team>, IUpdateable<Team, Contract.Team>, ISearchable<Team>
    {
        /// <summary>The database column for <see cref="LookupId"/>.</summary>
        private const string LookupIdColumn = "LookupId";

        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>The database column for <see cref="GameScores"/>.</summary>
        private const string ScoreColumn = "Score";

        /// <summary>Initializes a new instance of the <see cref="Team"/> class.</summary>
        /// <param name="name">The <see cref="Name"/>.</param>
        public Team(string name)
        {
            this.SchemaName = "game";
            this.Name = name;
            this.LookupId = Guid.NewGuid();
        }

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
        
        /// <summary>Gets the <see cref="Team"/>'s scores per <see cref="Game"/>.</summary>
        //// ToDo: Create ChildList - class, like listable but with reference to parent
        public Dictionary<int, int> GameScores { get; } = new Dictionary<int, int>();

        /// <summary>Updates the <see cref="GameScores"/>.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task UpdateScoreAsync(UserSession session)
        {
            var team = await this.GetAsync(session, this.Id);
            foreach (var pair in team.GameScores)
            {
                this.GameScores[pair.Key] = pair.Value;
            }
        }

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

        /// <summary>Converts this <see cref="Team"/> to a <see cref="Contract.Team"/>.</summary>
        /// <param name="gameId">The game Id.</param>
        /// <returns>The <see cref="Contract.Team"/>.</returns>
        public Contract.Team ToContract(int gameId)
        {
            this.GameScores.TryGetValue(gameId, out var score);
            return new Contract.Team
            {
                Id = this.Id,
                LookupId = this.LookupId,
                Name = this.Name,
                TeamScore = score
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
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                throw new InvalidSaveCandidateException("The team needs a name.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Team"/> is not valid to be saved.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task UpdateAsync(Contract.Team contract, UserSession session)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (this.Id != contract.Id)
            {
                throw new InvalidSaveCandidateException($"The id {contract.Id} doesn't match the expected {this.Id}.");
            }

            this.Name = contract.Name;
            await this.SaveAsync(session);
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
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Team"/> is not valid to be saved.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
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

        /// <inheritdoc/>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<IEnumerable<Team>> SearchAsync(SearchParametersDto parametersDto, UserSession session)
        {
            // ToDo: Change SearchDatabase to return the real results instead
            var results = (await this.SearchDatabaseAsync(parametersDto, session)).ToList();
            if (results.Any())
            {
                return await this.GetAsync(session, results);
            }

            return new List<Team>();
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
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

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return teams;
            }

            while (await reader.ReadAsync())
            {
                var team = (Team)this.GetFromResultsByIdInRecord(teams, reader, IdColumn);
                team.GameScores[(int)reader[Game.IdColumn]] = (int)reader[ScoreColumn];
            }

            return teams;
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(LookupIdColumn), this.LookupId },
                    { Persistent.ColumnToVariable(NameColumn), this.Name },
                    { Persistent.ColumnToVariable($"{Game.IdColumn}s"), Persistent.CreateDictionaryCollectionTable(this.GameScores,  new KeyValuePair<string, string>("Id", "Order")) }
                });
        }
    }
}