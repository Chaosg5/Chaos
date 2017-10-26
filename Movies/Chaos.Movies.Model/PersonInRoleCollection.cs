//-----------------------------------------------------------------------
// <copyright file="PersonInRoleCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents <see cref="Person"/>s in a movie.</summary>
    public class PersonInRoleCollection : IReadOnlyCollection<PersonInRole>
    {
        /// <summary>The list of <see cref="Person"/>s in this <see cref="PersonInRoleCollection"/>.</summary>
        private readonly List<PersonInRole> people = new List<PersonInRole>();

        /// <summary>Gets the id and type of the parent which this <see cref="PersonInRoleCollection"/> belongs to.</summary>
        private Parent parent;

        /// <summary>Initializes a new instance of the <see cref="PersonInRoleCollection"/> class.</summary>
        public PersonInRoleCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PersonInRoleCollection"/> class.</summary>
        /// <param name="parent">The parent which this <see cref="PersonInRoleCollection"/> belongs to.</param>
        public PersonInRoleCollection(Parent parent)
        {
            this.SetParent(parent);
        }

        /// <summary>Gets the number of elements contained in this <see cref="PersonInRoleCollection"/>.</summary>
        public int Count => this.people.Count;

        /// <summary>Returns an enumerator which iterates through this <see cref="PersonInRoleCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator which iterates through this <see cref="PersonInRoleCollection"/>.</summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<PersonInRole> GetEnumerator()
        {
            return this.people.GetEnumerator();
        }

        /// <summary>Sets the parent of this <see cref="PersonInRoleCollection"/>.</summary>
        /// <param name="newParent">The parent which this <see cref="PersonInRoleCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        public void SetParent(Parent newParent)
        {
            if (this.parent != null)
            {
                throw new ValueLogicalReadOnlyException("The parent can't be changed once set.");
            }

            this.parent = newParent;
        }

        /// <summary>Removes specified <paramref name="personInRole"/> item to the list.</summary>
        /// <param name="personInRole">The item to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="personInRole"/> is <see langword="null" />.</exception>
        public void AddPerson(PersonInRole personInRole)
        {
            if (personInRole == null)
            {
                throw new ArgumentNullException(nameof(personInRole));
            }

            if (this.people.Exists(p => p.Person.Id == personInRole.Person.Id && p.Department.Id == personInRole.Department.Id && p.Role.Id == personInRole.Role.Id))
            {
                return;
            }

            this.people.Add(personInRole);
        }

        /// <summary>Removes specified <paramref name="personInRole"/> item from the list and saves the change to the database.</summary>
        /// <param name="personInRole">The item to remove.</param>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void AddPersonAndSave(PersonInRole personInRole)
        {
            this.AddPerson(personInRole);
            if (this.parent == null)
            {
                throw new PersistentObjectRequiredException("The movie needs to be saved before adding people.");
            }

            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("PersonInMovieAdd", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@parentId", this.parent);
                command.Parameters.AddWithValue("@personId", personInRole.Person.Id);
                command.Parameters.AddWithValue("@departmentId", personInRole.Department.Id);
                command.Parameters.AddWithValue("@roleId", personInRole.Role.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Removes specified <paramref name="personInRole"/> item from the list.</summary>
        /// <param name="personInRole">The item to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="personInRole"/> is <see langword="null" />.</exception>
        /// <returns><see langword="true"/> if the <paramref name="personInRole"/> was found and removed; <see langword="false"/> if it was not found.</returns>
        public bool RemovePerson(PersonInRole personInRole)
        {
            if (personInRole == null)
            {
                throw new ArgumentNullException(nameof(personInRole));
            }

            if (!this.people.Contains(personInRole))
            {
                return false;
            }

            this.people.Remove(personInRole);
            return true;
        }

        /// <summary>Removes specified <paramref name="personInRole"/> item from the list and saves the change to the database.</summary>
        /// <param name="personInRole">The item to remove.</param>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void RemovePersonAndSave(PersonInRole personInRole)
        {
            if (!this.RemovePerson(personInRole))
            {
                return;
            }

            this.ValidateParent();
            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("PersonInMovieRemove", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@parentId", this.parent);
                command.Parameters.AddWithValue("@personId", personInRole.Person.Id);
                command.Parameters.AddWithValue("@departmentId", personInRole.Department.Id);
                command.Parameters.AddWithValue("@roleId", personInRole.Role.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Saves all <see cref="PersonInRole"/> to the database.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void Save()
        {
            this.ValidateParent();
            using (var table = new DataTable())
            {
                table.Locale = CultureInfo.InvariantCulture;
                table.Columns.Add(new DataColumn("PersonId"));
                table.Columns.Add(new DataColumn("DepartmentId"));
                table.Columns.Add(new DataColumn("RoleId"));
                foreach (var personInMovie in this.people)
                {
                    table.Rows.Add(personInMovie.Person.Id, personInMovie.Department.Id, personInMovie.Role.Id);
                }

                using (var connection = new SqlConnection(BlaBla.ConnectionString))
                using (var command = new SqlCommand("PeopleInMovieSave", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@parentId", this.parent);
                    command.Parameters.AddWithValue("@people", table);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Loads <see cref="Person"/>s for the current movie.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid id of a <see cref="Movie"/>.</exception>
        /// <exception cref="ArgumentNullException">If any parameter is null.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public void LoadPeople()
        {
            this.ValidateParent();
            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("PeopleInMovieGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@parentId", this.parent);
                connection.Open();

                var loadData = new List<PersonLoadShell>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loadData.Add(new PersonLoadShell(reader));
                    }
                }
                
                this.people.Clear();
                // ToDo:
                ////this.people.AddRange(
                ////    loadData.Select(
                ////        p =>
                ////            new PersonInMovie(
                ////                GlobalCache.GetPerson(p.PersonId),
                ////                GlobalCache.GetDepartment(p.DepartmentId),
                ////                GlobalCache.GetRole(p.RoleId))));
            }
        }

        /// <summary>Validates that the <see cref="parent"/> is set.</summary>
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="parent"/> is not a valid parent.</exception>
        private void ValidateParent()
        {
            if (this.parent == null)
            {
                throw new PersistentObjectRequiredException("The parent needs to be set.");
            }
        }

        /// <summary>Temporarily holds ids related to a <see cref="Person"/> in a <see cref="Movie"/> during loaded.</summary>
        private class PersonLoadShell
        {
            /// <summary>Initializes a new instance of the <see cref="PersonLoadShell" /> class.</summary>
            /// <param name="record">The record containing the data for the <see cref="PersonLoadShell"/>.</param>
            /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
            public PersonLoadShell(IDataRecord record)
            {
                ReadFromRecord(this, record);
            }

            /// <summary>Gets the id of the <see cref="Person"/>.</summary>
            public int PersonId { get; private set; }

            /// <summary>Gets the id of the <see cref="Person"/>'s <see cref="DepartmentId"/>.</summary>
            public int DepartmentId { get; private set; }

            /// <summary>Gets the id of the <see cref="Person"/>'s <see cref="RoleId"/>.</summary>
            public int RoleId { get; private set; }

            /// <summary>Updates a <see cref="PersonLoadShell"/> from a record.</summary>
            /// <param name="person">The <see cref="PersonLoadShell"/> to update.</param>
            /// <param name="record">The record containing the data for the <see cref="PersonLoadShell"/>.</param>
            /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
            private static void ReadFromRecord(PersonLoadShell person, IDataRecord record)
            {
                Helper.ValidateRecord(record, new[] { "PersonId", "DepartmentId", "RoleId" });
                person.PersonId = (int)record["PersonId"];
                person.DepartmentId = (int)record["DepartmentId"];
                person.RoleId = (int)record["RoleId"];
            }
        }
    }
}
