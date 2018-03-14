//-----------------------------------------------------------------------
// <copyright file="RoleCollection.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A list of <see cref="Role"/>s.</summary>
    public class RoleCollection : Listable<Role, RoleDto, RoleCollection>
    {
        /// <summary>The database column for this <see cref="RoleCollection"/>.</summary>
        internal const string RolesColumn = "Roles";

        /// <inheritdoc />
        public override DataTable GetSaveTable
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn(Role.IdColumn, typeof(int)));
                    foreach (var role in this.Items)
                    {
                        table.Rows.Add(role.Id);
                    }

                    return table;
                }
            }
        }

        /// <inheritdoc />
        public override ReadOnlyCollection<RoleDto> ToContract()
        {
            return new ReadOnlyCollection<RoleDto>(this.Items.Select(item => item.ToContract()).ToList());
        }

        /// <inheritdoc />
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        public override RoleCollection FromContract(ReadOnlyCollection<RoleDto> contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var list = new RoleCollection();
            foreach (var item in contract)
            {
                list.Add(Role.Static.FromContract(item));
            }

            return list;
        }
    }
}