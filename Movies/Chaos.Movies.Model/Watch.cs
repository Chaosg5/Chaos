//-----------------------------------------------------------------------
// <copyright file="Watch.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    /// <summary>Represents an event where a <see cref="User"/> watched a <see cref="Movie"/>.</summary>
    public class Watch
    {
        #region Constructors

        public Watch(uint movieId, uint userId, DateTime watchDate, bool dateUncertain, uint watchLocationId, uint watchTypeId)
        {
            this.MovieId = movieId;
            this.UserId = userId;
            this.WatchDate = watchDate;
            this.DateUncertain = dateUncertain;
            this.WatchLocationId = watchLocationId;
            this.WatchTypeId = watchTypeId;
        }

        public Watch(IDataRecord record)
        {
            
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

        #endregion

        #endregion
    }
}
