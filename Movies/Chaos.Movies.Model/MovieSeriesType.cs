//-----------------------------------------------------------------------
// <copyright file="MovieSeriesType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using Chaos.Movies.Model.Exceptions;

namespace Chaos.Movies.Model
{
    /// <summary>Represents a type of a movie series.</summary>
    public class MovieSeriesType
    {
        /// <summary>Initializes a new instance of the <see cref="MovieSeriesType" /> class.</summary>
        public MovieSeriesType()
        {
            this.Titles = new LanguageTitles();
        }

        /// <summary>Initializes a new instance of the <see cref="MovieSeriesType" /> class.</summary>
        /// <param name="record">The record containing the data for the movie series type.</param>
        public MovieSeriesType(IDataRecord record)
        {
            this.Titles = new LanguageTitles();
            ReadFromRecord(this, record);
        }

        /// <summary>The id of the type.</summary>
        public int Id { get; private set; }

        /// <summary>The list of title of the movie series type in different languages.</summary>
        public LanguageTitles Titles { get; private set; }

        /// <summary>Saves this movie series type to the database.</summary>
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Validates that the <paramref name="type"/> is valid to be saved.</summary>
        /// <param name="type">The movie series type to validate.</param>
        private static void ValidateSaveCandidate(MovieSeriesType type)
        {
            if (type.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        /// <summary>Saves a movie series type to the database.</summary>
        /// <param name="type">The movie series type to save.</param>
        private static void SaveToDatabase(MovieSeriesType type)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesTypeSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieSeriesTypeId", type.Id));
                command.Parameters.Add(new SqlParameter("@titles", type.Titles.GetSaveTitles));
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

        /// <summary>Updates a movie series type from a record.</summary>
        /// <param name="type">The movie series type to update.</param>
        /// <param name="record">The record containing the data for the movie series type.</param>
        private static void ReadFromRecord(MovieSeriesType type, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "MovieSeriesTypeId" });
            type.Id = (int)record["MovieSeriesTypeId"];
        }
    }
}
