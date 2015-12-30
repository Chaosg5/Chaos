//-----------------------------------------------------------------------
// <copyright file="Logger.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Exceptions
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>Log handler for exceptions.</summary>
    public static class Logger
    {
        /// <summary>Logs an exception to the database.</summary>
        /// <param name="exception">The exception to log.</param>
        public static void Log(Exception exception)
        {
            using (var connection = new SqlConnection(Persistent.ConnectionString))
            using (var command = new SqlCommand("ExceptionLog", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", GlobalCache.User.Id));
                command.Parameters.Add(new SqlParameter("@Time", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@Type", exception.GetType().ToString()));
                command.Parameters.Add(new SqlParameter("@Source", exception.Source));
                command.Parameters.Add(new SqlParameter("@Method", exception.TargetSite));
                command.Parameters.Add(new SqlParameter("@Message", exception.Message));
                command.Parameters.Add(new SqlParameter("@Exception", exception.ToString()));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
