//-----------------------------------------------------------------------
// <copyright file="PersonInRole.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a person in a movie.</summary>
    public class PersonInRole : Loadable<PersonInRole, PersonInRoleDto>
    {
       /// <summary>Private part of the <see cref="Person"/> property.</summary>
        private Person person;

        /// <summary>Private part of the <see cref="Role"/> property.</summary>
        private Role role;

        /// <summary>Private part of the <see cref="Department"/> property.</summary>
        private Department department;

        /// <summary>Initializes a new instance of the <see cref="PersonInRole"/> class.</summary>
        /// <param name="person">The person in the movie.</param>
        /// <param name="department">The department which the <paramref name="person"/> belongs to.</param>
        /// <param name="role">The role of the person in the <paramref name="department"/>.</param>
        public PersonInRole(Person person, Department department, Role role)
        {
            this.Person = person;
            this.Role = role;
            this.Department = department;
        }

        /// <summary>Prevents a default instance of the <see cref="PersonInRole"/> class from being created.</summary>
        private PersonInRole()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static PersonInRole Static { get; } = new PersonInRole();

        /// <summary>Gets the person.</summary>
        public Person Person
        {
            get => this.person;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(this.Person)} has to be saved.");
                }

                this.person = value;
            }
        }

        /// <summary>Gets the role of the <see cref="Person"/>.</summary>
        public Role Role
        {
            get => this.role;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(this.Role)} has to be saved.");
                }

                this.role = value;
            }
        }

        /// <summary>Gets the department of the <see cref="Person"/>.</summary>
        /// <remarks>The <see cref="Role"/> has to be set before this <see cref="Department"/>.</remarks>
        public Department Department
        {
            get => this.department;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(this.Department)} has to be saved.");
                }

                if (value.Roles.All(r => r.Id != this.Role.Id))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new RelationshipException($"The {nameof(this.Role)} needs to be a part of the {nameof(this.Department)}.");
                }

                this.department = value;
            }
        }

        /// <summary>Gets the user ratings.</summary>
        public UserSingleRating UserRatings { get; private set; } = new UserSingleRating();

        /// <summary>Gets the total rating score from all users.</summary>
        public double TotalRating { get; private set; }

        /// <inheritdoc />
        public override PersonInRoleDto ToContract()
        {
            return new PersonInRoleDto
            {
                Person = this.Person.ToContract(),
                Role = this.Role.ToContract(),
                Department = this.Department.ToContract(),
                UserRatings = this.UserRatings.ToContract(),
                TotalRating = this.TotalRating
            };
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="contract"/> is <see langword="null"/></exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        public override PersonInRole FromContract(PersonInRoleDto contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new PersonInRole
            {
                Person = Person.Static.FromContract(contract.Person),
                Role = Role.Static.FromContract(contract.Role),
                Department = Department.Static.FromContract(contract.Department),
                UserRatings = this.UserRatings.FromContract(contract.UserRatings),
                TotalRating = contract.TotalRating
            };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">The <see cref="PersonInRole"/> is not valid to be saved.</exception>
        internal override void ValidateSaveCandidate()
        {
            this.Person.ValidateSaveCandidate();
            this.Role.ValidateSaveCandidate();
            this.Department.ValidateSaveCandidate();
            this.UserRatings.ValidateSaveCandidate();
        }

        /// <summary>Creates new <see cref="PersonInRole"/>s from the <paramref name="reader"/>.</summary>
        /// <param name="reader">The reader containing data sets and records the data for the <see cref="PersonInRole"/>s.</param>
        /// <returns>The list of <see cref="PersonInRole"/>s.</returns>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        internal async Task<IEnumerable<PersonInRole>> ReadFromRecordsAsync(DbDataReader reader)
        {
            var personInRoles = new List<PersonInRole>();
            if (!reader.HasRows)
            {
                return personInRoles;
            }

            while (await reader.ReadAsync())
            {
                personInRoles.Add(await this.NewFromRecordAsync(reader));
            }

            return personInRoles;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<PersonInRole> NewFromRecordAsync(IDataRecord record)
        {
            var result = new PersonInRole();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override async Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { Person.IdColumn, Role.IdColumn, Department.IdColumn });
            this.Person = await GlobalCache.GetPersonAsync((int)record[Person.IdColumn]);
            this.Role = await GlobalCache.GetRoleAsync((int)record[Role.IdColumn]);
            this.Department = await GlobalCache.GetDepartmentAsync((int)record[Department.IdColumn]);
            this.TotalRating = (int)record[UserSingleRating.TotalRatingColumn];
        }
    }
}
