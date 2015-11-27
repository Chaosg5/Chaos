//-----------------------------------------------------------------------
// <copyright file="ServiceRequiredException.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Exceptions
{
    using System;
    using System.Data;
    using System.Runtime.Serialization;

    /// <summary>Exception caused by trying to update an object with a <see cref="IDataRecord"/> which does not contain data that matched the instanced class.</summary>
    [Serializable]
    public class ServiceRequiredException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ServiceRequiredException"/> class.</summary>
        public ServiceRequiredException()
            : base("The requested action has to be performed by the service.")
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ServiceRequiredException"/> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ServiceRequiredException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ServiceRequiredException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ServiceRequiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ServiceRequiredException"/> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ServiceRequiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
