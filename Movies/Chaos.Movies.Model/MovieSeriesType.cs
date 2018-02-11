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
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a type of a movie series.</summary>
    public class MovieSeriesType : Typeable<MovieSeriesType, MovieSeriesTypeDto>, ITypeable<MovieSeriesType, MovieSeriesTypeDto>
    {
        /// <summary>Initializes a new instance of the <see cref="MovieSeriesType" /> class.</summary>
        public MovieSeriesType()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static MovieSeriesType Static { get; } = new MovieSeriesType();

        /// <summary>Gets the id of the type.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the list of titles of the movie series type in different languages.</summary>
        public LanguageTitleCollection Titles { get; private set; } = new LanguageTitleCollection();
       
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
        public override void ValidateSaveCandidate()
        {
            if (this.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }
        }

        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            throw new NotImplementedException();
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
                command.Parameters.AddWithValue("@titles", this.Titles.GetSaveTable);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                      ////  await this.ReadFromRecordAsync(reader);
                    }

                    if (reader.NextResult())
                    {
                        this.Titles = new LanguageTitleCollection(reader);
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <exception cref="T:Chaos.Movies.Model.Exceptions.MissingColumnException">A required column is missing in the <paramref name="record" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="record" /> is <see langword="null" />.</exception>
        public override Task<MovieSeriesType> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "MovieSeriesTypeId" });
            return Task.FromResult(new MovieSeriesType { Id = (int)record["MovieSeriesTypeId"] });
        }

        /// <summary>Creates a list of <see cref="MovieSeriesType"/>s from a reader.</summary>
        /// <param name="reader">The reader containing the data for the <see cref="MovieSeriesType"/>s.</param>
        /// <returns>The list of <see cref="MovieSeriesType"/>s.</returns>
        /// <exception cref="MissingResultException">A required result is missing from the database.</exception>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        protected override async Task<IEnumerable<MovieSeriesType>> ReadFromRecordsAsync(DbDataReader reader)
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
                        type = await this.ReadFromRecordAsync(reader);
                        result.Add(type);
                    }
                }

                type.Titles.SetTitle(new LanguageTitle(reader));
            }

            return result;
        }

        public MovieSeriesTypeDto ToContract()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        public async Task<MovieSeriesType> GetAsync(UserSession session, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieSeriesType>> GetAsync(UserSession session, IEnumerable<int> idList)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAllAsync(UserSession session)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieSeriesType>> GetAllAsync(UserSession session)
        {
            throw new NotImplementedException();
        }
    }
}
