using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ForeignStars
{
    /// <summary>
    /// An infantry based unit
    /// 
    /// Team Dynasty
    /// @author: William Powell
    /// </summary>
    /// </summary>
    class Infantry : Unit
    {
        //Constructor
        /*public Infantry(Texture2D sprite)
            : base(15, 7, 2, sprite)
        {
            maxHealth = 100;
            maxMovement = 3;
            Health = maxHealth;
            Movement = maxMovement;
            fullMembers = 10;
            members = 10;
            collision = 1;
        }*/


        public Infantry(Texture2D sprite, int gridX, int gridY, int ArmyNumber, string direction)
            : base(15, 7, 2, sprite, gridX, gridY, ArmyNumber, "infantry", direction)
        {
            name = "Infantry";
            maxHealth = 100;
            maxMovement = 3;
            Health = maxHealth;
            Movement = maxMovement;
            fullMembers = 10;
            members = 10;
            oriAttack = attack;
            oriDefense = defense;
            collision = 1;
            specialMoves.Add(new SpecialMove("Bunker Down"));
            specialMoves.Add(new SpecialMove("Web Grenade"));
            specialMoves.Add(new SpecialMove("Frag Grenade"));
        }
    }
}
