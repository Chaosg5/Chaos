﻿//-----------------------------------------------------------------------
// <copyright file="ExternalLookup.cs" company="Erik Bunnstad">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Holds an id of an item in an <see cref="ExternalSource"/>.</summary>
    public sealed class ExternalLookup : Loadable<ExternalLookup, ExternalLookupDto>, ILoadable<ExternalLookup, ExternalLookupDto>
    {
        /// <summary>The database column for <see cref="ExternalId"/>.</summary>
        public const string ExternalIdColumn = "ExternalId";

        /// <summary>Private part of the <see cref="ExternalSource"/> property.</summary>
        private ExternalSource externalSource;

        /// <summary>Private part of the <see cref="ExternalId"/> property.</summary>
        private string externalId;
        
        /// <summary>Initializes a new instance of the <see cref="ExternalLookup"/> class.</summary>
        /// <param name="externalSource">The external source.</param>
        /// <param name="externalId">The external id.</param>
        public ExternalLookup(ExternalSource externalSource, string externalId)
        {
            this.ExternalSource = externalSource;
            this.ExternalId = externalId;
        }

        /// <summary>Prevents a default instance of the <see cref="ExternalLookup"/> class from being created.</summary>
        private ExternalLookup()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static ExternalLookup Static { get; } = new ExternalLookup();

        /// <summary>Gets the <see cref="ExternalSource"/>.</summary>
        public ExternalSource ExternalSource
        {
            get => this.externalSource;
            private set
            {
                if (value == null)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(this.ExternalSource));
                }

                if (value.Id <= 0)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new PersistentObjectRequiredException($"The {nameof(this.ExternalSource)} has to be saved.");
                }

                this.externalSource = value;
            }
        }

        /// <summary>Gets the id of the item in the <see cref="ExternalSource"/>.</summary>
        public string ExternalId
        {
            get => this.externalId;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentNullException(nameof(this.ExternalId));
                }

                this.externalId = value;
            }
        }
        
        /// <inheritdoc />
        public ExternalLookupDto ToContract()
        {
            return new ExternalLookupDto { ExternalSource = this.ExternalSource.ToContract(), ExternalId = this.ExternalId };
        }

        /// <inheritdoc />
        /// <exception cref="InvalidSaveCandidateException">This <see cref="ExternalLookup"/> is not valid to be saved.</exception>
        public override void ValidateSaveCandidate()
        {
            if (string.IsNullOrEmpty(this.ExternalId))
            {
                throw new InvalidSaveCandidateException($"The {nameof(this.ExternalId)} can't be empty.");
            }
        }
        
        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        public override async Task<ExternalLookup> ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { ExternalSource.ExternalSourceIdColumn, ExternalIdColumn });
            return new ExternalLookup(
                await GlobalCache.GetExternalSourceAsync((int)record[ExternalSource.ExternalSourceIdColumn]),
                (string)record[ExternalIdColumn]);
        }
    }
}