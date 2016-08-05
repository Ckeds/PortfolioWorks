using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForeignStars
{
    /// <summary>
    /// Performs damage calculation for battles
    /// 
    /// @author: Ryan Conrad
    /// </summary>
    static class DamageCalculator
    {
        /* Initial equation for damage (will be tweaked upon testing)
         * THE EQUATION:
         * Damage = ((((2 * Level*10 / 5 + 2) * AttackStat * AttackPower / DefenseStat) / 50) + 2) * CriticalHit * Weakness/Resistance * RandomNumber / 100
         * RESTRICTIONS FOR VALUES:
         * 1<Level<3 (ex. 3 = veteran)
         * AttackStat = attack attribute of the units you want to attack with
         * AttackPower = power of the attack the unit uses
         * DefenseStat = defense attribute of the units you want to attack
         * 85 <= RandomNumber <= 100
         * CriticalHit = 1.5 if a RandomNumber from 0 to 9 equals 9
         * Weakness/Resistance = .8 if soldiers attack tanks (more resistant)
         * Weakness/Resistance = 1.2 if tanks attack soldiers (less resistant)
         * Weakness/Resistance may not be in the final equation as more unit types are added
         * The equation is derived from the Pokemon damage calculation.
         * */

        public static int doDamage(Unit attacker, Unit defender, bool isCounter)
        {
            // new Random object
            Random rgen = new Random();

            // This will check attacker's levelN (currently, it just checks exp)
            int level = attacker.Exp / 10 + 1;

            int attackStat = attacker.Attack;

            // This will check attacker.attackTypePower (may not be in final equation)
            int attackPower = 1;

            int defenseStat = defender.Defense;

            // This is the critical hit (type is double)
            double criticalHit;
            if (rgen.Next(0, 10) == 9)
            {
                criticalHit = 1.5;
            }
            else criticalHit = 1;

            // A random number to give the equation a little variability
            int rand = rgen.Next(85, 101);

            // the final damage calculation
            int damage = (int)(((((level * 25 + 2) * attackStat * attackPower / defenseStat) / 2) + 2) * criticalHit * rand / 100);

            // a modifier for the damage based on number of members left
            damage = (int)Math.Round((double)(damage * attacker.Members / attacker.FullMembers));

            // a modifier for the damage if it is a counter attack
            if (isCounter)
            {
                rand = rgen.Next(1, 101);
                damage = (int)(damage * (.4 + .002 * rand));
            }

            // damage is returned
            return damage;
        }
    }
}
