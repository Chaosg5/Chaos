//-----------------------------------------------------------------------
// <copyright file="RatingType.cs">
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

    /// <summary>A sub rating with a defined type.</summary>
    public class RatingType : Typeable<RatingType, RatingTypeDto>
    {
        /// <summary>The database column for <see cref="ParentRatingTypeId"/>.</summary>
        private const string ParentRatingTypeIdColumn = "ParentRatingTypeId";
        
        /// <inheritdoc />
        public RatingType()
        {
            this.Subtypes = new RatingTypeCollection(this);
        }

        /// <summary>Initializes a new instance of the <see cref="RatingType"/> class.</summary>
        /// <remarks>Only intended for tests.</remarks>
        /// <param name="id">The id.</param>
        internal RatingType(int id)
        {
            // ToDo: Remove and read Types from the database in the test.
            this.Id = id;
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static RatingType Static { get; } = new RatingType();

        /// <summary>Gets the titles of the rating type.</summary>
        public LanguageDescriptionCollection Titles { get; private set; } = new LanguageDescriptionCollection();

        /// <summary>Gets the <see cref="RatingType"/>s that makes up the derived children of this <see cref="RatingType"/>.</summary>
        public RatingTypeCollection Subtypes { get; private set; }

        /// <summary>Gets the id of the parent <see cref="RatingType"/>.</summary>
        public int ParentRatingTypeId { get; private set; }
        
        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="RatingType"/> is not valid to be saved.</exception>
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
                await service.RatingTypeSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        public override RatingTypeDto ToContract()
        {
            return new RatingTypeDto { Id = this.Id, Titles = this.Titles.ToContract(), Subtypes = this.Subtypes.ToContract() };
        }

        /// <inheritdoc />
        public override RatingTypeDto ToContract(string languageName)
        {
            return new RatingTypeDto { Id = this.Id, Titles = this.Titles.ToContract(languageName) };
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override RatingType FromContract(RatingTypeDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new RatingType
            {
                Id = contract.Id,
                Titles = this.Titles.FromContract(contract.Titles),
                Subtypes = this.Subtypes.FromContract(contract.Subtypes)
            };
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<RatingType> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<IEnumerable<RatingType>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.RatingTypeGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<RatingType>> GetAllAsync(UserSession session)
        {
            if (!Persistent.UseService)
            {
                return await this.GetAllFromDatabaseAsync(this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.RatingTypeGetAllAsync(session.ToContract())).Select(this.FromContract);
            }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="RatingType"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }

            foreach (var subtype in this.Subtypes)
            {
                subtype.ValidateSaveCandidate();
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override async Task<IEnumerable<RatingType>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var ratingTypes = new List<RatingType>();
            if (!reader.HasRows)
            {
                return ratingTypes;
            }

            while (await reader.ReadAsync())
            {
                ratingTypes.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, $"{nameof(RatingType)}{LanguageTitleCollection.TitlesColumn}");
            }

            while (await reader.ReadAsync())
            {
                var ratingType = (RatingType)this.GetFromResultsByIdInRecord(ratingTypes, reader, IdColumn);
                ratingType.Titles.Add(await LanguageDescription.Static.NewFromRecordAsync(reader));
            }

            foreach (var ratingType in ratingTypes.Where(r => r.ParentRatingTypeId > 0))
            {
                var parent = ratingTypes.FirstOrDefault(p => p.Id == ratingType.ParentRatingTypeId);
                if (parent != null)
                {
                    parent.Subtypes.Add(ratingType);
                }
                else
                {
                    parent = await GlobalCache.GetRatingTypeAsync(ratingType.ParentRatingTypeId);
                    parent.Subtypes.Add(ratingType);
                }
            }

            return ratingTypes;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override async Task<RatingType> NewFromRecordAsync(IDataRecord record)
        {
            var result = new RatingType();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, ParentRatingTypeIdColumn });
            this.Id = (int)record[IdColumn];
            this.ParentRatingTypeId = (int)record[ParentRatingTypeIdColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(LanguageTitleCollection.TitlesColumn), this.Titles.GetSaveTable }
                });
        }
    }
}
