using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace ForeignStars
{

    public abstract class Unit
    {
        /// <summary>
        /// Abstract for making units
        /// 
        /// Team Dynasty
        /// @author: William Powell
        /// </summary>
        #region attributes
        /// <summary>
        /// Used to say what type of unit it is
        /// </summary>
        protected String name; //INFOBAR STAT** (above the unit's portrait)
        /// <summary>
        /// How hard the unit hits
        /// </summary>
        protected int attack; //INFOBAR STAT**
        /// <summary>
        /// Stores the original attack attribute
        /// </summary>
        protected int oriAttack; //used to revert from bunker
        /// <summary>
        /// How much experience a unit has
        /// </summary>
        protected int exp;
        /// <summary>
        /// How much resistance a unit has to damage
        /// </summary>
        protected int defense; //INFOBAR STAT**
        /// <summary>
        /// Stores the original defense attribute
        /// </summary>
        protected int oriDefense; //used to revert from bunker
        /// <summary>
        /// How much damage a unit can take before it dies
        /// </summary>
        protected int health; //INFOBAR STAT**
        /// <summary>
        /// How far the unit has left to move
        /// </summary>
        protected int movement; //INFOBAR STAT**
        /// <summary>
        /// The largest amount of exp a unit can have
        /// </summary>
        protected int expCap;
        /// <summary>
        /// The max health value of a unit
        /// </summary>
        protected int maxHealth; //INFOBAR STAT**
        /// <summary>
        /// Maximum movement a unit can have each turn
        /// </summary>
        protected int maxMovement; //INFOBAR STAT**
        /// <summary>
        /// The level the unit is at
        /// </summary>
        protected int level; //INFOBAR STAT** (next to the Name, above portrait)
        /// <summary>
        /// Distance at which a unit can attack
        /// </summary>
        protected int range;
        /// <summary>
        /// Variable used to determine when a special can be used again
        /// </summary>
        protected int coolDown;
        /// <summary>
        /// Shows which way the unit is facing
        /// </summary>
        protected int direction;
        /// <summary>
        /// The tile this unit is standing on
        /// </summary>
        protected Tile tile;
        /// <summary>
        /// The X and Y and size of the texture on the source sprite sheet
        /// </summary>
        protected Vector2 textureSource;
        /// <summary>
        /// Current units remaining (people left in the squad)
        /// </summary>
        protected int members; //INFOBAR STAT**       
        /// <summary>
        /// Units in group at full health (how many people can be in the squad)
        /// </summary>
        protected int fullMembers; //INFOBAR STAT**
        /// <summary>
        /// Used to calculate the units remaining
        /// </summary>
        protected int HPPerMember;
        /// <summary>
        /// Unit location on map (tied to grid)
        /// </summary>
        protected Rectangle unitBox;
        /// <summary>
        /// Map picture
        /// </summary>
        protected Texture2D unitSprite;
        /// <summary>
        /// Toolbar picture
        /// </summary>
        protected Texture2D unitPic;
        /// <summary>
        /// Tells if the unit has attacked this turn
        /// </summary>
        protected bool hasAttacked;
        /// <summary>
        /// Tells if the unit is bunkered
        /// </summary>
        protected bool bunkered; 
        /// <summary>
        /// Tells what type of tiles we can move over
        /// </summary>
        protected int collision;
        /// <summary>
        /// Army identity
        /// </summary>
        protected Army army;
        /// <summary>
        /// Stores our special moves
        /// </summary>
        protected List<SpecialMove> specialMoves;
        
        protected Rectangle spriteSource;

        protected Rectangle displaySource;

        #endregion

        #region constructor

        public Unit(int a, int d, int r, Texture2D sprite, int gridX, int gridY, int ArmyNumber, string UnitType, string direction)
        {
            string[] attributes = SpriteSheetLookup.UnitSprites[UnitType + direction];

            spriteSource = new Rectangle((int.Parse(attributes[0]) * 32) + (128 * ArmyNumber), int.Parse(attributes[1]) * 32, 32, 32);
            displaySource = new Rectangle(0 + (128 * ArmyNumber), int.Parse(attributes[1]) * 32, 32, 32);
            attack = a;
            oriAttack = a;
            defense = d;
            oriDefense = d;
            range = r;
            exp = 0;

            unitSprite = sprite;

            unitPic = sprite; //TESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTEST

            fullMembers = 10;
            members = 10;
            hasAttacked = false;
            collision = 0;

            unitBox = new Rectangle(gridX * GV.TileSize, gridY * GV.TileSize, GV.TileSize, GV.TileSize);
            // For now every unit will just have every single special move
            specialMoves = new List<SpecialMove>();
        }

        #endregion

        /// <summary>
        /// Grid based location of the unit
        /// </summary>
        public Rectangle UnitBox
        {
            get { return unitBox; }
            set { unitBox = value; }
        }
        /// <summary>
        /// The Location attribute of the UnitBox
        /// </summary>
        public Point UnitPoint
        {
            get { return unitBox.Location; }
            set { unitBox.Location = value; }
        }

        /// <summary>
        /// The type of unit
        /// </summary>
        public String Name
        {
            get { return name; }
        }
        /// <summary>
        /// attack range of a unit
        /// </summary>
        public int Range
        {
            get { return range; }
        }
        /// <summary>
        /// The attack power of the unit
        /// </summary>
        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        /// <summary>
        /// original attack value
        /// </summary>
        public int OriAttack
        {
            get { return oriAttack; }
        }
        /// <summary>
        /// Determines if we can use a special
        /// </summary>
        public int CoolDown
        {
            get { return coolDown; }
            set { coolDown = value; if (coolDown < 0) coolDown = 0; }
        }
        /// <summary>
        /// The defensive power of the unit
        /// </summary>
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }
        /// <summary>
        /// Original Defense value
        /// </summary>
        public int OriDefense
        {
            get { return oriDefense; }
        }
        /// <summary>
        /// How much further a unit can move this turn
        /// </summary>
        public int Movement
        {
            get { return movement; }
            set { movement = value; }
        }
        /// <summary>
        /// The max amount of spaces a unit can traverse
        /// </summary>
        public int MaxMovement
        {
            get { return maxMovement; }
        }
        /// <summary>
        /// The current level of the unit
        /// </summary>
        public int Level
        {
            get { return level; }
        }
        /// <summary>
        /// How much health unit has left, used for taking damage as well
        /// </summary>
        public int Health
        {
            get { return health; }
            set
            {
                if (value > 0 && value <= maxHealth)
                    health = value;
                else if (value <= 0) health = 0;
                else health = maxHealth;
                if (health == 0)
                {
                    army.Units.Remove(this);
                }
            }
        }
        /// <summary>
        /// Value for whether we have attacked this turn or not
        /// </summary>
        public bool HasAttacked
        {
            get { return hasAttacked; }
            set { hasAttacked = value; }
        }
        /// <summary>
        /// Determines whether we are bunkered right now
        /// </summary>
        public bool Bunkered
        {
            get { return bunkered; }
            set { bunkered = value; }
        }
        /// <summary>
        /// Maximum health for this unit
        /// </summary>
        public int MaxHealth
        {
            get { return maxHealth; }
        }
        /// <summary>
        /// How much experience this unit has
        /// </summary>
        public int Exp
        {
            get { return exp; }
            set { exp += value; }
        }
        /// <summary>
        /// The tile this unit is standing on
        /// </summary>
        public Tile Tile
        {
            get { return tile; }
            set { tile = value; }
        }
        /// <summary>
        /// How many members are left in the unit
        /// </summary>
        public int Members
        {
            get
            {
                members = (int)Math.Ceiling((double)((health * fullMembers) / maxHealth) + 1);
                if (members > fullMembers)
                {
                    members = fullMembers;
                }
                return members;
            }
        }
        /// <summary>
        /// The full amount of members in the unit
        /// </summary>
        public int FullMembers
        {
            get { return fullMembers; }
        }
        /// <summary>
        /// Get and Set direction
        /// </summary>
        public int Direction
        {
            get { return direction; }
            set
            {
                if (value >= 0 && value <= 3)
                {
                    direction = value;
                }
            }
        }
        /// <summary>
        /// Gets what army this unit is a part of
        /// </summary>
        public Army Army
        {
            get { return army; }
            set { army = value; }
        }

        /// <summary>
        /// Accessor for unitPic, so it can be used in InfoBar
        /// </summary>
        public Texture2D UnitPic
        {
            get { return unitPic; }
            set { unitPic = value; }
        }
        /// <summary>
        /// Spirte that shows up on the map
        /// </summary>
        public Texture2D UnitSprite
        {
            get { return unitSprite; }
            set { unitSprite = value; }
        }
        /// <summary>
        /// Value for what kind of tiles unit can move through
        /// </summary>
        public int Collision
        {
            get { return collision; }
        }
        /// <summary>
        /// Y value for where this unit is on the grid
        /// </summary>
        public int UnitBoxY
        {
            get { return unitBox.Y; }
            set { unitBox.Y = value; }
        }
        /// <summary>
        /// X value for where this unit is on the grid
        /// </summary>
        public int UnitBoxX
        {
            get { return unitBox.X; }
            set { unitBox.X = value; }
        }
        /// <summary>
        /// List of the special moves this unit has
        /// </summary>
        public List<SpecialMove> SpecialMoves
        {
            get { return specialMoves; }
        }

        public Rectangle SpriteSource
        {
            get { return spriteSource; }
            set { spriteSource = value; }
        }

        public int SpriteSourceX
        {
            get { return spriteSource.X; }
            set { spriteSource.X = value; }
        }

        public int SpriteSourceY
        {
            get { return spriteSource.Y; }
            set { spriteSource.Y = value; }
        }

        public Rectangle DisplaySource
        {
            get { return displaySource; }
        }

        #region game methods
        /// <summary>
        /// Draws our unit onto the map
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(unitSprite, unitBox, spriteSource, Color.White);
        }
        #endregion
    }
}
