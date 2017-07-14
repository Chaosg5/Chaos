//-----------------------------------------------------------------------
// <copyright file="PersonUserRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Data;

    /// <summary>Represents a <see cref="User"/>'s rating of a <see cref="Person"/> in a <see cref="Movie"/>.</summary>
    public class PersonUserRating
    {
        /// <summary>Initializes a new instance of the <see cref="PersonUserRating"/> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="PersonUserRating"/>.</param>
        public PersonUserRating(IDataRecord record)
        {
            ReadFromRecord(this, record);
        }

        /// <summary>Gets id of the <see cref="Movie"/>.</summary>
        public int MovieId { get; private set; }

        /// <summary>Gets the <see cref="User"/>'s rating of the <see cref="Person"/> in the <see cref="Movie"/>.</summary>
        public int Rating { get; private set; }

        /// <summary>Gets the number of times the <see cref="User"/> has watched the <see cref="Movie"/>.</summary>
        public int Watches { get; private set; }

        /// <summary>Updates a person user rating from a record.</summary>
        /// <param name="personUserRating">The person user rating to update.</param>
        /// <param name="record">The record containing the data for the person user rating.</param>
        private static void ReadFromRecord(PersonUserRating personUserRating, IDataRecord record)
        {
            Helper.ValidateRecord(record, new[] { "MovieId", "Rating", "Watches" });
            personUserRating.MovieId = (int)record["MovieId"];
            personUserRating.Rating = (int)record["Rating"];
            personUserRating.Watches = (int)record["Watches"];
        }
    }
}
