//-----------------------------------------------------------------------
// <copyright file="DataCharactersInMovieCollection.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Data
{
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>SQL logic and database communication for a <see cref="PersonAsCharacterCollection"/>.</summary>
    public class DataPersonAsCharacterCollection : PersonAsCharacterCollection
    {
        /// <summary>Initializes a new instance of the <see cref="DataPersonAsCharacterCollection"/> class.</summary>
        public DataPersonAsCharacterCollection()
            : base()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DataPersonAsCharacterCollection"/> class.</summary>
        /// <param name="parent">The parent which this <see cref="DataPersonAsCharacterCollection"/> belongs to.</param>
        /// <exception cref="ValueLogicalReadOnlyException">The <see cref="Parent"/> can't be changed once set.</exception>
        public DataPersonAsCharacterCollection(Parent parent)
            : base(parent)
        {
        }
    }
}