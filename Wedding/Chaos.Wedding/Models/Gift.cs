//-----------------------------------------------------------------------
// <copyright file="Gift.cs">
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
    /// <summary>A gift.</summary>
    public class Gift : Typeable<Gift, Gift>, IReadableLookup<Gift, Gift>
    {
        /// <summary>The database column for <see cref="Booked"/>.</summary>
        private const string BookedColumn = "Booked";

        /// <summary>The database column for <see cref="Price"/>.</summary>
        private const string PriceColumn = "Price";

        /// <summary>The database column for <see cref="ImageId"/>.</summary>
        private const string ImageIdColumn = "ImageId";

        /// <summary>Prevents a default instance of the <see cref="Gift"/> class from being created.</summary>
        private Gift()
        {
            this.SchemaName = "wed";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Gift Static { get; } = new Gift();

        /// <summary>Gets the titles.</summary>
        public LanguageDescriptionCollection Titles { get; } = new LanguageDescriptionCollection();

        /// <summary>Gets or sets the <see cref="InvitationStatus"/> of this <see cref="Gift"/>.</summary>
        public InvitationStatus Booked { get; set; } = InvitationStatus.None;

        /// <summary>Gets or sets the estimated price of the gift.</summary>
        public int Price { get; set; }

        /// <summary>Gets or sets the image id.</summary>
        public string ImageId { get; set; }

        /// <inheritdoc/>
        public override Gift ToContract()
        {
            return this;
        }

        /// <inheritdoc/>
        public override Gift ToContract(string languageName)
        {
            return this;
        }

        /// <inheritdoc/>
        public override Gift FromContract(Gift contract)
        {
            return this;
        }

        /// <inheritdoc/>
        public override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc/>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<Gift> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Gift();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, BookedColumn });
            this.Id = (int)record[IdColumn];
            this.Booked = (InvitationStatus)(byte)record[BookedColumn];
            this.Price = (int)record[PriceColumn];
            this.ImageId = (string)record[ImageIdColumn];

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
        public override async Task<Gift> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<Gift> GetAsync(UserSession session, Guid lookupId)
        {
            return (await this.GetFromDatabaseAsync(new List<Guid> { lookupId }, this.ReadFromRecordsAsync, session)).First();
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<IEnumerable<Gift>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Gift>> GetAllAsync(UserSession session)
        {
            return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc/>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<Gift>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var gifts = new List<Gift>();
            if (!reader.HasRows)
            {
                return gifts;
            }

            while (await reader.ReadAsync())
            {
                gifts.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return gifts;
            }

            while (await reader.ReadAsync())
            {
                var gift = (Gift)this.GetFromResultsByIdInRecord(gifts, reader, IdColumn);
                gift.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            return gifts;
        }

        /// <inheritdoc/>
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(BookedColumn), this.Booked },
                    { Persistent.ColumnToVariable(PriceColumn), this.Price },
                    { Persistent.ColumnToVariable(ImageIdColumn), this.ImageId }
                });
        }
    }
}