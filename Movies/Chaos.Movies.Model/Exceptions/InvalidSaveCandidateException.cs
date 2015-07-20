//-----------------------------------------------------------------------
// <copyright file="InvalidSaveCandidateException.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Exception caused when trying to save an object which does not contain all required data for the instanced class to save it to the database.</summary>
    [Serializable]
    public class InvalidSaveCandidateException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="InvalidSaveCandidateException"/> class.</summary>
        public InvalidSaveCandidateException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InvalidSaveCandidateException"/> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidSaveCandidateException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InvalidSaveCandidateException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidSaveCandidateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InvalidSaveCandidateException"/> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected InvalidSaveCandidateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
