//-----------------------------------------------------------------------
// <copyright file="ExternalSource.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using Chaos.Movies.Contract;

    /// <summary>Represents a user.</summary>
    public class ExternalSource
    {
        /// <summary>Gets the id of the external source.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the name.</summary>
        public string Name { get; private set; }

        /// <summary>Gets the base address.</summary>
        public string BaseAddress { get; private set; }

        /// <summary>Gets the people address.</summary>
        public string PeopleAddress { get; private set; }

        /// <summary>Gets the character address.</summary>
        public string CharacterAddress { get; private set; }

        /// <summary>Gets the genre address.</summary>
        public string GenreAddress { get; private set; }

        /// <summary>Gets the episode address.</summary>
        public string EpisodeAddress { get; private set; }


        /// <summary>Converts this <see cref="ExternalSource"/> to a <see cref="ExternalSourceDto"/>.</summary>
        /// <returns>The <see cref="ExternalSourceDto"/>.</returns>
        public ExternalSourceDto ToContract()
        {
            return new ExternalSourceDto
            {
                Id = this.Id,
                Name = this.Name,
                BaseAddress = this.BaseAddress,
                PeopleAddress = this.PeopleAddress,
                CharacterAddress = this.CharacterAddress,
                GenreAddress = this.GenreAddress,
                EpisodeAddress = this.EpisodeAddress
            };
        }
    }
}