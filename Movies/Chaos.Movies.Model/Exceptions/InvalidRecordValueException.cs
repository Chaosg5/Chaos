//-----------------------------------------------------------------------
// <copyright file="InvalidRecordValueException.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Exception caused by invalid configuration in the form's settings XML.</summary>
    [Serializable]
    public class InvalidRecordValueException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="InvalidRecordValueException"/> class.</summary>
        public InvalidRecordValueException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InvalidRecordValueException"/> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidRecordValueException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InvalidRecordValueException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidRecordValueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InvalidRecordValueException"/> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected InvalidRecordValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
