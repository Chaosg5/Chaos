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
    public class Person : Readable<Person, PersonDto>, ISearchable<Person>
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
        public UserSingleRating Ratings { get; private set; } = new UserSingleRating();

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
                Ratings = this.Ratings.ToContract()
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
                Ratings = this.Ratings.FromContract(contract.Ratings)
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
                throw new MissingResultException(1, $"{nameof(Person)}s");
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
            this.BirthDate = (DateTime)record[BirthDateColumn];
            this.DeathDate = (DateTime)record[DeathDateColumn];
            this.Ratings = await this.Ratings.NewFromRecordAsync(record);
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
    }
}
