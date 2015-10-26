//-----------------------------------------------------------------------
// <copyright file="RatingType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using Exceptions;

    /// <summary>A sub rating with a defined type.</summary>
    public class RatingType
    {
        // ToDo: Split Save to Save and SaveAll

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
        /// <param name="record">The record containing the data for the rating type.</param>
        public RatingType(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of this rating type.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the name of this rating type.</summary>
        public string Name { get; private set; }

        /// <summary>Gets the description of this rating type.</summary>
        public string Description { get; private set; }

        /// <summary>Gets the <see cref="RatingType"/>s that makes up the derived children of this <see cref="RatingType"/>.</summary>
        public ReadOnlyCollection<RatingType> Subtypes
        {
            get { return this.subtypes.AsReadOnly(); }
        }

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
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Validates that the <paramref name="type"/> is valid to be saved.</summary>
        /// <param name="type">The rating type to validate.</param>
        private static void ValidateSaveCandidate(RatingType type)
        {
            if (string.IsNullOrEmpty(type.Name))
            {
                throw new InvalidSaveCandidateException("The 'Name' can not be empty.");
            }

            foreach (var subtype in type.subtypes)
            {
                ValidateSaveCandidate(subtype);
            }
        }

        /// <summary>Updates a rating type from a record.</summary>
        /// <param name="type">The rating type to update.</param>
        /// <param name="record">The record containing the data for the rating type.</param>
        private static void ReadFromRecord(RatingType type, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "RatingTypeId", "Name", "Description" });
            type.Id = (int)record["RatingTypeId"];
            type.Name = record["Name"].ToString();
            type.Description = record["Description"].ToString();
        }

        /// <summary>Saves a rating type to the database.</summary>
        /// <param name="type">The rating type to save.</param>
        private static void SaveToDatabase(RatingType type)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("RatingTypeSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RatingTypeId", type.Id));
                command.Parameters.Add(new SqlParameter("@Name", type.Name));
                command.Parameters.Add(new SqlParameter("@Description", type.Description));
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(type, reader);
                    }
                }
            }

            foreach (var subtype in type.subtypes)
            {
                SaveToDatabase(subtype);
            }
        }
    }
}
