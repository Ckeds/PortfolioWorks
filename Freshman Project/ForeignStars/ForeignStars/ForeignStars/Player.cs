using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Collections.PriorityQueue;

namespace ForeignStars
{
    public abstract class Player : IFocusable
    {

        const int moveSpeed = 4;

        #region attributes



        /// <summary>
        /// The army the player controls
        /// </summary>
        protected Army ownArmy;


        /// <summary>
        /// The array of tiles of the current battle
        /// </summary>
        protected Tile[,] tiles;

        protected Unit currentUnit;

        protected int currentGridX;
        protected int currentGridY;


        /// <summary>
        /// The tile a unit is moving to/attacking/etc.
        /// </summary>
        protected Tile selectedTile;

        /// <summary>
        /// The current tile we are inspecting
        /// </summary>
        protected Tile currentTile;

        protected Stack<PathNode> beatenPath;
        protected PathNode nextNode;

        protected Vector2 position;
        protected Point position2; //

        /// <summary>
        /// The actual position we are at in the game
        /// </summary>
        protected Rectangle actualPos;

        /// <summary>
        /// The Camera-Lockable position we are in the game
        /// </summary>
        protected Rectangle rectPos;

        #endregion

        #region properties

        public Army OwnArmy
        {
            get { return ownArmy; }
        }


        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The 2D Array of tiles the player cursor traverses
        /// </summary>
        public Tile[,] Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
        /// <summary>
        /// The position of the player icon on the screen
        /// </summary>
        public Rectangle RectPos
        {
            get { return rectPos; }
            set { rectPos = value; }
        }

        public Rectangle ActualPosition
        {
            get { return actualPos; }
            set { actualPos = value; }
        }
        /// <summary>
        /// An alternate means of getting the current position
        /// </summary>
        public Point Position2
        {
            get { return position2; }
            set { position2 = value; }
        }

        public int Position2X
        {
            get { return position2.X; }
            set { position2.X = value; }
        }
        public int Position2Y
        {
            get { return position2.Y; }
            set { position2.Y = value; }
        }

        /// <summary>
        /// Returns the currently selected unit
        /// </summary>
        public Unit CurrentUnit
        {
            get { return currentUnit; }
        }

        public Unit HighlightedUnit
        {
            get
            {
                if (currentUnit != null)
                    return currentUnit;
                else
                    return tiles[currentGridX, currentGridY].Unit;
            }
        }



        #endregion



        #region Constructor

        public Player() { }

        #endregion



        #region methods

        protected virtual Stack<PathNode> AStar(Tile source, Tile destination)
        {
            List<Tile> closed = new List<Tile>();
            List<Tile> toDetermine;
            List<Tile> toAdd = new List<Tile>();
            Stack<PathNode> finalPath = null;
            MinPriorityQueue<PathNode> open = new MinPriorityQueue<PathNode>();
            List<PathNode> openList = new List<PathNode>();
            open.Enqueue(new PathNode(source, destination));
            openList.Add(open.Peek());
            PathNode currentNode = null;
            bool endReached = false;
            do
            {
                currentNode = open.Dequeue();
                openList.Remove(currentNode);
                if (currentNode.HCost == 0)
                {
                    endReached = true;
                    finalPath = currentNode.GetPath();
                }
                else
                {
                    closed.Add(tiles[currentNode.Current.X, currentNode.Current.Y]);
                    toDetermine = getAdjacent(currentNode);
                    foreach (Tile t in toDetermine)
                    {
                        bool remove = false;
                        if (t.Collision > source.Unit.Collision)
                            remove = true;
                        if (t.HasUnit())
                            if (t.Unit.Army != source.Unit.Army) //added so that AI works
                                remove = true;
                        if (closed.Contains(t))
                            remove = true;
                        //Add this if I want to have no duplicate pathnodes (currently, 
                        //multiple exist with different source nodes
                        /*
                        PathNode temp = new PathNode(t.GridwiseLocation, currentNode, destination.GridwiseLocation);
                        foreach (PathNode p in openList)
                        {
                            if (p.Current == temp.Current)
                            {
                                if (p.GCost > temp.GCost)
                                {
                                    p.Source = temp.Source;

                                    remove = true;
                                }
                            }

                        }'*/

                        if (!remove)
                            toAdd.Add(t);
                    }

                    foreach (Tile t in toAdd)
                    {
                        PathNode temp = new PathNode(t.GridwiseLocation, currentNode, destination.GridwiseLocation);
                        open.Enqueue(temp);
                    }
                    toAdd.Clear();
                }
            } while (!endReached);

            return finalPath;
        }

        protected List<Tile> getAdjacent(PathNode origin)
        {
            List<Tile> adjacent = new List<Tile>();
            if (origin.Current.X + 1 < Math.Sqrt(tiles.Length))
                adjacent.Add(tiles[origin.Current.X + 1, origin.Current.Y]);
            if (origin.Current.X - 1 >= 0)
                adjacent.Add(tiles[origin.Current.X - 1, origin.Current.Y]);
            if (origin.Current.Y + 1 < Math.Sqrt(tiles.Length))
                adjacent.Add(tiles[origin.Current.X, origin.Current.Y + 1]);
            if (origin.Current.Y - 1 >= 0)
                adjacent.Add(tiles[origin.Current.X, origin.Current.Y - 1]);
            return adjacent;
        }

        /// <summary>
        /// Animates current unit
        /// </summary>
        protected void AnimateMovement()
        {
            if (beatenPath == null)
            {
                beatenPath = AStar(currentTile, selectedTile);
                currentUnit.Movement -= (beatenPath.Count - 1);
                nextNode = beatenPath.Pop();
            }
            bool xSet = true;
            bool ySet = true;
            if (currentUnit.UnitBoxX != (nextNode.Current.X * GV.TileSize))
            {
                xSet = false;
                if (currentUnit.UnitBoxX > (nextNode.Current.X * GV.TileSize))
                {
                    currentUnit.SpriteSourceX = 32 + (128 * int.Parse(currentUnit.Army.ToString()));
                    currentUnit.UnitBoxX += -moveSpeed;
                    
                }
                else
                {
                    currentUnit.SpriteSourceX = (128 * int.Parse(currentUnit.Army.ToString()));
                    currentUnit.UnitBoxX += moveSpeed;
                }

            }
            if (currentUnit.UnitBoxY != (nextNode.Current.Y * GV.TileSize))
            {
                ySet = false;
                if (currentUnit.UnitBoxY > (nextNode.Current.Y * GV.TileSize))
                {
                    currentUnit.SpriteSourceX = 64 + (128 * int.Parse(currentUnit.Army.ToString()));
                    currentUnit.UnitBoxY += -moveSpeed;
                }
                else
                {
                    currentUnit.SpriteSourceX = 96 + (128 * int.Parse(currentUnit.Army.ToString()));
                    currentUnit.UnitBoxY += moveSpeed;
                }
            }
            if (ySet && xSet)
            {
                if (beatenPath.Count != 0)
                    nextNode = beatenPath.Pop();
                else
                {
                    AfterAnimate();

                }

            }

        }

        abstract public void ChangeTurnConditions();


        abstract protected void AfterAnimate();

        //abstract protected void Attack;

        abstract public void SetTiles(Tile[,] tileArray);

        #endregion

        #region game methods


        public virtual bool Update(Camera2D camera)
        {
            return true;
        }

        //public abstract void Draw(SpriteBatch spriteBatch);

        #endregion
    }
}
