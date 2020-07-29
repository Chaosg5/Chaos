//-----------------------------------------------------------------------
// <copyright file="TeamAnswer.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models.Games
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Threading.Tasks;

    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <inheritdoc cref="Readable{T, TDto}"/>
    /// <summary>An answer to a <see cref="Alternative"/> on a <see cref="Question"/> done by a <see cref="Team"/>.</summary>
    public class TeamAnswer : Persistable<TeamAnswer, Contract.TeamAnswer>, IUpdateable<TeamAnswer, Contract.TeamAnswer>
    {
        /// <summary>The database column for <see cref="AnsweredRow"/>.</summary>
        private const string AnsweredRowColumn = "AnsweredRow";

        /// <summary>The database column for <see cref="AnsweredColumn"/>.</summary>
        private const string AnsweredColumnColumn = "AnsweredColumn";

        /// <summary>The database column for <see cref="IsAnswered"/>.</summary>
        private const string IsAnsweredColumn = "IsAnswered";

        /// <summary>The database column for <see cref="Answer"/>.</summary>
        private const string AnswerColumn = "Answer";

        /// <summary>Initializes a new instance of the <see cref="TeamAnswer"/> class.</summary>
        /// <param name="contract">The <see cref="Contract.TeamAnswer"/>.</param>
        public TeamAnswer(Contract.TeamAnswer contract)
        {
            this.SchemaName = "game";
            this.TeamId = contract.TeamId;
            this.ChallengeId = contract.ChallengeId;
            this.QuestionId = contract.QuestionId;
            this.AlternativeId = contract.AlternativeId;
            this.AnsweredRow = contract.AnsweredRow;
            this.AnsweredColumn = contract.AnsweredColumn;
            this.IsAnswered = contract.IsAnswered;
            this.Answer = contract.Answer;
        }

        /// <summary>Prevents a default instance of the <see cref="TeamAnswer"/> class from being created.</summary>
        private TeamAnswer()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static TeamAnswer Static { get; } = new TeamAnswer();

        /// <summary>Gets the <see cref="Team.Id"/>.</summary>
        public int TeamId { get; private set; }

        /// <summary>Gets the <see cref="Challenge.Id"/>.</summary>
        public int ChallengeId { get; private set; }

        /// <summary>Gets the <see cref="Question.Id"/>.</summary>
        public int QuestionId { get; private set; }

        /// <summary>Gets the <see cref="Alternative.Id"/>.</summary>
        public int AlternativeId { get; private set; }

        /// <summary>Gets the answered row of the <see cref="Alternative"/>.</summary>
        public byte AnsweredRow { get; private set; }

        /// <summary>Gets the answered column of the <see cref="Alternative"/>.</summary>
        public byte AnsweredColumn { get; private set; }

        /// <summary>Gets a value indicating whether the <see cref="Alternative"/> is answered.</summary>
        public bool IsAnswered { get; private set; }

        /// <summary>Gets the answer of the <see cref="Alternative"/>.</summary>
        public string Answer { get; private set; } = string.Empty;

        /// <inheritdoc />
        public override Contract.TeamAnswer ToContract()
        {
            return new Contract.TeamAnswer
            {
                TeamId = this.TeamId,
                ChallengeId = this.ChallengeId,
                QuestionId = this.QuestionId,
                AlternativeId = this.AlternativeId,
                AnsweredRow = this.AnsweredRow,
                AnsweredColumn = this.AnsweredColumn,
                IsAnswered = this.IsAnswered,
                Answer = this.Answer
            };
        }

        /// <inheritdoc />
        public override Contract.TeamAnswer ToContract(string languageName)
        {
            return new Contract.TeamAnswer
            {
                TeamId = this.TeamId,
                ChallengeId = this.ChallengeId,
                QuestionId = this.QuestionId,
                AlternativeId = this.AlternativeId,
                AnsweredRow = this.AnsweredRow,
                AnsweredColumn = this.AnsweredColumn,
                IsAnswered = this.IsAnswered,
                Answer = this.Answer
            };
        }

        /// <inheritdoc />
        public override TeamAnswer FromContract(Contract.TeamAnswer contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamAnswer"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.TeamId <= 0)
            {
                throw new InvalidSaveCandidateException("A team needs to be specified.");
            }

            if (this.AlternativeId <= 0)
            {
                throw new InvalidSaveCandidateException("An alternative needs to be specified.");
            }

            if (this.AnsweredColumn > 0 || this.AnsweredRow > 0)
            {
                if (this.AnsweredColumn == 0 || this.AnsweredRow == 0)
                {
                    this.AnsweredColumn = 0;
                    this.AnsweredRow = 0;
                    this.IsAnswered = false;
                }
                else
                {
                    this.IsAnswered = true;
                }
            }
            else if (!string.IsNullOrWhiteSpace(this.Answer))
            {
                this.IsAnswered = true;
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<TeamAnswer> NewFromRecordAsync(IDataRecord record)
        {
            var result = new TeamAnswer();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { Team.IdColumn });
            this.TeamId = (int)record[Team.IdColumn];
            this.ChallengeId = (int)record[Challenge.IdColumn];
            this.QuestionId = (int)record[Question.IdColumn];
            this.AlternativeId = (int)record[Alternative.IdColumn];
            this.AnsweredRow = (byte)record[AnsweredRowColumn];
            this.AnsweredColumn = (byte)record[AnsweredColumnColumn];
            this.IsAnswered = (bool)record[IsAnsweredColumn];
            this.Answer = (string)record[AnswerColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamAnswer"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamAnswer"/> is not valid to be saved.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public async Task UpdateAsync(Contract.TeamAnswer contract, UserSession session)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (this.TeamId != contract.TeamId)
            {
                throw new InvalidSaveCandidateException($"The team id {contract.TeamId} doesn't match the expected {this.TeamId}.");
            }

            if (this.AlternativeId != contract.AlternativeId)
            {
                throw new InvalidSaveCandidateException($"The alternative id {contract.AlternativeId} doesn't match the expected {this.AlternativeId}.");
            }

            this.AnsweredRow = contract.AnsweredRow;
            this.AnsweredColumn = contract.AnsweredColumn;
            this.IsAnswered = contract.IsAnswered;
            this.Answer = contract.Answer;
            await this.SaveAsync(session);
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(Team.IdColumn), this.TeamId },
                    { Persistent.ColumnToVariable(Alternative.IdColumn), this.AlternativeId },
                    { Persistent.ColumnToVariable(AnsweredRowColumn), this.AnsweredRow },
                    { Persistent.ColumnToVariable(AnsweredColumnColumn), this.AnsweredColumn },
                    { Persistent.ColumnToVariable(IsAnsweredColumn), this.IsAnswered },
                    { Persistent.ColumnToVariable(AnswerColumn), this.Answer }
                });
        }
    }
}