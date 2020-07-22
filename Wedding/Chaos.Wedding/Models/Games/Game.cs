//-----------------------------------------------------------------------
// <copyright file="Game.cs">
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
    /// <summary>A game containing a set of <see cref="Zone" />s.</summary>
    public sealed class Game : Typeable<Game, Contract.Game>, IUpdateable<Game, Contract.Game>
    {
        /// <summary>The database column for <see cref="ImageId"/>.</summary>
        private const string ImageIdColumn = "ImageId";

        /// <summary>The database column for <see cref="Height"/>.</summary>
        private const string HeightColumn = "Height";

        /// <summary>The database column for <see cref="Width"/>.</summary>
        private const string WidthColumn = "Width";

        /// <summary>Private part of the <see cref="Zones"/> property.</summary>
        private readonly List<Zone> zones = new List<Zone>();

        /// <summary>Private part of the <see cref="ImageId"/> property.</summary>
        private string imageId = string.Empty;

        /// <summary>Private part of the <see cref="Height"/> property.</summary>
        private short height;

        /// <summary>Private part of the <see cref="Width"/> property.</summary>
        private short width;

        /// <summary>Initializes a new instance of the <see cref="Game"/> class.</summary>
        /// <param name="imageId">The <see cref="ImageId"/>.</param>
        /// <param name="width">The <see cref="Width"/>.</param>
        /// <param name="height">The <see cref="Height"/>.</param>
        /// <param name="titles">The <see cref="Titles"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        public Game(string imageId, short width, short height, string titles)
        {
            this.SchemaName = "game";
            this.ImageId = imageId;
            this.Width = width;
            this.Height = height;
            this.Titles.UpdateFromText(titles);
        }

        /// <summary>Prevents a default instance of the <see cref="Game"/> class from being created.</summary>
        private Game()
        {
            this.SchemaName = "game";
        }
        
        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Game Static { get; } = new Game();

        /// <summary>Gets the image of the <see cref="Game"/>.</summary>
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

        /// <summary>Gets the height of the <see cref="Game"/>.</summary>
        public short Height
        {
            get => this.height;
            private set => this.height = value < 0 ? (short)0 : value;
        }

        /// <summary>Gets the width of the <see cref="Game"/>.</summary>
        public short Width
        {
            get => this.width;
            private set => this.width = value < 0 ? (short)0 : value;
        }

        /// <summary>Gets the titles of the <see cref="Game"/>.</summary>
        public LanguageDescriptionCollection Titles { get; } = new LanguageDescriptionCollection();

        /// <summary>Gets the children <see cref="Zone"/>s.</summary>
        //// ToDo: Create ChildList - class, like listable but with reference to parent
        public List<Zone> Zones => this.zones;
        
        /// <summary>Gets the <see cref="Game"/>s that the <see cref="Team"/> has played.</summary>
        //// ToDo: Create ChildList - class, like listable but with reference to parent
        public List<int> TeamIds { get; } = new List<int>();

        /// <inheritdoc />
        public override Contract.Game ToContract()
        {
            return new Contract.Game
            {
                Id = this.Id,
                ImageId = this.ImageId,
                Height = this.Height,
                Width = this.Width,
                Titles = this.Titles.ToContract(),
                Zones = new ReadOnlyCollection<Contract.Zone>(this.Zones.Select(z => z.ToContract()).ToList())
            };
        }

        /// <inheritdoc />
        public override Contract.Game ToContract(string languageName)
        {
            return new Contract.Game
            {
                Id = this.Id,
                ImageId = this.ImageId,
                Height = this.Height,
                Width = this.Width,
                Titles = this.Titles.ToContract(languageName),
                Zones = new ReadOnlyCollection<Contract.Zone>(this.Zones.Select(z => z.ToContract(languageName)).ToList())
            };
        }

        /// <inheritdoc />
        public override Game FromContract(Contract.Game contract)
        {
            var game = new Game { ImageId = contract.ImageId, Width = contract.Width, Height = contract.Height };
            if (contract.Zones != null)
            {
                game.zones.AddRange(contract.Zones.Select(z => Zone.Static.FromContract(z)));
            }

            game.Titles.AddRange(game.Titles.FromContract(contract.Titles));
            return game;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            this.Titles.ValidateSaveCandidate();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public async Task UpdateAsync(Contract.Game contract, UserSession session)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (this.Id != contract.Id)
            {
                throw new InvalidSaveCandidateException($"The id {contract.Id} doesn't match the expected {this.Id}.");
            }

            this.ImageId = contract.ImageId;
            this.Height = contract.Height;
            this.Width = contract.Width;
            this.Titles.Update(this.Titles.FromContract(contract.Titles));
            await this.SaveAsync(session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<Game> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Game();
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
            this.Height = (short)record[HeightColumn];
            this.Width = (short)record[WidthColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Game"/> is not valid to be saved.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Game> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Game>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Game>> GetAllAsync(UserSession session)
        {
            return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        public override async Task<IEnumerable<Game>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var games = new List<Game>();
            if (!reader.HasRows)
            {
                return games;
            }

            while (await reader.ReadAsync())
            {
                games.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return games;
            }

            while (await reader.ReadAsync())
            {
                var game = (Game)this.GetFromResultsByIdInRecord(games, reader, IdColumn);
                game.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return games;
            }

            while (await reader.ReadAsync())
            {
                var game = (Game)this.GetFromResultsByIdInRecord(games, reader, IdColumn);
                game.zones.Add(await GameCache.ZoneGetAsync((int)reader[Zone.IdColumn]));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                return games;
            }

            while (await reader.ReadAsync())
            {
                var game = (Game)this.GetFromResultsByIdInRecord(games, reader, IdColumn);
                game.TeamIds.Add((int)reader[Team.IdColumn]);
            }

            return games;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(ImageIdColumn), this.ImageId },
                    { Persistent.ColumnToVariable(HeightColumn), this.Height },
                    { Persistent.ColumnToVariable(WidthColumn), this.Width },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}