//-----------------------------------------------------------------------
// <copyright file="WatchType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Data;
    using System.Data.SqlClient;
    using Exceptions;

    /// <summary>Represents the way in which a <see cref="User"/> watched a <see cref="Movie"/>.</summary>
    public class WatchType
    {
        /// <summary>Initializes a new instance of the <see cref="WatchType" /> class.</summary>
        /// <param name="name">The name to set for the watch type.</param>
        public WatchType(string name)
        {
            this.Id = 0;
            this.Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="WatchType" /> class.</summary>
        /// <param name="record">The data record containing the data to create the watch type from.</param>
        public WatchType(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of this watch type.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the name of this watch type.</summary>
        public string Name { get; private set; }

        /// <summary>Saves this watch type.</summary>
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Updates a watch type from a record.</summary>
        /// <param name="type">The watch type to update.</param>
        /// <param name="record">The record containing the data for the watch type.</param>
        private static void ReadFromRecord(WatchType type, IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "Id", "Name" });
            type.Id = (int)record["Id"];
            type.Name = record["Name"].ToString();
        }

        /// <summary>Validates that the <paramref name="type"/> is valid to be saved.</summary>
        /// <param name="type">The watch type to validate.</param>
        private static void ValidateSaveCandidate(WatchType type)
        {
            if (string.IsNullOrEmpty(type.Name))
            {
                throw new InvalidSaveCandidateException("The name of the watch type cant be empty.");
            }
        }

        /// <summary>Saves a watch type to the database.</summary>
        /// <param name="type">The watch type to save.</param>
        private static void SaveToDatabase(WatchType type)
        {
            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("WatchTypeSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", type.Id);
                command.Parameters.AddWithValue("@Name", type.Name);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(type, reader);
                    }
                }
            }
        }
    }
}
