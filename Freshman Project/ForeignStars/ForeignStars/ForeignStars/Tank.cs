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
    /// A heavy rolling unit.
    /// 
    /// Team Dynasty
    /// @author: Robert Husfeldt
    /// @author: William Powell
    /// </summary>
    /// </summary>
    class Tank : Unit
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


        public Tank(Texture2D sprite, int gridX, int gridY, int ArmyNumber, string direction)
            : base(25, 10, 3, sprite, gridX, gridY, ArmyNumber, "tank",direction)
        {
            name = "Tank";
            maxHealth = 100;
            maxMovement = 2;
            Health = maxHealth;
            Movement = maxMovement;
            fullMembers = 2;
            members = fullMembers;

            collision = 1;

            SpecialMoves.Add(new SpecialMove("Repair"));
        }

    }
}
