﻿//-----------------------------------------------------------------------
// <copyright file="LanguageDescriptionDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class LanguageDescriptionDto
    {
        /// <summary>Gets or sets the title.</summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>Gets or sets the description.</summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>Gets or sets the language of the title.</summary>
        [DataMember]
        public CultureInfo Language { get; set; }
    }
}