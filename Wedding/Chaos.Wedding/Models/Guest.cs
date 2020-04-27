//-----------------------------------------------------------------------
// <copyright file="Guest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models
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
    /// <summary>A guest.</summary>
    public class Guest : Readable<Guest, Guest>, IReadableLookup<Guest, Guest>
    {
        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>The database column for <see cref="Born"/>.</summary>
        private const string BornColumn = "Born";

        /// <summary>The database column for <see cref="Dead"/>.</summary>
        private const string DeadColumn = "Dead";

        /// <summary>The database column for <see cref="LookupId"/>.</summary>
        private const string LookupIdColumn = "LookupId";

        /// <summary>The database column for <see cref="Reception"/>.</summary>
        private const string ReceptionColumn = "Reception";

        /// <summary>The database column for <see cref="Dinner"/>.</summary>
        private const string DinnerColumn = "Dinner";

        /// <summary>The database column for <see cref="Information"/>.</summary>
        private const string InformationColumn = "Information";

        /// <summary>The address id.</summary>
        private readonly int addressId;

        /// <summary>Initializes a new instance of the <see cref="Guest"/> class.</summary>
        /// <param name="name">The <see cref="Name"/> to set.</param>
        /// <param name="addressId">The address Id.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/></exception>
        public Guest(string name, int addressId)
        {
            this.SchemaName = "wed";
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (addressId <= 0)
            {
                throw new ArgumentNullException(nameof(addressId));
            }

            this.Name = name;
            this.LookupId = Guid.NewGuid();
            this.addressId = addressId;
        }

        /// <summary>Prevents a default instance of the <see cref="Guest"/> class from being created.</summary>
        private Guest()
        {
            this.SchemaName = "wed";
        }
        
        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Guest Static { get; } = new Guest();

        /// <summary>Gets the name.</summary>
        public string Name { get; private set; }

        /// <summary>Gets the born.</summary>
        public DateTime? Born { get; private set; }

        /// <summary>Gets the dead.</summary>
        public DateTime? Dead { get; private set; }

        /// <summary>Gets the lookup id.</summary>
        public Guid LookupId { get; private set; }

        /// <summary>Gets the titles.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <summary>Gets or sets the reception.</summary>
        public InvitationStatus Reception { get; set; } = InvitationStatus.None;

        /// <summary>Gets or sets the dinner.</summary>
        public InvitationStatus Dinner { get; set; } = InvitationStatus.None;

        /// <summary>Gets or sets the information.</summary>
        public string Information { get; set; }

        /// <inheritdoc/>
        public override Guest ToContract()
        {
            return this;
        }

        /// <inheritdoc/>
        public override Guest ToContract(string languageName)
        {
            return this;
        }

        /// <inheritdoc/>
        public override Guest FromContract(Guest contract)
        {
            return this;
        }

        /// <inheritdoc/>
        public override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc/>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<Guest> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Guest();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.Name = (string)record[NameColumn];
            if (!DBNull.Value.Equals(record[BornColumn]))
            {
                this.Born = (DateTime)record[BornColumn];
            }

            if (!DBNull.Value.Equals(record[DeadColumn]))
            {
                this.Dead = (DateTime)record[DeadColumn];
            }

            this.LookupId = (Guid)record[LookupIdColumn];
            this.Reception = (InvitationStatus)(int)record[ReceptionColumn];
            this.Dinner = (InvitationStatus)(int)record[DinnerColumn];
            this.Information = (string)record[InformationColumn];

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc/>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Guest> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<Guest> GetAsync(UserSession session, Guid lookupId)
        {
            return (await this.GetFromDatabaseAsync(new List<Guid> { lookupId }, this.ReadFromRecordsAsync, session)).First();
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<IEnumerable<Guest>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc/>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<Guest>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var guests = new List<Guest>();
            if (!reader.HasRows)
            {
                return guests;
            }

            while (await reader.ReadAsync())
            {
                guests.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return guests;
            }

            while (await reader.ReadAsync())
            {
                var guest = (Guest)this.GetFromResultsByIdInRecord(guests, reader, IdColumn);
                guest.Titles.Add(await LanguageTitle.Static.NewFromRecordAsync(reader));
            }

            return guests;
        }

        /// <inheritdoc/>
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(NameColumn), this.Name },
                    { Persistent.ColumnToVariable(LookupIdColumn), this.LookupId },
                    { Persistent.ColumnToVariable(BornColumn), this.Born },
                    { Persistent.ColumnToVariable(DeadColumn), this.Dead },
                    { Persistent.ColumnToVariable(ReceptionColumn), this.Reception },
                    { Persistent.ColumnToVariable(DinnerColumn), this.Dinner },
                    { Persistent.ColumnToVariable(InformationColumn), this.Information },
                    { Persistent.ColumnToVariable(Address.IdColumn), this.addressId }
                });
        }
    }
}