//-----------------------------------------------------------------------
// <copyright file="ErrorDto.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Represents an error.</summary>
    [DataContract]
    public class ErrorDto
    {
        /// <summary>Gets or sets the id.</summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>Gets or sets the user id.</summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>Gets or sets the error time.</summary>
        [DataMember]
        public DateTime ErrorTime { get; set; }

        /// <summary>Gets or sets the error type.</summary>
        [DataMember]
        public string ErrorType { get; set; }

        /// <summary>Gets or sets the source.</summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>Gets or sets the target site.</summary>
        [DataMember]
        public string TargetSite { get; set; }

        /// <summary>Gets or sets the message.</summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>Gets or sets the value.</summary>
        [DataMember]
        public string Value { get; set; }
    }
}