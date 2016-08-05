using System;
using System.Collections.Generic;
using Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ForeignStars
{
    /// <summary>
    /// A box that stays up to let the player know of unit's basic info upon hovering, and battle info during battle (i.e. damage dealt).
    /// 
    /// @Author: Jenny Li
    /// </summary>
    public class UpdateBox
    {
        #region attributes

        /// <summary>
        /// The update box that appears in the top-right corner
        /// </summary>
        private Rectangle updateBox;

        /// <summary>
        /// The black frame around the white update box
        /// </summary>
        private Rectangle updateBoxFrame;

        /// <summary>
        /// The "End Turn" button
        /// </summary>
        private Rectangle endTurnButton;

        /// <summary>
        /// Truth value for if update box is visible or not
        /// </summary>
        private bool visible;

        private Texture2D white; // White bg for the unit info bar itself (Just a placeholder for now. Replace as you like with your own bg image sometime.)
        private SpriteFont updateBoxFont;

        // How long the box should display a message for
        private float messageDisplayTime = 2.0f;
        // The amount of time remaining before the updateBox switches messages
        private float remainingDisplayTime = 0.0f;

        // These variables store the damage dealt and received in the attack being reported
        // (These no longer need to be used now that we are using the message queue system)
        private int attackDamage;
        private int counterDamage;

        // End Turn button color
        private Color endTurnButtonColor;

        // A queue used for holding the messages displayed by the update box
        // The messages will get displayed in the order they are stored in the queue
        private Collections.Queue<String> messageQueue;

        // The message currently being displayed
        string currentMessage = "";

        private Unit targetUnit;

        // Whether or not this box is currently displaying the results of an attack
        // This has no use now that the message queue has been implemented
        public bool ShowingAttackResults
        {
            get { return remainingDisplayTime > 0.0f; }
        }

        public bool HasNewMessage
        {
            get { return remainingDisplayTime > 1.2; }
        }

        public Rectangle EndTurnButton
        {
            get { return endTurnButton; }
        }

        public Unit TargetUnit
        {
            get { return targetUnit; }
            set { targetUnit = value; }
        }

        #endregion

        public UpdateBox()
        {
            targetUnit = null;
            white = Game1.GameContent.Load<Texture2D>("infobar_bg");
            updateBoxFont = Game1.GameContent.Load<SpriteFont>("InfoBarFont");

            updateBoxFrame = new Rectangle(Game1.GameWidth - 150, 0, 150, 130);
            updateBox = new Rectangle(updateBoxFrame.X + 10, updateBoxFrame.Y + 10, updateBoxFrame.Width - 20, updateBoxFrame.Height - 20);
            endTurnButton = new Rectangle(updateBoxFrame.X + 10, updateBox.Y + 120, updateBoxFrame.Width - 20, 20);
            endTurnButtonColor = Color.BlueViolet;

            messageQueue = new Collections.Queue<string>();
        }


        public void Update(GameTime gameTime, ref Battle battle)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (remainingDisplayTime > 0.0f)
            {
                remainingDisplayTime -= dt;
            }

            if (remainingDisplayTime <= 0.0f && messageQueue.size() > 0)
            {
                currentMessage = messageQueue.dequeue();
                remainingDisplayTime = messageDisplayTime;
            }
            if (endTurnButton.Contains(InputManager.MousePos))
            {
                if (InputManager.MouseHeld())
                {
                    endTurnButtonColor = Color.Crimson;

                }
                else if (InputManager.MouseReleased())
                {
                    // Switches player's turn number after the End Turn button has been pressed
                    battle.CurrentPlayer.ChangeTurnConditions();

                }
                else endTurnButtonColor = Color.DarkSlateBlue;
            }
            else endTurnButtonColor = Color.BlueViolet;
        }

        public void AddMessage(string message)
        {
            messageQueue.enqueue(message);
        }


        public void DrawCurrentMessage(SpriteBatch spriteBatch)
        {

            if (currentMessage != null && currentMessage != "")
            {
                spriteBatch.Draw(white, updateBoxFrame, Color.Black);
                spriteBatch.Draw(white, updateBox, Color.White);
                spriteBatch.DrawString(updateBoxFont, "UPDATES", new Vector2(updateBox.X + 40, updateBox.Y + 5), Color.Black);

                spriteBatch.DrawString(updateBoxFont, currentMessage, new Vector2(updateBox.X + 10, updateBox.Y + 30), Color.Black);
            }
        }

        // This has no practical use now that the message queue has been implemented
        public void ReportAttack(int dealt, int received)
        {
            remainingDisplayTime = messageDisplayTime;
            attackDamage = dealt;
            counterDamage = received;
        }

        #region Draw methods

        // This has no practical use now that the message queue has been implemented
        public void DrawAttackResults(SpriteBatch spriteBatch)
        {
            if (ShowingAttackResults)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(white, updateBoxFrame, Color.Black);
                spriteBatch.Draw(white, updateBox, Color.White);

                spriteBatch.DrawString(updateBoxFont, "Attack Results", new Vector2(updateBox.X + 25, updateBox.Y + 5), Color.Black);

                spriteBatch.DrawString(updateBoxFont, "Player unit attacks, \ndealing " + attackDamage + " damage!", new Vector2(updateBox.X + 10, updateBox.Y + 30), Color.Blue);
                spriteBatch.DrawString(updateBoxFont, "Enemy unit \ncounterattacks, \ndealing " + counterDamage + " damage!", new Vector2(updateBox.X + 10, updateBox.Y + 60), Color.Red);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Draws the update box and its content
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="targetUnit"></param>
        /// <param name="battle">***PLEASE leave this here for future use: I will be using it to check whose turn it is (playerTurn is in Battle.cs) for the ENDTURN button once AI has been implemented.</param>
        public void Draw(SpriteBatch spriteBatch, Battle battle)
        {
            if (!HasNewMessage)
            {
                spriteBatch.Draw(white, updateBoxFrame, Color.Black);
                spriteBatch.Draw(white, updateBox, Color.White);

                if (targetUnit != null)
                {
                    spriteBatch.DrawString(updateBoxFont, "Army: " + (targetUnit.Army), new Vector2(updateBox.X + 30, updateBox.Y + 5), Color.Black);



                    // The text in the Update Box
                    spriteBatch.DrawString(updateBoxFont, "Level: " + targetUnit.Level, new Vector2(updateBox.X + 10, updateBox.Y + 30), Color.Black);
                    spriteBatch.DrawString(updateBoxFont, "Health: " + targetUnit.Health + " / " + targetUnit.MaxHealth, new Vector2(updateBox.X + 10, updateBox.Y + 45), Color.Black);
                    spriteBatch.DrawString(updateBoxFont, "Attack: " + targetUnit.Attack, new Vector2(updateBox.X + 10, updateBox.Y + 60), Color.Black);
                    spriteBatch.DrawString(updateBoxFont, "Defense: " + targetUnit.Defense, new Vector2(updateBox.X + 10, updateBox.Y + 75), Color.Black);
                }
            }


            // The text in the End Turn button
            spriteBatch.Draw(white, endTurnButton, null, endTurnButtonColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.DrawString(updateBoxFont, "END TURN", new Vector2(endTurnButton.X + 36, endTurnButton.Y + 3), Color.GhostWhite);
        }







        #endregion

    }
}
