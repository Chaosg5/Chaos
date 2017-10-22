//-----------------------------------------------------------------------
// <copyright file="LanguageTitleDto.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>Represents a user.</summary>
    [DataContract]
    public class LanguageTitleDto
    {
        /// <summary>Gets or sets the title.</summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>Gets the language of the title.</summary>
        [DataMember]
        public CultureInfo Language { get; private set; }
    }
}
