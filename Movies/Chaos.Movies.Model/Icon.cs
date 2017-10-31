//-----------------------------------------------------------------------
// <copyright file="Icon.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System.Data;
    using System.Data.Linq;

    using Chaos.Movies.Contract;

    /// <summary>An image icon.</summary>
    public class Icon
    {
        /// <summary>Private part of the <see cref="Data"/> property.</summary>
        private Binary data;

        /// <summary>Initializes a new instance of the <see cref="Icon" /> class.</summary>
        /// <param name="record">The record containing the data for the character.</param>
        public Icon(IDataRecord record)
        {
        }

        /// <summary>Gets the id of the <see cref="Icon"/>.</summary>
        public int Id { get; private set; }

        /// <summary>Gets the image of the icon.</summary>
        public IconType IconType { get; private set; }

        /// <summary>Gets the URL of the image of the icon.</summary>
        public string Url { get; private set; }

        /// <summary>Gets the binary data of the image of the icon.</summary>
        public Binary Data
        {
            get => this.data;

            private set
            {
                this.data = value;
                this.Size = value.Length;
            }
        }

        /// <summary>Gets the size of the <see cref="Data"/>.</summary>
        public int Size { get; private set; } 

        /// <summary>Converts this <see cref="Icon"/> to a <see cref="IconDto"/>.</summary>
        /// <returns>The <see cref="IconDto"/>.</returns>
        public IconDto ToContract()
        {
            return new IconDto { Id = this.Id, IconType = this.IconType.ToContract(), Data = this.Data, Url = this.Url };
        }

        public void ChangeDataAsync(Binary data)
        {
            
        }

        public void ChangeDataAndSaveAsync(Binary data)
        {
            
        }
    }
}
