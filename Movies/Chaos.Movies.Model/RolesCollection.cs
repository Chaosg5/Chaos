﻿//-----------------------------------------------------------------------
// <copyright file="RolesCollection.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;

    using Chaos.Movies.Contract;

    /// <summary>A list of <see cref="Role"/>s.</summary>
    public class RolesCollection : Listable<Role, RoleDto>, IListable<Role>, ICommunicable<RolesCollection, ReadOnlyCollection<RoleDto>>
    {
        /// <inheritdoc />
        public DataTable GetSaveTable { get; }
        
        /// <inheritdoc />
        public ReadOnlyCollection<RoleDto> ToContract()
        {
            return new ReadOnlyCollection<RoleDto>(this.Items.Select(item => item.ToContract()).ToList());
        }
    }
}