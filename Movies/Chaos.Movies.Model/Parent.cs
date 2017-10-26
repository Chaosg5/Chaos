//-----------------------------------------------------------------------
// <copyright file="Parent.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    /// <summary>A genre of <see cref="Movie"/>s.</summary>
    public class Parent
    {
        /// <summary>The parent id.</summary>
        private int parentId;

        /// <summary>Initializes a new instance of the <see cref="Parent"/> class.</summary>
        /// <param name="movie">The movie parent.</param>
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
        /// <param name="record">The data record containing the data to create the <see cref="Watch" /> from.</param>
        internal Parent(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets the type of the parent.</summary>
        public ParentType ParentType { get; private set; }

        /// <summary>Gets the id of the parent.</summary>
        public int ParentId
        {
            get => this.parentId;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The id of the parent has to be greater than zero.");
                }

                this.parentId = value;
            }
        }

        /// <summary>Updates a <see cref="Parent"/> from a record.</summary>
        /// <param name="parent">The <see cref="Parent"/> to update.</param>
        /// <param name="record">The record containing the data for the <see cref="Parent"/>.</param>
        private static void ReadFromRecord(Parent parent, IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "ParentId", "ParentType" });
            if (!Enum.TryParse((string)record["ParentType"], out ParentType parentType) || !Enum.IsDefined(typeof(ParentType), parentType))
            {
                throw new ArgumentOutOfRangeException(nameof(parent), $"The value '{parent}' is not a valid {nameof(ParentType)}.");
            }

            parent.ParentType = parentType;
            parent.ParentId = (int)record["ParentId"];
        }
    }
}