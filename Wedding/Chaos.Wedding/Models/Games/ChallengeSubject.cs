//-----------------------------------------------------------------------
// <copyright file="ChallengeSubject.cs">
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
    /// <summary>A subject of a <see cref="Challenge"/> or <see cref="Question"/>.</summary>
    public sealed class ChallengeSubject : Typeable<ChallengeSubject, Contract.ChallengeSubject>
    {
        /// <summary>The database column for <see cref="ImageId"/>.</summary>
        private const string ImageIdColumn = "ImageId";

        /// <summary>Prevents a default instance of the <see cref="ChallengeSubject"/> class from being created.</summary>
        private ChallengeSubject()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static ChallengeSubject Static { get; } = new ChallengeSubject();

        /// <summary>Gets the image of the <see cref="Game"/>.</summary>
        public string ImageId { get; private set; }

        /// <summary>Gets the titles of the <see cref="ChallengeSubject"/>.</summary>
        public LanguageTitleCollection Titles { get; } = new LanguageTitleCollection();

        /// <inheritdoc />
        public override Contract.ChallengeSubject ToContract()
        {
            return new Contract.ChallengeSubject
            {
                Id = this.Id,
                ImageId = this.ImageId,
                Titles = this.Titles.ToContract()
            };
        }

        /// <inheritdoc />
        public override Contract.ChallengeSubject ToContract(string languageName)
        {
            return new Contract.ChallengeSubject
            {
                Id = this.Id,
                ImageId = this.ImageId,
                Titles = this.Titles.ToContract(languageName)
            };
        }

        /// <inheritdoc />
        public override ChallengeSubject FromContract(Contract.ChallengeSubject contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="ChallengeSubject"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            this.Titles.ValidateSaveCandidate();
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<ChallengeSubject> NewFromRecordAsync(IDataRecord record)
        {
            var result = new ChallengeSubject();
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
        public override async Task<ChallengeSubject> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<ChallengeSubject>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<ChallengeSubject>> GetAllAsync(UserSession session)
        {
            return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<ChallengeSubject>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var challengeSubjects = new List<ChallengeSubject>();
            if (!reader.HasRows)
            {
                return challengeSubjects;
            }

            while (await reader.ReadAsync())
            {
                challengeSubjects.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return challengeSubjects;
            }

            while (await reader.ReadAsync())
            {
                var challengeSubject = (ChallengeSubject)this.GetFromResultsByIdInRecord(challengeSubjects, reader, IdColumn);
                challengeSubject.Titles.Add(await LanguageTitle.Static.NewFromRecordAsync(reader));
            }

            return challengeSubjects;
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