using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ForeignStars
{
    /// <summary>
    /// A selector is used to select tiles on the battle map
    /// 
    /// @author: Zachary Behrmann
    /// @author: William Powell
    /// </summary>
    public class Selector
    {
        #region attributes


        Vector2 currentMouseVector;

        /// <summary>
        /// The current X position on the grid
        /// </summary>
        private int currentGridX;
        /// <summary>
        /// The current Y position on the grid
        /// </summary>
        private int currentGridY;
        /// <summary>
        /// The position (and size) we are relative to the game
        /// </summary>
        private Rectangle rectPos;
        private Texture2D texture;
        /// <summary>
        /// The array of tiles we traverse with the selector
        /// </summary>
        private Tile[,] tiles;
        private Unit unitSelected; // used to determine what side the unit is on for movement and attack

        private Tile currentTile;

        private Tile previousTile;

        private Rectangle curMousePos;
        private Rectangle prevMousePos;

        private Unit highlightedUnit;

        #endregion

        #region properties

        public Tile PreviousTile
        {
            get { return previousTile; }
            set { previousTile = value; }
        }

        public int CurrentGridX
        {
            get { return currentGridX; }
            set { currentGridX = value; }
        }

        public int CurrentGridY
        {
            get { return currentGridY; }
            set { currentGridY = value; }
        }

        public Tile[,] Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Tile CurrentTile
        {
            get { return currentTile; }
            set { currentTile = value; }
        }

        public Unit UnitSelected
        {
            get { return unitSelected; }
            set { unitSelected = value; }
        }

        public Unit HighlightedUnit
        {
            get { return highlightedUnit; }
        }

        #endregion

        #region constructor

        public Selector(Tile[,] t, Rectangle pos, Texture2D sprite, int X, int Y)
        {
            tiles = t;
            rectPos = new Rectangle(pos.X, pos.Y, pos.Width, pos.Height);
            texture = sprite;
            currentGridX = X;
            currentGridY = Y;
            curMousePos = new Rectangle(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y, 1, 1);
        }

        #endregion

        #region methods
        public Tile Update(ref Rectangle pos, ref int X, ref int Y, ref Camera2D camera, int range, Rectangle endTurnButton)
        {
            currentMouseVector = Vector2.Transform(new Vector2(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y), Matrix.Invert(camera.Transform));
            if (currentMouseVector.X > 0 && currentMouseVector.Y > 0 &&
               currentMouseVector.X < tiles.GetLength(0) * GV.TileSize - 1 && currentMouseVector.Y < tiles.GetLength(1) * GV.TileSize - 1)
            {
                prevMousePos = curMousePos;

                curMousePos.X = (int)MathHelper.Clamp(currentMouseVector.X, 0f, ((float)Math.Sqrt(tiles.Length - 1) * GV.TileSize));
                curMousePos.Y = (int)MathHelper.Clamp(currentMouseVector.Y, 0f, ((float)Math.Sqrt(tiles.Length - 1) * GV.TileSize));

                if (!tiles[prevMousePos.X / GV.TileSize, prevMousePos.Y / GV.TileSize].Location.Intersects(curMousePos))
                {
                    controlCursorHighlight(prevMousePos.X / GV.TileSize, prevMousePos.Y / GV.TileSize, range, false);

                }
                PreviousTile = tiles[curMousePos.X / GV.TileSize, curMousePos.Y / GV.TileSize];
                if (currentMouseVector.X > 0 && currentMouseVector.Y > 0 &&
                    currentMouseVector.X < tiles.GetLength(0) * GV.TileSize - 1 && currentMouseVector.Y < tiles.GetLength(1) * GV.TileSize - 1)
                {
                    controlCursorHighlight(curMousePos.X / GV.TileSize, curMousePos.Y / GV.TileSize, range, true);
                    unitSelected = tiles[curMousePos.X / GV.TileSize, curMousePos.Y / GV.TileSize].Unit;
                }
            }
            else
            {
                controlCursorHighlight(curMousePos.X / GV.TileSize, curMousePos.Y / GV.TileSize, range, false);
            }


            if (InputManager.MouseClicked() &&
                currentMouseVector.X > 0 && currentMouseVector.Y > 0 &&
                currentMouseVector.X < tiles.GetLength(0) * GV.TileSize - 1 && currentMouseVector.Y < tiles.GetLength(1) * GV.TileSize - 1 && 
                !endTurnButton.Contains(new Rectangle(InputManager.CurrentMouseState.X,InputManager.CurrentMouseState.Y,1,1)))
            {
                if (currentGridX == curMousePos.X / GV.TileSize && currentGridY == curMousePos.Y / GV.TileSize)
                {
                    //Mouse.SetPosition(InputManager.CurrentMouseState.X - (int)(camera.Origin.X - camera.Position.X), InputManager.CurrentMouseState.Y - (int)(camera.Origin.Y - camera.Position.Y));
                    controlCursorHighlight(prevMousePos.X / GV.TileSize, prevMousePos.Y / GV.TileSize, range, false);
                    tiles[currentGridX, currentGridY].HighlightCursor = false;
                    return tiles[currentGridX, currentGridY];

                }
                /*
                                    if (new Rectangle(tiles[i, j].Location.X + (int)(camera.Origin.X - camera.Position.X), tiles[i, j].Location.Y + (int)(camera.Origin.Y - camera.Position.Y),
                                        tiles[i, j].Location.Width, tiles[i, j].Location.Height).Contains(mousePos))
                                    {
                                        PreviousTile = tiles[i, j];
                                        tiles[i, j].HighlightCursor = true;
                                        if (InputManager.MouseClicked())
                                        {
                                            if (currentGridX == i && currentGridY == j)
                                            {
                                                return tiles[currentGridX, currentGridY];
                                            }
                */
                currentGridX = curMousePos.X / GV.TileSize;
                currentGridY = curMousePos.Y / GV.TileSize;

            }
            if (InputManager.KeyReady(Keys.W))
            {
                if (currentGridY > 0)
                    currentGridY--;
            }

            if (InputManager.KeyReady(Keys.S))
            {
                // -1 is there because sqrt(400) is 20, and array goes 0 - 19
                if (currentGridY < Math.Sqrt(tiles.Length) - 1)
                    currentGridY++;
            }

            if (InputManager.KeyReady(Keys.A))
            {
                if (currentGridX > 0)
                    currentGridX--;
            }
            if (InputManager.KeyReady(Keys.D))
            {
                // -1 is there because sqrt(400) is 20, and array goes 0 - 19
                if (currentGridX < Math.Sqrt(tiles.Length) - 1)
                    currentGridX++;
            }
            if (InputManager.KeyReady(Keys.Enter))
            {
                return tiles[currentGridX, currentGridY];
                //unit.UnitBox.Offset(tiles[currentGridX, currentGridY].Location.X - unit.UnitBox.X, tiles[currentGridX, currentGridY].Location.Y - unit.UnitBox.Y);
            }
            X = currentGridX;
            Y = currentGridY;
            currentTile = tiles[currentGridX, currentGridY];
            pos.X = tiles[currentGridX, currentGridY].Location.X;
            pos.Y = tiles[currentGridX, currentGridY].Location.Y;
            return null;
        }
        /// <summary>
        /// Calls the recursive movement highlighter
        /// </summary>
        /// <param name="movement">How far the unit can move</param>
        public void HighlightTiles(int movement)
        {

            HighlightTiles2(currentGridX, currentGridY, movement);
        }
        /// <summary>
        /// Highlights the tiles within range of unit movement
        /// </summary>
        /// <param name="x">X postition of the tile in 2D array</param>
        /// <param name="y">Y postition of the tile in 2D array</param>
        /// <param name="movesLeftOver">How much further the unit can move after this tile</param>
        private void HighlightTiles2(int x, int y, int movesLeftOver)
        {
            bool canHighlight = false;
            //checks to see if unit can move there
            if (tiles[x, y].Unit == null)
            {
                tiles[x, y].HighlightSpace = true;
                canHighlight = true;
            }
            //if a unit is there, is it an ally?
            else if (tiles[x, y].Unit != null)
            {
                if (unitSelected.Army == tiles[x, y].Unit.Army)
                    canHighlight = true;
            }
            //Can I move into this type of tile?
            if (tiles[x, y].Collision > unitSelected.Collision)
            {
                tiles[x, y].HighlightSpace = false;
                canHighlight = false;
            }
            //can I keep moving?
            if (movesLeftOver > 0)
            {
                //If nothing is blocking my way (enemy or unpassable terrain)
                if (canHighlight)
                {
                    if (x > 0)
                        HighlightTiles2(x - 1, y, movesLeftOver - 1);
                    if (y > 0)
                        HighlightTiles2(x, y - 1, movesLeftOver - 1);
                    if (x < Math.Sqrt(tiles.Length) - 1)
                        HighlightTiles2(x + 1, y, movesLeftOver - 1);
                    if (y < Math.Sqrt(tiles.Length) - 1)
                        HighlightTiles2(x, y + 1, movesLeftOver - 1);

                }
            }
        }

        /// <summary>
        /// Call for the recrusive attack highligher
        /// </summary>
        /// <param name="range">How far the unit can attack from it's current position</param>
        public void HighlightEnemies(int range)
        {
            if (tiles[currentGridX, currentGridY].Unit.HasAttacked == false)
                HighlightEnemies2(currentGridX, currentGridY, range);
        }
        /// <summary>
        /// Highlights the tiles within range of unit movement
        /// </summary>
        /// <param name="x">X postition of the tile in 2D array</param>
        /// <param name="y">Y postition of the tile in 2D array</param>
        /// <param name="rangeRemaining">How far the unit is able to attack</param>
        private void HighlightEnemies2(int x, int y, int rangeRemaining)
        {
            //Is there an enemy unit in this tile?
            if (tiles[x, y].Unit != null && tiles[x, y].Unit.Army != unitSelected.Army)
            {
                tiles[x, y].HighlightEnemy = true;
                //unable to attack through an enemy
                rangeRemaining = 0;
            }
            /*
            if (tiles[x, y].Unit != null)
            {
                if (unitSelected.Enemy == tiles[x, y].Unit.Enemy)
                    canHighlight = true;
            }
             * */
            //try to find other enemies in range
            if (rangeRemaining > 0)
            {
                if (x > 0)
                    HighlightEnemies2(x - 1, y, rangeRemaining - 1);
                if (y > 0)
                    HighlightEnemies2(x, y - 1, rangeRemaining - 1);
                if (x < Math.Sqrt(tiles.Length) - 1)
                    HighlightEnemies2(x + 1, y, rangeRemaining - 1);
                if (y < Math.Sqrt(tiles.Length) - 1)
                    HighlightEnemies2(x, y + 1, rangeRemaining - 1);
            }
        }
        /// <summary>
        /// Calls the recursive method for highlighting tiles used in special moves
        /// </summary>
        /// <param name="range">How far the special can be used</param>
        /// <param name="ignore">Decides if the special cares about unit position</param>
        public void HighlightSpecial(int range, bool ignore)
        {
            if (tiles[currentGridX, currentGridY].Unit.HasAttacked == false)
                HighlightSpecial2(currentGridX, currentGridY, range, ignore);
        }
        /// <summary>
        /// Highlights where the unit can use its special
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="range"></param>
        /// <param name="ignore">Says whether or not it matters if something is here</param>
        public void HighlightSpecial2(int x, int y, int range, bool ignore)
        {
            if (ignore)
            {
                tiles[x, y].HighlightEnemy = true;
            }
            else
            {
                if (tiles[x, y].Unit != null && tiles[x, y].Unit.Army != unitSelected.Army)
                {
                    tiles[x, y].HighlightEnemy = true;
                    range = 0;
                }
            }
            /*
            if (tiles[x, y].Unit != null)
            {
                if (unitSelected.Enemy == tiles[x, y].Unit.Enemy)
                    canHighlight = true;
            }
             * */
            if (range > 0)
            {
                if (x > 0)
                    HighlightSpecial2(x - 1, y, range - 1, ignore);
                if (y > 0)
                    HighlightSpecial2(x, y - 1, range - 1, ignore);
                if (x < Math.Sqrt(tiles.Length) - 1)
                    HighlightSpecial2(x + 1, y, range - 1, ignore);
                if (y < Math.Sqrt(tiles.Length) - 1)
                    HighlightSpecial2(x, y + 1, range - 1, ignore);
            }
        }
        /// <summary>
        /// Clear the highlights
        /// </summary>
        public void HighlightDelete()
        {
            foreach (Tile t in tiles)
            {
                t.HighlightSpace = false;
                t.HighlightEnemy = false;
            }
        }
        #endregion




        private void controlCursorHighlight(int x, int y, int range, bool visibility)
        {
            tiles[x, y].HighlightCursor = visibility;

            int negX;
            int negY;
            int posX;
            int posY;

            for (int i = 0; i <= range; i++)
            {
                negX = -i;
                posX = i;
                for (int j = range - i; j >= 0; j--)
                {
                    posY = j;
                    negY = -j;
                    if (x + negX >= 0)
                    {
                        if (negY + y >= 0)
                            tiles[x + negX, y + negY].HighlightCursor = visibility;
                        if (y + posY <= Math.Sqrt(tiles.Length) - 1)
                            tiles[x + negX, y + posY].HighlightCursor = visibility;
                    }
                    if (x + posX <= Math.Sqrt(tiles.Length) - 1)
                    {
                        if (negY + y >= 0)
                            tiles[x + posX, y + negY].HighlightCursor = visibility;
                        if (y + posY <= Math.Sqrt(tiles.Length) - 1)
                            tiles[x + posX, y + posY].HighlightCursor = visibility;

                    }
                }
            }
        }

        #region game methods

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectPos, Color.White);

        }

        #endregion

    }
}
