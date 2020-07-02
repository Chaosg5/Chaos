//-----------------------------------------------------------------------
// <copyright file="ChallengeType.cs">
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
    /// <summary>A type of a <see cref="Challenge"/> or <see cref="Question"/>.</summary>
    public sealed class ChallengeType : Typeable<ChallengeType, Contract.ChallengeType>
    {
        /// <summary>The database column for <see cref="ImageId"/>.</summary>
        private const string ImageIdColumn = "ImageId";

        /// <summary>Prevents a default instance of the <see cref="ChallengeType"/> class from being created.</summary>
        private ChallengeType()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static ChallengeType Static { get; } = new ChallengeType();

        /// <summary>Gets the image of the <see cref="Game"/>.</summary>
        public string ImageId { get; private set; }

        /// <summary>Gets the titles of the <see cref="ChallengeType"/>.</summary>
        public LanguageDescriptionCollection Titles { get; } = new LanguageDescriptionCollection();

        /// <inheritdoc />
        public override Contract.ChallengeType ToContract()
        {
            return new Contract.ChallengeType
            {
                Id = this.Id,
                ImageId = this.ImageId,
                Titles = this.Titles.ToContract()
            };
        }

        /// <inheritdoc />
        public override Contract.ChallengeType ToContract(string languageName)
        {
            return new Contract.ChallengeType
            {
                Id = this.Id,
                ImageId = this.ImageId,
                Titles = this.Titles.ToContract(languageName)
            };
        }

        /// <inheritdoc />
        public override ChallengeType FromContract(Contract.ChallengeType contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="ChallengeType"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            this.Titles.ValidateSaveCandidate();
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<ChallengeType> NewFromRecordAsync(IDataRecord record)
        {
            var result = new ChallengeType();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.ImageId = (string)record[ImageIdColumn];
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
        public override async Task<ChallengeType> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<ChallengeType>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<ChallengeType>> GetAllAsync(UserSession session)
        {
            return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<ChallengeType>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var challengeTypes = new List<ChallengeType>();
            if (!reader.HasRows)
            {
                return challengeTypes;
            }

            while (await reader.ReadAsync())
            {
                challengeTypes.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return challengeTypes;
            }

            while (await reader.ReadAsync())
            {
                var challengeType = (ChallengeType)this.GetFromResultsByIdInRecord(challengeTypes, reader, IdColumn);
                challengeType.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            return challengeTypes;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(ImageIdColumn), this.ImageId },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}