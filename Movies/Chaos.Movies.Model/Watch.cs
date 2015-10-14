//-----------------------------------------------------------------------
// <copyright file="Watch.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using Exceptions;

    /// <summary>Represents an event where a <see cref="User"/> watched a <see cref="Movie"/>.</summary>
    public class Watch
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Watch" /> class.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> that was watched.</param>
        /// <param name="userId">The id of the <see cref="User"/> who watched the <see cref="Movie"/>.</param>
        /// <param name="watchDate">The date when the movie was watched.</param>
        /// <param name="dateUncertain">If the <paramref name="watchDate"/> when the <see cref="Movie"/> was watched is just estimated and thus uncertain.</param>
        /// <param name="watchLocationId">The id of the <see cref="WatchLocation"/> where the <see cref="Movie"/> was watched.</param>
        /// <param name="watchTypeId">The id of the <see cref="WatchType"/> describing in what format the <see cref="Movie"/> was watched.</param>
        public Watch(uint movieId, uint userId, DateTime watchDate, bool dateUncertain, uint watchLocationId, uint watchTypeId)
        {
            this.MovieId = movieId;
            this.UserId = userId;
            this.WatchDate = watchDate;
            this.DateUncertain = dateUncertain;
            this.WatchLocationId = watchLocationId;
            this.WatchTypeId = watchTypeId;
        }

        /// <summary>Initializes a new instance of the <see cref="Watch" /> class.</summary>
        /// <param name="record">The data record containing the data to create the watch type from.</param>
        public Watch(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        #endregion

        #region Properties

        /// <summary>Gets the id of the watch.</summary>
        public uint Id { get; private set; }

        /// <summary>Gets the id of the <see cref="Movie"/> watched.</summary>
        public uint MovieId { get; private set; }

        /// <summary>Gets the id of the <see cref="User"/> who watched <see cref="Movie"/>.</summary>
        public uint UserId { get; private set; }

        /// <summary>Gets the user who watched the <see cref="Movie"/>.</summary>
        public User User { get; private set; }

        /// <summary>Gets the date when the <see cref="Movie"/> was watched.</summary>
        public DateTime WatchDate { get; private set; }

        /// <summary>Gets a value indication whether the <see cref="WatchDate"/> is uncertain or not.</summary>
        public bool DateUncertain { get; private set; }

        /// <summary>Gets the id of the <see cref="WatchLocation"/> where the <see cref="Movie"/> was watched.</summary>
        public uint WatchLocationId { get; private set; }

        /// <summary>Gets the  <see cref="WatchLocation"/> where the <see cref="Movie"/> was watched.</summary>
        public WatchLocation WatchLocation { get; private set; }

        /// <summary>Gets the id of the <see cref="WatchType"/> of how the <see cref="Movie"/> was watched.</summary>
        public uint WatchTypeId { get; private set; }

        /// <summary>Gets the <see cref="WatchType"/> how the <see cref="Movie"/> was watched.</summary>
        public WatchType WatchType { get; private set; }

        #endregion

        #region Methods

        #region Public

        /// <summary>Sets the user who watched the <see cref="Movie"/>.</summary>
        /// <param name="user">The user to set.</param>
        public void SetUser(User user)
        {
            if (user == null || user.Id == 0)
            {
                throw new ArgumentNullException("user");
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
                throw new ArgumentNullException("location");
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
                throw new ArgumentNullException("type");
            }

            this.WatchTypeId = type.Id;
            this.WatchType = type;
        }

        #endregion

        #region Private

        /// <summary>Updates a watch from a record.</summary>
        /// <param name="watch">The watch to update.</param>
        /// <param name="record">The record containing the data for the watch.</param>
        private static void ReadFromRecord(Watch watch, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "Id", "MovieId", "UserId", "WatchedDate", "DateUncertain" });
            watch.Id = (uint)record["Id"];
            watch.MovieId = (uint)record["MovieId"];
            watch.UserId = (uint)record["UserId"];

            DateTime watchDate;
            if (!DateTime.TryParse(record["WatchedDate"].ToString(), out watchDate))
            {
                throw new InvalidRecordValueException();
            }

            watch.WatchDate = watchDate;

            bool dateUncertain;
            if (!bool.TryParse(record["DateUncertain"].ToString(), out dateUncertain))
            {
                throw new InvalidRecordValueException();
            }

            watch.DateUncertain = dateUncertain;

            if (record["WatchLocationId"] != null)
            {
                watch.WatchLocationId = (uint)record["WatchLocationId"];
            }

            if (record["WatchTypeId"] != null)
            {
                watch.WatchTypeId = (uint)record["WatchTypeId"];
            }
        }

        #endregion

        #endregion
    }
}
