using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ForeignStars
{
    /// <summary>
    /// The menu that displays as the Game1 boots up.
    /// Starting the Game1, exiting the program, options and help are in this menu.
    /// 
    /// @author: Ryan Conrad
    /// </summary>
    class TitleMenu : Menu
    {
        /// <summary>
        /// enum that represents the menu item selected
        /// </summary>
        enum MenuState
        {
            StartGame,
            Options,
            Help,
            Exit,
            OnHelp
        }

        #region fields

        /*
         * RECTANGLES ARRAY ("names" for them):
         * 0: startGameButton
         * 1: optionsButton
         * 2: helpButton
         * 3: exitButton
         * */

        private Texture2D instructionsBG;   // background for the instructions page
        private SoundEffect exitSound;      // custom exit sound
        private bool redrawn;               // bool used to fix the drawing of the exit button when releasing the mouse

        #endregion
        
        /// <summary>
        /// The constructor for the title menu
        /// </summary>
        /// <param name="numMenuItems">The number of menu items</param>
        /// <param name="startPos">Starting position of the first menu item (y direction)</param>
        /// <param name="sourceLoc">The source location on the sprite sheet (y direction)</param>
        public TitleMenu(int numMenuItems, int startPos, int sourceLoc)
            : base(numMenuItems, startPos, sourceLoc)
        {
            Background = Game1.GameContent.Load<Texture2D>("titleBG");              // the title background is loaded
            instructionsBG = Game1.GameContent.Load<Texture2D>("instructionsBG");   // the custom background for instructions is loaded
            exitSound = Game1.GameContent.Load<SoundEffect>("ExitSound");           // the custom exit sound is loaded
        }

        /// <summary>
        /// Updates the title menu
        /// </summary>
        public override void Update()
        {
            if (redrawn)                                            // If the game redrew itself (in this case, one time)
            {
                exitSound.Play();                                   // the exit sound is played
                System.Threading.Thread.Sleep(exitSound.Duration);  // it waits for the sound to finish before exiting the Game1
                Game1.ExitCall();                                   // a static exit method is called
            }
            else if (IsSelectedChoice && InputManager.MouseReleased()) // if an item was selected and the mouse was released
            {
                if (CurrMenuState == (int)MenuState.StartGame)      // if the start game button is chosen
                {
                    Game1.ConfirmSound.Play();                      // play confirm sound
                    Game1.Menus.Pop();                              // the menu is exited and the Game1 starts.
                }
                else if (CurrMenuState == (int)MenuState.Options)   // if the options button is chosen
                {
                    Game1.ConfirmSoundInstance.Play();              // play confirm sound
                    Game1.Menus.Push(Game1.OptionsMenu);            // the options menu is put into the stack
                }
                else if (CurrMenuState == (int)MenuState.Help)      // if the help button is chosen
                {
                    Game1.ConfirmSoundInstance.Play();              // play confirm sound
                    CurrMenuState = (int)MenuState.OnHelp;          // the menu state is set to being on the help screen
                }
                else if (CurrMenuState == (int)MenuState.Exit)      // if the exit button is chosen
                {
                    redrawn = true;                                 // the game will now draw and update one time before exiting.  This should make the button go from red to blue
                }
                else if (CurrMenuState == (int)MenuState.OnHelp)    // if the menu is currently on the help screen when the mouse is released (IsHighlighted is true by default)
                {
                    ++CurrMenuState;                                // the current menu state property handles this and sets it to 0.
                    Game1.ConfirmSoundInstance.Play();              // the confirm sound is played for this occurrence
                }
            }

            base.Update();                                          // the base menu class is updated
        }

        /// <summary>
        /// Draw the title menu
        /// </summary>
        /// <param name="spriteBatch">the current SpriteBatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CurrMenuState == (int)MenuState.OnHelp) // if the menu is on the help screen, draw the instructions
            {
                spriteBatch.Draw(instructionsBG, new Rectangle(0,0,Game1.GameWidth,Game1.GameHeight),Color.White); // the background for the instructions screen is shown
                // DELIMITER: -, add \n between multiple instances of -
                // The delimiter will most likely change.
                SpriteBatchExtensions.DrawStringCentered(spriteBatch, Game1.DefFont, "Instructions:-\n-" +
                    "Your units are the lighter colored units (top left corner).-\n-" +
                    "Move on the grid with the arrow keys or W, A, S and D, or mouse.-\n-" +
                    "Select a unit with ENTER. Go back to the main menu with ESC.-\n-" +
                    "Once a unit is selected, select the desired action with the mouse.-\n-" +
                    "With ENTER and the arrow keys, Select one of the dark green-\n- squares to move, or select a unit to attack (depending on your choice).-\n-" +
                    "Press M to enable/resume music in Game1. Press Q to cancel actions.-\n-" +
                    "Press ENTER to go back to the main menu.", 25, Color.White); // the entire instructions
            }
            else // otherwise, draw the menu items
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
