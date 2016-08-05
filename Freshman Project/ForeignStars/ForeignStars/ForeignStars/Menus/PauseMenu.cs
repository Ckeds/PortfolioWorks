using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ForeignStars
{
    /// <summary>
    /// A pause menu for the main game.
    /// 
    /// @author: Ryan Conrad
    /// </summary>
    class PauseMenu : Menu
    {
        /// <summary>
        /// An enum representing the current options item selected.
        /// </summary>
        enum OptionsState
        {
            Resume,
            Sound,
            Quit,
        }

        //variables for volume control (this is basically a test, as volume can be controlled with - and = anyway).
        bool sliderVisible;     // bool for if the slider is visible
        Texture2D slider;       // texture for the slider
        Rectangle sliderRect;   // rectangle for the slider's position
        int sliderY;            // integer for the slider's Y position
        Texture2D volumeLevel;  // texture for the volume level picture
        Rectangle stopRect;     // rectangle for stopping the song when pressed

        /// <summary>
        /// The constructor for the pause menu
        /// </summary>
        /// <param name="numMenuItems">The number of menu items</param>
        /// <param name="startPos">Starting position of the first menu item (y direction)</param>
        /// <param name="sourceLoc">The source location on the sprite sheet (y direction)</param>
        public PauseMenu(int numMenuItems, int startPos, int sourceLoc)
            : base(numMenuItems, startPos, sourceLoc)
        {
            Background = Game1.GameContent.Load<Texture2D>("pauseBG");      // the background is loaded
            sliderVisible = false;                                          // the slider isn't visible
            slider = Game1.GameContent.Load<Texture2D>("sword");            // the slider's graphic is loaded
            sliderY = (int)((1 - Game1.CreditsSongInstance.Volume) * 430f); // set the slider to a current song's volume
            sliderRect = new Rectangle(64, sliderY, 50, 50);                // the slider's rectangle is initialized
            volumeLevel = Game1.GameContent.Load<Texture2D>("VolumeLevel"); // the volume level texture is loaded
            stopRect = new Rectangle(128, 445, 50, 25);                     // the rectangle for the stop button is loaded
        }

        /// <summary>
        /// The update method for the pause menu
        /// </summary>
        public override void Update()
        {
            if (IsSelectedChoice && InputManager.MouseReleased())   // if the mouse is released on the selected choice (decided upon initial mouse click)
            {
                if (CurrMenuState == (int)OptionsState.Resume)      // if the resume button is clicked
                {
                    Game1.TestSongInstance.Stop();                  // stop the test song
                    Game1.ConfirmSound.Play();                      // play the confirm sound
                    Game1.Menus.Pop();                              // remove this menu
                }
                else if (CurrMenuState == (int)OptionsState.Sound)  // if the sound button is clicked
                {
                    Game1.TestSongInstance.Stop();                  // stop the test song
                    Game1.ConfirmSound.Play();                      // play the confirm sound
                    sliderVisible = !sliderVisible;                 // reverse slider visibility
                }
                else if (CurrMenuState == (int)OptionsState.Quit)   // if the quit button is clicked
                {
                    Game1.TestSongInstance.Stop();                  // stop the test song
                    Game1.ConfirmSoundInstance.Play();              // play the confirm sound
                    Game1.Menus.Pop();                              // remove this menu
                    Game1.Menus.Push(new GameOverScreen(0,int.MaxValue,int.MaxValue,3));         // push game over screen on the stack
                }
            }
            // if the slider is visible
            if (sliderVisible)
            {
                // if the mouse is held and the slider rectangle contains the mouse position
                if (InputManager.MouseHeld() && sliderRect.Contains(InputManager.MousePos))
                {
                    // if the test song isn't playing yet
                    if (Game1.TestSongInstance.State != Microsoft.Xna.Framework.Audio.SoundState.Playing)
                    {
                        Game1.TestSongInstance.Play(); // play the test song
                    }
                    // if the slider rectangle is bounded by the screen
                    if (sliderRect.Y >= 0 && sliderRect.Y <= 430)
                    {
                        sliderY = InputManager.MousePos.Y - sliderRect.Height / 2;  // move the slider's Y position to be center with the mouse
                        if (sliderY > 430) sliderY = 430;                           // upper bound of slider
                        else if (sliderY < 0) sliderY = 0;                          // lower bound of slider

                        // set the volume of each song according to the position of the slider
                        Game1.CreditsSongInstance.Volume = 1 - sliderRect.Y / 430f;
                        Game1.BattleSongInstance.Volume = 1 - sliderRect.Y / 430f;
                        Game1.TestSongInstance.Volume = 1 - sliderRect.Y / 430f;

                        sliderRect = new Rectangle(64, sliderY, 50, 50); // update the slider's rectangle
                    }
                }
                else
                {
                    sliderRect = new Rectangle(64, sliderY, 50, 50); // update the slider's rectangle
                }
                if (stopRect.Contains(InputManager.MousePos) && InputManager.MouseHeld()) // if "stop" is clicked
                {
                    Game1.TestSongInstance.Stop(); // stop the test song
                }
            }

            sliderY = (int)((1 - Game1.CreditsSongInstance.Volume) * 430f); // set the slider to a current song's volume

            base.Update(); // update the base menu's class
        }

        // draw method (called by the game class)
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch); // draw the base menu

            // if the slider is visible, draw its components
            if (sliderVisible)
            {
                spriteBatch.Draw(slider, sliderRect, Color.White);
                spriteBatch.Draw(volumeLevel, new Rectangle(0, 0, 64, 480), Color.White);
                spriteBatch.DrawString(Game1.DefFont,"Volume",new Vector2(128,5),Color.White);
                spriteBatch.DrawString(Game1.DefFont, "Stop", new Vector2(128, 445), Color.White);
            }
        }
    }
}
