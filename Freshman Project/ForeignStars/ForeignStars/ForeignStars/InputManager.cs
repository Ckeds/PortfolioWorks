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
    /// This is the game component that manages input.
    /// @author:Zachary Behrmann
    /// @author:Ryan Conrad
    /// @author:William Powell
    /// </summary>
    public static class InputManager
    {
        static GameTime gameTime;
        static TimeSpan keyTime;
        static TimeSpan previousKeyTime;
        static TimeSpan previousScroll;
        static TimeSpan allowScrolling;
        static KeyboardState currentKeyboardState;
        static KeyboardState previousKeyboardState;
        static GamePadState currentGamepadState1;
        static GamePadState previousGamepadState1;
        static List<GamePadState> padStates;
        static MouseState currentMouseState;
        static MouseState previousMouseState;
        static Rectangle mousePos;

        public static MouseState CurrentMouseState { get { return currentMouseState; } }
        public static Rectangle MousePos { get { return mousePos; } }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public static void Initialize()
        {
            padStates = new List<GamePadState>();
            for (int i = 0; i < 8; i++)
            {
                padStates.Add(new GamePadState());
            }
            keyTime = TimeSpan.FromMilliseconds(10);
            previousKeyTime = TimeSpan.FromMilliseconds(0);
            previousScroll = TimeSpan.FromMilliseconds(0);
            allowScrolling = TimeSpan.FromMilliseconds(200);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public static void Update(GameTime newGameTime)
        {

            for (int i = 0; i < 4; i++)
            {
                padStates[i + 4] = padStates[i];
                padStates[i] = GamePad.GetState((PlayerIndex)i);
            }
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            previousGamepadState1 = currentGamepadState1;
            currentGamepadState1 = GamePad.GetState(PlayerIndex.One);
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            mousePos = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

            gameTime = newGameTime;
        }
        /// <summary>
        /// Test whether we can allow a keypress
        /// </summary>
        /// <param name="testKey">The key to test</param>
        /// <returns></returns>
        public static bool KeyReady(Keys testKey)
        {
            if (currentKeyboardState.IsKeyDown(testKey) && previousKeyboardState.IsKeyDown(testKey))
            {
                if (gameTime.TotalGameTime - previousScroll > allowScrolling)
                {
                    previousScroll = gameTime.TotalGameTime;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Test whether we can allow a button press
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <param name="playerIndex">The controller to check</param>
        /// <returns></returns>
        public static bool ButtonReady(Buttons button, int playerIndex)
        {
            if (padStates[playerIndex].IsButtonDown(button) && padStates[playerIndex + 4].IsButtonDown(button))
            {
                if (gameTime.TotalGameTime - previousScroll > allowScrolling)
                {
                    previousScroll = gameTime.TotalGameTime;
                    return true;
                }
            }
            return false;
        }

        public static bool MouseClicked()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (gameTime.TotalGameTime - previousScroll > allowScrolling)
                {
                    previousScroll = gameTime.TotalGameTime;
                    return true;
                }
            }
            return false;
        }

        public static bool MouseHeld()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        public static bool MouseReleased()
        {
            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                /*if (gameTime.TotalGameTime - previousScroll > allowScrolling)
                {
                    previousScroll = gameTime.TotalGameTime;
                    return true;
                }*/
                return true;
            }
            return false;
        }

        // method for key AND button input (for possible future implementation of xbox controller use)
        public static bool KeyButtonReady(Keys testKey, Buttons testButton, int playerIndex, bool isScroll)
        {
            if (isScroll) // If the button will have the "scrolling" effect
            {
                if ((currentKeyboardState.IsKeyDown(testKey) && previousKeyboardState.IsKeyDown(testKey)) || currentGamepadState1.IsButtonDown(testButton) && currentGamepadState1.IsButtonDown(testButton))
                {
                    if (gameTime.TotalGameTime - previousScroll > allowScrolling)
                    {
                        previousScroll = gameTime.TotalGameTime;
                        return true;
                    }
                }
                return false;
            }
            else // If the button will be evaluated only once
            {
                if ((currentKeyboardState.IsKeyDown(testKey) && previousKeyboardState.IsKeyUp(testKey)) || currentGamepadState1.IsButtonDown(testButton) && currentGamepadState1.IsButtonUp(testButton))
                {
                    return true;
                }
                else return false;
            }
        }
    }
}
