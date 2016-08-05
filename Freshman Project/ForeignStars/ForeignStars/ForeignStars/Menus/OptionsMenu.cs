using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace ForeignStars
{
    /// <summary>
    /// A menu that will display configurable settings for the user
    /// 
    /// @author: Ryan Conrad
    /// </summary>
    class OptionsMenu : Menu
    {
        /// <summary>
        /// An enum representing the current options item selected.
        /// </summary>
        enum OptionsState
        {
            Back,
            MapEditor,
            Credits,
            PlayingCredits
        }

        /// <summary>
        /// Number representing how far the credits have scrolled up
        /// </summary>
        double creditY;

        /// <summary>
        /// Child process where the map editor will run
        /// </summary>
        Process mapEditor;

        /// <summary>
        /// Constructor for the options menu
        /// </summary>
        /// <param name="numMenuItems">The number of menu items</param>
        /// <param name="startPos">Starting position of the first menu item (y direction)</param>
        /// <param name="sourceLoc">The source location on the sprite sheet (y direction)</param>
        public OptionsMenu(int numMenuItems, int startPos, int sourceLoc)
            : base(numMenuItems, startPos, sourceLoc)
        {
            CurrMenuState = 0;
            creditY = 0;
            Background = Game1.GameContent.Load<Texture2D>("optionsBG");
        }

        /// <summary>
        /// Method which starts the map editor and waits for it to finish
        /// </summary>
        public void StartMapEditor()
        {
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            while (d.ToString() != "dynasty")
                d = d.Parent;
            string p = d.FullName + @"\ForeignStarsMapEditor\ForeignStarsMapEditor\ForeignStarsMapEditor\bin\x86\Debug\" + "ForeignStarsMapEditor.exe";

            //Game1.Graphics.ToggleFullScreen(); // make the game not full screen
            mapEditor = new Process();
            mapEditor.StartInfo.UseShellExecute = false;
            mapEditor.StartInfo.RedirectStandardOutput = true;
            mapEditor.StartInfo.RedirectStandardInput = true;
            mapEditor.StartInfo.FileName = p;
            mapEditor.Start();
            string s = null;                                    // make a string s that's null
            while (s != "Exit" && !mapEditor.HasExited)                                   // while s is equal to null
            {
                s = mapEditor.StandardOutput.ReadLine();        // read the standard output of the map editor
            }

            // once the string s is set to something, there could be some options here that change this game based on what the other exe does.
            // It outputs "Exit" to make the program close, but outputting other strings could make this exe do other stuff to.
            // this could potentially speed up the process of importing maps into the game.

            mapEditor.Close(); // close the map editor
            System.Threading.Thread.Sleep(200); // sleep a bit (ensures the other exe finishes
            //Game1.Graphics.ToggleFullScreen(); // make the game full screen again
        }

        /// <summary>
        /// Update method for the options menu
        /// </summary>
        public override void Update()
        {
            if (IsSelectedChoice && InputManager.MouseReleased()) // if the mouse is released on the selected choice (decided upon initial mouse click)
            {
                if (CurrMenuState == (int)OptionsState.Back) // if the back option is selected
                {
                    Game1.ConfirmSoundInstance.Play();  // play a confirmation sound
                    Game1.Menus.Pop();                  // get rid of this menu
                }
                else if (CurrMenuState == (int)OptionsState.MapEditor) // if the map editor option is selected
                {
                    Game1.ConfirmSound.Play();                  // play a confirmation sound
                    Thread.Sleep(Game1.ConfirmSound.Duration);  // wait for the confirmation sound to finish
                    StartMapEditor();                           // start the map editor
                }
                else if (CurrMenuState == (int)OptionsState.Credits) // if the credits option is selected
                {
                    CurrMenuState = (int)OptionsState.PlayingCredits; // the menu is now playing the credits
                    Game1.CreditsSongInstance.Play();                 // the credits song is started
                }
                else if (CurrMenuState == (int)OptionsState.PlayingCredits) // if the menu is currently playing the credits 
                {
                    ++CurrMenuState;                    // the current menu state is added to (which just resets it to 0)
                    Game1.CreditsSongInstance.Stop();   // the credits are stopped
                    Game1.ConfirmSound.Play();          // the confirm sound plays
                    creditY = 0;                        // the credits are reset
                }
            }
        
            base.Update(); // update the base menu class
        }

        /// <summary>
        /// Draw method for the options menu
        /// </summary>
        /// <param name="spriteBatch">the current SpriteBatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CurrMenuState == (int)OptionsState.PlayingCredits) // if the credits are playing, they are drawn instead of the options menu
            {
                // DELIMITER: -, add \n between multiple instances of -
                // This will probably change.
                SpriteBatchExtensions.DrawStringCentered(spriteBatch, Game1.DefFont, "Lead Designer:-\n-Robert Husfeldt-\n-\n-\n-" +
                "Game Architects:-\n-Zachary Behrmann-Ryan Conrad-\n-\n-\n-" +
                "Menu Designer:-\n-Ryan Conrad-\n-\n-\n-" + 
                "GUI Programmer:-\n-Jenny Li-\n-\n-\n-" +
                "Character Designer:-\n-William Powell-\n-\n-\n-" +
                "Sound Designers:-\n-Ryan Conrad-William Powell", Game1.GameHeight - (int)creditY, Color.White);    // credits roll
                creditY += 1243 / (Game1.CreditsSong.Duration.TotalSeconds * 60);                                   // equation for finding scroll speed of the credits

                if (creditY > 1243) // if the credit's offset is greater than an integer based off the draw string size
                {
                    creditY = 0;                        // reset the offset
                    Game1.CreditsSongInstance.Stop();   // stop the credits song
                    ++CurrMenuState;                    // use CurrMenuState property to reset it to 0
                }
            }
            else // otherwise, the menu items are drawn
            {
                base.Draw(spriteBatch); // call base method to draw

                // if the current state is on the map editor and it is highlighted
                if (CurrMenuState == (int)OptionsState.MapEditor && IsHighlighted)
                {
                    // draw a message stating that it's stored in a separate project
                    spriteBatch.DrawString(Game1.DefFont, "(Stored in a \nseparate project)", new Vector2(Game1.GameWidth / 2 + 100, Game1.GameHeight / 2 + 107), Color.White);
                }
            }
        }
    }
}
