using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Collections;

namespace ForeignStars
{
    /// <summary>
    /// A Battle between two armys begun after encountering each other on a strategy map
    /// 
    /// @author: Zachary Behrmann
    /// </summary>
    public class Battle
    {
        private Tile[,] tiles; //A 2D array of tiles - whether this will stay is unknown
        //private Human player; // The player - used to center the camera
        private Player[] players;
        private int mapSize;
        private int turn;
        private Army[] armies;

        private Army testArmy;
        //private Army eTestArmy;

        private Texture2D terrainSheet;
        private Texture2D playerSheet;
        private Texture2D unitSheet;

        

        private bool endTurn;

        private bool playerWin;

        private PauseMenu pauseMenu;
        //private Dictionary<Point, Tile> b;
        //private int numArmies;
        //private Point endPoint;
        //private List<Tile> spawns;

        public Army[] Armies
        {
            get { return armies; }
        }

        public Player CurrentPlayer
        {
            get { return players[turn]; }
        }

        public Player[] Players
        {
            get { return players; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The player - allows us to give player the array of tiles</param>
        /// <param name="terrainSheet">The spriteSheet that tiles will load their texture from</param>
        /// <param name="_armies">An array of armies participating in the battle</param>
        public Battle(Game1 g, Scenario loadedScenario)
        {
            endTurn = false;
            turn = 0;
            players = new Player[loadedScenario.ArmyAmount];
            pauseMenu = new PauseMenu(3, Game1.GameHeight / 2 - 25, 192);

            terrainSheet = Game1.GameContent.Load<Texture2D>("Spritesheet");
            playerSheet = Game1.GameContent.Load<Texture2D>("selector");
            unitSheet = Game1.GameContent.Load<Texture2D>("ArmySpriteSheet");
           
            testArmy = loadedScenario.Armies[0];
            g.InfoBar.YMod = 110;
            g.InfoBar.Visible = false;

            players[0] = new Human(g.InfoBar,Game1.UpdateBox, playerSheet, loadedScenario.Armies[0]);
            players[0].RectPos = new Rectangle(64, 64, GV.TileSize, GV.TileSize);
            players[0].ActualPosition = new Rectangle(64, 64, GV.TileSize, GV.TileSize);
            for (int i = 1; i < players.Length; i++)
            {
                List<Army> allies = new List<Army>();
                List<Army> enemies = new List<Army>();
                for (int j = 0; j < loadedScenario.ArmyAmount; j++)
                {
                    if (j != i)
                    {
                        if (loadedScenario.Armies[j].Team == loadedScenario.Armies[i].Team)
                        {
                            allies.Add(loadedScenario.Armies[j]);
                        }
                        else
                        {
                            enemies.Add(loadedScenario.Armies[j]);
                        }
                    }
                }
                players[i] = new AI(allies, enemies, loadedScenario.Armies[i]);
            }
            g.Camera.Focus = players[0];


            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            while (d.ToString() != "dynasty")
                d = d.Parent;
            string path = d.FullName + @"\GameData\Maps\";



            XmlReader mapReader = XmlReader.Create(path + loadedScenario.MapFile);
            int currentRow = 0;
            int currentCol = 0;


            mapReader.ReadToFollowing("map"); //read to the first map element available

            players[0].Position2 = new Point(160, 160); //place the player at a default point for now

            mapSize = Convert.ToInt32(mapReader.GetAttribute("size")); //get the size of the map (both the length and width of the map are the same, so we use "size")
            tiles = new Tile[mapSize, mapSize]; //Create a 2D array that holds the required amount of tiles
            int lastPlaced = 0;
            currentRow = 0;
            currentCol = 0;
            mapReader.ReadToFollowing("tile"); //Read to the first tile elements of the map so we can begin creating tiles
            do
            {
                int length = Convert.ToInt32(mapReader.GetAttribute("length")); //Get the rectangle size of the tile
                //string[] bound = bounds.Split(','); //Split the values provided into an array
                //int startX = int.Parse(bound[0]); //our starting x location for creating tiles
                //int startY = int.Parse(bound[1]);  //our starting y location for creating tiles
                //int endX = int.Parse(bound[2]);  //our ending x location for creating tiles
                //int endY = int.Parse(bound[3]); //our ending y location for creating tiles
                for (int i = 0; i < length; i++)
                {
                    tiles[currentRow, currentCol] = new Tile(mapReader.GetAttribute("terrain"), terrainSheet, currentRow, currentCol);
                    currentRow++;
                    if (currentRow >= mapSize)
                    {
                        currentRow = 0;
                        currentCol++;
                    }
                    if ((currentCol == mapSize) && (currentRow == mapSize))
                        break;
                }
            } while (mapReader.ReadToNextSibling("tile"));

            mapReader.ReadToNextSibling("spawns");
            //spawns = new List<Tile>();
            mapReader.ReadToFollowing("tile");
            do
            {
                string bounds = mapReader.GetAttribute("location");
                string[] bound = bounds.Split(',');
                int startX = int.Parse(bound[0]);
                int startY = int.Parse(bound[1]);
                int endX = int.Parse(bound[2]);
                int endY = int.Parse(bound[3]);

                for (int i = startX; i < endX; i++)
                {
                    for (int j = startY; j < endY; j++)
                    {
                        //_tiles[i, j].Spawn = true;
                        //spawns.Add(_tiles[i, j]);
                    }
                }
            } while (mapReader.ReadToNextSibling("tile")); //head to the next tile element if one exists, otherwise we exit this loop


            mapReader.Close();

            foreach (Player p in players)
            {
                p.SetTiles(tiles);
                foreach (Unit u in p.OwnArmy.Units)
                {
                        tiles[u.UnitBox.X / GV.TileSize, u.UnitBox.Y / GV.TileSize].Unit = u;
                        u.UnitSprite = unitSheet;
                        u.UnitPic = unitSheet;  
                }
            }
        }

        public void Update(Game1 g)
        {
            if (InputManager.KeyReady(Keys.Escape))
            {
                // TODO:
                // Make a "Save Progress?" confirmation message come up.
                // Pressing no will actually erase the game's current state
                // otherwise, the menu will just be pushed on and "Start Game" will be changed to "Resume Game"
                Game1.PauseSound.Play();
                Game1.Menus.Push(pauseMenu);
            }
            /*
            if (players[turn] is Human)
            {
                endTurn = (players[turn] as Human).Update(g.Camera, boxxy.EndTurnButton);
            }
            else
            {
                endTurn = players[turn].Update(g);
            }
              
             if (endTurn)
            {
                turn++;
                if (turn >= players.Count)
                    turn = 0;
            }
    */
            if (players[turn].Update(g.Camera))
            {
                turn++;
                if (turn >= players.Length)
                    turn = 0;
            }

            for (int i = 0; i < players.Length-1; i++)
            {
                if (players[i + 1].OwnArmy.Units.Count == 0)
                {
                    playerWin = true;
                }
                else playerWin = false;
            }
            if (players[0].OwnArmy.Units.Count == 0)
            {
                if(playerWin)
                    Game1.Menus.Push(new GameOverScreen(0, int.MaxValue, int.MaxValue, 2));         // push game over screen on the stack
                else Game1.Menus.Push(new GameOverScreen(0, int.MaxValue, int.MaxValue, 1));         // push game over screen on the stack
            }
            if (playerWin)
            {
                Game1.Menus.Push(new GameOverScreen(0, int.MaxValue, int.MaxValue, 0));         // push game over screen on the stack
            }
        }

        /// <summary>
        /// Draw the battle (draws tiles)
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, InfoBar infoBar, UpdateBox updateBox, Camera2D camera)
        {
            /*for(int i = 0; i < _mapSize * _mapSize; i++)
                    _tiles[i].Draw(spriteBatch);*/
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    tiles[i, j].Draw(spriteBatch);
                }
            }
            if (players[turn] is Human)
            {
                (players[turn] as Human).Draw(spriteBatch);
            }
        }
    }
}
