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
    /// A fast hit and run unit.
    /// 
    /// Team Dynasty
    /// @author: Robert Husfeldt
    /// @author: William Powell
    /// </summary>
    /// </summary>
    class Skimmer : Unit
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


        public Skimmer(Texture2D sprite, int gridX, int gridY, int ArmyNumber, string direction)
            : base(25,3, 1, sprite, gridX, gridY, ArmyNumber, "skimmer",direction)
        {
            name = "Skimmer";
            collision = 1;
            maxHealth = 80;
            maxMovement = 6;
            Health = maxHealth;
            Movement = maxMovement;
            fullMembers = 4;
            members = 4;

            collision = 1;
            specialMoves.Add(new SpecialMove("Cripple"));
        }

    }
}
