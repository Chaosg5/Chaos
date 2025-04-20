//-----------------------------------------------------------------------
// <copyright file="SpaceGameTest.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Model.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public static class SpaceGameTest
    {
        [TestCase(3, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, -3, 1, 0, 0, 0, 0)]
        public static void ShipCombatCalculator(int def1, int def3, int def5, int def4, int def8, int def12, int att1, int att3, int att5, int att4, int att8, int att12, int exp1, int exp3, int exp5, int exp4, int exp8, int exp12)
        {
            var defenders = new Ships(new Technology(), TargetType.Small, def1, def3, def5, def4, def8, def12);
            var attackers = new Ships(new Technology(), TargetType.Large, att1, att3, att5, att4, att8, att12);
            var result = Ships.CalculateCombat(defenders, attackers);
            Assert.AreEqual(exp1, result.Item1.Ship1.Count - def1 + att1 - result.Item2.Ship1.Count);
            Assert.AreEqual(exp3, result.Item1.Ship3.Count - def3 + att1 - result.Item2.Ship3.Count);
            Assert.AreEqual(exp5, result.Item1.Ship5.Count - def5 + att1 - result.Item2.Ship5.Count);
            Assert.AreEqual(exp4, result.Item1.Station4.Count - def4 + att4 - result.Item2.Station4.Count);
            Assert.AreEqual(exp8, result.Item1.Station8.Count - def8 + att8 - result.Item2.Station8.Count);
            Assert.AreEqual(exp12, result.Item1.Station12.Count - def12 + att12 - result.Item2.Station12.Count);
        }

        public class Ships
        {
            public Technology Tech { get; }
            public TargetType TargetType { get; }

            public Ships(Technology tech, TargetType targetType, int ship1, int ship3, int ship5, int station4, int station8, int station12)
            {
                this.Tech = tech;
                this.TargetType = targetType;
                this.Ship1 = new ShipType(tech, ShipClass.Ship1, ship1);
                this.Ship3 = new ShipType(tech, ShipClass.Ship3, ship3);
                this.Ship5 = new ShipType(tech, ShipClass.Ship5, ship5);
                this.Station4 = new ShipType(tech, ShipClass.Station4, station4);
                this.Station8 = new ShipType(tech, ShipClass.Station8, station8);
                this.Station12 = new ShipType(tech, ShipClass.Station12, station12);
            }

            public ShipType Ship1 { get; }
            public ShipType Ship3 { get; }
            public ShipType Ship5 { get; }
            public ShipType Station4 { get; }
            public ShipType Station8 { get; }
            public ShipType Station12 { get; }

            private bool Any => this.Ship1.Any || this.Ship3.Any || this.Ship5.Any || this.Station4.Any || this.Station8.Any || this.Station12.Any;

            private IEnumerable<ShipType> GetSmall =>
                new List<ShipType>
                {
                    Ship1,
                    Ship3,
                    Ship5,
                    Station4,
                    Station8,
                    Station12
                };

            private IEnumerable<ShipType> GetLarge =>
                new List<ShipType>
                {
                    Ship5,
                    Ship3,
                    Ship1,
                    Station12,
                    Station8,
                    Station4
                };

            private IEnumerable<ShipType> GetStation =>
                new List<ShipType>
                {
                    Ship5,
                    Ship3,
                    Ship1,
                    Station12,
                    Station8,
                    Station4
                };

            private Ships Clone => new Ships(this.Tech, this.TargetType, this.Ship1.Count, this.Ship3.Count, this.Ship5.Count, this.Station4.Count, this.Station8.Count, this.Station12.Count);

            private int Damage =>
                this.Ship1.Damage + this.Ship3.Damage + this.Ship5.Damage + this.Station4.Damage + this.Station8.Damage + this.Station12.Damage;

            private int Health =>
                this.Ship1.Health + this.Ship3.Health + this.Ship5.Health + this.Station4.Health + this.Station8.Health + this.Station12.Health;

            public static Tuple<Ships, Ships, int> CalculateCombat(Ships defenders, Ships attackers)
            {
                var rounds = 0;
                defenders = defenders.Clone;
                attackers = attackers.Clone;
                while (defenders.Any && attackers.Any)
                {
                    rounds++;
                    var attackersDamage = attackers.Damage;
                    var defendersDamage = defenders.Damage;
                    defenders.CalculateLosses(attackersDamage, attackers.TargetType);
                    attackers.CalculateLosses(defendersDamage, defenders.TargetType);
                }

                return new Tuple<Ships, Ships, int>(defenders, attackers, rounds);
            }

            private int CalculateLosses(int damage, TargetType targetType)
            {
                while (damage > 0 && this.Any)
                {
                    var shipType = this.GetTarget(targetType);
                    damage += shipType.MissingHealth;
                    for (var i = 0; i < shipType.Count; i++)
                    {
                        if (damage > shipType.Health - shipType.MissingHealth)
                        {
                            shipType.Count--;
                            damage -= shipType.Health;
                            shipType.MissingHealth = 0;
                        }
                        else
                        {
                            shipType.MissingHealth += damage;
                            damage = 0;
                            break;
                        }
                    }

                    if (!shipType.Any)
                    {
                        shipType.MissingHealth = 0;
                    }
                }

                return damage;
            }

            private ShipType GetTarget(TargetType targetType)
            {
                switch (targetType)
                {
                    case TargetType.Small:
                        return this.GetSmall.FirstOrDefault(st => st.Any);
                    case TargetType.Large:
                        return this.GetLarge.FirstOrDefault(st => st.Any);
                    case TargetType.Station:
                        return this.GetStation.FirstOrDefault(st => st.Any);
                    case TargetType.Random:
                    default:
                        return this.GetSmall.FirstOrDefault(st => st.Any);
                }
            }
        }


        public class Technology
        {
            public int Damage { get; } = 1;

            public int Health { get; } = 1;
        }

        public enum ShipClass
        {
            Ship1 = 1,
            Ship3 = 3,
            Ship5 = 5,
            Station4 = 4,
            Station8 = 8,
            Station12 = 12
        }

        public enum TargetType
        {
            Small,
            Large,
            Station,
            Random
        }

        public class ShipType
        {
            private int baseDamage;

            private int baseHealth;

            public ShipType(Technology tech, ShipClass shipClass, int count)
            {
                this.Tech = tech;
                this.ShipClass = shipClass;
                this.Count = count;

                switch (this.ShipClass)
                {
                    case ShipClass.Ship1:
                        this.baseDamage = 1;
                        this.baseHealth = 5;
                        break;
                    case ShipClass.Ship3:
                        this.baseDamage = 3;
                        this.baseHealth = 15;
                        break;
                    case ShipClass.Ship5:
                        this.baseDamage = 5;
                        this.baseHealth = 25;
                        break;
                    case ShipClass.Station4:
                        this.baseDamage = 4;
                        this.baseHealth = 20;
                        break;
                    case ShipClass.Station8:
                        this.baseDamage = 8;
                        this.baseHealth = 40;
                        break;
                    case ShipClass.Station12:
                        this.baseDamage = 12;
                        this.baseHealth = 60;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            public Technology Tech { get; }

            public ShipClass ShipClass { get; }

            public int Count { get; set; }

            public bool Any => this.Count > 0;

            public int Damage => this.baseDamage * Tech.Damage * Count;

            public int Health => this.baseHealth * Tech.Health * Count;

            public int MissingHealth { get; set; }
        }
    }

    public static class Extensions
    {
        private static readonly Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
