using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ForeignStars
{
    public class GameOverScreen : Menu
    {
        int playerWin; // integer representing the status of the end of the battle
        string winStatus; // string representing the player's outcome

        /// <summary>
        /// The constructor for the game over screen
        /// </summary>
        /// <param name="numMenuItems">The number of menu items</param>
        /// <param name="startPos">Starting position of the first menu item (y direction)</param>
        /// <param name="sourceLoc">The source location on the sprite sheet (y direction)</param>
        /// /// <param name="playerWin">0 = player lost, 1 = player won, 2 = tie, 3 = game ended early</param>
        public GameOverScreen(int numMenuItems, int startPos, int sourceLoc, int playerWin)
            : base(numMenuItems, startPos, sourceLoc)
        {
            Background = Game1.GameContent.Load<Texture2D>("pauseBG");      // the background is loaded
            switch (playerWin)
            {
                case 0:
                    winStatus = "You won!";
                    break;
                case 1:
                    winStatus = "You lost!";
                    break;
                case 2:
                    winStatus = "It was a tie!";
                    break;
                case 3:
                    winStatus = "The game ended early!";
                    break;
            }
        }

        /// <summary>
        /// The update method for the pause menu
        /// </summary>
        public override void Update()
        {
            if (IsSelectedChoice && InputManager.MouseReleased())   // if the mouse is released on the selected choice (decided upon initial mouse click)
            {
                // currently no options to choose from
            }
            if(InputManager.KeyReady(Keys.Escape))
            {
                Game1.Menus.Pop();
                Game1.Menus.Push(Game1.ScenarioMenu);
                Game1.Menus.Push(Game1.TitleMenu);
            }

            base.Update(); // update the base menu's class
        }

        // draw method (called by the game class)
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch); // draw the base menu
                SpriteBatchExtensions.DrawStringCentered(spriteBatch, Game1.DefFont, "Game Over!-\n-" + winStatus + "-\n-Press escape to continue...", 150, Color.White);
        }
    }
}
