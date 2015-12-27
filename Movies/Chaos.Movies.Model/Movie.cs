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

    /// <summary>A movie or a series.</summary>
    public class Movie
    {
        /// <summary>Private part of the <see cref="Titles"/> property.</summary>
        private readonly List<Genre> genres = new List<Genre>();

        /// <summary>Private part of the <see cref="Images"/> property.</summary>
        private readonly List<Icon> images = new List<Icon>();

        /// <summary>Private part of the <see cref="UserRating"/> property.</summary>
        private readonly Rating userRating = new Rating(new RatingType(1));

        /// <summary>Private part of the <see cref="TotalRating"/> property.</summary>
        private readonly Rating totalRating = new Rating(new RatingType(1));

        /// <summary>Private part of the <see cref="Characters"/> property.</summary>
        private readonly Dictionary<Character, Person> characters = new Dictionary<Character, Person>();

        /// <summary>Private part of the <see cref="People"/> property.</summary>
        private readonly List<MoviePerson> people = new List<MoviePerson>();

        /// <summary>Initializes a new instance of the <see cref="Movie" /> class.</summary>
        public Movie()
        {
        }
        
        /// <summary>The id of the movie.</summary>
        public int Id { get; private set; }

        /// <summary>The id of the movie in IMDB.</summary>
        public string ImdbId { get; private set; }

        /// <summary>The id of the movie in TMDB.</summary>
        public int TmdbId { get; private set; }

        /// <summary>The list of title of the movie in different languages.</summary>
        public LanguageTitles Titles { get; private set; } = new LanguageTitles();

        /// <summary>The list of genres that the movie belongs to.</summary>
        public ReadOnlyCollection<Genre> Genres
        {
            get { return this.genres.AsReadOnly(); }
        }

        /// <summary>The list of images for the movie and their order as represented by the key.</summary>
        public ReadOnlyCollection<Icon> Images
        {
            get { return this.images.AsReadOnly(); }
        }

        /// <summary>The total rating score from the current user.</summary>
        public Rating UserRating
        {
            get { return this.userRating; }
        }

        /// <summary>The total rating score from all users.</summary>
        public Rating TotalRating
        {
            get { return this.totalRating; }
        }

        /// <summary>Gets the list of characters and their actors in the movie.</summary>
        public ReadOnlyDictionary<Character, Person> Characters
        {
            get { return new ReadOnlyDictionary<Character, Person>(this.characters); }
        }

        /// <summary>Gets the list of people and their roles in the movie.</summary>
        public ReadOnlyCollection<MoviePerson> People
        {
            get { return this.people.AsReadOnly(); }
        }

        public void SavePeople()
        {
            
        }

        public void AddCharacter(Character character, Person person)
        {
            if (character == null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            this.characters.Add(character, person);
            this.AddPersonAsActor(person);
        }

        public void AddCharacterAndSave(Character character, Person person)
        {
            
        }

        public void RemoveCharacter(Character character)
        {
            if (character == null)
            {
                return;
            }

            this.characters.Remove(character);
            ////this.RemovePersonAsActor(person);
        }

        public void RemoveCharacterAndSave(Character character)
        {



        }

        public void SaveCharacters()
        {
            
        }

        public void AddPersonAsActor(Person person)
        {
            // ToDo: These should be global

        }

        public void RemovePersonAsActor(Person person)
        {
            if (this.characters.ContainsValue(person))
            {
                return;
            }


        }

        ////private void LoadCharacters()
        ////{
        ////    using (var connection = new SqlConnection(Persistent.ConnectionString))
        ////    using (var command = new SqlCommand("MovieCharactersGet", connection))
        ////    {
        ////        command.CommandType = CommandType.StoredProcedure;
        ////        command.Parameters.Add(new SqlParameter("@movieId", this.Id));
        ////        connection.Open();

        ////        using (var reader = command.ExecuteReader())
        ////        {
        ////            while (reader.Read())
        ////            {
        ////                ReadFromRecord(character, reader);
        ////            }
        ////        }
        ////    }
        ////}
    }
}