﻿//-----------------------------------------------------------------------
// <copyright file="WatchTypeDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class WatchTypeDto
    {
        /// <summary>Gets or sets the id of the location.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the location.</summary>
        [DataMember]
        public string Name { get; set; }
    }
}