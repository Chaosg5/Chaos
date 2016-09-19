//-----------------------------------------------------------------------
// <copyright file="Icon.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Data;
    using System.Windows.Media;

    /// <summary>An image icon.</summary>
    public class Icon
    {
        /// <summary>Initializes a new instance of the <see cref="Icon" /> class.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        public Icon(IDataRecord record)
        {
        }

        /// <summary>The image of the icon.</summary>
        public ImageSource Image { get; private set; }
    }
}
