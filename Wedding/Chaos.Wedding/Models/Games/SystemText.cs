//-----------------------------------------------------------------------
// <copyright file="SystemText.cs">
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
    /// <summary>A subject of a <see cref="Challenge"/> or <see cref="Question"/>.</summary>
    public sealed class SystemText : Typeable<SystemText, Contract.SystemText>
    {
        /// <summary>The database column for <see cref="TextKey"/>.</summary>
        private const string TextKeyColumn = "TextKey";

        /// <summary>Prevents a default instance of the <see cref="SystemText"/> class from being created.</summary>
        private SystemText()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static SystemText Static { get; } = new SystemText();

        /// <summary>Gets the image of the <see cref="Game"/>.</summary>
        public string TextKey { get; private set; }

        /// <summary>Gets the titles of the <see cref="SystemText"/>.</summary>
        public LanguageDescriptionCollection Titles { get; } = new LanguageDescriptionCollection();

        /// <inheritdoc />
        public override Contract.SystemText ToContract()
        {
            return new Contract.SystemText
            {
                Id = this.Id,
                TextKey = this.TextKey,
                Titles = this.Titles.ToContract()
            };
        }

        /// <inheritdoc />
        public override Contract.SystemText ToContract(string languageName)
        {
            return new Contract.SystemText
            {
                Id = this.Id,
                TextKey = this.TextKey,
                Titles = this.Titles.ToContract(languageName)
            };
        }

        /// <inheritdoc />
        public override SystemText FromContract(Contract.SystemText contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="SystemText"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            this.Titles.ValidateSaveCandidate();
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<SystemText> NewFromRecordAsync(IDataRecord record)
        {
            var result = new SystemText();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.TextKey = (string)record[TextKeyColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="SystemText"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<SystemText> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<SystemText>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<SystemText>> GetAllAsync(UserSession session)
        {
            return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<SystemText>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var systemTexts = new List<SystemText>();
            if (!reader.HasRows)
            {
                return systemTexts;
            }

            while (await reader.ReadAsync())
            {
                systemTexts.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return systemTexts;
            }

            while (await reader.ReadAsync())
            {
                var systemText = (SystemText)this.GetFromResultsByIdInRecord(systemTexts, reader, IdColumn);
                systemText.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            return systemTexts;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(TextKeyColumn), this.TextKey },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}