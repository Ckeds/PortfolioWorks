using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForeignStars
{
    /// <summary>
    /// SpecialMove class.
    /// Hopefully, there won't be a need for multiple classes for special moves.
    /// @author: Jenny Li
    /// @author: William Powell
    /// </summary>
    public class SpecialMove
    {
        private string name = "";
        private string description = "";
        private int range;
        private int areaOfEffect;
        private int coolDown;
        private bool damageAll;
        private bool unitStatBoost;
        private bool needsTarget;
        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public int Range { get { return range; } }
        public int AreaOfEffect { get { return areaOfEffect; } }
        public int CoolDown { get { return coolDown; } }
        public bool DamageAll { get { return damageAll; } }
        public bool UnitStatBoost { get { return unitStatBoost; } }
        public bool NeedsTarget { get { return needsTarget; } }
        

        public SpecialMove(string type)
        {
            name = type;
            switch (type)
            {
                //Used on self
                //Can only be used if unit does nothing beforehand.
                //Uses all movement, doubles defense, halves attack
                //Unit can't move until he cancels bunker down (by using it again)
                //This also uses all movement.
                    
                case "Bunker Down":
                    description = "Boost your unit's DEFENSE.";
                    unitStatBoost = true;
                    needsTarget = false;
                    areaOfEffect = 0;  //used to determine what type of cursor is drawn, does not deal damage
                    coolDown = 1;
                    break;
                    
                case "Web Grenade":
                    description = "Lower enemy's MOVE by 2.";
                    range = 5;
                    areaOfEffect = 0;  //used to determine what type of cursor is drawn, does not deal damage
                    needsTarget = true;
                    coolDown = 2; //Number of turns you can't use specials after the turn you used one
                    break;
                    //Damages enemy move value (reset when enemy ended their turn)
                case "Frag Grenade":
                    description = "Use this to blow away your foes!";
                    range = 2;
                    areaOfEffect = 1; //used to determine what type of cursor is drawn, shows what units should take damage
                    needsTarget = true;
                    damageAll = true;
                    coolDown = 3; //Number of turns you can't use specials after the turn you used one
                    break;
                case "Missile":
                    description = "Attacks enemy units at an \nincreased range.";
                    range = 4;
                    areaOfEffect = 0; //used to determine what type of cursor is drawn, shows what units should take damage
                    needsTarget = true;
                    coolDown = 2; //Number of turns you can't use specials after the turn you used one
                    break;
                case "Cripple":
                    description = "Deals not as much damage, \nbut reduces enemy movement for next turn.";
                    range = 1;
                    areaOfEffect = 0; //used to determine what type of cursor is drawn, shows what units should take damage
                    needsTarget = true;
                    coolDown = 2; //Number of turns you can't use specials after the turn you used one
                    break;
                case "Repair":
                    description = "Make an attempt to repair \nyour tanks.";
                    needsTarget = false;
                    areaOfEffect = 0;  //used to determine what type of cursor is drawn, does not deal damage
                    coolDown = 4; //Number of turns you can't use specials after the turn you used one
                    break;
                default:
                    break;
            }
        }
    }
}
