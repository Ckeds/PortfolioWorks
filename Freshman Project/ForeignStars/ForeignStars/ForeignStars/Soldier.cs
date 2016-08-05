using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
namespace ForeignStars
{
	class Soldier : Unit
	{

        private Texture2D spriteSheet;
        //Attribute(s?)
        bool hasJetpack;
        //Constructor
        public Soldier()
            : base(15,7,1)
        {
            maxHealth = 100;
            maxMovement = 3;
            Health = maxHealth;
            Movement = maxMovement;
            members = 10;
            HPPerMember = 10;
            fullMembers = 10;
            expCap = 150;
        }
        //Overrides move method if unit has jetpack
        override public void Move()
        {
            if (hasJetpack == true)
            {

            }
            else
            {
                base.Move();
            }
        }

	}
}
