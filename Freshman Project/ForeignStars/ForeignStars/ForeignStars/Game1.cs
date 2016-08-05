using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ForeignStars
{

    /// <summary>
    /// This is the main type for your game
    /// 
    /// Team Dynasty
    /// @author: Rob Husfeldt
    /// @author: Jenny Li
    /// @author: William Powell
    /// @author: Ryan Conrad
    /// @author: Zachary Behrmann
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        static ContentManager gameContent; // This allows images to be loaded from any class, hence the 'static'. It's bothersome to have to tell every class what sprites it has, whenever a new object of the class gets created, by passing them in from the constructor. Every single time. So this lets any class (in the whole project) be able to load things in, so that no other classes have to know about the images.

        // game height and width attributes
        static int GAME_WIDTH;
        static int GAME_HEIGHT;

        static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static SpriteFont defFont;
        Camera2D camera;

        // menus implemented as a Stack
        static Collections.Stack<Menu> menus;
        static TitleMenu titleMenu;
        static OptionsMenu optionsMenu;
        static ScenarioMenu scenarioMenu;
        private Menu currMenu;
        static Texture2D menuButtons;

        //Texture2D terrainSheet;
        //Texture2D playerSheet;

        InfoBar infoBar;
        static UpdateBox updateBox;
        static Battle battle;

        // Make armies have linked lists (instead of just lists) of units?
        //Army testArmy;
        //Army eTestArmy;

        bool debugMode;
        Human debugPlayer;
        Army[] armyArray;
        //Texture2D infantryTexture;

        //sounds to be loaded in the game. instances with similar names are for songs instead of sound effects (ability to stop/resume music)
        static SoundEffect testSong;
        static SoundEffectInstance testSongInstance;
        static SoundEffect chooseSound;
        static SoundEffect confirmSound;
        static SoundEffectInstance confirmSoundInstance;
        static SoundEffect pauseSound;
        static SoundEffect clickSound;
        static SoundEffect creditsSong;
        static SoundEffect battleSong;
        static SoundEffectInstance creditsSongInstance;
        static SoundEffectInstance battleSongInstance;
        static SoundEffect mouseHoverSound;    // sound played when menu item becomes highlighted
        
        // properties
        public static SpriteFont DefFont { get { return defFont; } }
        public static SoundEffect ChooseSound { get { return chooseSound; } }
        public static SoundEffect TestSong { get { return testSong; } }
        public static SoundEffectInstance TestSongInstance { get { return testSongInstance; } }
        public static SoundEffect ConfirmSound { get { return confirmSound; } }
        public static SoundEffectInstance ConfirmSoundInstance { get { return confirmSoundInstance; } }
        public static SoundEffect PauseSound { get { return pauseSound; } }
        public static SoundEffect ClickSound { get { return clickSound; } }
        public static SoundEffect CreditsSong { get { return creditsSong; } }
        public static SoundEffectInstance CreditsSongInstance { get { return creditsSongInstance; } }
        public static SoundEffectInstance BattleSongInstance { get { return battleSongInstance; } }
        public static SoundEffect MouseHoverSound { get { return mouseHoverSound; } }
        public static Collections.Stack<Menu> Menus { get { return menus; } set { menus = value; } }
        public static Menu OptionsMenu { get { return optionsMenu; } }
        public static Menu TitleMenu { get { return titleMenu; } }
        public static Menu ScenarioMenu { get { return scenarioMenu; } }
        public InfoBar InfoBar { get { return infoBar; } }
        public static UpdateBox UpdateBox { get { return updateBox; } }
        public Camera2D Camera { get { return camera; } set { camera = value; } }
        public static Battle Battle { get { return battle; } set { battle = value; } }
        
        // Since we don't have more than two players, I'm just going to make 1 = Player turn and -1 = Enemy Turn
        private int numPlayers;
        private Scenario selectedScenario;

        static bool exiting;

        private Texture2D battleBG;

        // Represents which team is selected
        static int team; // USE THE PROPERTY ONLY

        // These properties exist for these static variables to prevent the alteration of them outside of the class (encapsulation).
        /// <summary>
        /// This property is so that this game's ContentManager can be accessed in all other classes.
        /// (Originally created for InfoBar.)
        /// </summary>
        public static ContentManager GameContent
        {
            get { return gameContent; } 
        }
        /// <summary>
        /// Accessors created for GameHeight so that it's easier to draw 
        /// InfoBar to the bottom of the screen (GameHeight (minus) info bar's height).
        /// </summary>
        public static int GameHeight
        {
            get { return GAME_HEIGHT; }
        }
        /// <summary>
        /// Accessors created for GameHeight so that it's easier to draw 
        /// InfoBar to the bottom of the screen (GameHeight (minus) info bar's height).
        /// </summary>
        public static int GameWidth
        {
            get { return GAME_WIDTH; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static Texture2D MenuButtons
        {
            get { return menuButtons; }
        }

        public static GraphicsDeviceManager Graphics { get { return graphics; } set { graphics = value; } }




        // Property that restricts what team can equal
        public static int Team
        {
            get { return team; }
            set
            {
                if (team + 1 > 1)
                    team = 0;
                else if (team - 1 < 0)
                    team = 1;
                else team = value;
            }
        }



        public Game1()
        {
            gameContent = Content; // *refer to the "static ContentManager gameContent" *
            graphics = new GraphicsDeviceManager(this);

            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = 854;
            graphics.PreferredBackBufferHeight = 480;

            Content.RootDirectory = "Content";
            camera = new Camera2D(this);
            //InputManager = new InputManager(this);

            // It's player one's turn on each startup of the game


            //Components.Add(camera); // Add camera to the components to be updated when base.update is called
            //Components.Add(InputManager); // Add input manager to the list of of components to be updated when base.update is called

            //graphics.IsFullScreen = true;

            exiting = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic 
            IsMouseVisible = true;

            GAME_WIDTH = GraphicsDevice.Viewport.Width;
            GAME_HEIGHT = GraphicsDevice.Viewport.Height;
            defFont = Content.Load<SpriteFont>("DefaultFont");
            testSong = Content.Load<SoundEffect>("TestSong");
            testSongInstance = testSong.CreateInstance();
            chooseSound = Content.Load<SoundEffect>("ChooseItem");
            confirmSound = Content.Load<SoundEffect>("ConfirmItem");
            confirmSoundInstance = confirmSound.CreateInstance();
            pauseSound = Content.Load<SoundEffect>("PauseSound");
            clickSound = Content.Load<SoundEffect>("ClickSound");
            creditsSong = Content.Load<SoundEffect>("CreditsSong");
            battleSong = Content.Load<SoundEffect>("BattleSong");
            creditsSongInstance = creditsSong.CreateInstance();
            battleSongInstance = battleSong.CreateInstance();
            mouseHoverSound = Game1.GameContent.Load<SoundEffect>("MouseHoverSound");
            titleMenu = new TitleMenu(4, Game1.GameHeight / 2 + 25, 0);
            optionsMenu = new OptionsMenu(3, Game1.GameHeight / 2 + 75, 96);
            scenarioMenu = new ScenarioMenu();
            menus = new Collections.Stack<Menu>();
            
            menus.Push(scenarioMenu);
            menus.Push(titleMenu);

            InputManager.Initialize();
            base.Initialize();
            SpriteSheetLookup.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            infoBar = new InfoBar(camera);
            updateBox = new UpdateBox();



            battleBG = Game1.GameContent.Load<Texture2D>("battleBG");

            menuButtons = Game1.GameContent.Load<Texture2D>("ButtonsSpriteSheet");      // initialize texture array to the number of menu items

            /*
            testArmy = new Army();
            testArmy.Units.Add(new Infantry(infantryTexture));
            testArmy.Units.Add(new Infantry(infantryTexture));
            testArmy.Units.Add(new Infantry(infantryTexture));
            armyArray = new Army[2];
            armyArray[0] = testArmy;
            player = new Player(infoBar, playerSheet, testArmy);
            player.RectPos = new Rectangle(64, 64, 32, 32);
            eTestArmy = new Army();
            eTestArmy.Units.Add(new Infantry(infantryTexture));
            eTestArmy.Units.Add(new Infantry(infantryTexture));
            eTestArmy.Units.Add(new Infantry(infantryTexture));
            foreach (Unit u in eTestArmy.Units)
                u.Enemy = true;
            armyArray[1] = eTestArmy;
            */

            //camera.Focus = player;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public static void ExitCall()
        {
            exiting = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // VOLUME CONTROL (with plus and minus keys next to backspace)
            if (InputManager.KeyReady(Keys.OemPlus) && creditsSongInstance.Volume < 1)
            {
                if (creditsSongInstance.Volume + .1f < 1)
                    creditsSongInstance.Volume += .1f;
                else creditsSongInstance.Volume = 1f;
                if (battleSongInstance.Volume + .1f < 1)
                    battleSongInstance.Volume += .1f;
                else battleSongInstance.Volume = 1f;
            }
            if (InputManager.KeyReady(Keys.OemMinus) && creditsSongInstance.Volume > 0)
            {
                if (creditsSongInstance.Volume - .1f > 0)
                    creditsSongInstance.Volume -= .1f;
                else creditsSongInstance.Volume = 0f;
                if (battleSongInstance.Volume - .1f > 0)
                    battleSongInstance.Volume -= .1f;
                else battleSongInstance.Volume = 0f;
            }
            


            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (menus.Count > 0) // if there's a menu
            {
                currMenu = menus.Peek();
                if (currMenu is ScenarioMenu)
                {
                    selectedScenario = (currMenu as ScenarioMenu).Update(this);
                    if (selectedScenario != null)
                    {
                        battle = new Battle(this, selectedScenario);
                        if (!Components.Contains(camera))
                            Components.Add(camera); // Add camera to the components to be updated when base.update is called
                        camera.ResetPosition();
                    }
                }
                else
                {
                    currMenu.Update();
                }

                if (battleSongInstance.State == SoundState.Playing)
                    battleSongInstance.Stop();
            }
            else
            {
                ///  Toggle battle music on/off with the "M" key!!!!
                if (battleSongInstance.State == SoundState.Playing)
                {
                    if (InputManager.KeyReady(Keys.M))
                    {
                        battleSongInstance.Pause();
                    }
                }
                else if (battleSongInstance.State == SoundState.Paused)
                {
                    if (InputManager.KeyReady(Keys.M))
                    {
                        battleSongInstance.Resume();
                    }
                }
                else if (battleSongInstance.State == SoundState.Stopped)
                {
                    if (InputManager.KeyReady(Keys.M))
                    {
                        battleSongInstance.Play();
                    }
                }


                battle.Update(this);
                //if (Components.Contains(camera)) Components.Remove(camera);
                infoBar.Update();
                updateBox.Update(gameTime, ref battle);
            }
            InputManager.Update(gameTime);

            if (exiting == true)
            {
                Exit();
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            /* TESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTEST
            InfoBar ibar = new InfoBar(this);
            Infantry s = new Infantry();
            spriteBatch.Begin();
            ibar.Draw(spriteBatch, s);

            spriteBatch.End();
            // TESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTEST
            */
            // TODO: Add your drawing code here

            if (menus.Count > 0) // if there is a menu
            {
                if (optionsMenu.CurrMenuState == 3) // if it's on the credits
                    GraphicsDevice.Clear(Color.Black); // the background is a different color
                else GraphicsDevice.Clear(Color.CornflowerBlue); // otherwise, it's the default
                spriteBatch.Begin();
                menus.Peek().Draw(spriteBatch); // the menu on top is drawn
                spriteBatch.End();
            }
            else // otherwise, the battle has started
            {
                GraphicsDevice.Clear(Color.Aqua);

                spriteBatch.Begin();
                spriteBatch.Draw(battleBG, new Rectangle(0, 0, Game1.GameWidth, Game1.GameHeight), Color.White);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, camera.Transform);

                battle.Draw(spriteBatch, infoBar, updateBox, camera);

                spriteBatch.End();

                spriteBatch.Begin();

                updateBox.Draw(spriteBatch, battle);

                infoBar.Draw(spriteBatch, battle.CurrentPlayer.CurrentUnit); // this has its own spriteBatch.Begin() and End() calls. This will most likely change

                // (Checks to see the timer for how long the attacks show up for) So, if the attacks are still supposed to be displayed,
                if (true)
                {
                    // then display the results.
                    //updateBox.DrawAttackResults(spriteBatch);
                    updateBox.DrawCurrentMessage(spriteBatch);
                }

                if (battle.CurrentPlayer != null)
                //if (battle.CurrentPlayer.HighlightedUnit != null)                
                {    // the unit's info will show up in the UpdateBox.
                    //updateBox.Draw(spriteBatch, battle.CurrentPlayer.HighlightedUnit, battle);                

                    // Same as above, but FOR MOUSE: (Obviously won't work simultaneously with keyboard - ONE OR THE OTHER)
                    if (true)
                    {
                        for (int i = 0; i < battle.Players.Length; i++)
                        {
                            foreach (Unit u in battle.Players[i].OwnArmy.Units)
                            {
                                if (new Rectangle(u.UnitBox.X + (int)(camera.Origin.X - camera.Position.X), u.UnitBox.Y + (int)(camera.Origin.Y - camera.Position.Y), u.UnitBox.Width, u.UnitBox.Height).Contains(InputManager.MousePos))
                                {
                                    // the unit's info will show up in the UpdateBox.
                                    updateBox.Draw(spriteBatch, battle);
                                }
                            }
                        }
                    }
                }

                // FOR KEYBOARD: If the main selector is hovered over any unit, 
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
