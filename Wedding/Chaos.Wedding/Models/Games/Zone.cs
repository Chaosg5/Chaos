//-----------------------------------------------------------------------
// <copyright file="Zone.cs">
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
    /// <summary>A zone in a <see cref="Game" /> containing a set of <see cref="Challenge" />s.</summary>
    public sealed class Zone : Readable<Zone, Contract.Zone>, IUpdateable<Zone, Contract.Zone>
    {
        /// <summary>The database column for <see cref="ImageId"/>.</summary>
        private const string ImageIdColumn = "ImageId";

        /// <summary>The database column for <see cref="Height"/>.</summary>
        private const string HeightColumn = "Height";

        /// <summary>The database column for <see cref="Width"/>.</summary>
        private const string WidthColumn = "Width";

        /// <summary>The database column for <see cref="PositionX"/>.</summary>
        private const string PositionXColumn = "X";

        /// <summary>The database column for <see cref="PositionY"/>.</summary>
        private const string PositionYColumn = "Y";

        /// <summary>Private part of the <see cref="Challenges"/> property.</summary>
        private readonly List<Challenge> challenges = new List<Challenge>();

        /// <summary>Private part of the <see cref="GameId"/> property.</summary>
        private int gameId;

        /// <summary>Private part of the <see cref="ImageId"/> property.</summary>
        private string imageId = string.Empty;

        /// <summary>Private part of the <see cref="Height"/> property.</summary>
        private short height;

        /// <summary>Private part of the <see cref="Width"/> property.</summary>
        private short width;

        /// <summary>Private part of the <see cref="PositionX"/> property.</summary>
        private short positionX;

        /// <summary>Private part of the <see cref="PositionY"/> property.</summary>
        private short positionY;

        /// <summary>Initializes a new instance of the <see cref="Zone"/> class.</summary>
        /// <param name="gameId">The <see cref="GameId"/>.</param>
        /// <param name="width">The <see cref="Width"/>.</param>
        /// <param name="height">The <see cref="Height"/>.</param>
        /// <param name="positionX">The <see cref="PositionX"/>.</param>
        /// <param name="positionY">The <see cref="PositionY"/>.</param>
        /// <param name="imageId">The <see cref="ImageId"/>.</param>
        /// <param name="titles">The <see cref="Titles"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Zone"/> is not valid to be saved.</exception>
        public Zone(int gameId, short width, short height, short positionX, short positionY, string imageId, string titles)
        {
            this.SchemaName = "game";
            this.GameId = gameId;
            this.Width = width;
            this.Height = height;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.ImageId = imageId;
            this.Titles.UpdateFromText(titles);
        }

        /// <summary>Prevents a default instance of the <see cref="Zone"/> class from being created.</summary>
        private Zone()
        {
            this.SchemaName = "game";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Zone Static { get; } = new Zone();

        /// <summary>Gets the <see cref="Game.Id"/> of the parent <see cref="Game"/>.</summary>
        public int GameId
        {
            get => this.gameId;
            private set
            {
                if (value > 0)
                {
                    this.gameId = value;
                }
            }
        }

        /// <summary>Gets the image <see cref="Zone"/>.</summary>
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

        /// <summary>Gets the height of the <see cref="Zone"/>.</summary>
        public short Height
        {
            get => this.height;
            private set => this.height = value < 0 ? (short)0 : value;
        }

        /// <summary>Gets the width of the <see cref="Zone"/>.</summary>
        public short Width
        {
            get => this.width;
            private set => this.width = value < 0 ? (short)0 : value;
        }

        /// <summary>Gets the X position of the <see cref="Zone"/>.</summary>
        public short PositionX
        {
            get => this.positionX;
            private set => this.positionX = value < 0 ? (short)0 : value;
        }

        /// <summary>Gets the Y position of the <see cref="Zone"/>.</summary>
        public short PositionY
        {
            get => this.positionY;
            private set => this.positionY = value < 0 ? (short)0 : value;
        }

        /// <summary>Gets the titles of the <see cref="Zone"/>.</summary>
        public LanguageDescriptionCollection Titles { get; } = new LanguageDescriptionCollection();

        /// <summary>Gets the children <see cref="Challenge"/>s.</summary>
        public IEnumerable<Challenge> Challenges => this.challenges;

        /// <inheritdoc />
        public override Contract.Zone ToContract()
        {
            return new Contract.Zone
            {
                Id = this.Id,
                GameId = this.GameId,
                ImageId = this.ImageId,
                Height = this.Height,
                Width = this.Width,
                PositionX = this.PositionX,
                PositionY = this.PositionY,
                Titles = this.Titles.ToContract(),
                Challenges = this.Challenges.Select(c => c.ToContract())
            };
        }

        /// <inheritdoc />
        public override Contract.Zone ToContract(string languageName)
        {
            return new Contract.Zone
            {
                Id = this.Id,
                GameId = this.GameId,
                ImageId = this.ImageId,
                Height = this.Height,
                Width = this.Width,
                PositionX = this.PositionX,
                PositionY = this.PositionY,
                Titles = this.Titles.ToContract(languageName),
                Challenges = this.Challenges.Select(c => c.ToContract(languageName))
            };
        }

        /// <inheritdoc />
        public override Zone FromContract(Contract.Zone contract)
        {
            // ReSharper disable once ExceptionNotDocumented
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Zone"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.GameId <= 0)
            {
                throw new InvalidSaveCandidateException("The zone can't be saved without a game.");
            }

            this.Titles.ValidateSaveCandidate();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Zone"/> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public async Task UpdateAsync(Contract.Zone contract, UserSession session)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (this.Id != contract.Id)
            {
                throw new InvalidSaveCandidateException($"The id {contract.Id} doesn't match the expected {this.Id}.");
            }

            if (contract.ImageId == null)
            {
                throw new InvalidSaveCandidateException("The image id can't be null.");
            }

            if (contract.GameId <= 0)
            {
                throw new InvalidSaveCandidateException("The zone can't be saved without a game.");
            }

            this.GameId = contract.GameId;
            this.Width = contract.Width;
            this.Height = contract.Height;
            this.PositionX = contract.PositionX;
            this.PositionY = contract.PositionY;
            this.ImageId = contract.ImageId;
            await this.SaveAsync(session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<Zone> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Zone();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.GameId = (int)record[Game.IdColumn];
            this.ImageId = (string)record[ImageIdColumn];
            this.Height = (short)record[HeightColumn];
            this.Width = (short)record[WidthColumn];
            this.PositionX = (short)record[PositionXColumn];
            this.PositionY = (short)record[PositionYColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Zone"/> is not valid to be saved.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Zone> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Zone>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<Zone>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var zones = new List<Zone>();
            if (!reader.HasRows)
            {
                return zones;
            }

            while (await reader.ReadAsync())
            {
                zones.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return zones;
            }

            while (await reader.ReadAsync())
            {
                var zone = (Zone)this.GetFromResultsByIdInRecord(zones, reader, IdColumn);
                zone.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return zones;
            }

            while (await reader.ReadAsync())
            {
                var zone = (Zone)this.GetFromResultsByIdInRecord(zones, reader, IdColumn);
                zone.challenges.Add(await GameCache.ChallengeGetAsync((int)reader[Challenge.IdColumn]));
            }

            return zones;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(Game.IdColumn), this.GameId },
                    { Persistent.ColumnToVariable(ImageIdColumn), this.ImageId },
                    { Persistent.ColumnToVariable(HeightColumn), this.Height },
                    { Persistent.ColumnToVariable(WidthColumn), this.Width },
                    { Persistent.ColumnToVariable(PositionXColumn), this.PositionX },
                    { Persistent.ColumnToVariable(PositionYColumn), this.PositionY },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}