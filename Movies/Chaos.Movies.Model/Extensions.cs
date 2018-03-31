//-----------------------------------------------------------------------
// <copyright file="Extensions.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    /// <summary>Holds extensions methods for classes.</summary>
    public static class Extensions
    {
        /// <summary>Saves the exception.</summary>
        /// <param name="exceptionToSave">The exception to save/log.</param>
        /// <param name="session">The user session causing the exception.</param>
        /// <param name="callerName">The name of the calling method. Note: The caller is added automatically.</param>
        /// <exception cref="ArgumentNullException">Argument cannot be null.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <returns>The <see cref="Task"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Built in feature to get the name of the caller, without using the stack trace.")]
        public static async Task SaveAsync(this Exception exceptionToSave, UserSession session, [CallerMemberName] string callerName = null)
        {
            if (exceptionToSave == null)
            {
                throw new ArgumentNullException(nameof(exceptionToSave));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (exceptionToSave.GetType() == typeof(AggregateException))
            {
                var aggregateException = (AggregateException)exceptionToSave;

                var exceptions = aggregateException.Flatten();
                var innerExceptions = exceptions.InnerExceptions;
                if (innerExceptions == null)
                {
                    return;
                }

                foreach (var innerException in innerExceptions)
                {
                    if (innerException != null)
                    {
                        await new Error(innerException, session.UserId, callerName).SaveAsync(session);
                    }
                }
            }
            else
            {
                await new Error(exceptionToSave, session.UserId, callerName).SaveAsync(session);
            }
        }
    }
}
