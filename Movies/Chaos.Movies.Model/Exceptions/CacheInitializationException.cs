//-----------------------------------------------------------------------
// <copyright file="CacheInitializationException.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Exception caused when a cache object is not correctly or failed to initialize.</summary>
    [Serializable]
    public class CacheInitializationException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="CacheInitializationException"/> class.</summary>
        public CacheInitializationException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CacheInitializationException"/> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public CacheInitializationException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CacheInitializationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CacheInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CacheInitializationException"/> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CacheInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
