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
    /// Helicopter class
    /// 
    /// Team Dynasty
    /// @author: William Powell
    /// </summary>
    class Helicopter : Unit
    {
        public Helicopter(Texture2D sprite, int gridX, int gridY, int ArmyNumber, string direction)
            : base(20, 4, 2, sprite, gridX, gridY, ArmyNumber, "heli",direction)
        {
            name = "Helicopter";
            maxHealth = 125;
            maxMovement = 4;
            Health = maxHealth;
            Movement = maxMovement;
            fullMembers = 1;
            members = 1;

            collision = 3;
            specialMoves.Add(new SpecialMove("Missile"));
        }
    }
}
