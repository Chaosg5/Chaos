//-----------------------------------------------------------------------
// <copyright file="Icon.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Data;
    using System.Data.Linq;
    using System.Windows.Media;

    using Chaos.Movies.Contract;

    /// <summary>An image icon.</summary>
    public class Icon : IIcon
    {
        /// <summary>Initializes a new instance of the <see cref="Icon" /> class.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        public Icon(IDataRecord record)
        {
        }

        /// <summary>Gets the image of the icon.</summary>
        public Binary Image { get; private set; }
    }
}
