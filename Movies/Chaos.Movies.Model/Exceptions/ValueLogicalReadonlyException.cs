﻿//-----------------------------------------------------------------------
// <copyright file="ValueLogicalReadonlyException.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Exception caused when trying to change <see cref="User"/> for an object which does not allow the user to be changed.</summary>
    [Serializable]
    public class ValueLogicalReadOnlyException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ValueLogicalReadOnlyException"/> class.</summary>
        public ValueLogicalReadOnlyException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ValueLogicalReadOnlyException"/> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ValueLogicalReadOnlyException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ValueLogicalReadOnlyException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ValueLogicalReadOnlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ValueLogicalReadOnlyException"/> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ValueLogicalReadOnlyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
