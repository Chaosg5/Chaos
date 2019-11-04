//-----------------------------------------------------------------------
// <copyright file="Watch.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;

    using Exceptions;

    /// <summary>Represents an event where a <see cref="User"/> watched a <see cref="Movie"/>.</summary>
    public class Watch : Persistable<Watch, WatchDto>
    {
        /// <summary>The database column for <see cref="WatchDate"/>.</summary>
        internal const string WatchDateColumn = "WatchDate";

        /// <summary>The database column for <see cref="DateUncertain"/>.</summary>
        internal const string DateUncertainColumn = "DateUncertain";

        /// <summary>The database procedure for saving a <see cref="User"/> <see cref="Watch"/> of a a <see cref="Movie"/>.</summary>
        internal const string UserWatchSaveProcedure = "UserWatchSave";

        /// <summary>The database procedure for deleting a <see cref="User"/> <see cref="Watch"/> of a a <see cref="Movie"/>.</summary>
        internal const string UserWatchDeleteProcedure = "UserWatchDelete";

        /// <summary>The earliest allowed date for the <see cref="WatchDate"/>.</summary>
        private readonly DateTime minDate = new DateTime(1900, 1, 1);

        /// <summary>Private part of the <see cref="WatchType"/> property.</summary>
        private WatchType watchType = new WatchType();

        /// <summary>Private part of the <see cref="UserId"/> property.</summary>
        private int userId;

        /// <summary>Private part of the <see cref="WatchDate"/> property.</summary>
        private DateTime watchDate;

        /// <inheritdoc />
        /// <param name="userId">The id of the <see cref="User"/> who watched the <see cref="Movie"/>.</param>
        /// <param name="watchDate">The date when the movie was watched.</param>
        /// <param name="dateUncertain">If the <paramref name="watchDate"/> when the <see cref="Movie"/> was watched is just estimated and thus uncertain.</param>
        /// <param name="watchType">The <see cref="WatchType"/> describing in what format the <see cref="Movie"/> was watched.</param>
        public Watch(int userId, DateTime watchDate, bool dateUncertain, WatchType watchType)
        {
            this.userId = userId;
            this.WatchDate = watchDate;
            this.DateUncertain = dateUncertain;
            this.WatchType = watchType;
        }

        /// <summary>Prevents a default instance of the <see cref="Watch"/> class from being created.</summary>
        private Watch()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Watch Static { get; } = new Watch();

        /// <summary>Gets the id of the <see cref="User"/> who watched <see cref="Movie"/>.</summary>
        public int UserId
        {
            get => this.userId;
            private set
            {
                if (value <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(WatchType)} has to be saved.");
                }

                this.userId = value;
            }
        }

        /// <summary>Gets or sets the date when the <see cref="Movie"/> was watched.</summary>
        public DateTime WatchDate
        {
            get => this.watchDate;
            set
            {
                if (value <= this.minDate)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.watchDate = value;
            }
        }

        /// <summary>Gets or sets a value indicating whether the <see cref="WatchDate"/> is uncertain or not.</summary>
        public bool DateUncertain { get; set; }

        /// <summary>Gets or sets the <see cref="WatchType"/> how the <see cref="Movie"/> was watched.</summary>
        public WatchType WatchType
        {
            get => this.watchType;
            set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(WatchType)} has to be saved.");
                }

                this.watchType = value;
            }
        }

        /// <inheritdoc />
        public override WatchDto ToContract()
        {
            return new WatchDto
            {
                Id = this.Id,
                UserId = this.UserId,
                WatchDate = this.WatchDate,
                DateUncertain = this.DateUncertain,
                WatchType = this.WatchType.ToContract()
            };
        }

        /// <inheritdoc />
        public override WatchDto ToContract(string languageName)
        {
            return new WatchDto
            {
                Id = this.Id,
                UserId = this.UserId,
                WatchDate = this.WatchDate,
                DateUncertain = this.DateUncertain,
                WatchType = this.WatchType.ToContract(languageName)
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override Watch FromContract(WatchDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new Watch
            {
                Id = contract.Id,
                UserId = contract.UserId,
                WatchDate = contract.WatchDate,
                DateUncertain = contract.DateUncertain,
                WatchType = contract.WatchType != null ? this.WatchType.FromContract(contract.WatchType) : new WatchType()
            };
        }

        /// <inheritdoc />
        public override Task SaveAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Watch"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            this.WatchType.ValidateSaveCandidate();
        }

        /// <summary>Creates new <see cref="Watch"/>s from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing data sets and records the data for the <see cref="Watch"/>s.</param>
        /// <returns>The list of <see cref="Watch"/>s.</returns>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        internal async Task<IEnumerable<Watch>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var watches = new List<Watch>();
            if (!reader.HasRows)
            {
                return watches;
            }

            while (await reader.ReadAsync())
            {
                watches.Add(await this.NewFromRecordAsync(reader));
            }

            return watches;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">record is <see langword="null" />.</exception>
        public override async Task<Watch> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Watch();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">record is <see langword="null" />.</exception>
        public override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, User.IdColumn, WatchDateColumn, DateUncertainColumn, WatchType.IdColumn });
            this.Id = (int)record[IdColumn];
            this.UserId = (int)record[User.IdColumn];
            this.WatchDate = (DateTime)record[WatchDateColumn];
            this.DateUncertain = (bool)record[DateUncertainColumn];
            this.WatchType = await GlobalCache.GetWatchTypeAsync((int)record[WatchType.IdColumn]);
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new NotImplementedException();
        }
    }
}
