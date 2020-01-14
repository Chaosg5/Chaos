//-----------------------------------------------------------------------
// <copyright file="Address.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <inheritdoc cref="Readable{T, TDto}" />
    /// <summary>An address.</summary>
    public class Address : Readable<Address, Address>, IReadableExtension<Address, Address>, ISearchable<Address>
    {
        /// <summary>The database column for <see cref="Street"/>.</summary>
        private const string StreetColumn = "Street";

        /// <summary>The database column for <see cref="Apartment"/>.</summary>
        private const string ApartmentColumn = "Apartment";

        /// <summary>The database column for <see cref="PostalCode"/>.</summary>
        private const string PostalCodeColumn = "PostalCode";

        /// <summary>The database column for <see cref="City"/>.</summary>
        private const string CityColumn = "City";

        /// <summary>The database column for <see cref="Country"/>.</summary>
        private const string CountryColumn = "Country";

        /// <summary>The database column for <see cref="LookupId"/>.</summary>
        private const string LookupIdColumn = "LookupId";

        /// <summary>Prevents a default instance of the <see cref="Address"/> class from being created.</summary>
        private Address()
        {
            this.SchemaName = "wed";
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Address Static { get; } = new Address();
        
        /// <summary>Gets the street.</summary>
        public string Street { get; private set; }

        /// <summary>Gets the apartment.</summary>
        public string Apartment { get; private set; }

        /// <summary>Gets the postal code.</summary>
        public string PostalCode { get; private set; }

        /// <summary>Gets the city.</summary>
        public string City { get; private set; }

        /// <summary>Gets the country.</summary>
        public string Country { get; private set; }

        /// <summary>Gets the lookup id.</summary>
        public Guid LookupId { get; private set; }

        /// <summary>Gets the guests.</summary>
        public List<Guest> Guests { get; private set; } = new List<Guest>();

        /// <inheritdoc/>
        public override Address ToContract()
        {
            return this;
        }

        /// <inheritdoc/>
        public override Address ToContract(string languageName)
        {
            return this;
        }

        /// <inheritdoc/>
        public override Address FromContract(Address contract)
        {
            return this;
        }

        /// <inheritdoc/>
        public override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc/>
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.MissingColumnException">A required column is missing in the record.</exception>
        public override async Task<Address> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Address();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.MissingColumnException">A required column is missing in the record.</exception>
        public override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { IdColumn });
            this.Id = (int)record[IdColumn];
            this.Street = (string)record[StreetColumn];
            this.Apartment = (string)record[ApartmentColumn];
            this.PostalCode = (string)record[PostalCodeColumn];
            this.City = (string)record[CityColumn];
            this.Country = (string)record[CountryColumn];
            this.LookupId = (Guid)record[LookupIdColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override Task SaveAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        /// <exception cref="T:System.Exception">A delegate callback throws an exception.</exception>
        public override async Task<Address> GetAsync(UserSession session, int id)
        {
            return (await this.GetAsync(session, new[] { id })).First();
        }

        /// <inheritdoc/>
        /// <exception cref="T:System.Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<Address> GetAsync(UserSession session, Guid lookupId)
        {
            return (await this.GetFromDatabaseAsync(new List<Guid> { lookupId }, this.ReadFromRecordsAsync, session)).First();
        }

        /// <inheritdoc/>
        /// <exception cref="T:System.Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public override async Task<IEnumerable<Address>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            return await this.GetFromDatabaseAsync(idList, this.ReadFromRecordsAsync, session);
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="PersistentObjectRequiredException">All items to get needs to be persisted.</exception>
        public async Task<IEnumerable<Address>> SearchAsync(SearchParametersDto parametersDto, UserSession session)
        {
            // ToDo: Change SearchDatabase to return the real results instead
            var results = (await this.SearchDatabaseAsync(parametersDto, session)).ToList();
            if (results.Any())
            {
                return await this.GetAsync(session, results);
            }

            return new List<Address>();
        }

        /// <inheritdoc/>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="SqlResultSyncException">Two or more of the SQL results are out of sync with each other.</exception>
        /// <exception cref="InvalidRecordValueException">A required record is missing from the database.</exception>
        public override async Task<IEnumerable<Address>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var addresses = new List<Address>();
            if (!reader.HasRows)
            {
                return addresses;
            }

            while (await reader.ReadAsync())
            {
                addresses.Add(await this.NewFromRecordAsync(reader));
            }

            if (!await reader.NextResultAsync() || !reader.HasRows)
            {
                throw new MissingResultException(2, $"{nameof(Address)}{Guest.IdColumn}");
            }

            var guestAddresses = new List<KeyValuePair<int, int>>();
            while (await reader.ReadAsync())
            {
                guestAddresses.Add(new KeyValuePair<int, int>((int)reader[IdColumn], (int)reader[Guest.IdColumn]));
            }
            
            if (!await reader.NextResultAsync())
            {
                throw new MissingResultException(3, $"{nameof(Address)}{Guest.IdColumn}");
            }

            var guests = (await Guest.Static.ReadFromRecordsAsync(reader)).ToList();
            foreach (var guestAddress in guestAddresses)
            {
                var address = addresses.FirstOrDefault(a => a.Id == guestAddress.Key);
                var guest = guests.FirstOrDefault(g => g.Id == guestAddress.Value);
                if (address == null)
                {
                    throw new InvalidRecordValueException($"Missing address with id {guestAddress.Key}");
                }

                if (guest == null)
                {
                    throw new InvalidRecordValueException($"Missing guest with id {guestAddress.Value}");
                }

                address.Guests.Add(guest);
            }

            return addresses;
        }

        /// <inheritdoc/>
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new NotImplementedException();
        }
    }
}