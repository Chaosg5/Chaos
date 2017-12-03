//-----------------------------------------------------------------------
// <copyright file="PersonUserRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>Represents a <see cref="User"/>'s rating of a <see cref="Person"/> in a <see cref="Movie"/>.</summary>
    public class PersonUserRating
    {
        /// <summary>Gets the id and type of the parent which this <see cref="Watch"/> belongs to.</summary>
        private Parent parent;

        /// <summary>Initializes a new instance of the <see cref="PersonUserRating"/> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="PersonUserRating"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        public PersonUserRating(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }
        
        /// <summary>Gets the <see cref="User"/>'s rating of the <see cref="Person"/> in the <see cref="Movie"/>.</summary>
        public int Rating { get; private set; }

        /// <summary>Gets the number of times the <see cref="User"/> has watched the <see cref="Movie"/>.</summary>
        public int Watches { get; private set; }

        /// <summary>Sets the parent of this <see cref="PersonAsCharacterCollection"/>.</summary>
        /// <param name="newParent">The parent which this <see cref="PersonAsCharacterCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        internal void SetParent(Parent newParent)
        {
            if (this.parent != null)
            {
                throw new ValueLogicalReadOnlyException("The parent can't be changed once set.");
            }

            this.parent = newParent;
        }

        /// <summary>Updates this <see cref="PersonUserRating"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="PersonUserRating"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "Rating", "Watches" });
            this.SetParent(new Parent(record));
            this.Rating = (int)record["Rating"];
            this.Watches = (int)record["Watches"];
        }
    }
}
