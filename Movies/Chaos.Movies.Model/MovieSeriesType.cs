//-----------------------------------------------------------------------
// <copyright file="MovieSeriesType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a type of a movie series.</summary>
    public class MovieSeriesType
    {
        /// <summary>Initializes a new instance of the <see cref="MovieSeriesType" /> class.</summary>
        public MovieSeriesType()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MovieSeriesType" /> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="MovieSeriesType"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public MovieSeriesType(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the id of the type.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the movie series type in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();

        /// <summary>Loads all movie series types from the database.</summary>
        /// <returns>All <see cref="MovieSeriesType"/>s.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This method performs a time-consuming operation.")]
        public static IEnumerable<MovieSeriesType> GetAll()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesTypesGetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    return ReadFromReader(reader);
                }
            }
        }

        /// <summary>Gets the specified <see cref="MovieSeriesType"/>s.</summary>
        /// <remarks>
        /// Uses stored procedure <c>MovieSeriesTypesGet</c>.
        /// Result 1 columns: MovieSeriesTypeId, Language, Title
        /// </remarks>
        /// <param name="idList">The list of ids of the <see cref="MovieSeriesType"/>s to get.</param>
        /// <returns>The specified <see cref="MovieSeriesType"/>s.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="idList"/> is <see langword="null" />.</exception>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public static IEnumerable<MovieSeriesType> Get(IEnumerable<int> idList)
        {
            if (idList == null || !idList.Any())
            {
                throw new ArgumentNullException(nameof(idList));
            }

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesTypesGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idList", idList);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    return ReadFromReader(reader);
                }
            }
        }

        /// <summary>Saves this movie series type to the database.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieSeriesType"/> is not valid to be saved.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public void Save()
        {
            this.ValidateSaveCandidate();
            this.SaveToDatabase();
        }

        /// <summary>Validates that this <see cref="MovieSeriesType"/> is valid to be saved.</summary>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="MovieSeriesType"/> is not valid to be saved.</exception>
        private void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        /// <summary>Saves this <see cref="MovieSeriesType"/> to the database.</summary>
        /// <remarks>
        /// Uses stored procedure <c>MovieSeriesTypeSave</c>.
        /// Result 1 columns: MovieSeriesTypeId
        /// Result 2 columns: Language, Title
        /// </remarks>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private void SaveToDatabase()
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesTypeSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@movieSeriesTypeId", this.Id);
                command.Parameters.AddWithValue("@titles", this.Titles.GetSaveTitles);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.ReadFromRecord(reader);
                    }

                    if (reader.NextResult())
                    {
                        this.Titles = new LanguageTitleCollection(reader);
                    }
                }
            }
        }

        /// <summary>Updates this <see cref="MovieSeriesType"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="MovieSeriesType"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "MovieSeriesTypeId" });
            this.Id = (int)record["MovieSeriesTypeId"];
        }

        /// <summary>Creates a list of <see cref="MovieSeriesType"/>s from a reader.</summary>
        /// <param name="reader">The reader containing the data for the <see cref="MovieSeriesType"/>s.</param>
        /// <returns>The list of <see cref="MovieSeriesType"/>s.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        private static IEnumerable<MovieSeriesType> ReadFromReader(SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                throw new MissingResultException(1, "");
            }

            var result = new List<MovieSeriesType>();
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

            return result;
        }
    }
}
