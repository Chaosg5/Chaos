//-----------------------------------------------------------------------
// <copyright file="Helper.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System.Collections.Generic;

    /// <summary>The helper for tests.</summary>
    internal static class Helper
    {
        /// <summary>The first names.</summary>
        private static readonly List<string> FirstNames = new List<string> { "Sean", "Ranee", "Nicola", "Margy", "Evelin", "Ned", "Alishia", "Lucille", "Juliane", "Janetta", "Idalia", "Lili", "Jeanmarie", "Elna", "Trudi", "Santa", "Bart", "Gilberto", "Patrica", "Evie", "Dodie", "Arnette", "Bernardo", "James", "Frederica", "Bruno", "Faye", "Devin", "Verena", "Laverne", "Jocelyn", "Anabel", "Cesar", "Milton", "Young", "Roy", "Abraham", "Deidre", "Samira", "Randy", "Christel", "Lenora", "Signe", "Shavon", "Fred", "Latrisha", "Deon", "Bernarda", "Rufina", "Cheri" };

        /// <summary>The last names.</summary>
        private static readonly List<string> LastNames = new List<string> { "Goodyear", "Jimmerson", "Vanscoy", "Comerford", "Winnett", "Oday", "Gilmer", "Peacock", "Helm", "Centers", "Fruchter", "Feagins", "Seago", "Mohler", "Purdom", "Fusaro", "Caughman", "Edmonson", "Kanner", "Balch", "Algarin", "Rodriquez", "Leisinger", "Schoonmaker", "Malec", "Hardesty", "Beane", "Vanbuskirk", "Sowell", "Schepers", "Fagan", "Dodrill", "Keitt", "Schebler", "Myles", "Baskette", "Loman", "Rattler", "Urbano", "Piercy", "Wordlaw", "Bixler", "Pappan", "Caldera", "Vanderford", "Haslett", "Cripps", "Madrid", "Macdonnell", "Kinman" };

        /// <summary>The get random name.</summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetRandomName()
        {
            return $"{GetRandomFirstName()} {GetRandomLastName()}";
        }

        /// <summary>The get random first name.</summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetRandomFirstName()
        {
            return FirstNames.PickRandom();
        }

        /// <summary>The get random last name.</summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetRandomLastName()
        {
            return LastNames.PickRandom();
        }
    }
}
