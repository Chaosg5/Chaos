﻿//-----------------------------------------------------------------------
// <copyright file="PersonAsCharacter.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a character in a movie.</summary>
    public class PersonAsCharacter
    {
        /// <summary>Initializes a new instance of the <see cref="PersonAsCharacter"/> class.</summary>
        /// <param name="person">The person playing the <paramref name="character"/> in the movie.</param>
        /// <param name="character">The character in the movie.</param>
        /// <param name="userRating">The current user's rating.</param>
        /// <exception cref="ArgumentNullException">If any parameter is null.</exception>
        /// <exception cref="PersistentObjectRequiredException">If any of the objects aren't saved.</exception>
        public PersonAsCharacter(Person person, Character character, int userRating)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            if (person.Id <= -1)
            {
                throw new PersistentObjectRequiredException("The person needs to be saved before being added to a character.");
            }

            if (character == null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            if (character.Id <= -1)
            {
                throw new PersistentObjectRequiredException("The character needs to be saved before being added as a character.");
            }

            this.Person = person;
            this.Character = character;
            this.UserRating = userRating;
        }

        /// <summary>Gets the character.</summary>
        public Character Character { get; }

        /// <summary>Gets the person playing the <see cref="Character"/>.</summary>
        public Person Person { get; }

        /// <summary>Gets the current user's rating of the <see cref="Character"/> in the <see cref="Movie"/>.</summary>
        public int UserRating { get; }
    }
}