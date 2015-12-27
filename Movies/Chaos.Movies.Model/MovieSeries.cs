//-----------------------------------------------------------------------
// <copyright file="MovieSeries.cs">
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
    using System.Globalization;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A series of movies.</summary>
    public class MovieSeries
    {
        /// <summary>Private part of the <see cref="Movies"/> property.</summary>
        private List<Movie> movies = new List<Movie>();

        /// <summary>Initializes a new instance of the <see cref="MovieSeries" /> class.</summary>
        public MovieSeries()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MovieSeries" /> class.</summary>
        /// <param name="record">The record containing the data for the movie series.</param>
        public MovieSeries(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the id of the movie series.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the type of the movie series.</summary>
        public MovieSeriesType MovieSeriesType { get; set; }

        /// <summary>Gets the list of title of the movie collection in different languages.</summary>
        public LanguageTitles Titles { get; } = new LanguageTitles();

        /// <summary>Gets the movies which are a part of this collection with the keys representing their order.</summary>
        public ReadOnlyCollection<Movie> Movies
        {
            get { return this.movies.AsReadOnly(); }
        }

        /// <summary>Gets the ids of the movies in this series to save.</summary>
        private DataTable GetSaveMovies
        {
            get
            {
                using (var table = new DataTable())
                {
                    table.Locale = CultureInfo.InvariantCulture;
                    table.Columns.Add(new DataColumn("MovieId"));
                    table.Columns.Add(new DataColumn("Order"));
                    for (var i = 0; i < this.movies.Count; i++)
                    {
                        table.Rows.Add(this.movies[i].Id, i + 1);
                    }

                    return table;
                }
            }
        }

        #region Methods

        #region Public

        /// <summary>Saves this movie series to the database.</summary>
        public void Save()
        {
            ValidateSaveCandidate(this);
            SaveToDatabase(this);
        }

        /// <summary>Saves this movie series and underlying objects to the database.</summary>
        public void SaveAll()
        {
            // ToDo:
        }

        /// <summary>Sets the order of the movies in this collection.</summary>
        /// <param name="newOrder">The order to set based on the indexes of the old order.</param>
        public void ReorderMovies(ICollection<int> newOrder)
        {
            this.movies = Helper.ReorderList(this.movies, newOrder).ToList();
        }

        /// <summary>Sets the order of the movies in this collection and saves the change.</summary>
        /// <param name="newOrder">The order to set based on the indexes of the old order.</param>
        public void ReorderMoviesAndSave(ICollection<int> newOrder)
        {
            this.ReorderMovies(newOrder);
            SaveMoviesToDatabase(this);
        }

        /// <summary>Adds the specified movie to this series.</summary>
        /// <param name="movie">The movie to add.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="movie"/> is null.</exception>
        /// <exception cref="PersistentObjectRequiredException">If the <paramref name="movie"/> hasn't been saved to the database.</exception>
        public void AddMovie(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            if (movie.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The movie needs to be persited before added to a series.");
            }

            if (this.movies.Exists(m => m.Id == movie.Id))
            {
                return;
            }

            this.movies.Add(movie);
        }

        /// <summary>Adds the specified movie to this series and saves the change.</summary>
        /// <param name="movie">The movie to add.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="movie"/> is null.</exception>
        /// <exception cref="PersistentObjectRequiredException">If the <paramref name="movie"/> hasn't been saved to the database.</exception>
        public void AddMovieAndSave(Movie movie)
        {
            this.AddMovie(movie);

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesAddMovie", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieSeriesId", this.Id));
                command.Parameters.Add(new SqlParameter("@movieId", movie.Id));
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        /// <summary>Removes the specified movie from this series.</summary>
        /// <param name="movie">The movie to remove.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="movie"/> is null.</exception>
        /// <exception cref="PersistentObjectRequiredException">If the <paramref name="movie"/> hasn't been saved to the database.</exception>
        public void RemoveMovie(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            if (movie.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The movie needs to be persited before removed from a series.");
            }

            this.movies.RemoveAll(m => m.Id == movie.Id);
        }

        /// <summary>Removes the specified movie from this series.</summary>
        /// <param name="movie">The movie to remove.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="movie"/> is null.</exception>
        /// <exception cref="PersistentObjectRequiredException">If the <paramref name="movie"/> hasn't been saved to the database.</exception>
        public void RemoveMovieAndSave(Movie movie)
        {
            this.RemoveMovie(movie);

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesRemoveMovie", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieSeriesId", this.Id));
                command.Parameters.Add(new SqlParameter("@movieId", movie.Id));
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Private

        /// <summary>Validates that the <paramref name="series"/> is valid to be saved.</summary>
        /// <param name="series">The movie series to validate.</param>
        private static void ValidateSaveCandidate(MovieSeries series)
        {
            if (series.Titles.Count == 0)
            {
                throw new InvalidSaveCandidateException("At least one title needs to be specified.");
            }

            if (series.MovieSeriesType == null || series.MovieSeriesType.Id <= 0)
            {
                throw new InvalidSaveCandidateException("A valid type needs to be specified.");
            }
        }

        /// <summary>Saves a movie series to the database.</summary>
        /// <param name="series">The movie series to save.</param>
        private static void SaveToDatabase(MovieSeries series)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesSave", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieSeriesId", series.Id));
                command.Parameters.Add(new SqlParameter("@movieSeriesTypeId", series.MovieSeriesType.Id));
                command.Parameters.Add(new SqlParameter("@titles", series.Titles.GetSaveTitles));
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ReadFromRecord(series, reader);
                    }
                }
            }
        }

        /// <summary>Saves the relations to the movies in this series.</summary>
        /// <param name="series">The movie series to save movies for.</param>
        private static void SaveMoviesToDatabase(MovieSeries series)
        {
            // ToDo: This requires a special SQL type
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesSaveMovies", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieSeriesId", series.Id));
                command.Parameters.Add(new SqlParameter("@movieIds", series.GetSaveMovies));
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        /// <summary>Updates a movie series from a record.</summary>
        /// <param name="series">The movie series to update.</param>
        /// <param name="record">The record containing the data for the movie series.</param>
        private static void ReadFromRecord(MovieSeries series, IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "MovieSeriesId" });
            series.Id = (int)record["MovieSeriesId"];
        }

        #endregion

        #endregion
    }
}
