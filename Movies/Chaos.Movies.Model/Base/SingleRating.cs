//-----------------------------------------------------------------------
// <copyright file="SingleRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Base
{
    using System;
    using System.Globalization;
    using System.Windows.Media;

    using Chaos.Movies.Contract.Interface;

    /// <inheritdoc cref="IRating" />
    public abstract class SingleRating<T, TDto> : Loadable<T, TDto> where T : Loadable<T, TDto>, IRating
    {
        /// <summary>The database column for <see cref="Value"/>.</summary>
        internal const string RatingColumn = "Rating";

        /// <summary>The database column for when the <see cref="Value"/> was created.</summary>
        internal const string CreatedDateColumn = "CreatedDate";

        /// <summary>Private part of the <see cref="Value"/> property.</summary>
        private double value;

        /// <inheritdoc cref="IRating.Value" />
        public double Value
        {
            get => this.value;
            protected set
            {
                if (value < 0 || value > 10)
                {
                    // ReSharper disable once ExceptionNotDocumented
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.value = value;
            }
        }

        /// <summary>Gets the display color in RBG hex for this <see cref="UserDerivedRating"/>'s <see cref="Value"/>.</summary>
        public string HexColor => GetHexColor(this.Value);

        /// <summary>Gets the display color for this <see cref="UserDerivedRating"/>'s <see cref="Value"/>.</summary>
        public Color Color => GetColor(this.Value);

        /// <inheritdoc cref="IRating.DisplayValue" />
        public string DisplayValue
        {
            get
            {
                var round = Math.Round(this.Value, 1, MidpointRounding.AwayFromZero);
                if (round >= 10)
                {
                    return ((int)round).ToString(CultureInfo.InvariantCulture);
                }

                return round.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <inheritdoc cref="IRating.Width" />
        public string Width => $"{(this.Value * 10).ToString(CultureInfo.InvariantCulture)}%";

        /// <summary>Normalizes the <paramref name="value"/> to a value from 0 to 10.</summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The smallest possible value.</param>
        /// <param name="max">The largest possible value.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public static double NormalizeValue(double value, int min = 0, int max = 10)
        {
            var minValueOffset = 0 - min;
            max += minValueOffset;
            value += minValueOffset;
            var maxValueScale = max / 10;
            return value / maxValueScale;
        }

        /// <summary>Gets the display color in RBG hex for this <see cref="UserDerivedRating"/>'s <see cref="Value"/>.</summary>
        /// <param name="value">The value to get the <see cref="Color"/> for.</param>
        /// <returns>The <see cref="Color"/>.</returns>
        public static string GetHexColor(double value)
        {
            var color = GetColor(value);
            return string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        /// <summary>Gets the display color for the <paramref name="value"/>.</summary>
        /// <param name="value">The value to get the <see cref="Color"/> for.</param>
        /// <returns>The <see cref="Color"/>.</returns>
        private static Color GetColor(double value)
        {
            byte redValue = 255;
            byte greenValue = 0;
            if (value > 1 && value <= 5)
            {
                greenValue = (byte)((value - 1) * 51);
            }

            if (value > 5 && value < 6)
            {
                greenValue = (byte)(204 + ((value - 5) * 26));
            }

            if (value >= 6)
            {
                greenValue = (byte)(230 - ((value - 6) * 25.5));
            }

            if (value > 5)
            {
                redValue = (byte)(255 - ((value - 5) * 51));
            }

            return Color.FromRgb(redValue, greenValue, 0);
        }
    }
}
