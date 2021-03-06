﻿//-----------------------------------------------------------------------
// <copyright file="SqlResultSyncException.cs">
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
    public class SqlResultSyncException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="SqlResultSyncException"/> class.</summary>
        public SqlResultSyncException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SqlResultSyncException"/> class with a specified error message.</summary>
        /// <param name="objectId">The id of the out of sync object.</param>
        public SqlResultSyncException(int objectId)
            : base(string.Format(CultureInfo.InvariantCulture, "An object with id {0} was out of sync.", objectId))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SqlResultSyncException"/> class with a specified error message.</summary>
        /// <param name="objectId">The id of the out of sync object.</param>
        /// <param name="type">The type of the object.</param>
        public SqlResultSyncException(int objectId, Type type)
            : base($"An {nameof(type)} with id {objectId} was out of sync.")
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SqlResultSyncException"/> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public SqlResultSyncException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SqlResultSyncException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public SqlResultSyncException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SqlResultSyncException"/> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected SqlResultSyncException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
