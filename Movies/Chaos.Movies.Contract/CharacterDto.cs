//-----------------------------------------------------------------------
// <copyright file="CharacterDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>Represents a character in a movie.</summary>
    [DataContract]
    public class CharacterDto
    {
        /// <summary>Gets or sets the id of the character.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the character.</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>Gets or sets the id of the <see cref="CharacterDto"/> in <see cref="ExternalSourceDto"/>s.</summary>
        [DataMember]
        public ReadOnlyCollection<ExternalLookupDto> ExternalLookup { get; set; }

        /// <summary>Gets or sets the list of images for the movie and their order as represented by the key.</summary>
        [DataMember]
        public IEnumerable<IconDto> Images { get; set; }
    }
}
