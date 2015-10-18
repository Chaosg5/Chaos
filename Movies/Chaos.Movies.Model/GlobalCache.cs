//-----------------------------------------------------------------------
// <copyright file="GlobalCache.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace Chaos.Movies.Model
{
    /// <summary>Provides a global cache of objects.</summary>
    public class GlobalCache
    {
        /// <summary>Private part of the <see cref="MovieSeriesTypes"/> property.</summary>
        private List<MovieSeriesType> movieSeriesTypes = new List<MovieSeriesType>();

        /// <summary>Initializes a new instance of the <see cref="GlobalCache"/> class.</summary>
        public GlobalCache()
        {
        }

        /// <summary>All avialable movie series types.</summary>
        public ReadOnlyCollection<MovieSeriesType> MovieSeriesTypes
        {
            get { return this.movieSeriesTypes.AsReadOnly(); }
        }

        /// <summary>Updates the current list of movie series types with the specified <paramref name="type"/>.</summary>
        /// <param name="type">The movie series type to update.</param>
        public void MovieSeriesTypesUpdate(MovieSeriesType type)
        {
            var existingType = this.movieSeriesTypes.Find(t => t.Id == type.Id);
            if (existingType != null)
            {
                existingType = type;
            }
            else
            {
                this.movieSeriesTypes.Add(type);
            }
        }

        /// <summary>Loads all movie series types from the database.</summary>
        public void MovieSeriesTypesLoad()
        {
            this.movieSeriesTypes.Clear();

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("MovieSeriesTypesGet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    MovieSeriesType type = null;
                    while (reader.Read())
                    {
                        var id = (uint)reader["MovieSeriesTypeId"];
                        if (type == null || type.Id != id)
                        {
                            type = this.movieSeriesTypes.Find(t => t.Id == id);
                            if (type == null)
                            {
                                type = new MovieSeriesType(reader);
                                this.movieSeriesTypes.Add(type);
                            }
                        }

                        type.Titles.SetTitle(new LanguageTitle(reader));
                    }
                }
            }
        }
    }
}
