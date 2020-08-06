//-----------------------------------------------------------------------
// <copyright file="TeamZone.cs">
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

    /// <inheritdoc />
    /// <summary>A <see cref="Zone"/> played by a <see cref="Team"/>.</summary>
    public class TeamZone : Readable<TeamZone, Contract.TeamZone> ////, IUpdateable<TeamZone, Contract.TeamZone>
    {
        /// <summary>The database column for <see cref="Unlocked"/>.</summary>
        private const string UnlockedColumn = "Unlocked";

        /// <summary>Initializes a new instance of the <see cref="TeamZone"/> class.</summary>
        /// <param name="teamId">The <see cref="TeamId"/>.</param>
        /// <param name="zoneId">The <see cref="ZoneId"/>.</param>
        /// <param name="unlocked">The <see cref="Unlocked"/>.</param>
        public TeamZone(int teamId, int zoneId, bool unlocked = false)
        {
            this.SchemaName = "game";
            this.TeamId = teamId;
            this.ZoneId = zoneId;
            this.Unlocked = unlocked;
        }

        /// <summary>Prevents a default instance of the <see cref="TeamZone"/> class from being created.</summary>
        private TeamZone()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static TeamZone Static { get; } = new TeamZone();

        /// <summary>Gets the <see cref="Team.Id"/>.</summary>
        public int TeamId { get; private set; }

        /// <summary>Gets the <see cref="Zone.Id"/>.</summary>
        public int ZoneId { get; private set; }

        /// <summary>Gets or sets a value indicating whether the <see cref="Zone"/> is unlocked.</summary>
        public bool Unlocked { get; set; }
        
        /// <summary>Ensures a <see cref="TeamZone"/>.</summary>
        /// <param name="teamId">The <see cref="TeamId"/>.</param>
        /// <param name="zoneId">The <see cref="ZoneId"/>.</param>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="TeamZone"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamZone"/> is not valid to be saved.</exception>
        public static async Task<TeamZone> EnsureTeamZoneAsync(int teamId, int zoneId, UserSession session)
        {
            var teamZones = await Static.GetFromDatabaseAsync(GetLoadParameters(teamId, zoneId), Static.ReadFromRecordsAsync, session);
            var teamZone = teamZones.FirstOrDefault();
            if (teamZone == null)
            {
                teamZone = new TeamZone(teamId, zoneId);
                await teamZone.SaveAsync(session);
            }

            return teamZone;
        }

        /// <summary>Adds <see cref="Contract.TeamZone"/>s to the <see cref="Contract.Zone.TeamZone"/>.</summary>
        /// <param name="zone">The <see cref="Zone"/>.</param>
        /// <param name="teamId">The active <see cref="Team.Id"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static async Task AddTeamZonesAsync(Contract.Zone zone, int teamId)
        {
           zone.TeamZone = (await GameCache.TeamZoneGetAsync(teamId, zone.Id)).ToContract();
        }

        /// <inheritdoc />
        public override Contract.TeamZone ToContract()
        {
            return new Contract.TeamZone
            {
                TeamId = this.TeamId,
                ZoneId = this.ZoneId,
                Unlocked = this.Unlocked
            };
        }

        /// <inheritdoc />
        public override Contract.TeamZone ToContract(string languageName)
        {
            return new Contract.TeamZone
            {
                TeamId = this.TeamId,
                ZoneId = this.ZoneId,
                Unlocked = this.Unlocked
            };
        }

        /// <inheritdoc />
        public override TeamZone FromContract(Contract.TeamZone contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamZone"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.TeamId <= 0)
            {
                throw new InvalidSaveCandidateException("A team needs to be specified.");
            }

            if (this.ZoneId <= 0)
            {
                throw new InvalidSaveCandidateException("A zone needs to be specified.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<TeamZone> NewFromRecordAsync(IDataRecord record)
        {
            var result = new TeamZone();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { Team.IdColumn });
            this.Id = (int)record[Team.IdColumn];
            this.TeamId = this.Id;
            this.ZoneId = (int)record[Zone.IdColumn];
            this.Unlocked = (bool)record[UnlockedColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task<TeamZone> GetAsync(UserSession session, int id)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }
        
        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<TeamZone>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamZone"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<IEnumerable<TeamZone>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var teamZones = new List<TeamZone>();
            if (!reader.HasRows)
            {
                return teamZones;
            }

            while (await reader.ReadAsync())
            {
                teamZones.Add(await this.NewFromRecordAsync(reader));
            }

            return teamZones;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(Team.IdColumn), this.TeamId },
                    { Persistent.ColumnToVariable(Zone.IdColumn), this.ZoneId },
                    { Persistent.ColumnToVariable(UnlockedColumn), this.Unlocked }
                });
        }

        /// <summary>Gets parameters for <see cref="EnsureTeamZoneAsync"/>.</summary>
        /// <param name="teamId">The <see cref="TeamId"/>.</param>
        /// <param name="zoneId">The <see cref="ZoneId"/>.</param>
        /// <returns>The parameters.</returns>
        private static IReadOnlyDictionary<string, object> GetLoadParameters(int teamId, int zoneId)
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(Team.IdColumn), teamId },
                    { Persistent.ColumnToVariable(Zone.IdColumn), zoneId }
                });
        }
    }
}