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
    using System.Globalization;
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

        /// <summary>Gets the list of answered <see cref="Alternative"/>s.</summary>
        public List<TeamAnswer> Answers { get; } = new List<TeamAnswer>();
        
        /// <summary>Ensures a <see cref="TeamChallenge"/>.</summary>
        /// <param name="teamId">The <see cref="TeamId"/>.</param>
        /// <param name="challengeId">The <see cref="ChallengeId"/>.</param>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>The <see cref="TeamChallenge"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="TeamChallenge"/> is not valid to be saved.</exception>
        public static async Task<TeamChallenge> EnsureTeamChallengeAsync(int teamId, int challengeId, UserSession session)
        {
            var teamChallenges = await Static.GetFromDatabaseAsync(GetLoadParameters(teamId, challengeId), Static.ReadFromRecordsAsync, session);
            var teamChallenge = teamChallenges.FirstOrDefault();
            if (teamChallenge == null)
            {
                teamChallenge = new TeamChallenge(teamId, challengeId);
                await teamChallenge.SaveAsync(session);
            }

            return teamChallenge;
        }

        /// <summary>Adds <see cref="Contract.TeamChallenge"/>s to the <see cref="Contract.Zone.Challenges"/> of the <paramref name="game"/>.</summary>
        /// <param name="game">The <see cref="Game"/>.</param>
        /// <param name="teamId">The active <see cref="Team.Id"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static async Task AddTeamChallengesAsync(Contract.Game game, int teamId)
        {
            foreach (var zone in game.Zones)
            {
                await AddTeamChallengesAsync(zone, teamId);
            }
        }

        /// <summary>Adds <see cref="Contract.TeamChallenge"/>s to the <see cref="Contract.Zone.Challenges"/> of the <paramref name="zone"/>.</summary>
        /// <param name="zone">The <see cref="Zone"/>.</param>
        /// <param name="teamId">The active <see cref="Team.Id"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static async Task AddTeamChallengesAsync(Contract.Zone zone, int teamId)
        {
            foreach (var challenge in zone.Challenges)
            {
                challenge.TeamChallenge = (await GameCache.TeamChallengeGetAsync(teamId, challenge.Id)).ToContract();
            }
        }

        /// <summary>Merges <see cref="Answers"/> into a <see cref="Contract.Challenge"/> of the <paramref name="challengeModel"/>.</summary>
        /// <param name="challengeModel">The <see cref="Challenge"/>.</param>
        /// <param name="languageName">The <see cref="CultureInfo.Name"/>.</param>
        /// <returns>The <see cref="Contract.Challenge"/>.</returns>
        /// <exception cref="ArgumentNullException">The <see cref="Challenge.Id"/> doesn't match the <see cref="ChallengeId"/>.</exception>
        /// <exception cref="Exception">A <see cref="Question"/> or <see cref="Alternative"/> is not valid.</exception>
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

        /// <summary>Calculates the <see cref="Score"/> for a <see cref="Question"/>s based on the <see cref="Answers"/>.</summary>
        /// <param name="challengeModel">The <see cref="Challenge"/>.</param>
        /// <param name="languageName">The <see cref="CultureInfo.Name"/>.</param>
        /// <exception cref="Exception">A <see cref="Question"/> is not valid.</exception>
        public void CalculateScore(Challenge challengeModel, string languageName)
        {
            var challenge = this.MergeAnswersIntoChallenge(challengeModel, languageName);
            this.Score = challenge.Questions.Sum(question => question.GetScore());
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
            this.Id = (int)record[Challenge.IdColumn];
            this.TeamId = (int)record[Team.IdColumn];
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
        public override Task<IEnumerable<TeamChallenge>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
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
                var teamChallenge = (TeamChallenge)this.GetFromResultsByIdInRecord(teamChallenges, reader, Challenge.IdColumn);
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

        /// <summary>Gets parameters for <see cref="EnsureTeamChallengeAsync"/>.</summary>
        /// <param name="teamId">The <see cref="TeamId"/>.</param>
        /// <param name="challengeId">The <see cref="ChallengeId"/>.</param>
        /// <returns>The parameters.</returns>
        private static IReadOnlyDictionary<string, object> GetLoadParameters(int teamId, int challengeId)
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(Team.IdColumn), teamId },
                    { Persistent.ColumnToVariable(Challenge.IdColumn), challengeId }
                });
        }
    }
}