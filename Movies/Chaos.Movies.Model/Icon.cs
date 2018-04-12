//-----------------------------------------------------------------------
// <copyright file="Icon.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Data.Linq;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>An image icon.</summary>
    public class Icon : Readable<Icon, IconDto>
    {
        /// <summary>The database column for <see cref="Data"/>.</summary>
        private const string DataColumn = "Data";

        /// <summary>The database column for <see cref="Url"/>.</summary>
        private const string UrlColumn = "IconUrl";
        
        /// <summary>The database column for <see cref="Size"/>.</summary>
        private const string SizeColumn = "Size";

        /// <summary>Private part of the <see cref="Data"/> property.</summary>
        private Binary data;

        /// <summary>Private part of the <see cref="IconType"/> property.</summary>
        private IconType iconType = new IconType();

        /// <summary>Private part of the <see cref="Url"/> property.</summary>
        private string url = string.Empty;

        /// <summary>Initializes a new instance of the <see cref="Icon"/> class.</summary>
        /// <param name="iconType">The <see cref="IconType"/> to set.</param>
        /// <param name="url">The <see cref="Url"/> to set.</param>
        /// <param name="data">The <see cref="Data"/> to set.</param>
        /// <exception cref="ArgumentNullException">Either data or URL has to be set. <paramref name="data"/></exception>
        public Icon(IconType iconType, string url, Binary data)
        {
            if ((data == null || data.Length == 0) && string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(data), "Either data or URL has to be set.");
            }

            this.IconType = iconType;
            this.Url = url;
            this.Data = data;
        }

        /// <summary>Prevents a default instance of the <see cref="Icon"/> class from being created.</summary>
        private Icon()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Icon Static { get; } = new Icon();

        /// <summary>Gets the image type of the icon.</summary>
        public IconType IconType
        {
            get => this.iconType;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(IconType)} has to be saved.");
                }

                this.iconType = value;
            }
        }

        /// <summary>Gets the URL of the image of the icon.</summary>
        /// <remarks>If emptying the URL the <see cref="Data"/> needs to be set first.</remarks>
        public string Url
        {
            get => this.url;
            private set
            {
                if (string.IsNullOrEmpty(value) && (this.Data == null || this.Data.Length == 0))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                this.url = value ?? string.Empty;
            }
        }

        /// <summary>Gets the binary data of the image of the icon.</summary>
        /// <remarks>If emptying the URL the <see cref="Url"/> needs to be set first.</remarks>
        public Binary Data
        {
            get => this.data;
            private set
            {
                if ((value == null || value.Length == 0) && string.IsNullOrEmpty(this.Url))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                this.data = value;
            }
        }

        /// <summary>Gets the size of the <see cref="Data"/>.</summary>
        public int Size => this.Data?.Length ?? 0;

        /// <summary>Converts this <see cref="Icon"/> to a <see cref="IconDto"/>.</summary>
        /// <returns>The <see cref="IconDto"/>.</returns>
        public override IconDto ToContract()
        {
            return new IconDto { Id = this.Id, IconType = this.IconType.ToContract(), Data = this.Data, Url = this.Url };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override Icon FromContract(IconDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new Icon { Id = contract.Id, IconType = this.IconType.FromContract(contract.IconType), Data = contract.Data, Url = contract.Url };
        }

        /// <summary>Saves this <see cref="Character"/> to the database.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <returns>No return.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                await service.IconSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Icon> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<IEnumerable<Icon>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.IconGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <summary>Changes the image data.</summary>
        /// <param name="newData">The new data to set.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task SetDataAndSaveAsync(Binary newData, UserSession session)
        {
            this.Data = newData;
            this.Url = string.Empty;
            await this.SaveAsync(session);
        }

        /// <summary>Changes the image data.</summary>
        /// <param name="newUrl">The new data to set.</param>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public async Task SetDataAndSaveAsync(string newUrl, UserSession session)
        {
            this.Url = newUrl;
            this.Data = null;
            await this.SaveAsync(session);
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<Icon> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Icon();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        internal override async Task<IEnumerable<Icon>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var icons = new List<Icon>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, $"{nameof(Icon)}s");
            }

            while (await reader.ReadAsync())
            {
                icons.Add(await this.NewFromRecordAsync(reader));
            }

            return icons;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, IconType.IdColumn, UrlColumn, DataColumn });
            this.Id = (int)record[IdColumn];
            this.IconType = await GlobalCache.GetIconTypeAsync((int)record[IconType.IdColumn]);
            this.Url = (string)record[UrlColumn];
            this.Data = (Binary)record[DataColumn];
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(IconType.IdColumn), this.IconType.Id },
                    { Persistent.ColumnToVariable(UrlColumn), this.Url },
                    { Persistent.ColumnToVariable(DataColumn), this.Data },
                    { Persistent.ColumnToVariable(SizeColumn), this.Size }
                });
        }
    }
}
