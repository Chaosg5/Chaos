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
        /// <summary>Gets the id and type of the parent which this <see cref="Watch"/> belongs to.</summary>
        private Parent parent;

        /// <summary>Initializes a new instance of the <see cref="Watch" /> class.</summary>
        /// <param name="parent">The parent which this <see cref="Watch"/> belongs to.</param>
        /// <param name="userId">The id of the <see cref="User"/> who watched the <see cref="Movie"/>.</param>
        /// <param name="watchDate">The date when the movie was watched.</param>
        /// <param name="dateUncertain">If the <paramref name="watchDate"/> when the <see cref="Movie"/> was watched is just estimated and thus uncertain.</param>
        /// <param name="watchLocationId">The id of the <see cref="WatchLocation"/> where the <see cref="Movie"/> was watched.</param>
        /// <param name="watchTypeId">The id of the <see cref="WatchType"/> describing in what format the <see cref="Movie"/> was watched.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        public Watch(Parent parent, int userId, DateTime watchDate, bool dateUncertain, int watchLocationId, int watchTypeId)
        {
            this.SetParent(parent);
            this.UserId = userId;
            this.WatchDate = watchDate;
            this.DateUncertain = dateUncertain;
            this.WatchLocationId = watchLocationId;
            this.WatchTypeId = watchTypeId;
        }

        /// <summary>Initializes a new instance of the <see cref="Watch" /> class.</summary>
        /// <param name="record">The data record containing the data to create the <see cref="Watch" /> from.</param>
        public Watch(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of the watch.</summary>
        public int Id { get; private set; }
        
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
        
        /// <summary>Sets the parent of this <see cref="PersonAsCharacterCollection"/>.</summary>
        /// <param name="newParent">The parent which this <see cref="PersonAsCharacterCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        public void SetParent(Parent newParent)
        {
            if (this.parent != null)
            {
                throw new ValueLogicalReadOnlyException("The parent can't be changed once set.");
            }

            this.parent = newParent;
        }

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

        /// <summary>Updates a watch from a record.</summary>
        /// <param name="watch">The watch to update.</param>
        /// <param name="record">The record containing the data for the watch.</param>
        private static void ReadFromRecord(Watch watch, IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "Id", "ParentId", "ParentType", "UserId", "WatchedDate", "DateUncertain" });
            watch.Id = (int)record["Id"];
            watch.SetParent(new Parent(record));
            watch.UserId = (int)record["UserId"];

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
                watch.WatchLocationId = (int)record["WatchLocationId"];
            }

            if (record["WatchTypeId"] != null)
            {
                watch.WatchTypeId = (int)record["WatchTypeId"];
            }
        }
    }
}
