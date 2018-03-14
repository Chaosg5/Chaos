//-----------------------------------------------------------------------
// <copyright file="WatchType.cs">
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
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;

    using Exceptions;

    /// <summary>Represents the way in which a <see cref="User"/> watched a <see cref="Movie"/>.</summary>
    public class WatchType : Typeable<WatchType, WatchTypeDto>
    {
        /// <summary>The database column for <see cref="Name"/>.</summary>
        internal const string NameColumn = "Name";

        /// <summary>Initializes a new instance of the <see cref="WatchType" /> class.</summary>
        /// <param name="name">The name to set for the watch type.</param>
        public WatchType(string name)
        {
            this.Id = 0;
            this.Name = name;
        }

        /// <summary>Prevents a default instance of the <see cref="WatchType"/> class from being created.</summary>
        private WatchType()
        {
        }
        
        /// <summary>Gets the name of this watch type.</summary>
        public string Name { get; private set; }

        /// <inheritdoc />
        public override WatchTypeDto ToContract()
        {
            return new WatchTypeDto
            {
                Id = this.Id,
                Name = this.Name
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override WatchType FromContract(WatchTypeDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new WatchType
            {
                Id = contract.Id,
                Name = contract.Name
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="WatchType"/> is not valid to be saved.</exception>
        /// <exception cref="ArgumentNullException">commandParameters or <parmref name="readFromRecord"/> is <see langword="null"/></exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.({T})SaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<WatchType> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<WatchType>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAsync(session.ToContract(), idList.ToList())).Select(x => new ({T})(x));
                return new List<WatchType>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<WatchType>> GetAllAsync(UserSession session)
        {
            if (!Persistent.UseService)
            {
                return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAllAsync(session.ToContract())).Select(x => new ({T})(x));
                return new List<WatchType>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">record is <see langword="null" />.</exception>
        internal override Task<WatchType> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, NameColumn });
            return Task.FromResult(new WatchType { Id = (int)record[IdColumn], Name = (string)record[NameColumn] });
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="WatchType"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException("The name of the watch type cant be empty.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        internal override async Task<IEnumerable<WatchType>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var watchTypes = new List<WatchType>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, $"{nameof(Department)}s");
            }

            while (await reader.ReadAsync())
            {
                watchTypes.Add(await this.ReadFromRecordAsync(reader));
            }

            return watchTypes;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(NameColumn), this.Name }
                });
        }
    }
}