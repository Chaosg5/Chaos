//-----------------------------------------------------------------------
// <copyright file="Parent.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;

    using Chaos.Movies.Model.Exceptions;

    /// <summary>A genre of <see cref="Movie"/>s.</summary>
    /// <typeparam name="T">The type of the parent class.</typeparam>
    internal class Parent<T>
    {
        /// <summary>The parent id.</summary>
        private int parentId;

        /// <summary>Initializes a new instance of the <see cref="Parent{T}"/> class.</summary>
        /// <param name="parentId">The id of the parent.</param>
        /// <exception cref="PersistentObjectRequiredException">The parent needs to be saved.</exception>
        public Parent(int parentId)
        {
            if (parentId <= 0)
            {
                throw new PersistentObjectRequiredException($"The id '{nameof(parentId)}' of the parent has to be greater than zero.");
            }

            this.ParentId = parentId;
        }

        /// <summary>Initializes a new instance of the <see cref="Parent{T}"/> class.</summary>
        /// <param name="record">The record containing the data for the <see cref="Parent{T}"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        internal Parent(IDataRecord record)
        {
            this.ReadFromRecord(record);
        }

        /// <summary>Gets the type of the parent.</summary>
        public string ParentType => nameof(T);

        /// <summary>The <see cref="ParentType"/> as a variable name.</summary>
        public string VariableName => Persistent.ColumnToVariable(this.ParentType);

        /// <summary>Gets the id of the parent.</summary>
        /// <exception cref="PersistentObjectRequiredException" accessor="set">The parent needs to be saved.</exception>
        public int ParentId
        {
            get => this.parentId;

            private set
            {
                if (value <= 0)
                {
                    throw new PersistentObjectRequiredException("The parent needs to be saved.");
                }

                this.parentId = value;
            }
        }

        /// <summary>Updates this <see cref="Parent{T}"/> from the <paramref name="record"/>.</summary>
        /// <param name="record">The record containing the data for the <see cref="Parent{T}"/>.</param>
        /// <exception cref="MissingColumnException">A required column is missing in the <paramref name="record"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="record"/> is <see langword="null" />.</exception>
        private void ReadFromRecord(IDataRecord record)
        {
            Persistent.ValidateRecord(record, new[] { "ParentType" });
            if ((string)record["ParentType"] != this.ParentType)
            {
                // ReSharper disable once ExceptionNotDocumented - This should not occur, unless database is out of sync with the application and documentation is not needed
                throw new ArgumentOutOfRangeException(nameof(this.ParentType), $"The value '{(string)record["ParentType"]}' is not a valid {nameof(this.ParentType)}.");
            }

            Persistent.ValidateRecord(record, new[] { $"{this.ParentType}Id" });
            // ReSharper disable once ExceptionNotDocumented - This should not occur since all id columns are identity(1) and documentation is not needed
            this.ParentId = (int)record[$"{this.ParentType}Id"];
        }
    }
}