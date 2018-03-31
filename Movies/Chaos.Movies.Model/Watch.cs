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
    /// <typeparam name="TParent">The type of the parent class.</typeparam>
    public class Watch : Readable<Watch, WatchDto>
    {
        /// <summary>Initializes a new instance of the <see cref="Watch{TParent}" /> class.</summary>
        /// <param name="userId">The id of the <see cref="User"/> who watched the <see cref="Movie"/>.</param>
        /// <param name="watchDate">The date when the movie was watched.</param>
        /// <param name="dateUncertain">If the <paramref name="watchDate"/> when the <see cref="Movie"/> was watched is just estimated and thus uncertain.</param>
        /// <param name="watchLocationId">The id of the <see cref="WatchLocation"/> where the <see cref="Movie"/> was watched.</param>
        /// <param name="watchTypeId">The id of the <see cref="WatchType"/> describing in what format the <see cref="Movie"/> was watched.</param>
        public Watch(int userId, DateTime watchDate, bool dateUncertain, int watchLocationId, int watchTypeId)
        {
            this.UserId = userId;
            this.WatchDate = watchDate;
            this.DateUncertain = dateUncertain;
            this.WatchLocationId = watchLocationId;
            this.WatchTypeId = watchTypeId;
        }

        private Watch()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Watch Static { get; } = new Watch();

        /// <summary>Gets the id of the watch.</summary>
        public int Id { get; private set; }

        public override Task SaveAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new NotImplementedException();
        }

        /// <summary>Gets the id of the <see cref="User"/> who watched <see cref="Movie"/>.</summary>
        public int UserId { get; private set; }

        /// <summary>Gets the user who watched the <see cref="Movie"/>.</summary>
        public User User { get; private set; }

        /// <summary>Gets or sets the date when the <see cref="Movie"/> was watched.</summary>
        public DateTime WatchDate { get; set; }

        /// <summary>Gets a value indicating whether the <see cref="WatchDate"/> is uncertain or not.</summary>
        public bool DateUncertain { get; private set; }

        /// <summary>Gets the id of the <see cref="WatchLocation"/> where the <see cref="Movie"/> was watched.</summary>
        public int WatchLocationId { get; private set; }

        /// <summary>Gets the  <see cref="WatchLocation"/> where the <see cref="Movie"/> was watched.</summary>
        public WatchLocation WatchLocation { get; private set; }

        /// <summary>Gets the id of the <see cref="WatchType"/> of how the <see cref="Movie"/> was watched.</summary>
        public int WatchTypeId { get; private set; }

        /// <summary>Gets the <see cref="WatchType"/> how the <see cref="Movie"/> was watched.</summary>
        public WatchType WatchType { get; private set; }


        /// <summary>Sets the user who watched the <see cref="Movie"/>.</summary>
        /// <param name="user">The user to set.</param>
        public void SetUser(User user)
        {
            if (user == null || user.Id == 0)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Id != this.UserId)
            {
                throw new UserChangeNotAllowedException();
            }

            this.UserId = user.Id;
            this.User = user;
        }

        /// <summary>Sets the location where the <see cref="Movie"/> was watched.</summary>
        /// <param name="location">The watch location to set.</param>
        public void SetWatchLocation(WatchLocation location)
        {
            if (location == null || location.Id == 0)
            {
                throw new ArgumentNullException(nameof(location));
            }

            this.WatchLocationId = location.Id;
            this.WatchLocation = location;
        }

        /// <summary>Sets the type of how the <see cref="Movie"/> was watched.</summary>
        /// <param name="type">The watch type to set.</param>
        public void SetWatchType(WatchType type)
        {
            if (type == null || type.Id == 0)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.WatchTypeId = type.Id;
            this.WatchType = type;
        }

        /// <summary>Updates this <see cref="Watch"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Watch"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "Id", "ParentId", "ParentType", "UserId", "WatchedDate", "DateUncertain" });
            this.Id = (int)record["Id"];
            this.UserId = (int)record["UserId"];

            DateTime watchDate;
            if (!DateTime.TryParse(record["WatchedDate"].ToString(), out watchDate))
            {
                throw new InvalidRecordValueException();
            }

            this.WatchDate = watchDate;

            bool dateUncertain;
            if (!bool.TryParse(record["DateUncertain"].ToString(), out dateUncertain))
            {
                throw new InvalidRecordValueException();
            }

            this.DateUncertain = dateUncertain;

            if (record["WatchLocationId"] != null)
            {
                this.WatchLocationId = (int)record["WatchLocationId"];
            }

            if (record["WatchTypeId"] != null)
            {
                this.WatchTypeId = (int)record["WatchTypeId"];
            }
        }

        public override WatchDto ToContract()
        {
            throw new NotImplementedException();
        }

        public override Watch FromContract(WatchDto contract)
        {
            throw new NotImplementedException();
        }

        internal override void ValidateSaveCandidate()
        {
            throw new NotImplementedException();
        }

        internal override Task<Watch> NewFromRecordAsync(IDataRecord record)
        {
            throw new NotImplementedException();
        }

        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            throw new NotImplementedException();
        }

        public override Task<Watch> GetAsync(UserSession session, int id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Watch>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new NotImplementedException();
        }

        internal override Task<IEnumerable<Watch>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
