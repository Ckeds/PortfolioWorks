using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ForeignStarsMapEditor
{
    public class FileNameEntry : EditorTile
    {
        public OptionsMenu menu;
        public bool highlighted;
        public String name;
        public bool lasthighLighted;
        public Vector2 location;
        private bool typingStarted; // Stated whether the user has started typing valid input for saving a file

        bool backspaceHeld;
        int totalTime;
        int holdTime;

        char cursor;
        int cursorFlashTime;
        int cursorTime;

        public FileNameEntry(MouseRectangle mouse, OptionsMenu ops, Rectangle possible, string name)
        {
            menu = ops;

            if(possible == null)
            {
            rec = new Rectangle((menu.game.gameWidth / 2) - 100, (menu.game.gameHeight / 2) - 100, 200, 200);
            }
            else
            {
                rec = possible;
            }

            location = new Vector2((float)rec.X, (float)rec.Y);
            this.mouse = mouse;
            this.name = name;

            typingStarted = false; // The file name entry starts with no typing yet

            backspaceHeld = false;
            totalTime = 45;
            holdTime = 0;

            cursor = '|';
            cursorFlashTime = 33;
            cursorTime = 0;
        }

        public void update()
        {
            if(menu.game.mouse.Rec.Intersects(rec) && menu.game.mouse.IsMouseClicked())
            {
                highlighted = true;
                cursor = ' ';
            }
            if (highlighted == true && menu.saving)
            {
                UpdateInput();
            }
        }

        public void draw()
        {
            if (menu.saving || menu.loading)
            {
                if (!highlighted)
                {
                    menu.game.spriteBatch.DrawString(menu.game.font, name, location, Color.White);
                }
                else
                {
                    menu.game.spriteBatch.DrawString(menu.game.font, name + cursor, location, Color.Black);
                }
            }
        }

        private void UpdateCursor()
        {
            if (cursorFlashTime - cursorTime > 0)
            {
                cursor = '|';
                cursorTime++;
            }
            else if (cursorFlashTime - cursorTime > -cursorFlashTime)
            {
                cursor = ' ';
                cursorTime++;
            }
            else cursorTime = 0;
        }

        private void UpdateInput()
        {
            Keys[] pressedKeys;
            pressedKeys = menu.game.currentKeyBoard.GetPressedKeys();
            foreach (Keys key in pressedKeys)
            {
                int ascii = (int)key;
                Keys back = (Keys)ascii;
                // ascii -> 65-90 : A-Z, lowercase uses ToString().ToLower(), 48-57 : 0-9, 8 : backspace, 32 : space
                if (((ascii >= 65 && ascii <= 90) || (ascii >= 48 && ascii <= 57) || ascii == 8 || ascii == 32))
                    if (menu.game.lastKeyBoard.IsKeyUp(key))
                    {
                        if (key == Keys.Back && name.Length > 0) // overflows
                        {
                            //name = name.Remove(name.Length - 1, 1);
                            if (!typingStarted)
                            {
                                typingStarted = true; // The typing has started
                                name = ""; // clear the string (it is common for a user to press backspace on an "Enter this here" string in a search bar, expecting it to completely disappear)
                            }
                        }
                        else if (key != Keys.Back)
                            if (key == Keys.Space)
                                name = name.Insert(name.Length, " ");
                            else
                                if (ascii >= 48 && ascii <= 57)
                                    name += key.ToString().Last();
                                else if ((menu.game.currentKeyBoard.IsKeyDown(Keys.LeftShift) || menu.game.currentKeyBoard.IsKeyDown(Keys.RightShift)))
                                    name += CapsCases(key).ToString();
                                else name += (key.ToString()).ToLower();
                    }
            }

            UpdateBackspace();
            UpdateCursor();

            // restarts the string on first valid input only
            if (name.Length > 0 && name.Remove(name.Length - 1, 1) == "EnterName" && !typingStarted)
            {
                name = name.Last().ToString();
                typingStarted = true; // The typing has started
            }
        }

        /// <summary>
        /// This will soon cover ALL the damn cases for holding shift in terms of ascii.
        /// </summary>
        /// <param name="key">The key currently pressed</param>
        /// <returns>A character based on shift held down</returns>
        private char CapsCases(Keys key)
        {
            char character = ' ';
            character = key.ToString().Last();
            return character;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateBackspace()
        {
            menu.game.currentKeyBoard = Keyboard.GetState();
            if (name.Length > 0 && menu.game.currentKeyBoard.IsKeyDown(Keys.Back))
            {
                if (!backspaceHeld)
                {
                    backspaceHeld = true;
                    name = name.Remove(name.Length - 1, 1);
                }
                if (totalTime - holdTime > 0)
                {
                    holdTime++;
                }
                else
                {
                    totalTime = 2;
                    holdTime = 0;
                    name = name.Remove(name.Length - 1, 1);
                }
            }
            else
            {
                totalTime = 45;
                backspaceHeld = false;
            }
        }
    }
}
