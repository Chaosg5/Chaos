//-----------------------------------------------------------------------
// <copyright file="CharactersInMovie.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>Represents all <see cref="Character"/>s in a movie.</summary>
    public class CharactersInMovie : IReadOnlyCollection<CharacterInMovie>
    {
        /// <summary>The id of the <see cref="Movie"/> that the <see cref="Character"/> belongs to.</summary>
        private readonly int movieId;

        /// <summary>Private part of the <see cref="Characters"/> property.</summary>
        private readonly List<CharacterInMovie> characters = new List<CharacterInMovie>();

        public CharactersInMovie(int movieId)
        {
            this.movieId = movieId;
        }

        /// <summary>Gets the list of <see cref="Character"/>s and their actor <see cref="Person"/>s in a <see cref="Movie"/>.</summary>
        public ReadOnlyCollection<CharacterInMovie> Characters => this.characters.AsReadOnly();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this.characters.Count;

        public IEnumerator<CharacterInMovie> GetEnumerator()
        {
            return this.characters.GetEnumerator();
        }
        
        public void AddCharacter(CharacterInMovie characterInMovie)
        {
            if (this.characters.Exists(c => c.Character.Id == characterInMovie.Character.Id))
            {
                return;
            }

            this.characters.Add(characterInMovie);
            ////this.AddPerson(new PersonInMovie(characterInMovie.Person, CastDepartment, ActorRole));
        }

        public void AddCharacterAndSave(CharacterInMovie characterInMovie)
        {
            this.AddCharacter(characterInMovie);

            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("CharacterInMovieAdd", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@movieId", this.movieId));
                command.Parameters.Add(new SqlParameter("@characterId", characterInMovie.Character.Id));
                command.Parameters.Add(new SqlParameter("@personId", characterInMovie.Person.Id));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RemoveCharacter(CharacterInMovie characterInMovie)
        {
            if (characterInMovie == null)
            {
                return;
            }

            this.characters.Remove(characterInMovie);
            ////this.RemovePersonAsActor(person);
        }

        public void RemoveCharacterAndSave(Character character)
        {



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
