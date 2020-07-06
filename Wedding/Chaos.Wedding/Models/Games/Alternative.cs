//-----------------------------------------------------------------------
// <copyright file="Alternative.cs">
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
    /// <summary>An alternative in a <see cref="Question"/>.</summary>
    public sealed class Alternative : Readable<Alternative, Contract.Alternative>, IUpdateable<Alternative, Contract.Alternative>
    {
        /// <summary>The database column for <see cref="CorrectRow"/>.</summary>
        private const string CorrectRowColumn = "CorrectRow";

        /// <summary>The database column for <see cref="CorrectColumn"/>.</summary>
        private const string CorrectColumnColumn = "CorrectColumn";

        /// <summary>The database column for <see cref="IsCorrect"/>.</summary>
        private const string IsCorrectColumn = "IsCorrect";

        /// <summary>The database column for <see cref="ScoreValue"/>.</summary>
        private const string ScoreValueColumn = "ScoreValue";

        /// <summary>The database column for <see cref="CorrectAnswer"/>.</summary>
        private const string CorrectAnswerColumn = "CorrectAnswer";

        /// <summary>The database column for <see cref="ImageId"/>.</summary>
        private const string ImageIdColumn = "ImageId";

        /// <summary>Private part of the <see cref="CorrectAnswer"/> property.</summary>
        private string correctAnswer = string.Empty;

        /// <summary>Private part of the <see cref="ImageId"/> property.</summary>
        private string imageId = string.Empty;

        /// <summary>Private part of the <see cref="QuestionId"/> property.</summary>
        private int questionId;

        /// <summary>Initializes a new instance of the <see cref="Alternative"/> class.</summary>
        /// <param name="questionId">The <see cref="QuestionId"/>.</param>
        /// <param name="correctRow">The <see cref="CorrectRow"/>.</param>
        /// <param name="correctColumn">The <see cref="CorrectColumn"/>.</param>
        /// <param name="isCorrect">The <see cref="IsCorrect"/>.</param>
        /// <param name="scoreValue">The <see cref="ScoreValue"/>.</param>
        /// <param name="correctAnswer">The <see cref="CorrectAnswer"/>.</param>
        /// <param name="imageId">The <see cref="ImageId"/>.</param>
        /// <param name="titles">The <see cref="Titles"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="LanguageDescription"/> is not valid to be saved.</exception>
        public Alternative(int questionId, byte correctRow, byte correctColumn, bool isCorrect, byte scoreValue, string correctAnswer, string imageId, string titles)
        {
            this.SchemaName = "game";
            this.QuestionId = questionId;
            this.CorrectRow = correctRow;
            this.CorrectColumn = correctColumn;
            this.IsCorrect = isCorrect;
            this.ScoreValue = scoreValue;
            this.CorrectAnswer = correctAnswer;
            this.ImageId = imageId;
            this.Titles.UpdateFromText(titles);
        }

        /// <summary>Prevents a default instance of the <see cref="Alternative"/> class from being created.</summary>
        private Alternative()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Alternative Static { get; } = new Alternative();

        /// <summary>Gets the <see cref="Question.Id"/> of the parent <see cref="Question"/>.</summary>
        public int QuestionId
        {
            get => this.questionId;
            private set
            {
                if (value > 0)
                {
                    this.questionId = value;
                }
            }
        }

        /// <summary>Gets the correct row of the <see cref="Alternative"/>.</summary>
        public byte CorrectRow { get; private set; }

        /// <summary>Gets the correct column of the <see cref="Alternative"/>.</summary>
        public byte CorrectColumn { get; private set; }

        /// <summary>Gets a value indicating whether this <see cref="Alternative"/> is correct for the parent <see cref="Question"/>.</summary>
        public bool IsCorrect { get; private set; }

        /// <summary>Gets the score value of the <see cref="Alternative"/>.</summary>
        public byte ScoreValue { get; private set; }

        /// <summary>Gets the correct answer of the <see cref="Alternative"/>.</summary>
        public string CorrectAnswer
        {
            get => this.correctAnswer;
            private set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.correctAnswer = value;
                }
            }
        }

        /// <summary>Gets the image of the <see cref="Alternative"/>.</summary>
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

        /// <summary>Gets the titles of the <see cref="Alternative"/>.</summary>
        public LanguageDescriptionCollection Titles { get; } = new LanguageDescriptionCollection();

        /// <inheritdoc />
        public override Contract.Alternative ToContract()
        {
            return new Contract.Alternative
            {
                Id = this.Id,
                QuestionId = this.QuestionId,
                CorrectRow = this.CorrectRow,
                CorrectColumn = this.CorrectColumn,
                IsCorrect = this.IsCorrect,
                ScoreValue = this.ScoreValue,
                CorrectAnswer = this.CorrectAnswer,
                ImageId = this.ImageId,
                Titles = this.Titles.ToContract()
            };
        }

        /// <inheritdoc />
        public override Contract.Alternative ToContract(string languageName)
        {
            return new Contract.Alternative
            {
                Id = this.Id,
                QuestionId = this.QuestionId,
                CorrectRow = this.CorrectRow,
                CorrectColumn = this.CorrectColumn,
                IsCorrect = this.IsCorrect,
                ScoreValue = this.ScoreValue,
                CorrectAnswer = this.CorrectAnswer,
                ImageId = this.ImageId,
                Titles = this.Titles.ToContract(languageName)
            };
        }

        /// <inheritdoc />
        public override Alternative FromContract(Contract.Alternative contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Alternative"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.QuestionId <= 0)
            {
                throw new InvalidSaveCandidateException("The alternative can't be saved without a question.");
            }

            this.Titles.ValidateSaveCandidate();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Alternative"/> is not valid to be saved.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public async Task UpdateAsync(Contract.Alternative contract, UserSession session)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (this.Id != contract.Id)
            {
                throw new InvalidSaveCandidateException($"The id {contract.Id} doesn't match the expected {this.Id}.");
            }

            this.Id = contract.Id;
            this.QuestionId = contract.QuestionId;
            this.CorrectRow = contract.CorrectRow;
            this.CorrectColumn = contract.CorrectColumn;
            this.IsCorrect = contract.IsCorrect;
            this.ScoreValue = contract.ScoreValue;
            this.CorrectAnswer = contract.CorrectAnswer;
            this.ImageId = contract.ImageId;
            this.Titles.FromContract(contract.Titles);
            await this.SaveAsync(session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<Alternative> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Alternative();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.QuestionId = (int)record[Question.IdColumn];
            this.CorrectRow = (byte)record[CorrectRowColumn];
            this.CorrectColumn = (byte)record[CorrectColumnColumn];
            this.IsCorrect = (bool)record[IsCorrectColumn];
            this.ScoreValue = (byte)record[ScoreValueColumn];
            this.CorrectAnswer = (string)record[CorrectAnswerColumn];
            this.ImageId = (string)record[ImageIdColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Alternative"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Alternative> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Alternative>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<Alternative>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var alternatives = new List<Alternative>();
            if (!reader.HasRows)
            {
                return alternatives;
            }

            while (await reader.ReadAsync())
            {
                alternatives.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return alternatives;
            }

            while (await reader.ReadAsync())
            {
                var alternative = (Alternative)this.GetFromResultsByIdInRecord(alternatives, reader, IdColumn);
                alternative.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            return alternatives;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(Question.IdColumn), this.QuestionId },
                    { Persistent.ColumnToVariable(CorrectRowColumn), this.CorrectRow },
                    { Persistent.ColumnToVariable(CorrectColumnColumn), this.CorrectColumn },
                    { Persistent.ColumnToVariable(IsCorrectColumn), this.IsCorrect },
                    { Persistent.ColumnToVariable(ScoreValueColumn), this.ScoreValue },
                    { Persistent.ColumnToVariable(CorrectAnswerColumn), this.CorrectAnswer },
                    { Persistent.ColumnToVariable(ImageIdColumn), this.ImageId },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}