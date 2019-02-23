//-----------------------------------------------------------------------
// <copyright file="DerivedRating.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model
{
    using Chaos.Movies.Contract.Interface;
    using Chaos.Movies.Model.Base;

    /// <inheritdoc cref="IRating" />
    /// <summary>The calculated value of a <see cref="UserDerivedRating"/>.</summary>
    public abstract class DerivedRating<T, TDto> : SingleRating<T, TDto> where T : Loadable<T, TDto>, IDerivedRating
    {
        /// <summary>The database column for <see cref="Derived"/>.</summary>
        internal const string DerivedColumn = "Derived";

        /// <summary>Gets or sets the derived value from sub ratings.</summary>
        public double Derived { get; protected set; }
    }
}