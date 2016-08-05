using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ForeignStars
{
    /// <summary>
    /// The tiles that make up battle and strategy maps
    /// 
    /// @author: Zacary Behrmann
    /// @author: William Powell
    /// @author: Ryan Conrad
    /// </summary>
    public class Tile
    {
        #region attributes

        /// <summary>
        /// The texture of the tile
        /// </summary>
        private Texture2D texture;
        private Rectangle textureLocation;  //The location of the tile's texture in the spritesheet
        private Rectangle location;    //The location of the tile
        /// <summary>
        /// The location of the tile based off of the battle grid
        /// </summary>
        private Point gridwiseLocation;
        /// <summary>
        /// The unit that is currently on this tile 
        /// </summary>
        private Unit unit;

        /// <summary>
        /// The type of collision implemented into the tile
        /// </summary>
        private int collision;        //Whether this tile has collision (unused for the moment)

        /// <summary>
        /// The property of whether units may spawn on this tile
        /// </summary>
        private bool spawn;
        /// <summary>
        /// Whether this tile is highlighted green and thus "selectable" (used for moving spaces)
        /// </summary>
        private bool highlightSpace;
        /// <summary>
        /// Whether this tile is highlighted red and thus "selectable" (used for attacking spaces)
        /// </summary>
        private bool highlightEnemy;
        /// <summary>
        /// Whether this tile is being moused over right now
        /// </summary>
        private bool highlightCursor;

        #endregion

        #region properties

        /// <summary>
        /// The property of whether units may spawn on this tile
        /// </summary>
        public bool Spawn
        {
            get { return spawn; }
            set { spawn = value; }
        }

        /// <summary>
        /// Whether this tile is highlighted and thus "selectable" (used for moving spaces)
        /// </summary>
        public bool HighlightSpace
        {
            get { return highlightSpace; }
            set { highlightSpace = value; }
        }
        /// <summary>
        /// Whether this tile is highlighted and thus "selectable" (used for attacking spaces)
        /// </summary>
        public bool HighlightEnemy
        {
            get { return highlightEnemy; }
            set { highlightEnemy = value; }
        }
        /// <summary>
        /// This displays whether the mouse is hovering over the tile or not
        /// </summary>
        public bool HighlightCursor
        {
            get { return highlightCursor; }
            set { highlightCursor = value; }
        }
        /// <summary>
        /// The unit that is currently in this tile - returns null if no unit exists
        /// </summary>
        public Unit Unit
        {
            get { return unit; }
            set
            {
                if (value != null)
                {
                    value.Tile = this;
                }
                unit = value;
            }
        }

        /// <summary>
        /// The location of the tile on the screen
        /// </summary>
        public Rectangle Location
        {
            get { return location; }
        }

        public Point GridwiseLocation
        {
            get { return gridwiseLocation; }
        }

        public int Collision
        {
            get { return collision; }
        }

        #endregion

        #region Constructors


        public Tile() { }

        public Tile(string terrain, Texture2D spriteSheet, int row, int column, int size)
        {
            string[] attributes = SpriteSheetLookup.TilesSprites[terrain];
            textureLocation = new Rectangle(int.Parse(attributes[0]) * GV.TileSize,
                int.Parse(attributes[1]) * GV.TileSize, GV.TileSize, GV.TileSize);
            collision = int.Parse(attributes[2]);

            texture = spriteSheet;
            location = new Rectangle(470 + row * size, 125 + column * size, size, size);
            spawn = false;
            highlightSpace = false;
            highlightEnemy = false;
            gridwiseLocation = new Point(location.Location.X / size, location.Location.Y / size);

        }
        public Tile(string terrain, Texture2D spriteSheet, int row, int column)
        {
            string[] attributes = SpriteSheetLookup.TilesSprites[terrain];
            textureLocation = new Rectangle(Convert.ToInt32(attributes[0]) * GV.TileSize,
               Convert.ToInt32(attributes[1]) * GV.TileSize, GV.TileSize, GV.TileSize);
            collision = int.Parse(attributes[2]);

            texture = spriteSheet;
            location = new Rectangle(row * GV.TileSize, column * GV.TileSize, GV.TileSize, GV.TileSize);
            spawn = false;
            highlightSpace = false;
            highlightEnemy = false;
            gridwiseLocation = new Point(location.Location.X / GV.TileSize, location.Location.Y / GV.TileSize);

        }
        #endregion


        public bool HasUnit()
        {
            return !(unit == null);
        }


        #region Game Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            if (highlightCursor)
                spriteBatch.Draw(texture, location, textureLocation, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            else
            {
                if (highlightSpace)
                    spriteBatch.Draw(texture, location, textureLocation, Color.CornflowerBlue, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                else if (highlightEnemy)
                    spriteBatch.Draw(texture, location, textureLocation, Color.DarkGreen, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                else
                    spriteBatch.Draw(texture, location, textureLocation, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
            if (unit != null)
            {
                unit.Draw(spriteBatch);
                if (unit.Health == 0)
                    unit = null;
            }
        }

        #endregion
    }
}
