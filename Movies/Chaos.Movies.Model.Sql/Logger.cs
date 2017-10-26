//-----------------------------------------------------------------------
// <copyright file="Logger.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Sql
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>Log handler for exceptions.</summary>
    public static class Logger
    {
        /// <summary>Logs an exception to the database.</summary>
        /// <param name="exception">The exception to log.</param>
        public static void Log(Guid userId, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            using (var connection = new SqlConnection(BlaBla.ConnectionString))
            using (var command = new SqlCommand("ExceptionLog", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@time", DateTime.Now);
                command.Parameters.AddWithValue("@type", exception.GetType().ToString());
                command.Parameters.AddWithValue("@source", exception.Source);
                command.Parameters.AddWithValue("@method", exception.TargetSite);
                command.Parameters.AddWithValue("@message", exception.Message);
                command.Parameters.AddWithValue("@exception", exception.ToString());
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
