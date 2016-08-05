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
    /// An abstract menu class that contains implemented attributes and methods that all menus must have.
    /// 
    /// @author: Ryan Conrad
    /// </summary>
    public abstract class Menu
    {
        #region fields

        protected int numMenuItems;             // number of menu items
        private int currMenuState;              // integer that represents the current menu item selected
        private int prevMenuState;              // integer that represents the previous menu item selected
        Rectangle[] positions;                  // rectangles for each option
        Rectangle[,] sourceRects;               // gets source rectangles for referencing sprite sheet (first dimension: number of items. second dimension: number of button states)
        private bool isHighlighted;             // checks when an item is highlighted
        private bool highlightControl;          // used to control clicking, then dragging the mouse over a button
        private bool isClicked;                 // checks when an item is clicked
        private bool clickControl;              // used to control clicking, then dragging the mouse off the button
        private bool mouseHoverSoundPlayed;     // used to check if the mouse hovering over sound was played
        private int selectedItem;               // contains the item currently clicked (used to perfect option selecting)
        private bool isSelectedChoice;          // a bool representing whether the item hovered over when the mouse is released is the same one as the one that was clicked
        private Texture2D background;           // background (different for each menu)

        #endregion

        #region properties

        public bool IsSelectedChoice { get { return isSelectedChoice; } }                       // property to check whether, on mouse release, that is the selected item
        public Texture2D Background { get { return background; } set { background = value; } }  // property for the menu's background
        public bool IsHighlighted { get { return isHighlighted; } }                             // property for checking if something is highlighted

        #endregion

        /// <summary>
        /// Constructor for the general fields of a menu
        /// </summary>
        public Menu(int numMenuItems, int startPos, int sourceLoc)
        {
            this.numMenuItems = numMenuItems;               // initialize number of menu items
            //CurrMenuState = 0;                            // goes to first menu item
            positions = new Rectangle[numMenuItems];        // initialize rectangle array to the number of menu items
            sourceRects = new Rectangle[numMenuItems,3];    // contains all source rects for all states of menu items
            for (int i = 0; i < numMenuItems; i++ )         // loops through all menu items available
            {
                positions[i] = new Rectangle(Game1.GameWidth / 2 - 100, startPos, 128, 32); // initialize each element in array
                sourceRects[i,0] = new Rectangle(i * 128, sourceLoc, 128, 32);              // the source rectangle for an idle button
                sourceRects[i,1] = new Rectangle(i * 128, sourceLoc + 32, 128, 32);         // the source rectangle for a highlighted button
                sourceRects[i,2] = new Rectangle(i * 128, sourceLoc + 64, 128, 32);         // the source rectangle for a clicked button
                startPos += 48;                                                             // add to the button's position for the next button's set of images
            }
        }

        /// <summary>
        /// Property for the current menu state with a built in checker for the lower (0) and upper (numMenuItems-1) limits.
        /// It currently also allows for the state to equal the number of menu items, where in this case, goes to another screen or state
        /// (such as the instructions or credits screen) without the need to instantiate those as another menu.
        /// </summary>
        public int CurrMenuState
        {
            get { return currMenuState; }               // gets the current menu state
            set
            {
                currMenuState = value;                  // sets current menu state
                if (currMenuState < 0)                  // if it's less than the first menu item
                    currMenuState = numMenuItems;       // it equals the last menu item
                else if (currMenuState > numMenuItems)  // if it's greater than the number of menu items
                    currMenuState = 0;                  // it equals the first menu item
            }
        }

        /// <summary>
        /// every menu checks up and down for enumerating the current menu state
        /// </summary>
        public virtual void Update()
        {
            prevMenuState = currMenuState;              // update the previous menu state
            if (currMenuState != numMenuItems)          // if the current menu state isn't equal to the number of menu items (or on the extra available state)
            {
                for (int i = 0; i < numMenuItems; i++)  // loop through the menu items
                {
                    // if the mouse is at the position of the item, and highlighting is under control
                    if (positions[i].Contains(InputManager.MousePos) && highlightControl)
                    {
                        CurrMenuState = i;                      // the current menu state is now the checked item
                        isHighlighted = true;                   // the item is now highlighted
                        
                        // if the menu state is new and the click is not under control
                        if (prevMenuState != currMenuState && !clickControl)
                        {
                            selectedItem = i;                   // set the selected item to the one that is highlighted
                        }
                        break;                                  // none of the other items need to be checked
                    }
                    else isHighlighted = false;                 // otherwise, nothing is highlighted, or the highlight isn't under control
                }

                // if the confirm sound isn't playing, the sound hasn't been played yet, and something is highlighted
                if (Game1.ConfirmSoundInstance.State != SoundState.Playing && !mouseHoverSoundPlayed && isHighlighted)
                {
                    Game1.MouseHoverSound.Play();       // play the mouse hover sound
                    mouseHoverSoundPlayed = true;       // it was now played
                }
                // otherwise, if noting is highlighted
                else if (!isHighlighted)
                    mouseHoverSoundPlayed = false;      // the sound wasn't played yet
            }
            if (InputManager.MouseHeld())                       // if the mouse is held
            {
                isClicked = true;                               // an option is clicked
            }
            else                                                // if the mouse isn't held
            {
                isClicked = false;                              // an option is not clicked
                selectedItem = currMenuState;                   // therefore, the selected item can equal the current one, since clicking currently isn't happening
            }
            if (isClicked && !isHighlighted && !clickControl)   // if a user tries to click without highlighting an option
                highlightControl = false;                       // the highlight control is set to false
            else highlightControl = true;                       // otherwise, it is true and an option can be chosen upon a mouse release
            if (isClicked && isHighlighted)                     // if something is highlighted and clicked
            {
                if(!clickControl)                               // if the click isn't under control
                    Game1.ClickSound.Play();                    // play the click sound
                clickControl = true;                            // the click control is set to true
            }
            else if (!isClicked)                                // otherwise, if an item currently isn't clicked
            {
                clickControl = false;                           // the click control is set to false
            }
        }

        /// <summary>
        /// draws all the items for the current menu
        /// </summary>
        /// <param name="spriteBatch">current SpriteBatch</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(background != null) // if the child class gave a background, draw it
                spriteBatch.Draw(background, new Rectangle(0, 0, Game1.GameWidth, Game1.GameHeight), Color.White);

            if (currMenuState != numMenuItems) // if the current menu state is not equal to the number of menu items (or the extra available state)
            {
                for (int i = 0; i < numMenuItems; i++) // loop through the items
                {
                    if (currMenuState == i) // if the current menu state is equal to the index
                    {
                        // if the item is highlighted and the selected item is equal to the index
                        if (isHighlighted && selectedItem == i)
                        {
                            // if the item is clicked and the click is under control
                            if(isClicked && clickControl)
                            {
                                // draw the item in a clicked state and verify that this is the selected choice
                                spriteBatch.Draw(Game1.MenuButtons, positions[i], sourceRects[i, 2], Color.White);
                                isSelectedChoice = true;
                            }
                            // otherwise, draw the item in the highlighted state
                            else spriteBatch.Draw(Game1.MenuButtons, positions[i], sourceRects[i, 1], Color.White);
                        }
                        // if it's not highlighted or the selected item, draw the default state of the button and verify that it is not the original selected choice
                        else
                        {
                            spriteBatch.Draw(Game1.MenuButtons, positions[i], sourceRects[i, 0], Color.White);
                            isSelectedChoice = false;
                        }
                    }
                    // for all other cases, draw the default state
                    else spriteBatch.Draw(Game1.MenuButtons, positions[i], sourceRects[i, 0], Color.White);
                }
            }
        }
    }
}
