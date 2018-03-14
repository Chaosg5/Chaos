//-----------------------------------------------------------------------
// <copyright file="User.cs">
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

    /// <summary>A user.</summary>
    public class User : Readable<User, UserDto>
    {
        public User(string userName, string name)
        {
            
        }

        public override Task SaveAsync(UserSession session)
        {
            throw new System.NotImplementedException();
        }

        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Gets the username of the <see cref="User"/>.</summary>
        public string UserName { get; private set; }

        /// <summary>Gets the name of the <see cref="User"/>.</summary>
        public string Name { get; private set; }

        public override UserDto ToContract()
        {
            throw new System.NotImplementedException();
        }

        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override User FromContract(UserDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            throw new System.NotImplementedException();
        }

        public override Task<User> GetAsync(UserSession session, int id)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<User>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new System.NotImplementedException();
        }

        internal override Task<User> ReadFromRecordAsync(IDataRecord record)
        {
            throw new System.NotImplementedException();
        }

        internal override void ValidateSaveCandidate()
        {
            throw new System.NotImplementedException();
        }

        internal override Task<IEnumerable<User>> ReadFromRecordsAsync(DbDataReader reader)
        {
            throw new System.NotImplementedException();
        }
    }
}
