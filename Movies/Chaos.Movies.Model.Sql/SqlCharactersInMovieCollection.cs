//-----------------------------------------------------------------------
// <copyright file="SqlCharactersInMovieCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Sql
{
    using Chaos.Movies.Model;

    /// <summary>SQL logic and database communication for a <see cref="CharactersInMovieCollection"/>.</summary>
    public class CharactersInMovieCollection : Model.CharactersInMovieCollection
    {
        /// <summary>Initializes a new instance of the <see cref="CharactersInMovieCollection"/> class.</summary>
        public CharactersInMovieCollection()
            : base()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CharactersInMovieCollection"/> class.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="CharactersInMovieCollection"/> belongs to.</param>
        public CharactersInMovieCollection(int movieId)
            : base(movieId)
        {
        }
    }
}