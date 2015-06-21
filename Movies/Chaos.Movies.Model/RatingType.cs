//-----------------------------------------------------------------------
// <copyright file="RatingType.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using Exceptions;

    /// <summary>A sub rating with a defined type.</summary>
    public class RatingType
    {
        /// <summary>The sub types of this rating type.</summary>
        private List<RatingType> subtypes = new List<RatingType>(); 

        /// <summary>Initializes a new instance of the <see cref="Rating" /> class.</summary>
        /// <param name="id">The id of the type.</param>
        public RatingType(int id)
        {
            this.Id = id;
        }

        /// <summary>Initializes a new instance of the <see cref="Rating" /> class.</summary>
        /// <param name="record">The record containing the data for the rating type.</param>
        public RatingType(IDataRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException("record");
            }

            if (record["Id"] == null)
            {
                throw new MissingColumnException("Id");
            }

            this.Id = (int)record["Id"];
            this.Name = record["Name"].ToString();
            this.Description = record["Description"].ToString();
        }

        /// <summary>The id of this rating type.</summary>
        public int Id { get; private set; }

        /// <summary>The name of this rating type.</summary>
        public string Name { get; private set; }

        /// <summary>The description of this rating type.</summary>
        public string Description { get; private set; }

        /// <summary>The <see cref="RatingType"/>s that makes up the derived children of this <see cref="RatingType"/>.</summary>
        public ReadOnlyCollection<RatingType> Subtypes
        {
            get { return this.subtypes.AsReadOnly(); } 
        }
    }
}
