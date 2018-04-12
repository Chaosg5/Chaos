//-----------------------------------------------------------------------
// <copyright file="ExternalSourceDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class ExternalSourceDto
    {
        /// <summary>Gets or sets the id of the external source.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>Gets or sets the base address.</summary>
        [DataMember]
        public string BaseAddress { get; set; }

        /// <summary>Gets or sets the people address.</summary>
        [DataMember]
        public string PeopleAddress { get; set; }

        /// <summary>Gets or sets the character address.</summary>
        [DataMember]
        public string CharacterAddress { get; set; }

        /// <summary>Gets or sets the genre address.</summary>
        [DataMember]
        public string GenreAddress { get; set; }

        /// <summary>Gets or sets the episode address.</summary>
        [DataMember]
        public string EpisodeAddress { get; set; }
    }
}