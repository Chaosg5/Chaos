//-----------------------------------------------------------------------
// <copyright file="Question.cs">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <inheritdoc cref="Readable{T, TDto}" />
    /// <summary>A question in a <see cref="Challenge" /> containing a set of <see cref="Alternative" />s.</summary>
    public sealed class Question : Readable<Question, Contract.Question>, IUpdateable<Question, Contract.Question>
    {
        /// <summary>The database column for <see cref="ImageId"/>.</summary>
        private const string ImageIdColumn = "ImageId";

        /// <summary>Private part of the <see cref="Alternatives"/> property.</summary>
        private readonly List<Alternative> alternatives = new List<Alternative>();

        /// <summary>Private part of the <see cref="ImageId"/> property.</summary>
        private string imageId = string.Empty;

        /// <summary>Private part of the <see cref="ChallengeId"/> property.</summary>
        private int challengeId;

        /// <summary>Private part of the <see cref="Type"/> property.</summary>
        private ChallengeType type;

        /// <summary>Private part of the <see cref="Subject"/> property.</summary>
        private ChallengeSubject subject;

        /// <summary>Private part of the <see cref="Difficulty"/> property.</summary>
        private Difficulty difficulty;

        /// <summary>Initializes a new instance of the <see cref="Question"/> class.</summary>
        /// <param name="challengeId">The <see cref="ChallengeId"/>.</param>
        /// <param name="challengeType">The <see cref="Type"/>.</param>
        /// <param name="challengeSubject">The <see cref="Subject"/>.</param>
        /// <param name="difficulty">The <see cref="Difficulty"/>.</param>
        /// <param name="imageId">The <see cref="ImageId"/>.</param>
        /// <param name="titles">The <see cref="Titles"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Question"/> is not valid to be saved.</exception>
        public Question(int challengeId, ChallengeType challengeType, ChallengeSubject challengeSubject, Difficulty difficulty, string imageId, string titles)
        {
            this.SchemaName = "game";
            this.ChallengeId = challengeId;
            this.Type = challengeType;
            this.Subject = challengeSubject;
            this.Difficulty = difficulty;
            this.ImageId = imageId ?? string.Empty;
            this.Titles.UpdateFromText(titles);
        }

        /// <summary>Prevents a default instance of the <see cref="Question"/> class from being created.</summary>
        private Question()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Question Static { get; } = new Question();

        /// <summary>Gets the <see cref="Challenge.Id"/> of the parent <see cref="Challenge"/>.</summary>
        public int ChallengeId
        {
            get => this.challengeId;
            private set
            {
                if (value > 0)
                {
                    this.challengeId = value;
                }
            }
        }

        /// <summary>Gets the type of the <see cref="Question"/>.</summary>
        public ChallengeType Type
        {
            get => this.type;
            [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
            private set
            {
                if (value != null)
                {
                    this.type = value;
                    this.QuestionType = (QuestionType)value.Id;
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

        /// <summary>Gets the image of the <see cref="Question"/>.</summary>
        public string ImageId
        {
            get => this.imageId;
            private set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.imageId = value;
                }
            }
        }

        /// <summary>Gets the type of the <see cref="Question"/> as defined by the <see cref="Type"/> <see cref="ChallengeType"/>.</summary>
        public QuestionType QuestionType { get; private set; }

        /// <summary>Gets the titles of the <see cref="Question"/>.</summary>
        public LanguageDescriptionCollection Titles { get; } = new LanguageDescriptionCollection();

        /// <summary>Gets the children <see cref="Zone"/>s.</summary>
        //// ToDo: Create ChildList - class, like listable but with reference to parent
        public List<Alternative> Alternatives => this.alternatives;

        /// <inheritdoc />
        public override Contract.Question ToContract()
        {
            return new Contract.Question
            {
                Id = this.Id,
                ChallengeId = this.ChallengeId,
                Type = this.Type?.ToContract(),
                Subject = this.Subject?.ToContract(),
                Difficulty = this.Difficulty?.ToContract(),
                ImageId = this.ImageId,
                QuestionType = this.QuestionType,
                Titles = this.Titles.ToContract(),
                Alternatives = new ReadOnlyCollection<Contract.Alternative>(this.Alternatives.Select(a => a.ToContract()).ToList())
            };
        }

        /// <inheritdoc />
        public override Contract.Question ToContract(string languageName)
        {
            return new Contract.Question
            {
                Id = this.Id,
                ChallengeId = this.ChallengeId,
                Type = this.Type?.ToContract(languageName),
                Subject = this.Subject?.ToContract(languageName),
                Difficulty = this.Difficulty?.ToContract(languageName),
                ImageId = this.ImageId,
                QuestionType = this.QuestionType,
                Titles = this.Titles.ToContract(languageName),
                Alternatives = new ReadOnlyCollection<Contract.Alternative>(this.Alternatives.Select(a => a.ToContract(languageName)).ToList())
            };
        }

        /// <inheritdoc />
        public override Question FromContract(Contract.Question contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Question"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.ChallengeId <= 0)
            {
                throw new InvalidSaveCandidateException("The question can't be saved without a challenge.");
            }

            if (this.Type == null)
            {
                throw new InvalidSaveCandidateException("The question can't be saved without a type.");
            }

            if (this.Subject == null)
            {
                throw new InvalidSaveCandidateException("The question can't be saved without a subject.");
            }

            if (this.Difficulty == null)
            {
                throw new InvalidSaveCandidateException("The question can't be saved without a difficulty.");
            }

            this.Titles.ValidateSaveCandidate();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Question"/> is not valid to be saved.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public async Task UpdateAsync(Contract.Question contract, UserSession session)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (this.Id != contract.Id)
            {
                throw new InvalidSaveCandidateException($"The id {contract.Id} doesn't match the expected {this.Id}.");
            }

            this.ChallengeId = contract.ChallengeId;
            this.ImageId = contract.ImageId;
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
        public override async Task<Question> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Question();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.ChallengeId = (int)record[Challenge.IdColumn];
            this.Type = await GameCache.ChallengeTypeGetAsync((int)record[ChallengeType.IdColumn]);
            this.Subject = await GameCache.ChallengeSubjectGetAsync((int)record[ChallengeSubject.IdColumn]);
            this.Difficulty = await GameCache.DifficultyGetAsync((int)record[Difficulty.IdColumn]);
            this.ImageId = (string)record[ImageIdColumn];
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Question"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Question> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Question>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<Question>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var questions = new List<Question>();
            if (!reader.HasRows)
            {
                return questions;
            }

            while (await reader.ReadAsync())
            {
                questions.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return questions;
            }

            while (await reader.ReadAsync())
            {
                var question = (Question)this.GetFromResultsByIdInRecord(questions, reader, IdColumn);
                question.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }
            
            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return questions;
            }

            while (await reader.ReadAsync())
            {
                var question = (Question)this.GetFromResultsByIdInRecord(questions, reader, IdColumn);
                question.alternatives.Add(await GameCache.AlternativeGetAsync((int)reader[Alternative.IdColumn]));
            }

            return questions;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(Challenge.IdColumn), this.ChallengeId },
                    { Persistent.ColumnToVariable(ChallengeSubject.IdColumn), this.Subject.Id },
                    { Persistent.ColumnToVariable(ChallengeType.IdColumn), this.Type.Id },
                    { Persistent.ColumnToVariable(Difficulty.IdColumn), this.Difficulty.Id },
                    { Persistent.ColumnToVariable(ImageIdColumn), this.ImageId },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}