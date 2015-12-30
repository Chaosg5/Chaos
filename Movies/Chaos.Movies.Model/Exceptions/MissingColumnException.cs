//-----------------------------------------------------------------------
// <copyright file="MissingColumnException.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Exceptions
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>Exception caused when an <see cref="IDataRecord"/> does not contain all the required columns.</summary>
    [Serializable]
    public class MissingColumnException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="MissingColumnException"/> class.</summary>
        public MissingColumnException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingColumnException"/> class with a specified error message.</summary>
        /// <param name="column">The name of the missing column.</param>
        public MissingColumnException(string column)
            : base($"Missing column '{column}'.")
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingColumnException"/> class with a specified error message.</summary>
        /// <param name="column">The name of the missing column.</param>
        /// <param name="message">The message that describes the error.</param>
        public MissingColumnException(string column, string message)
            : base($"Missing column '{column}'. {message}")
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingColumnException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MissingColumnException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingColumnException"/> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected MissingColumnException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
