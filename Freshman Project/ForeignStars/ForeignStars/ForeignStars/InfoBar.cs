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

namespace ForeignStars
{
    /// <summary>
    /// A bar that displays information about a selected unit and provides ways to perform unit actions
    /// 
    /// @author: Jenny Li
    /// @author: Ryan Conrad
    /// </summary>
    public class InfoBar
    {
        // @@It pops up when any one unit is selected (otherwise it doesn't - use IF STATEMENT). When the unit has been selected, the cursor/selector
        // is automatically on the "Move" button. Using the arrow buttons, the player can get to the rest of the buttons (Down: Attack, Right: Special,
        // Left: back to Move). When the "Special" button has been selected, a new window with a list of special abilities unique to that unit
        // will pop up ---> and as the player moves up/down in that list, a text description of each ability (as it is highlighted) will appear
        // in the black Description area of the InfoBar. 
        // @@Each unit, as it is selected, will have its Name and current Level appear above the unit's portrait on the far left.
        // @@In the Stats box, have Health/MaxHealth [] Movement/MaxMovement [] Attack [] Defense [] Members/FullMembers.
        // @@This InfoBar must be hooked up the currentUnit that the Player has selected.

        #region Action Enum
        /// <summary>
        /// A enumeration of actions we can choose from the InfoBar
        /// </summary>
        public enum Action
        {
            None,
            Move,
            Attack,
            Special,
            Animate
        }
        #endregion

        #region attributes

        // ATTRIBUTES
        /// <summary>
        ///  the font used in the info bar
        /// </summary>
        private SpriteFont infobarFont;

        /// <summary>
        /// The unit info bar itself
        /// </summary>
        private Rectangle bgUnitBar;

        /// <summary>
        ///  The rectangle that will display unit pictures
        /// </summary>
        private Rectangle unitPicRect;

        /// <summary>
        /// The rectangle that will be behind the unit picture
        /// </summary>
        private Rectangle unitPicBackRect;

        /// <summary>
        /// The outer black border rectangle (contains white abilities+buttons rectangle)
        /// </summary>
        private Rectangle ctrOutRect;

        /// <summary>
        /// The inner white rectangle in the center
        /// </summary>
        private Rectangle ctrInRect;

        /// <summary>
        /// The black rectangle inside the 'ctrInRect'
        /// </summary>
        private Rectangle ctrInBlackRect;

        /// <summary>
        /// The outer black border rectangle (containing the white stats rectangle)
        /// </summary>
        private Rectangle statRectBorder;

        /// <summary>
        /// The rectangle on the far right with stats
        /// </summary>
        private Rectangle statRect;

        /// <summary>
        /// An int that represents how far down the info bar is from it's normal visible position.
        /// </summary>
        private int yMod;

        // BUTTONS (represented by rectangles)
        private Rectangle moveButton;
        private Rectangle atkButton;
        private Rectangle specialButton;

        // button colors
        private Color moveButtonColor;
        private Color atkButtonColor;
        private Color specialButtonColor;

        private Texture2D white; // White bg for the unit info bar itself (Just a placeholder for now. Replace as you like with your own bg image sometime.)

        /// <summary>
        /// mouse's position represented as a rectangle.
        /// </summary>
        private Rectangle mousePos;

        /// <summary>
        /// Truth value for whether or not info bar is visible
        /// </summary>
        private bool visible;

        /// <summary>
        /// The info bar's main position in the Y direction (game screen height minus info bar height).
        /// </summary>
        private int bgUnitBarY;

        /// <summary>
        /// Our currently selected action
        /// </summary>
        private Action selectedAction;

        // The rectangles representing the special move box and its border
        private Rectangle specialMoveBox;
        private Rectangle specialMoveBoxBorder;
        // The height and width of the special move box
        private int boxWidth = 150;
        private int boxHeight = 250;
        // The index of the special move currently highlighted
        private int selectedSpecialMoveIndex;
        // Is the specialMoveBox visible?
        private bool specialMoveBoxVisible;
        // The special move selected by the player
        private SpecialMove selectedSpecialMove;

        #endregion

        #region properties

        /// <summary>
        /// Accessor for the spritefont used in this info bar
        /// </summary>
        public SpriteFont InfobarFont
        {
            get { return infobarFont; }
            set { infobarFont = value; }
        }

        /// <summary>
        /// Accessor for the white background image (used as infobar background) - will also be used as background for the UPDATE BOX
        /// </summary>
        public Texture2D White
        {
            get { return white; }
        }

        /// <summary>
        /// A property for the yMod int.
        /// </summary>
        public int YMod
        {
            get { return yMod; }
            set { yMod = value; }
        }

        /// <summary>
        /// Accessor for visibility
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// A property for the selected action.
        /// </summary>
        public Action SelectedAction
        {
            get { return selectedAction; }
            set { selectedAction = value; }
        }

        /// <summary>
        /// Accessor for the visibility of the specialMoveBox
        /// </summary>
        public bool SpecialMoveBoxVisible
        {
            get { return specialMoveBoxVisible; }
            set { specialMoveBoxVisible = value; }
        }

        /// <summary>
        /// Accessor for the selectedSpecialMove (the move selected by the player)
        /// </summary>
        public SpecialMove SelectedSpecialMove
        {
            get { return selectedSpecialMove; }
            set { selectedSpecialMove = value; }
        }

        #endregion

        #region constructor

        /// <summary>
        /// Info Bar constructor
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="g">game1 object</param>
        public InfoBar(Camera2D camera)
        {
            visible = false;
            white = Game1.GameContent.Load<Texture2D>("infobar_bg");
            infobarFont = Game1.GameContent.Load<SpriteFont>("InfoBarFont");

            bgUnitBarY = Game1.GameHeight - 110;
            yMod = 110;
            bgUnitBar = new Rectangle(0, bgUnitBarY + yMod, 800, 110);
            Reposition();

            mousePos = new Rectangle(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y, 1, 1);

            moveButtonColor = Color.BlueViolet;

            // Create the rectangles for the special move 
            specialMoveBox = new Rectangle(10, 10, boxWidth, boxHeight);
            specialMoveBoxBorder = new Rectangle(specialMoveBox.X - 10, specialMoveBox.Y - 10, boxWidth + 20, boxHeight + 20);

            selectedAction = Action.None; // no action is selected by default
        }

        #endregion

        #region Methods

        /// <summary>
        /// Has all of the rectangles within the main info bar background hook up to it (so they can move together)
        /// </summary>
        private void Reposition()
        {
            bgUnitBar = new Rectangle(0, bgUnitBarY + yMod, 800, 110);
            unitPicRect = new Rectangle(bgUnitBar.X + 30, bgUnitBar.Y + 30, 55, 55);
            unitPicBackRect = new Rectangle(bgUnitBar.X + 15, bgUnitBar.Y + 8, 94, 94);
            ctrOutRect = new Rectangle(bgUnitBar.X + 130, bgUnitBar.Y + 8, 480, 94);
            ctrInRect = new Rectangle(bgUnitBar.X + 140, bgUnitBar.Y + 15, 460, 80);
            ctrInBlackRect = new Rectangle(bgUnitBar.X + 265, bgUnitBar.Y + 18, 331, 74);
            statRectBorder = new Rectangle(bgUnitBar.X + 630, bgUnitBar.Y + 8, 150, 94);
            statRect = new Rectangle(bgUnitBar.X + 635, bgUnitBar.Y + 13, 140, 84);

            moveButton = new Rectangle(bgUnitBar.X + 150, bgUnitBar.Y + 23, 100, 25); // MOVE BUTTON
            atkButton = new Rectangle(bgUnitBar.X + 150, bgUnitBar.Y + 60, 100, 25); // ATTACK BUTTON
            specialButton = new Rectangle(bgUnitBar.X + 275, bgUnitBar.Y + 45, 100, 25); // SPECIAL BUTTON
        }

        #endregion

        #region Game Methods
        /// <summary>
        /// update method (currently checks and changes colors of buttons)
        /// </summary> 
        public void Update()
        {
            Reposition();
            selectedAction = Action.None; // no action is selected by default
            /// if the info bar is visible, do all of the following...
            if (visible) // if the info bar is visible
            {
                // the if command below is what generates the scrolling effect of the info bar.
                if (yMod > 5) // if the bar is still under its normal position when visible
                {
                    yMod -= 10; // move the bar up 10 pixels
                    Reposition(); // move everything up by 10 pixels
                }
                else // otherwise, the info bar is where it should be and it can now be controlled.
                {
                    mousePos = new Rectangle(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y, 1, 1); // now it takes the mouse into account.

                    // each if command checks whether each button is pressed. If so, change the selectedAction.
                    // TODO:
                    // Put the buttons into an array and loop through the array to check with less if commands.
                    // Check whether the mouse was held before hovering over the button. Currently, you can hold then hover, and the button will still turn red.
                    if (mousePos.Intersects(moveButton))
                    {
                        if (InputManager.MouseHeld())
                            moveButtonColor = Color.Crimson;
                        else moveButtonColor = Color.DarkSlateBlue;
                        if (InputManager.MouseReleased())
                        {
                            selectedAction = Action.Move;
                        }
                    }
                    else moveButtonColor = Color.BlueViolet;
                    if (mousePos.Intersects(atkButton))
                    {
                        if (InputManager.MouseHeld())
                            atkButtonColor = Color.Crimson;
                        else atkButtonColor = Color.DarkSlateBlue;
                        if (InputManager.MouseReleased())
                        {
                            selectedAction = Action.Attack;
                        }
                    }
                    else atkButtonColor = Color.BlueViolet;
                    if (mousePos.Intersects(specialButton))
                    {
                        if (InputManager.MouseHeld())
                            specialButtonColor = Color.Crimson;
                        else specialButtonColor = Color.DarkSlateBlue;
                        if (InputManager.MouseReleased())
                        {
                            specialMoveBoxVisible = !specialMoveBoxVisible;
                            //selectedAction = Action.Special;
                        }
                    }
                    else specialButtonColor = Color.BlueViolet;
                }
            }
            else
            {
                mousePos = new Rectangle(); // mousePos is not taken into account

                // The if command below simply reverses the YMod change seen when the infoBar becomes visible.
                if (yMod < 105)
                {
                    yMod += 10;
                    Reposition();
                }
            }
        }


        /// <summary>
        /// Draw method for InfoBar.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="u">You can store any picture in this one variable (u). So each unit can have its own kind of picture.
        /// Each unit has its own picture so whenever you want to draw a unit, pass in a unit into the Draw method, and Draw the picture that way.</param>             
        public void Draw(SpriteBatch spriteBatch, Unit u)
        {
            if (yMod < 105)
            {
                // Currently, spriteBatch.Begin() and spriteBatch.End() are called in here.
                // If anything else uses the same spriteBatch.Begin() and spriteBatch.End(), they will be moved to the Game1.Draw() method before and after the call of this method.

                // This is the main bar (the big bar that has everything inside it)
                spriteBatch.Draw(white, bgUnitBar, Color.White);

                // This will be the background rectangle that is behind the unit pic.
                spriteBatch.Draw(white, unitPicBackRect, Color.Black);

                // This will draw unit pictures that are a part of the Unit class.
                spriteBatch.Draw(u.UnitPic, unitPicRect,u.DisplaySource, Color.White);

                // This is the black border rectangle (which contains the smaller white one with the buttons + descriptions)
                spriteBatch.Draw(white, ctrOutRect, Color.Black);

                // This is the smaller white rectangle (that contains the black rectangle that has buttons + description of abilities)
                spriteBatch.Draw(white, ctrInRect, Color.White);

                // This is the black rectangle inside the white rectangle (that contains special button + description text)
                spriteBatch.Draw(white, ctrInBlackRect, Color.Black);

                // This is the black rectangle (which contains the stats rectangle) - it serves as the border of the stats rect
                spriteBatch.Draw(white, statRectBorder, Color.Black);

                // The stats rectangle itself on the far right
                spriteBatch.Draw(white, statRect, Color.White);

                // THE BUTTONS
                spriteBatch.Draw(white, moveButton, moveButtonColor);
                spriteBatch.Draw(white, atkButton, atkButtonColor);
                spriteBatch.Draw(white, specialButton, specialButtonColor);

                // Drawing info bar text
                spriteBatch.DrawString(
                    infobarFont,
                    u.Name,
                    new Vector2(bgUnitBar.X + 20, bgUnitBar.Y + 10),
                    Color.Silver);
                spriteBatch.DrawString(
                    infobarFont,
                    "Level: " + u.Level,
                    new Vector2(bgUnitBar.X + 20, bgUnitBar.Y + 20),
                    Color.Silver);
                // Draw health/maxHealth
                spriteBatch.DrawString(
                    infobarFont,
                    "Health " + u.Health + "/" + u.MaxHealth,
                    new Vector2(statRect.X + 10, statRect.Y + 5),
                    Color.Black);
                // Draw move/maxMove
                spriteBatch.DrawString(
                    infobarFont,
                    "Movement " + u.Movement + "/" + u.MaxMovement,
                    new Vector2(statRect.X + 10, statRect.Y + 20),
                    Color.Black);
                // Draw Attack
                spriteBatch.DrawString(
                    infobarFont,
                    "Attack " + u.Attack,
                    new Vector2(statRect.X + 10, statRect.Y + 35),
                    Color.Black);
                // Draw Defense
                spriteBatch.DrawString(
                    infobarFont,
                    "Defense " + u.Defense,
                    new Vector2(statRect.X + 10, statRect.Y + 50),
                    Color.Black);
                // Draw members/fullMembers
                spriteBatch.DrawString(
                    infobarFont,
                    "Members " + u.Members + "/" + u.FullMembers,
                    new Vector2(statRect.X + 10, statRect.Y + 65),
                    Color.Black);
                // Draw Move Button Text
                spriteBatch.DrawString(
                    infobarFont,
                    "Move",
                    new Vector2(moveButton.X + 35, moveButton.Y + 5),
                    Color.GhostWhite);
                // Draw Attack Button Text
                spriteBatch.DrawString(
                    infobarFont,
                    "Attack",
                    new Vector2(atkButton.X + 33, atkButton.Y + 5),
                    Color.GhostWhite);
                // Draw Special Button Text
                spriteBatch.DrawString(
                    infobarFont,
                    "Special",
                    new Vector2(specialButton.X + 29, specialButton.Y + 5),
                    Color.GhostWhite);
                // Displays info about whether the unit has attacked this turn
                if (mousePos.Intersects(atkButton))
                {
                    // The string to be drawn
                    //itemSelected = (int)PlayerStatus.Special;
                    if (!u.HasAttacked)
                        spriteBatch.DrawString(
                            infobarFont,
                            "This unit has not attacked\n yet this turn.",
                            new Vector2(ctrInBlackRect.X + 135, ctrInBlackRect.Y + 10),
                            Color.Silver);
                    else
                        spriteBatch.DrawString(
                        infobarFont,
                        "This unit has already attacked\n this turn.",
                        new Vector2(ctrInBlackRect.X + 135, ctrInBlackRect.Y + 10),
                        Color.Silver);
                }
                // Displays info of Special button effect when mouse hovers over the special button
                else if (mousePos.Intersects(specialButton))
                {
                    // The string to be drawn
                    //itemSelected = (int)PlayerStatus.Special;
                    if(u.CoolDown == 0)
                        spriteBatch.DrawString(
                            infobarFont,
                            "Unit can use a special attack \nthis turn.",
                            new Vector2(ctrInBlackRect.X + 135, ctrInBlackRect.Y + 10),
                            Color.GhostWhite);
                    else
                        spriteBatch.DrawString(
                            infobarFont,
                            "Unit cannot use a special attack \nfor " + u.CoolDown + " turn(s).",
                            new Vector2(ctrInBlackRect.X + 135, ctrInBlackRect.Y + 10),
                            Color.GhostWhite);
                }

                if (specialMoveBoxVisible)
                {
                    selectedSpecialMoveIndex = -1;
                    selectedSpecialMove = null;

                    spriteBatch.Draw(white, specialMoveBoxBorder, Color.Black);
                    spriteBatch.Draw(white, specialMoveBox, Color.White);

                    spriteBatch.DrawString(infobarFont, "Special Moves", new Vector2(specialMoveBox.X + 35, specialMoveBox.Y + 5), Color.DarkCyan);

                    List<Rectangle> moveRects = new List<Rectangle>();
                    int offset = 0;
                    foreach (SpecialMove sm in u.SpecialMoves)
                    {
                        moveRects.Add(new Rectangle(specialMoveBox.X + 5, specialMoveBox.Y + 25 + offset, specialMoveBox.Width - 10, 20));
                        offset += 55;
                    }

                    for (int i = 0; i < moveRects.Count; i++)
                    {
                        if (moveRects[i].Contains(mousePos))
                        {
                            if (InputManager.MouseClicked())
                            {
                                selectedSpecialMove = u.SpecialMoves[i];
                            }
                            selectedSpecialMoveIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < u.SpecialMoves.Count; i++)
                    {
                        if (i == selectedSpecialMoveIndex)
                        {
                            spriteBatch.Draw(white, moveRects[i], Color.DarkSlateBlue);
                            spriteBatch.DrawString(infobarFont, u.SpecialMoves[i].Name, new Vector2(moveRects[i].X + 5, moveRects[i].Y + 2), Color.GhostWhite);
                            spriteBatch.DrawString(
                                infobarFont,
                                u.SpecialMoves[i].Description,
                                new Vector2(ctrInBlackRect.X + 135, ctrInBlackRect.Y + 10),
                                Color.Silver);
                        }
                        else
                        {
                            spriteBatch.Draw(white, moveRects[i], Color.BlueViolet);
                            spriteBatch.DrawString(infobarFont, u.SpecialMoves[i].Name, new Vector2(moveRects[i].X + 5, moveRects[i].Y + 2), Color.GhostWhite);
                        }
                    }

                    if (selectedSpecialMove != null)
                    {
                        selectedAction = Action.Special;
                        specialMoveBoxVisible = false;
                    }
                }
            }
        }

        #endregion
    }
}
