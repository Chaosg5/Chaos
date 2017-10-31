//-----------------------------------------------------------------------
// <copyright file="Parent.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>A genre of <see cref="Movie"/>s.</summary>
    internal class Parent
    {
        /// <summary>The parent id.</summary>
        private int parentId;

        /// <summary>Initializes a new instance of the <see cref="Parent"/> class.</summary>
        /// <param name="movie">The movie parent.</param>
        /// <exception cref="PersistentObjectRequiredException">The parent needs to be saved.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="movie"/> is <see langword="null"/></exception>
        public Parent(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            this.ParentId = movie.Id;
            this.ParentType = ParentType.Movie;
        }

        /// <summary>Initializes a new instance of the <see cref="Parent"/> class.</summary>
        /// <param name="movieSeries">The movie series parent.</param>
        /// <exception cref="PersistentObjectRequiredException">The parent needs to be saved.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="movieSeries"/> is <see langword="null"/></exception>
        public Parent(MovieSeries movieSeries)
        {
            if (movieSeries == null)
            {
                throw new ArgumentNullException(nameof(movieSeries));
            }

            this.ParentId = movieSeries.Id;
            this.ParentType = ParentType.MovieSeries;
        }

        /// <summary>Initializes a new instance of the <see cref="Parent"/> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="Parent"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal Parent(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the type of the parent.</summary>
        public ParentType ParentType { get; private set; }

        /// <summary>The <see cref="ParentType"/> as a variable name.</summary>
        public string VariableName => char.ToLowerInvariant(ParentType.ToString()[0]) + ParentType.ToString().Substring(1);

        /// <summary>Gets the id of the parent.</summary>
        /// <exception cref="PersistentObjectRequiredException" accessor="set">The parent needs to be saved.</exception>
        public int ParentId
        {
            get => this.parentId;

            private set
            {
                if (value <= 0)
                {
                    throw new PersistentObjectRequiredException("The parent needs to be saved.");
                }

                this.parentId = value;
            }
        }

        /// <summary>Updates this <see cref="Parent"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Parent"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "ParentType" });
            if (!Enum.TryParse((string)record["ParentType"], out ParentType parentType) || !Enum.IsDefined(typeof(ParentType), parentType))
            {
                // ReSharper disable once ExceptionNotDocumented - This should not occur, unless database is out of sync with the application and documentation is not needed
                throw new ArgumentOutOfRangeException(nameof(ParentType), $"The value '{(string)record["ParentType"]}' is not a valid {nameof(ParentType)}.");
            }

            Helper.ValidateRecord(record, new[] { $"{parentType}Id" });
            this.ParentType = parentType;
            // ReSharper disable once ExceptionNotDocumented - This should not occur since all id columns are identity(1) and documentation is not needed
            this.ParentId = (int)record[$"{parentType}Id"];
        }
    }
}