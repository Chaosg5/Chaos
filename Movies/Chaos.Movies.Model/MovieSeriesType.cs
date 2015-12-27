//-----------------------------------------------------------------------
// <copyright file="MovieSeriesType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a type of a movie series.</summary>
    public class MovieSeriesType
    {
        /// <summary>Initializes a new instance of the <see cref="MovieSeriesType" /> class.</summary>
        public MovieSeriesType()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MovieSeriesType" /> class.</summary>
        /// <param name="record">The record containing the data for the movie series type.</param>
        public MovieSeriesType(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of the type.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the movie series type in different languages.</summary>
        public LanguageTitles Titles { get; private set; } = new LanguageTitles();
        
        /// <summary>Loads all movie series types from the database.</summary>
        public static IEnumerable<MovieSeriesType> GetAll()
        {
            var result = new List<MovieSeriesType>();
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesTypesGetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    MovieSeriesType type = null;
                    while (reader.Read())
                    {
                        var id = (int)reader["MovieSeriesTypeId"];
                        if (type == null || type.Id != id)
                        {
                            type = result.Find(t => t.Id == id);
                            if (type == null)
                            {
                                type = new MovieSeriesType(reader);
                                result.Add(type);
                            }
                        }

                        type.Titles.SetTitle(new LanguageTitle(reader));
                    }
                }
            }

            return result;
        }

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
        /// <remarks>
        /// Uses stored procedure <c>MovieSeriesTypeSave</c>.
        /// Result 1 columns: MovieSeriesTypeId
        /// Result 2 columns: Language, Title
        /// </remarks>
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

                    if (reader.NextResult())
                    {
                        type.Titles = new LanguageTitles(reader);   
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
