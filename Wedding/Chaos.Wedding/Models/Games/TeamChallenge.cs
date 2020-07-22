//-----------------------------------------------------------------------
// <copyright file="TeamChallenge.cs">
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
    /// <summary>A <see cref="Challenge"/> played by a <see cref="Team"/>.</summary>
    public class TeamChallenge : Readable<TeamChallenge, Contract.TeamChallenge> ////, IUpdateable<TeamChallenge, Contract.TeamChallenge>
    {
        /// <summary>The database column for <see cref="IsLocked"/>.</summary>
        private const string IsLockedColumn = "IsLocked";

        /// <summary>The database column for <see cref="Score"/>.</summary>
        private const string ScoreColumn = "Score";

        /// <summary>Private part of the <see cref="Score"/> property.</summary>
        private int score;

        /// <summary>Initializes a new instance of the <see cref="TeamChallenge"/> class.</summary>
        /// <param name="teamId">The <see cref="TeamId"/>.</param>
        /// <param name="challengeId">The <see cref="ChallengeId"/>.</param>
        /// <param name="isLocked">The <see cref="IsLocked"/>.</param>
        /// <param name="score">The <see cref="Score"/>.</param>
        public TeamChallenge(int teamId, int challengeId, bool isLocked = false, int score = 0)
        {
            this.SchemaName = "game";
            this.TeamId = teamId;
            this.ChallengeId = challengeId;
            this.IsLocked = isLocked;
            this.Score = score;
        }

        /// <summary>Prevents a default instance of the <see cref="TeamChallenge"/> class from being created.</summary>
        private TeamChallenge()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static TeamChallenge Static { get; } = new TeamChallenge();

        /// <summary>Gets the <see cref="Team.Id"/>.</summary>
        public int TeamId { get; private set; }

        /// <summary>Gets the <see cref="Challenge.Id"/>.</summary>
        public int ChallengeId { get; private set; }

        /// <summary>Gets or sets a value indicating whether the <see cref="Challenge"/> is locked.</summary>
        public bool IsLocked { get; set; }

        /// <summary>Gets or sets the <see cref="Team"/>'s score on the <see cref="Challenge"/>.</summary>
        public int Score
        {
            get => this.score;
            set
            {
                if (value > 0)
                {
                    this.score = value;
                }
            }
        }

        /// <summary>Gets the list of answered <see cref="GetAlternative"/>s.</summary>
        public List<TeamAnswer> Answers { get; } = new List<TeamAnswer>();
        
        /// <summary>Ensures a <see cref="TeamChallenge"/>.</summary>
        /// <param name="teamId">The <see cref="TeamId"/>.</param>
        /// <param name="challengeId">The <see cref="ChallengeId"/>.</param>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="TeamChallenge"/>.</returns>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamChallenge"/> is not valid to be saved.</exception>
        public static async Task<TeamChallenge> EnsureTeamChallengeAsync(int teamId, int challengeId, UserSession session)
        {
            var teamChallenges = await Static.GetAsync(session, new List<int> { teamId });
            var teamChallenge = teamChallenges.FirstOrDefault(c => c.ChallengeId == challengeId);
            if (teamChallenge == null)
            {
                teamChallenge = new TeamChallenge(teamId, challengeId);
                await teamChallenge.SaveAsync(session);
            }

            return teamChallenge;
        }

        public Contract.Challenge MergeAnswersIntoChallenge(Challenge challengeModel, string languageName)
        {
            var challenge = challengeModel.ToContract(languageName);
            if (this.ChallengeId != challenge.Id)
            {
                throw new ArgumentNullException(nameof(challenge), $"The id {challenge.Id} doesn't match the expected {this.ChallengeId}.");
            }
            
            challenge.TeamChallenge = this.ToContract(languageName);
            foreach (var teamAnswer in this.Answers)
            {
                var question = challenge.Questions.FirstOrDefault(q => q.Id == teamAnswer.QuestionId);
                var alternative = question?.Alternatives?.FirstOrDefault(a => a.Id == teamAnswer.AlternativeId);
                if (alternative == null)
                {
                    throw new Exception("ToDo:");
                }

                alternative.TeamAnswer = teamAnswer.ToContract(languageName);
            }

            return challenge;
        }


        public void CalculateScore(Contract.Challenge challenge)
        {
            var calculatedScore = 0;
            foreach (var teamAnswer in this.Answers)
            {
                var question = challenge.Questions.FirstOrDefault(q => q.Id == teamAnswer.QuestionId);
                var alternative = question?.Alternatives?.FirstOrDefault(a => a.Id == teamAnswer.AlternativeId);
                if (alternative == null)
                {
                    throw new Exception("ToDo:");
                }

                calculatedScore += question.GetScore(alternative);
            }

            this.Score = calculatedScore;
        }

        /// <summary>The update answer async.</summary>
        /// <param name="contract">The contract.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamAnswer"/> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task<TeamAnswer> UpdateAnswerAsync(Contract.TeamAnswer contract, UserSession session)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (this.TeamId != contract.TeamId)
            {
                throw new InvalidSaveCandidateException($"The team id {contract.TeamId} doesn't match the expected {this.TeamId}.");
            }

            var answer = this.Answers.FirstOrDefault(a => a.AlternativeId == contract.AlternativeId);
            if (answer == null)
            {
                answer = new TeamAnswer(contract);
                await answer.SaveAsync(session);
                this.Answers.Add(answer);
            }
            else
            {
                await answer.UpdateAsync(contract, session);
            }
            
            return answer;
        }

        /// <inheritdoc />
        public override Contract.TeamChallenge ToContract()
        {
            return new Contract.TeamChallenge
            {
                TeamId = this.TeamId,
                ChallengeId = this.ChallengeId,
                IsLocked = this.IsLocked,
                Score = this.Score
            };
        }

        /// <inheritdoc />
        public override Contract.TeamChallenge ToContract(string languageName)
        {
            return new Contract.TeamChallenge
            {
                TeamId = this.TeamId,
                ChallengeId = this.ChallengeId,
                IsLocked = this.IsLocked,
                Score = this.Score
            };
        }

        /// <inheritdoc />
        public override TeamChallenge FromContract(Contract.TeamChallenge contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamChallenge"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.TeamId <= 0)
            {
                throw new InvalidSaveCandidateException("A team needs to be specified.");
            }

            if (this.ChallengeId <= 0)
            {
                throw new InvalidSaveCandidateException("A challenge needs to be specified.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<TeamChallenge> NewFromRecordAsync(IDataRecord record)
        {
            var result = new TeamChallenge();
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
            this.ChallengeId = (int)record[Challenge.IdColumn];
            this.IsLocked = (bool)record[IsLockedColumn];
            this.Score = (int)record[ScoreColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task<TeamChallenge> GetAsync(UserSession session, int id)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }
        
        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<TeamChallenge>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamChallenge"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<IEnumerable<TeamChallenge>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var teamChallenges = new List<TeamChallenge>();
            if (!reader.HasRows)
            {
                return teamChallenges;
            }

            while (await reader.ReadAsync())
            {
                teamChallenges.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return teamChallenges;
            }

            while (await reader.ReadAsync())
            {
                var teamChallenge = (TeamChallenge)this.GetFromResultsByIdInRecord(teamChallenges, reader, Team.IdColumn);
                teamChallenge.Answers.Add(await TeamAnswer.Static.NewFromRecordAsync(reader));
            }

            return teamChallenges;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(Team.IdColumn), this.TeamId },
                    { Persistent.ColumnToVariable(Challenge.IdColumn), this.ChallengeId },
                    { Persistent.ColumnToVariable(IsLockedColumn), this.IsLocked },
                    { Persistent.ColumnToVariable(ScoreColumn), this.Score }
                });
        }
    }
}