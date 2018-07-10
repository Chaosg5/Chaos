//-----------------------------------------------------------------------
// <copyright file="DepartmentTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Base;
    using Chaos.Movies.Model.Exceptions;

    using NUnit.Framework;

    /// <summary>Tests for <see cref="Error"/>.</summary>
    [TestFixture]
    public static class DepartmentTest
    {
        /// <summary>Tests the <see cref="DepartmentDto"/>.</summary>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T,TDto}"/> has to be saved before added.</exception>
        [Test]
        public static void TestDepartmentContract()
        {
            var contract = new DepartmentDto();
            var department = Department.Static.FromContract(contract);
            Assert.IsNotNull(department);

            contract.Id = 13;
            contract.Titles = new ReadOnlyCollection<LanguageTitleDto>(new List<LanguageTitleDto>());
            contract.Roles = new ReadOnlyCollection<RoleDto>(new List<RoleDto>());

            department = Department.Static.FromContract(contract);
            Assert.AreEqual(contract.Id, department.Id);
            Assert.AreEqual(contract.Titles.Count, department.Titles.Count);
            Assert.AreEqual(contract.Roles.Count, department.Roles.Count);
        }

        /// <summary>Tests the <see cref="Department.SaveAsync"/>.</summary>
        /// <param name="englishName">The english Name.</param>
        /// <param name="swedishName">The swedish Name.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="InvalidSaveCandidateException">The <see cref="Department"/> is not valid to be saved.</exception>
        /// <exception cref="PersistentObjectRequiredException">Items of type <see cref="Persistable{T, TDto}"/> has to be saved before added.</exception>
        [TestCase("Director", "Regiserad av", "Director:Regisör")]
        [TestCase("Writing", "Skriven av", "Writer:Författare")]
        [TestCase("Cast", "Rollista", "Actor:Skådespelare;Actress:Skådespelerska;Self:Sig själv")]
        [TestCase("Cinematography", "Foto", "Archive footage:Arkivfoto;Cinematographer:Filmfotograf")]
        [TestCase("Sound", "Ljud", "Archive sound:Arkivljud")]
        [TestCase("Music", "Musik", "Composer:Kompositör")]
        [TestCase("Editing", "Redigering", "Editor:Redigerare")]
        [TestCase("Production", "Producent", "Producer:Producent")]
        [TestCase("Production design", "Produktionsdesign", "Production designer:Produktionsdesigner")]
        public static async Task TestEnsureDepartmentAsync(string englishName, string swedishName, string roles)
        {
            var session = await UserTest.GetSystemSessionAsync();
            var englishTitle = new LanguageTitle(englishName, LanguageTitleTest.English);
            var swedishTitle = new LanguageTitle(swedishName, LanguageTitleTest.Swedish);
            var departments = await Department.Static.GetAllAsync(session);
            var department = departments.FirstOrDefault(g => g.Titles.Any(t => t == englishTitle || t == swedishTitle)) ?? new Department();

            if (!department.Titles.Contains(englishTitle))
            {
                department.Titles.Add(englishTitle);
            }

            if (!department.Titles.Contains(swedishTitle))
            {
                department.Titles.Add(swedishTitle);
            }

            var rolesList = roles.Split(new[] { ";" }, StringSplitOptions.None);
            foreach (var roleItem in rolesList)
            {
                var rolesValue = roleItem.Split(new[] { ":" }, StringSplitOptions.None);
                var englishRoleTitle = new LanguageTitle(rolesValue[0], LanguageTitleTest.English);
                var swedishRoleTitle = new LanguageTitle(rolesValue[1], LanguageTitleTest.Swedish);

                if (!department.Roles.Any(r => r.Titles.Contains(englishTitle) || r.Titles.Contains(swedishRoleTitle)))
                {
                    department.Roles.Add(await GlobalCache.GetRoleAsync(await RolesTest.GetRoleAsync(englishRoleTitle.Title, swedishRoleTitle.Title)));
                }
            }

            await department.SaveAsync(session);

            Assert.Greater(department.Id, 0);
            Assert.AreEqual(2, department.Titles.Count);
            var title = department.Titles.First(t => t == englishTitle);
            Assert.IsNotNull(title);
            title = department.Titles.First(t => t == swedishTitle);
            Assert.IsNotNull(title);
            Assert.AreEqual(rolesList.Length, department.Roles.Count);
        }
    }
}
