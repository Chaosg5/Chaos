//-----------------------------------------------------------------------
// <copyright file="RatingType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using Exceptions;

    /// <summary>A sub rating with a defined type.</summary>
    public class RatingType
    {
        /// <summary>The sub types of this rating type.</summary>
        private readonly List<RatingType> subtypes = new List<RatingType>();

        /// <summary>Initializes a new instance of the <see cref="RatingType" /> class.</summary>
        /// <param name="id">The id of the type.</param>
        public RatingType(int id)
        {
            this.Id = id;
        }

        /// <summary>Initializes a new instance of the <see cref="RatingType" /> class.</summary>
        /// <param name="id">The id of the type.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="description">The description of the type.</param>
        public RatingType(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>Initializes a new instance of the <see cref="RatingType" /> class.</summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="description">The description of the type.</param>
        public RatingType(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>Initializes a new instance of the <see cref="RatingType" /> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="RatingType"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public RatingType(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the id of this rating type.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the name of this rating type.</summary>
        public string Name { get; private set; }

        /// <summary>Gets the description of this rating type.</summary>
        public string Description { get; private set; }

        /// <summary>Gets the <see cref="RatingType"/>s that makes up the derived children of this <see cref="RatingType"/>.</summary>
        public ReadOnlyCollection<RatingType> Subtypes => this.subtypes.AsReadOnly();

        /// <summary>Adds a sub <see cref="RatingType"/> to this type.</summary>
        /// <param name="subtype">The subtype to add.</param>
        public void AddSubtype(RatingType subtype)
        {
            if (subtype != null)
            {
                this.subtypes.Add(subtype);
            }
        }

        /// <summary>Saves this rating type to the database.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="RatingType"/> is not valid to be saved.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public void Save()
        {
            this.ValidateSaveCandidate();
            this.SaveToDatabase();
        }

        /// <summary>Validates that this <see cref="RatingType"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="RatingType"/> is not valid to be saved.</exception>
        private void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidSaveCandidateException("The 'Name' can not be empty.");
            }

            foreach (var subtype in this.subtypes)
            {
                subtype.ValidateSaveCandidate();
            }
        }

        /// <summary>Updates this <see cref="RatingType"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="RatingType"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "RatingTypeId", "Name", "Description" });
            this.Id = (int)record["RatingTypeId"];
            this.Name = record["Name"].ToString();
            this.Description = record["Description"].ToString();
        }

        /// <summary>Saves this <see cref="RatingType"/> to the database.</summary>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private void SaveToDatabase()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("RatingTypeSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RatingTypeId", this.Id);
                command.Parameters.AddWithValue("@Name", this.Name);
                command.Parameters.AddWithValue("@Description", this.Description);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.ReadFromRecord(reader);
                    }
                }
            }

            foreach (var subtype in this.subtypes)
            {
                subtype.SaveToDatabase();
            }
        }
    }
}
