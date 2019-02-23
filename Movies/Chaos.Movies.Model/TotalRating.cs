//-----------------------------------------------------------------------
// <copyright file="TotalRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Contract.Interface;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    /// <inheritdoc cref="IRating" />
    public class TotalRating : SingleRating<TotalRating, TotalRatingDto>, IRating
    {
        /// <summary>The database column for <see cref="IRating.Value"/>.</summary>
        private const string TotalRatingColumn = "TotalRating";

        /// <summary>The <see cref="Type"/> of the parent object.</summary>
        private readonly Type parentType;
        
        /// <summary>Initializes a new instance of the <see cref="TotalRating"/> class.</summary>
        /// <param name="parentType">The parent type.</param>
        public TotalRating(Type parentType)
        {
            this.parentType = parentType;
        }

        /// <summary>Prevents a default instance of the <see cref="TotalRating"/> class from being created.</summary>
        private TotalRating()
        {
        }

        /// <inheritdoc/>
        public override TotalRatingDto ToContract()
        {
            return new TotalRatingDto
            {
                Value = this.Value,
                DisplayValue = this.DisplayValue,
                HexColor = this.HexColor,
                Width = this.Width
            };
        }

        /// <inheritdoc/>
        public override TotalRatingDto ToContract(string languageName)
        {
            return new TotalRatingDto
            {
                Value = this.Value,
                DisplayValue = this.DisplayValue,
                HexColor = this.HexColor,
                Width = this.Width
            };
        }

        /// <inheritdoc/>
        public override TotalRating FromContract(TotalRatingDto contract)
        {
            // ToDo: parentType will be null here
            return new TotalRating
            {
                Value = contract.Value
            };
        }

        /// <inheritdoc/>
        internal override void ValidateSaveCandidate()
        {
        }

        /// <inheritdoc/>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<TotalRating> NewFromRecordAsync(IDataRecord record)
        {
            var result = new TotalRating(this.parentType);
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            var parentName = this.parentType == null ? string.Empty : this.parentType.Name;
            var columnName = $"{parentName}{TotalRatingColumn}";
            Persistent.ValidateRecord(record, new[] { columnName });
            this.Value = (double)record[columnName];
            return Task.CompletedTask;
        }
    }
}
