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

namespace ForeignStarsMapEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public KeyboardState lastKeyBoard;
        public KeyboardState currentKeyBoard;
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont font;
        public MouseRectangle mouse;
        bool leftshiftkeyPressed = false;
        bool rightshiftkeyPressed = false;
        public bool sizeChosen = false;
        public bool paused = false;
        public Texture2D spriteSheet;
        public List<Rectangle> tiles;
        public List<String> tileIDs;
        public OptionsMenu menu;
        public EditableMap map;
        public int gameWidth;
        public int gameHeight;

        public int tileSize;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.PreferredBackBufferWidth = 1224;
            //graphics.PreferredBackBufferHeight = 924;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            gameWidth = graphics.PreferredBackBufferWidth;
            gameHeight = graphics.PreferredBackBufferHeight;
            graphics.ApplyChanges();
            
            tiles = new List<Rectangle>();
            tileIDs = new List<String>();

            graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteSheet = Content.Load<Texture2D>("Sprites/Spritesheet");
            font = Content.Load<SpriteFont>("Fonts/Font");
            IsMouseVisible = true;
            mouse = new MouseRectangle(spriteSheet);
            menu = new OptionsMenu(this);
            for(int y = 0; y < 4; y++)
            {
                for(int x = 0; x < 5; x++)
                {
                    tiles.Add(new Rectangle(32 * x, (32 * y) + 1, 32, 31));
                }
            }

                tileIDs.Add( "grass");    
                tileIDs.Add( "mountaingrass");   
                tileIDs.Add( "hill");  
                tileIDs.Add( "NWroadgrass");   
                tileIDs.Add( "NWroadwater");   
                tileIDs.Add( "NSroadgrass");   
                tileIDs.Add( "WEroadgrass");    
                tileIDs.Add( "NEroadgrass");  
                tileIDs.Add( "SEroadgrass");
                tileIDs.Add( "SWroadgrass");
                tileIDs.Add( "NSroadwater");
                tileIDs.Add( "WEroadwater");
                tileIDs.Add( "NEroadwater");
                tileIDs.Add("SEroadwater");
                tileIDs.Add("SWroadwater");
                tileIDs.Add( "mountainsand");
                tileIDs.Add( "sand");               
                tileIDs.Add( "water");
                tileIDs.Add("water");
                tileIDs.Add("water");
          
                    

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Need the keyboard stat to pass into various following lines.
            KeyboardState ks = Keyboard.GetState();

            if (currentKeyBoard != null)
            { lastKeyBoard = currentKeyBoard; }

            currentKeyBoard = ks;


            //Self explanatory.
            if (ks.IsKeyDown(Keys.Escape) && sizeChosen)
            { paused = true;}
            if (ks.IsKeyDown(Keys.Q) && paused && sizeChosen)
            { paused = false; }

            


            //Before it moves the mouseRectangle, it sees if the mouse is locked on one axis or another.
            LockMouseX(ks);
            LockMouseY(ks);
            mouse.FollowMouse();


           if (!sizeChosen)
            {
                menu.Update();
            }
            else if (!paused)
           {
               map.Update();
            }
           else if (paused)
           {
               menu.Update();
               /*Save/Load/Name Buttons*/
           }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Tomato);

            //The draw methods in the objects call spriteBatch.Draw so can just be
            //slipped between Begin and End.
            spriteBatch.Begin();


            if (!sizeChosen || (sizeChosen && paused))
            {
                menu.Draw();
            }
            else
            {
                map.Draw(spriteBatch);
            }

            spriteBatch.End();

                

            base.Draw(gameTime);
        }


        /// <summary>
        /// If the left shift key is being held down, the X cooridinates of the mouse
        /// when first held down is locked, but the Y is allowed to travel.
        /// </summary>
        /// <param name="ks">Keyboard state of update.</param>
        void LockMouseX(KeyboardState ks)
        {
            MouseState ms = Mouse.GetState();


            //If the shiftkey hasn't been pressed yet, the lockedX has 'no' value, and Leftshift is down,
            //give the lockX a value and let the program know left shiftkey has been pressed.
            if (!leftshiftkeyPressed && mouse.lockedX == 0 && ks.IsKeyDown(Keys.LeftShift))
            {
                leftshiftkeyPressed = true;
                
                mouse.lockedX = ms.X;
            }

            //If it has been pressed but the shiftkey is no longer down, unlock it.
            else if (leftshiftkeyPressed && !ks.IsKeyDown(Keys.LeftShift))
            {
                mouse.lockedX = 0;
                leftshiftkeyPressed = false;
            }
            //If it's locked (would've had to been) and the shiftkey's still down,
            //move the Mouse to the lockedX value but let it be wherever it is on the Y.
            else if (mouse.lockedX != 0)
            {
                Mouse.SetPosition(mouse.lockedX, ms.Y);
            }
        }
        
        /// <summary>
        ///  If the right shift key is being held down, the Y cooridinates of the mouse
        /// when first held down is locked, but the X is allowed to travel.
        /// </summary>
        /// <param name="ks">Keyboard state at time of update</param>
        void LockMouseY(KeyboardState ks)
        {
            //See LockMouseX comments, same concept. Different Button/Axis.
            MouseState ms = Mouse.GetState();

            if (!rightshiftkeyPressed && mouse.lockedY == 0 && ks.IsKeyDown(Keys.RightShift))
            {
                rightshiftkeyPressed = true;
               
                mouse.lockedY = ms.Y;
            }
            else if (rightshiftkeyPressed && !ks.IsKeyDown(Keys.RightShift))
            {
                mouse.lockedY = 0;
                rightshiftkeyPressed = false;
            }
            else if (mouse.lockedY != 0)
            {
                Mouse.SetPosition(ms.X, mouse.lockedY);
            }
        }
    }
}
