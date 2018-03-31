//-----------------------------------------------------------------------
// <copyright file="RatingSystem.cs">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;
    
    /// <summary>A system for giving different <see cref="RatingType"/>s different values when calculating a rating for a <see cref="Movie"/>.</summary>
    public class RatingSystem : Typeable<RatingSystem, RatingSystemDto>
    {
        // ToDo: Make RatingSystemValues it's own class

        /// <summary>The database column for the value in <see cref="Values"/>.</summary>
        private const string ValueColumn = "Weight";

        /// <summary>The database column for the <see cref="Values"/>.</summary>
        private const string RatingSystemValuesColumn = "Values";

        /// <summary>Private part of the <see cref="Values"/> property.</summary>
        private Dictionary<RatingType, short> values = new Dictionary<RatingType, short>();
        
        /// <summary>Gets a reference to simulate static methods.</summary>
        public static RatingSystem Static { get; } = new RatingSystem();

        /// <summary>Gets the titles of the rating type.</summary>
        public LanguageDescriptionCollection Titles { get; private set; } = new LanguageDescriptionCollection();

        /// <summary>Gets the the relative value for each <see cref="RatingType"/>.</summary>
        public ReadOnlyDictionary<RatingType, short> Values => new ReadOnlyDictionary<RatingType, short>(this.values);

        /// <summary>Gets properties from of each item in <see cref="values"/> in a table which can be used to save them to the database.</summary>
        private DataTable RatingSystemValueGetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(RatingType.IdColumn));
                    table.Columns.Add(new DataColumn(ValueColumn));
                    foreach (var value in this.Values)
                    {
                        table.Rows.Add(value.Key.Id, value.Value);
                    }

                    return table;
                }
            }
        }

        /// <summary>Sets the value for the specified type.</summary>
        /// <param name="ratingType">The type to set the value for.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ratingType"/> is <see langword="null"/></exception>
        public void SetValue(RatingType ratingType, short value)
        {
            if (ratingType == null)
            {
                throw new ArgumentNullException(nameof(ratingType));
            }

            if (this.values.ContainsKey(ratingType))
            {
                this.values[ratingType] = value;
            }
            else
            {
                this.values.Add(ratingType, value);
            }
        }

        /// <inheritdoc />
        public override RatingSystemDto ToContract()
        {
            return new RatingSystemDto
            {
                Id = this.Id,
                Titles = this.Titles.ToContract(),
                //Values = new ReadOnlyDictionary<RatingTypeDto, short>(this.Values.ToDictionary(p => p.Key.ToContract(), p => p.Value))
            };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override RatingSystem FromContract(RatingSystemDto contract)
        {
            return new RatingSystem
            {
                Id = contract.Id,
                Titles = this.Titles.FromContract(contract.Titles),
                //values = contract.Values.ToDictionary(v => RatingType.Static.FromContract(v.Key), v => v.Value)
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="RatingSystem"/> is not valid to be saved.</exception>
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
        public override async Task<RatingSystem> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<IEnumerable<RatingSystem>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAsync(session.ToContract(), idList.ToList())).Select(x => new ({T})(x));
                return new List<RatingSystem>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<RatingSystem>> GetAllAsync(UserSession session)
        {
            if (!Persistent.UseService)
            {
                return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                // ToDo: Service
                ////return (await service.({T})GetAllAsync(session.ToContract())).Select(x => new ({T})(x));
                return new List<RatingSystem>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        internal override async Task<IEnumerable<RatingSystem>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var ratingSystems = new List<RatingSystem>();
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, $"{nameof(RatingSystem)}s");
            }

            while (await reader.ReadAsync())
            {
                ratingSystems.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, $"{nameof(RatingSystem)}{LanguageDescriptionCollection.TitlesColumn}");
            }

            while (await reader.ReadAsync())
            {
                var ratingSystem = (RatingSystem)this.GetFromResultsByIdInRecord(ratingSystems, reader, IdColumn);
                ratingSystem.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(3, $"{nameof(RatingSystem)}{RatingSystemValuesColumn}");
            }

            while (await reader.ReadAsync())
            {
                Persistent.ValidateRecord(reader, new[] { RatingType.IdColumn, ValueColumn });
                var ratingSystem = (RatingSystem)this.GetFromResultsByIdInRecord(ratingSystems, reader, IdColumn);
                ratingSystem.values.Add(await GlobalCache.GetRatingTypeAsync((int)reader[RatingType.IdColumn]), (short)reader[ValueColumn]);
            }

            return ratingSystems;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="RatingSystem"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">record is <see langword="null" />.</exception>
        internal override async Task<RatingSystem> NewFromRecordAsync(IDataRecord record)
        {
            var result = new RatingSystem();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">record is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(LanguageDescriptionCollection.TitlesColumn), this.Titles.GetSaveTable },
                    { Persistent.ColumnToVariable(RatingSystemValuesColumn), this.RatingSystemValueGetSaveTable }
                });
        }
    }
}