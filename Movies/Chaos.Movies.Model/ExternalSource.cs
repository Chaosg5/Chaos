//-----------------------------------------------------------------------
// <copyright file="ExternalSource.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class ExternalSource
    {
        /// <summary>Gets the id of the external source.</summary>
        [DataMember]
        public int Id { get; private set; }

        /// <summary>Gets the name.</summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>Gets the base address.</summary>
        [DataMember]
        public string BaseAddress { get; private set; }

        /// <summary>Gets the people address.</summary>
        [DataMember]
        public string PeopleAddress { get; private set; }

        /// <summary>Gets the character address.</summary>
        [DataMember]
        public string CharacterAddress { get; private set; }

        /// <summary>Gets the genre address.</summary>
        [DataMember]
        public string GenreAddress { get; private set; }

        /// <summary>Gets the episode address.</summary>
        [DataMember]
        public string EpisodeAddress { get; private set; }
    }
}