//-----------------------------------------------------------------------
// <copyright file="Challenge.cs">
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
    /// <summary>A challenge in a <see cref="Zone" /> containing a set of <see cref="Question" />s.</summary>
    public sealed class Challenge : Readable<Challenge, Contract.Challenge>, IUpdateable<Challenge, Contract.Challenge>
    {
        /// <summary>Private part of the <see cref="Questions"/> property.</summary>
        private readonly List<Question> questions = new List<Question>();

        /// <summary>Private part of the <see cref="ZoneId"/> property.</summary>
        private int zoneId;

        /// <summary>Private part of the <see cref="Type"/> property.</summary>
        private ChallengeType type;

        /// <summary>Private part of the <see cref="Subject"/> property.</summary>
        private ChallengeSubject subject;

        /// <summary>Private part of the <see cref="Difficulty"/> property.</summary>
        private Difficulty difficulty;

        /// <summary>Initializes a new instance of the <see cref="Challenge"/> class.</summary>
        /// <param name="zoneId">The <see cref="ZoneId"/>.</param>
        /// <param name="challengeType">The <see cref="Type"/>.</param>
        /// <param name="challengeSubject">The <see cref="Subject"/>.</param>
        /// <param name="difficulty">The <see cref="Difficulty"/>.</param>
        /// <param name="titles">The <see cref="Titles"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Challenge"/> is not valid to be saved.</exception>
        public Challenge(int zoneId, ChallengeType challengeType, ChallengeSubject challengeSubject, Difficulty difficulty, string titles)
        {
            this.SchemaName = "game";
            this.ZoneId = zoneId;
            this.Type = challengeType;
            this.Subject = challengeSubject;
            this.Difficulty = difficulty;
            this.Titles.UpdateFromText(titles);
        }

        /// <summary>Prevents a default instance of the <see cref="Challenge"/> class from being created.</summary>
        private Challenge()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Challenge Static { get; } = new Challenge();

        /// <summary>Gets the <see cref="Zone.Id"/> of the parent <see cref="Zone"/>.</summary>
        public int ZoneId
        {
            get => this.zoneId;
            private set
            {
                if (value > 0)
                {
                    this.zoneId = value;
                }
            }
        }

        /// <summary>Gets the type of the <see cref="Question"/>.</summary>
        public ChallengeType Type
        {
            get => this.type;
            private set
            {
                if (value != null)
                {
                    this.type = value;
                }
            }
        }

        /// <summary>Gets the subject the <see cref="Question"/>.</summary>
        public ChallengeSubject Subject
        {
            get => this.subject;
            private set
            {
                if (value != null)
                {
                    this.subject = value;
                }
            }
        }

        /// <summary>Gets the difficulty the <see cref="Question"/>.</summary>
        public Difficulty Difficulty
        {
            get => this.difficulty;
            private set
            {
                if (value != null)
                {
                    this.difficulty = value;
                }
            }
        }

        /// <summary>Gets the titles of the <see cref="Challenge"/>.</summary>
        public LanguageDescriptionCollection Titles { get; } = new LanguageDescriptionCollection();

        /// <summary>Gets the children <see cref="Question"/>s.</summary>
        //// ToDo: Create ChildList - class, like listable but with reference to parent
        public List<Question> Questions => this.questions;

        /// <inheritdoc />
        public override Contract.Challenge ToContract()
        {
            return new Contract.Challenge
            {
                Id = this.Id,
                ZoneId = this.ZoneId,
                Type = this.Type?.ToContract(),
                Subject = this.Subject?.ToContract(),
                Difficulty = this.Difficulty?.ToContract(),
                Titles = this.Titles.ToContract(),
                Questions = new ReadOnlyCollection<Contract.Question>(this.Questions.Select(q => q.ToContract()).ToList())
            };
        }

        /// <inheritdoc />
        public override Contract.Challenge ToContract(string languageName)
        {
            return new Contract.Challenge
            {
                Id = this.Id,
                ZoneId = this.ZoneId,
                Type = this.Type?.ToContract(languageName),
                Subject = this.Subject?.ToContract(languageName),
                Difficulty = this.Difficulty?.ToContract(languageName),
                Titles = this.Titles.ToContract(languageName),
                Questions = new ReadOnlyCollection<Contract.Question>(this.Questions.Select(q => q.ToContract(languageName)).ToList())
            };
        }

        /// <inheritdoc />
        public override Challenge FromContract(Contract.Challenge contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Challenge"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.ZoneId <= 0)
            {
                throw new InvalidSaveCandidateException("The challenge can't be saved without a zone.");
            }

            if (this.Type == null)
            {
                throw new InvalidSaveCandidateException("The challenge can't be saved without a type.");
            }

            if (this.Subject == null)
            {
                throw new InvalidSaveCandidateException("The challenge can't be saved without a subject.");
            }

            if (this.Difficulty == null)
            {
                throw new InvalidSaveCandidateException("The challenge can't be saved without a difficulty.");
            }

            this.Titles.ValidateSaveCandidate();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Challenge"/> is not valid to be saved.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public async Task UpdateAsync(Contract.Challenge contract, UserSession session)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (this.Id != contract.Id)
            {
                throw new InvalidSaveCandidateException($"The id {contract.Id} doesn't match the expected {this.Id}.");
            }

            this.ZoneId = contract.ZoneId;
            if (contract.Type != null)
            {
                this.Type = await GameCache.ChallengeTypeGetAsync(contract.Type.Id);
            }

            if (contract.Subject != null)
            {
                this.Subject = await GameCache.ChallengeSubjectGetAsync(contract.Subject.Id);
            }

            if (contract.Difficulty != null)
            {
                this.Difficulty = await GameCache.DifficultyGetAsync(contract.Difficulty.Id);
            }

            this.Titles.FromContract(contract.Titles);
            await this.SaveAsync(session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<Challenge> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Challenge();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.ZoneId = (int)record[Zone.IdColumn];
            this.Type = await GameCache.ChallengeTypeGetAsync((int)record[ChallengeType.IdColumn]);
            this.Subject = await GameCache.ChallengeSubjectGetAsync((int)record[ChallengeSubject.IdColumn]);
            this.Difficulty = await GameCache.DifficultyGetAsync((int)record[Difficulty.IdColumn]);
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Challenge"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Challenge> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Challenge>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<Challenge>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var challenges = new List<Challenge>();
            if (!reader.HasRows)
            {
                return challenges;
            }

            while (await reader.ReadAsync())
            {
                challenges.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return challenges;
            }

            while (await reader.ReadAsync())
            {
                var challenge = (Challenge)this.GetFromResultsByIdInRecord(challenges, reader, IdColumn);
                challenge.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return challenges;
            }

            while (await reader.ReadAsync())
            {
                var challenge = (Challenge)this.GetFromResultsByIdInRecord(challenges, reader, IdColumn);
                challenge.questions.Add(await GameCache.QuestionGetAsync((int)reader[Question.IdColumn]));
            }

            return challenges;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(Zone.IdColumn), this.ZoneId },
                    { Persistent.ColumnToVariable(ChallengeSubject.IdColumn), this.Subject.Id },
                    { Persistent.ColumnToVariable(ChallengeType.IdColumn), this.Type.Id },
                    { Persistent.ColumnToVariable(Difficulty.IdColumn), this.Difficulty.Id },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}