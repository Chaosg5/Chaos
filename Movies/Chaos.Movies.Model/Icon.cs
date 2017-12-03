//-----------------------------------------------------------------------
// <copyright file="Icon.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using System;
    using System.Data;
    using System.Data.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;

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

        /// <summary>Saves this <see cref="Character"/> to the database.</summary>
        /// <param name="session">The <see cref="UserSession"/>.</param>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Character"/> is not valid to be saved.</exception>
        /// <returns>No return.</returns>
        public async Task SaveAsync(UserSession session)
        {
           // this.ValidateSaveCandidate();

        }

        /// <summary>Changes the image data.</summary>
        /// <param name="newData">The new data to set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="newData"/> is <see langword="null"/></exception>
        public void ChangeDataAsync(Binary newData)
        {
            if (newData == null || newData.Length == 0)
            {
                throw new ArgumentNullException(nameof(newData));
            }

            this.Url = string.Empty;
            this.Data = newData;
        }

        /// <summary>Changes the image data.</summary>
        /// <param name="newData">The new data to set.</param>
        public async Task ChangeDataAndSaveAsync(Binary newData)
        {
            this.ChangeDataAsync(newData);

        }


    }
}
