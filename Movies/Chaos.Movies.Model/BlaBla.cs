//-----------------------------------------------------------------------
// <copyright file="Persistent.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Configuration;

    /// <summary>Contains generic database persistence handling.</summary>
    [Obsolete]
    public static class BlaBla
    {
        /// <summary>The connection string to the database read from configuration application settings.</summary>
        public static readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>If database interaction should be made through the service.</summary>
        public static readonly bool UseService = ConfigurationManager.AppSettings["UseService"] == null || ConfigurationManager.AppSettings["UseSaveService"] != "false";
    }
}
