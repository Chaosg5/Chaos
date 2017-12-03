//-----------------------------------------------------------------------
// <copyright file="ExternalLookup.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a user.</summary>
    public class ExternalLookup
    {
        /// <summary>The database column for <see cref="ExternalId"/>.</summary>
        private const string ExternalIdColumn = "ExternalId";

        /// <summary>Initializes a new instance of the <see cref="ExternalLookup" /> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="ExternalLookup" />.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        public ExternalLookup(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the <see cref="ExternalSource"/>.</summary>
        public ExternalSource ExternalSource { get; private set; }

        /// <summary>Gets the id of the item in the <see cref="ExternalSource"/>.</summary>
        public string ExternalId { get; private set; }

        /// <summary>Converts this <see cref="ExternalLookup"/> to a <see cref="ExternalLookupDto"/>.</summary>
        /// <returns>The <see cref="ExternalLookupDto"/>.</returns>
        public ExternalLookupDto ToContract()
        {
            return new ExternalLookupDto { ExternalSource = this.ExternalSource.ToContract(), ExternalId = this.ExternalId };
        }
        
        /// <summary>Updates this <see cref="Character"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Character"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { ExternalIdColumn });
            this.ExternalId = (string)record[ExternalIdColumn];
        }
    }
}