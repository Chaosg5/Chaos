//-----------------------------------------------------------------------
// <copyright file="Movie.cs">
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
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>A movie or a series.</summary>
    public class Movie
    {
        /// <summary>The department for cast members.</summary>
        ////private static Department castDepartment = GlobalCache.GetDepartment("Cast", GlobalCache.DefaultLanguage);

        /// <summary>The role for cast actors.</summary>
        ////private static Role actorRole = GlobalCache.GetRole("Actor", "Cast", GlobalCache.DefaultLanguage);

        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<Genre> genres = new List<Genre>();

        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="Characters"/> property.</summary>
        private PersonAsCharacterCollection characters;

        /// <summary>Initializes a new instance of the <see cref="Movie" /> class.</summary>
        public Movie()
        {
        }

        /// <summary>Gets the id of the movie.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the id of the <see cref="Movie"/> in <see cref="ExternalSource"/>s.</summary>
        public ExternalLookupCollection ExternalLookup { get; } = new ExternalLookupCollection();
    
        /// <summary>Gets the ratings of the movie in <see cref="ExternalSource"/>s.</summary>
        public ExternalRatingsCollection ExternalRatings { get; } = new ExternalRatingsCollection();

        /// <summary>Gets the list of title of the movie in different languages.</summary>
        public LanguageTitleCollection Titles { get; } = new LanguageTitleCollection();

        /// <summary>Gets the list of genres that the movie belongs to.</summary>
        public ReadOnlyCollection<Genre> Genres => this.genres.AsReadOnly();

        /// <summary>Gets the list of images for the movie and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images => this.images.AsReadOnly();

        /// <summary>Gets the total rating score from the current user.</summary>
        public Rating UserRating { get; } = new Rating(new RatingType(1));

        /// <summary>Gets the total rating score from all users.</summary>
        public Rating TotalRating { get; } = new Rating(new RatingType(1));

        /// <summary>Gets the list of <see cref="Character"/>s in this <see cref="Movie"/>.</summary>
        public PersonAsCharacterCollection Characters
        {
            get
            {
                if (this.characters == null)
                {
                    this.characters = new PersonAsCharacterCollection(new Parent(this));
                    this.characters.LoadCharacters();
                }

                return this.characters;
            }
        }

        /// <summary>Gets the list of <see cref="Person"/>s in this <see cref="Movie"/>.</summary>
        public PersonInRoleCollection People { get; } = new PersonInRoleCollection();

        /// <summary>Gets or sets the type of the movie.</summary>
        public MovieType MovieType { get; set; }
        
        /// <exception cref="PersistentObjectRequiredException">If the <see cref="Id"/> is not a valid id of a <see cref="Movie"/>.</exception>
        public void SaveAll()
        {
            this.People.Save();
            this.Characters.Save();
        }
        
        /// <summary>Sets the id of the <see cref="Movie"/> this <see cref="PersonAsCharacterCollection"/> belongs to.</summary>
        /// <param name="movieId">The id of the <see cref="Movie"/> which this <see cref="PersonAsCharacterCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="movieId"/> is not valid.</exception>
        private void SetMovieId(int movieId)
        {
            if (movieId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(movieId), "The id of the movie has to be greater than zero.");
            }

            if (this.Id != 0)
            {
                throw new ValueLogicalReadOnlyException("The id of the movie can't be changed once set.");
            }

            this.Id = movieId;
            var parent = new Parent(this);
            this.Characters.SetParent(parent);
            this.People.SetParent(parent);
        }
    }
}