//-----------------------------------------------------------------------
// <copyright file="Error.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.ChaosMovieService;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents and error.</summary>
    public class Error : Persistable<Error, ErrorDto>
    {
        /// <summary>The database column for the id of the <see cref="UserId"/>.</summary>
        private const string UserIdColumn = "UserId";

        /// <summary>The database column for the id of the <see cref="ErrorTime"/>.</summary>
        private const string ErrorTimeColumn = "Time";

        /// <summary>The database column for the id of the <see cref="ErrorType"/>.</summary>
        private const string ErrorTypeColumn = "Type";

        /// <summary>The database column for the id of the <see cref="Source"/>.</summary>
        private const string SourceColumn = "Source";

        /// <summary>The database column for the id of the <see cref="TargetSite"/>.</summary>
        private const string TargetSiteColumn = "TargetSite";

        /// <summary>The database column for the id of the <see cref="Message"/>.</summary>
        private const string MessageColumn = "Message";

        /// <summary>The database column for the id of the <see cref="Value"/>.</summary>
        private const string ValueColumn = "StackTrace";

        /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="userId">The id of the user.</param>
        /// <param name="callerName">The name of the calling method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/></exception>
        public Error(Exception exception, int userId, string callerName)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            this.UserId = userId;
            this.ErrorTime = DateTime.Now;
            this.ErrorType = exception.GetType().FullName;
            this.Source = exception.Source;
            this.TargetSite = callerName != null ? string.Format(CultureInfo.InvariantCulture, "{0}:{1}", callerName, exception.TargetSite.ToString()) : exception.TargetSite.ToString();
            this.Message = exception.Message;
            this.Value = exception.StackTrace;
        }

        /// <summary>Prevents a default instance of the <see cref="Error"/> class from being created.</summary>
        private Error()
        {
        }

        /// <summary>Gets a reference to simulate static methods.</summary>
        public static Error Static { get; } = new Error();

        /// <summary>Gets the user id.</summary>
        public int UserId { get; private set; }

        /// <summary>Gets the error time.</summary>
        public DateTime ErrorTime { get; private set; }

        /// <summary>Gets the error type.</summary>
        public string ErrorType { get; private set; }

        /// <summary>Gets the source.</summary>
        public string Source { get; private set; }

        /// <summary>Gets the target site.</summary>
        public string TargetSite { get; private set; }

        /// <summary>Gets the message.</summary>
        public string Message { get; private set; }

        /// <summary>Gets the value.</summary>
        public string Value { get; private set; }

        /// <inheritdoc />
        public override ErrorDto ToContract()
        {
            return new ErrorDto
            {
                UserId = this.UserId,
                ErrorTime = this.ErrorTime,
                ErrorType = this.ErrorType,
                Source = this.Source,
                TargetSite = this.TargetSite,
                Message = this.Message,
                Value = this.Value
            };
        }

        /// <inheritdoc />
        public override Error FromContract(ErrorDto contract)
        {
            return new Error
            {
                UserId = contract.UserId,
                ErrorTime = contract.ErrorTime,
                ErrorType = contract.ErrorType,
                Source = contract.Source,
                TargetSite = contract.TargetSite,
                Message = contract.Message,
                Value = contract.Value
            };
        }

        /// <inheritdoc />
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public override async Task SaveAsync(UserSession session)
        {
            this.ValidateSaveCandidate();
            if (!Persistent.UseService)
            {
                await this.SaveToDatabaseAsync(this.GetSaveParameters(), this.ReadFromRecordAsync);
                return;
            }

            using (var service = new ChaosMoviesServiceClient())
            {
                await service.ErrorSaveAsync(session.ToContract(), this.ToContract());
            }
        }

        /// <inheritdoc />
        internal override void ValidateSaveCandidate()
        {
            // ToDo:
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal override async Task<Error> NewFromRecordAsync(IDataRecord record)
        {
            var result = new Error();
            await result.ReadFromRecordAsync(record);
            return result;
        }

        /// <inheritdoc />
        /// <exception cref="MissingColumnException">A required column is missing in the record.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        protected override Task ReadFromRecordAsync(IDataRecord record)
        {
            Persistent.ValidateRecord(
                record,
                new[] { IdColumn, UserIdColumn, ErrorTimeColumn, ErrorTypeColumn, SourceColumn, TargetSiteColumn, MessageColumn, ValueColumn });
            this.Id = (int)record[IdColumn];
            this.UserId = (int)record[UserIdColumn];
            this.ErrorTime = (DateTime)record[ErrorTimeColumn];
            this.ErrorType = (string)record[ErrorTypeColumn];
            this.Source = (string)record[SourceColumn];
            this.TargetSite = (string)record[TargetSiteColumn];
            this.Message = (string)record[MessageColumn];
            this.Value = (string)record[ValueColumn];
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IReadOnlyDictionary<string, object> GetSaveParameters()
        {
            return new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>
                {
                    { Persistent.ColumnToVariable(IdColumn), this.Id },
                    { Persistent.ColumnToVariable(UserIdColumn), this.UserId },
                    { Persistent.ColumnToVariable(ErrorTimeColumn), this.ErrorTime },
                    { Persistent.ColumnToVariable(ErrorTypeColumn), this.ErrorType },
                    { Persistent.ColumnToVariable(SourceColumn), this.Source },
                    { Persistent.ColumnToVariable(TargetSiteColumn), this.TargetSite },
                    { Persistent.ColumnToVariable(MessageColumn), this.Message },
                    { Persistent.ColumnToVariable(ValueColumn), this.Value }
                });
        }
    }
}
