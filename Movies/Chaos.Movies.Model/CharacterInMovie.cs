//-----------------------------------------------------------------------
// <copyright file="CharacterInMovie.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Linq;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public class CharacterInMovie
    {
        /// <summary>Initializes a new instance of the <see cref="CharacterInMovie"/> class.</summary>
        /// <param name="person">The person playing the <paramref name="character"/> in the movie.</param>
        /// <param name="character">The character in the movie.</param>
        /// <exception cref="ArgumentNullException">If any parameter is null.</exception>
        /// <exception cref="PersistentObjectRequiredException">If any of the objects aren't saved.</exception>
        public CharacterInMovie(Person person, Character character)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            if (person.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The person needs to be saved before being added to a character.");
            }

            if (character == null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            if (character.Id <= 0)
            {
                throw new PersistentObjectRequiredException("The character needs to be saved before being added as a character.");
            }

            this.Person = person;
            this.Character = character;
        }

        /// <summary>Gets the character.</summary>
        public Character Character { get; private set; }

        /// <summary>Gets the person playing the <see cref="Character"/>.</summary>
        public Person Person { get; private set; }

    }
}
