//-----------------------------------------------------------------------
// <copyright file="MissingResultException.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Exceptions
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>Exception caused by an <see cref="IDataReader"/> missing a SQL result.</summary>
    [Serializable]
    public class MissingResultException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="MissingResultException"/> class.</summary>
        public MissingResultException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingResultException"/> class with a specified error message.</summary>
        /// <param name="resultNumber">The order number of the missing SQL result.</param>
        /// <param name="resultName">The descriptive name of the missing SQL result.</param>
        public MissingResultException(int resultNumber, string resultName)
            : base(string.Format(CultureInfo.InvariantCulture, "Missing the result number '{0}' for '{1}' in the SQL.", resultNumber, resultName))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingResultException"/> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public MissingResultException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingResultException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MissingResultException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingResultException"/> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected MissingResultException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
