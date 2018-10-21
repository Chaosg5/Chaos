//-----------------------------------------------------------------------
// <copyright file="Person.cs">
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
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a person.</summary>
    public class Person : Rateable<Person, PersonDto>, ISearchable<Person>
    {
        // ToDo: Add Gender

        /// <summary>The database column for <see cref="Name"/>.</summary>
        private const string NameColumn = "Name";

        /// <summary>The database column for <see cref="BirthDate"/>.</summary>
        private const string BirthDateColumn = "BirthDate";

        /// <summary>The database column for <see cref="DeathDate"/>.</summary>
        private const string DeathDateColumn = "DeathDate";

        /// <summary>Private part of the <see cref="Name"/> property.</summary>
        private string name = string.Empty;

        /// <summary>Private part of the <see cref="BirthDate"/> property.</summary>
        private DateTime birthDate = SqlDateTime.MinValue.Value;

        /// <summary>Private part of the <see cref="DeathDate"/> property.</summary>
        private DateTime deathDate = SqlDateTime.MinValue.Value;

        /// <summary>Initializes a new instance of the <see cref="Person" /> class.</summary>
        /// <param name="name">The name of the person.</param>
        public Person(string name)
        {
            this.Name = name;
        }

        /// <summary>Prevents a default instance of the <see cref="Person"/> class from being created.</summary>
        private Person()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Person Static { get; } = new Person();

        /// <summary>Gets the name of the person.</summary>
        public string Name
        {
            get => this.name;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                this.name = value;
            }
        }

        /// <summary>Gets the year when the person was born.</summary>
        public DateTime BirthDate
        {
            get => this.birthDate;
            private set => this.birthDate = value < SqlDateTime.MinValue ? SqlDateTime.MinValue.Value : value;
        }

        /// <summary>Gets the year when the person died.</summary>
        public DateTime DeathDate
        {
            get => this.deathDate;
            private set => this.deathDate = value < SqlDateTime.MinValue ? SqlDateTime.MinValue.Value : value;
        }

        /// <summary>Gets the id of the <see cref="Character"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookups { get; private set; } = new ExternalLookupCollection();

        /// <summary>Gets the list of images for the <see cref="Person"/> and their order.</summary>
        public IconCollection Images { get; private set; } = new IconCollection();

        /// <summary>Gets the user ratings.</summary>
        public TotalRating UserRating { get; private set; } = new TotalRating(typeof(Person));

        /// <summary>Gets the total rating.</summary>
        public TotalRating TotalRating { get; private set; } = new TotalRating(typeof(Person));

        /// <summary>Saves a <see cref="UserSingleRating"/> for the <see cref="User"/> of the <paramref name="session"/> for the specified <paramref name="personId"/> and <paramref name="movieId"/>.</summary>
        /// <param name="personId">The id of the <see cref="Person"/> to rate.</param>
        /// <param name="roleId">The id of the <see cref="Role"/> for the <see cref="Person"/>.</param>
        /// <param name="departmentId">The id of the <see cref="Department"/> for the <see cref="Person"/>.</param>
        /// <param name="movieId">The id of the <see cref="Movie"/> to rate the <see cref="Person"/> in.</param>
        /// <param name="rating">The value of the <see cref="User"/>'s rating.</param>
        /// <param name="session">The <see cref="User"/>'s session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="PersistentObjectRequiredException">All items to save needs to be persisted.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="session"/> is <see langword="null"/></exception>
        public static async Task SaveUserRatingAsync(int personId, int roleId, int departmentId, int movieId, int rating, UserSession session)
        {
            if (personId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Person)} has to be saved.");
            }

            if (roleId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Role)} has to be saved.");
            }

            if (departmentId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Department)} has to be saved.");
            }

            if (movieId <= 0)
            {
                throw new PersistentObjectRequiredException($"The {nameof(Movie)} has to be saved.");
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (!Persistent.UseService)
            {
                var parameters = new ReadOnlyDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { Persistent.ColumnToVariable(IdColumn), personId },
                        { Persistent.ColumnToVariable(Role.IdColumn), roleId },
                        { Persistent.ColumnToVariable(Department.IdColumn), departmentId },
                        { Persistent.ColumnToVariable(Movie.IdColumn), movieId },
                        { Persistent.ColumnToVariable(User.IdColumn), session.UserId },
                        { Persistent.ColumnToVariable(UserSingleRating.RatingColumn), rating }
                    });
                await Person.CustomDatabaseActionAsync(parameters, UserSingleRating.UserRatingSaveProcedure, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                ////await service.PersonSearchAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        public override PersonDto ToContract()
        {
            return new PersonDto
            {
                Id = this.Id,
                Name = this.Name,
                BirthDate = this.BirthDate,
                DeathDate = this.DeathDate,
                ExternalLookups = this.ExternalLookups.ToContract(),
                Images = this.Images.ToContract(),
                UserRating = this.UserRating.ToContract(),
                TotalRating = this.TotalRating.ToContract()
            };
        }

        /// <inheritdoc />
        public override PersonDto ToContract(string languageName)
        {
            return new PersonDto
            {
                Id = this.Id,
                Name = this.Name,
                BirthDate = this.BirthDate,
                DeathDate = this.DeathDate,
                ExternalLookups = this.ExternalLookups.ToContract(languageName),
                Images = this.Images.ToContract(languageName),
                UserRating = this.UserRating.ToContract(languageName),
                TotalRating = this.TotalRating.ToContract(languageName)
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override Person FromContract(PersonDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new Person
            {
                Id = contract.Id,
                Name = contract.Name,
                BirthDate = contract.BirthDate,
                DeathDate = contract.DeathDate,
                ExternalLookups = this.ExternalLookups.FromContract(contract.ExternalLookups),
                Images = this.Images.FromContract(contract.Images),
                UserRating = this.UserRating.FromContract(contract.UserRating),
                TotalRating = this.TotalRating.FromContract(contract.TotalRating)
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Person"/> is not valid to be saved.</exception>
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
                await service.PersonSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<Person> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task<IEnumerable<Person>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            if (!Persistent.UseService)
            {
                return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return (await service.PersonGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/></exception>
        public override async Task GetUserRatingsAsync(IEnumerable<PersonDto> items, UserSession session)
        {
            if (!Persistent.UseService)
            {
                var itemList = items?.ToList();
                await this.GetUserRatingsFromDatabaseAsync(itemList, itemList?.Select(i => i.Id), this.ReadUserRatingsAsync, session);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                //return (await service.MovieGetAsync(session.ToContract(), idList.ToList())).Select(this.FromContract);
            }
        }

        public override Task GetUserItemDetailsAsync(PersonDto item, UserSession session, string languageName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public async Task<IEnumerable<Person>> SearchAsync(SearchParametersDto parametersDto, UserSession session)
        {
            if (!Persistent.UseService)
            {
                var items = new List<Person>();
                foreach (var id in await this.SearchDatabaseAsync(parametersDto, session))
                {
                    items.Add(await GlobalCache.GetPersonAsync(id));
                }

                return items;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                return new List<Person>();
                ////await service.CharacterSearchAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <summary>Validates that this <see cref="Person"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Person"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException($"The {nameof(this.Name)} can not be empty.");
            }
        }

        /// <inheritdoc />
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        internal override async Task<IEnumerable<Person>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var people = new List<Person>();
            if (!reader.HasRows)
            {
                return people;
            }

            while (await reader.ReadAsync())
            {
                people.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(2, $"{nameof(Person)}{IconCollection.IconsColumn}");
            }

            while (await reader.ReadAsync())
            {
                var person = (Person)this.GetFromResultsByIdInRecord(people, reader, IdColumn);
                person.Images.Add(await Icon.Static.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(3, $"{nameof(Person)}{ExternalLookupCollection.ExternalLookupColumn}");
            }

            while (await reader.ReadAsync())
            {
                var person = (Person)this.GetFromResultsByIdInRecord(people, reader, IdColumn);
                person.ExternalLookups.Add(await ExternalLookup.Static.NewFromRecordAsync(reader));
            }

            return people;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<Person> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Person();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.MissingColumnException">A required column is missing in the <paramref name="record" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="record" /> is <see langword="null" />.</exception>
        protected override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn, NameColumn, BirthDateColumn, DeathDateColumn });
            this.Id = (int)record[IdColumn];
            this.Name = (string)record[NameColumn];
            if (!DBNull.Value.Equals(record[BirthDateColumn]))
            {
                this.BirthDate = (DateTime)record[BirthDateColumn];
            }

            if (!DBNull.Value.Equals(record[DeathDateColumn]))
            {
                this.DeathDate = (DateTime)record[DeathDateColumn];
            }

            this.TotalRating = await this.TotalRating.NewFromRecordAsync(record);
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(NameColumn), this.Name },
                    { Persistent.ColumnToVariable(BirthDateColumn), this.BirthDate },
                    { Persistent.ColumnToVariable(DeathDateColumn), this.DeathDate },
                    { Persistent.ColumnToVariable(ExternalLookupCollection.ExternalLookupColumn), this.ExternalLookups.GetSaveTable },
                    { Persistent.ColumnToVariable(IconCollection.IconsColumn), this.Images.GetSaveTable }
                });
        }

        /// <inheritdoc />
        protected override async Task ReadUserRatingsAsync(IEnumerable<PersonDto> items, int userId, DbDataReader reader)
        {
            var ratings = new Dictionary<int, double>();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    ratings.Add((int)reader[IdColumn], (double)reader[UserSingleRating.RatingColumn]);
                }
            }

            foreach (var person in items)
            {
                double rating;
                if (!ratings.TryGetValue(person.Id, out rating))
                {
                    rating = 0;
                }
                
                // ToDo:
                //UserSingleRating.SetUserRating(person.UserRating, userId, rating);
            }
        }

        protected override Task ReadUserDetailsAsync(PersonDto item, int userId, DbDataReader reader, string languageName)
        {
            throw new NotImplementedException();
        }
    }
}
