using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Collections.PriorityQueue;
namespace ForeignStars
{
    /// <summary>
    /// The user controlled player.
    /// Player is more of the middleman between components that have more functionality
    /// 
    /// @author: Zachary Behrmann
    /// @author: William Powell
    /// </summary>
    public class Human : Player
    {

        #region attributes, properties




        // previous grid x and y components
        private int previousGridX;
        private int previousGridY;
        private Tile previousTile;
        private Rectangle previousRectPos;
        private Rectangle endTurnButton;
        private Battle battle;
        private Selector mainSelector; // The first selector of player - used to select a unit/view details
        private Selector secondarySelector; //The selector used for moving a unit and other unit related actions (attack, etc.)
        private bool changeTurn;
        /// <summary>
        /// The texture of the player's various selectors (we never actually draw selectors for now)
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// The infobar that displays unit stats and options
        /// </summary>
        private InfoBar infoBar;

        /// <summary>
        /// The action we want a unit to perform
        /// </summary>
        private InfoBar.Action selectedAction;

        /// <summary>
        /// Determines whether the player can move about the map
        /// </summary>
        private bool mapControl;



        /// <summary>
        /// Are we highlighting tiles?
        /// </summary>
        private bool highlightGone;

        private UpdateBox updateBox;




        #endregion

        #region properties

        public Selector MainSelector
        {
            get { return mainSelector; }
            set { mainSelector = value; }
        }

        public Selector SecondarySelector
        {
            get { return secondarySelector; }
            set { secondarySelector = value; }
        }



        /// <summary>
        /// Returns the currently selected tile
        /// </summary>
        public Tile CurrentTile
        {
            get { return currentTile; }
        }

        public Battle Battle
        {
            get { return battle; }
            set { battle = value; }
        }



        #endregion


        #region constructor

        /// <summary>
        /// Create a new player
        /// </summary>
        /// <param name="input">The input manager of the game</param>
        /// <param name="bar">The infobar, player handles displaying and hiding the infobar</param>
        public Human(InfoBar bar, UpdateBox box, Texture2D sprite, Army a)
        {
            changeTurn = false;
            mapControl = true;
            infoBar = bar;
            updateBox = box;
            endTurnButton = box.EndTurnButton;
            currentGridX = 2;
            currentGridY = 2;
            texture = sprite;
            ownArmy = a;
            mainSelector = new Selector(tiles, actualPos, sprite, currentGridX, currentGridY);
            secondarySelector = new Selector(tiles, actualPos, sprite, currentGridX, currentGridY);
            highlightGone = true;
            //texture = Game1.GameContent.Load<Texture2D>("generic_unit");
        }

        #endregion




        #region GameMethods

        public override bool Update(Camera2D camera)
        {
            
            if (mapControl) //Are we moving amongst the map (1st step)
            {
                updateBox.TargetUnit = mainSelector.UnitSelected;
                if (highlightGone == false) //Make sure we no longer are highlighting tiles
                {
                    secondarySelector.HighlightDelete();
                    highlightGone = true;
                }
                currentTile = mainSelector.Update(ref actualPos, ref currentGridX, ref currentGridY, ref camera, 0, endTurnButton);
                if (currentTile != null && currentTile.Unit != null && (Game1.Team == int.Parse(currentTile.Unit.Army.ToString()))) //We have selected a tile and it has a unit to select, invoke the infobar and prepare the secondary selector
                {
                    currentUnit = currentTile.Unit;
                    infoBar.Visible = true;
                    mapControl = false;
                    secondarySelector.UnitSelected = currentUnit;
                    mainSelector.PreviousTile.HighlightCursor = false;

                    secondarySelector.CurrentTile = currentTile;
                    secondarySelector.CurrentGridX = currentGridX;
                    secondarySelector.CurrentGridY = currentGridY;
                    mainSelector.CurrentTile = currentTile;
                    mainSelector.CurrentGridX = currentGridX;
                    mainSelector.CurrentGridY = currentGridY;
                    mainSelector.Tiles[currentGridX, currentGridY].HighlightCursor = true;
                }
            }
            else
            {
                
                if (infoBar.Visible)
                {
                    updateBox.TargetUnit = currentUnit;
                    if (infoBar.SelectedAction > 0)
                    {
                        selectedAction = infoBar.SelectedAction;
                        infoBar.Visible = false;
                        previousGridX = currentGridX;
                        previousGridY = currentGridY;
                        previousTile = currentTile;
                        previousRectPos = actualPos;
                    }
                    if (InputManager.KeyReady(Keys.Q))
                    {
                        infoBar.Visible = false;
                        infoBar.SpecialMoveBoxVisible = false;
                        mapControl = true;
                        // The code below may or may not be called based on the option of
                        // whether the player wants cancelling the move to bring them back to 
                        // the previous unit selected or if they just want it to keep them
                        // on the current tile.
                        /*
                        mainSelector.CurrentTile = currentTile;
                        mainSelector.CurrentGridX = currentGridX;
                        mainSelector.CurrentGridY = currentGridY;
                        */
                    }
                }
                else
                {
                    updateBox.TargetUnit = secondarySelector.UnitSelected;
                    if (InputManager.KeyReady(Keys.Q) && !highlightGone)
                    {
                        infoBar.Visible = true;
                        secondarySelector.HighlightDelete();
                        selectedAction = InfoBar.Action.None;
                        highlightGone = true;
                        mapControl = false;
                        currentTile = previousTile;
                        currentGridX = previousGridX;
                        currentGridY = previousGridY;
                        actualPos = previousRectPos;
                        secondarySelector.CurrentTile = currentTile;
                        secondarySelector.CurrentGridX = currentGridX;
                        secondarySelector.CurrentGridY = currentGridY;
                        mainSelector.CurrentTile = currentTile;
                        mainSelector.CurrentGridX = currentGridX;
                        mainSelector.CurrentGridY = currentGridY;
                        mainSelector.PreviousTile.HighlightCursor = false;
                        foreach (Tile tile in secondarySelector.Tiles)
                        {
                            tile.HighlightCursor = false;
                        }
                        mainSelector.Tiles[currentGridX, currentGridY].HighlightCursor = true;
                    }
                    switch (selectedAction)
                    {
                        case InfoBar.Action.Move:
                            {
                                if (!currentTile.Unit.Bunkered)
                                {
                                    if (highlightGone) //Prevent ourselves from highlighting continuously
                                    {
                                        secondarySelector.HighlightTiles(currentUnit.Movement);
                                        highlightGone = false;
                                    }
                                    selectedTile = secondarySelector.Update(ref actualPos, ref currentGridX, ref currentGridY, ref camera, 0, endTurnButton);
                                    if (selectedTile != null && selectedTile.Unit == null && selectedTile.HighlightSpace == true)
                                    {
                                        //infoBar.SelectedAction = (int)PlayerStatus.None;
/*
                                    secondarySelector.HighlightTiles(currentUnit.Movement);
                                    highlightGone = false;
                                }
                                selectedTile = secondarySelector.Update(ref actualPos, ref currentGridX, ref currentGridY, ref camera, 0/*, endTurnButton);
                                if (selectedTile != null && selectedTile.Unit == null && selectedTile.HighlightSpace == true)
                                {*/
                                    //infoBar.SelectedAction = (int)PlayerStatus.None;


                                        //currentUnit.UnitBox.Offset((selectedTile.Location.X - currentUnit.UnitBox.X), (selectedTile.Location.Y - currentUnit.UnitBox.Y));
                                        //currentUnit.UnitPoint = selectedTile.Location.Location;

                                        selectedAction = InfoBar.Action.Animate;
                                    }
                                }
                                else
                                {
                                    Game1.UpdateBox.AddMessage("Unit cannot move \nuntil you \ndeactivate bunker.");
                                    highlightGone = false; //Used so we can still q out of method
                                }
                            }
                            break;
                        /*default:
                            mapControl = true;
                            infoBar.Visible = false;

                            break;*/
                        case InfoBar.Action.Attack:
                            {
                                if (highlightGone) //Prevent ourselves from highlighting continuously
                                {
                                    secondarySelector.HighlightEnemies(currentUnit.Range);
                                    highlightGone = false;
                                }
                                selectedTile = secondarySelector.Update(ref actualPos, ref currentGridX, ref currentGridY, ref camera, 0, endTurnButton);
                                if (selectedTile != null && selectedTile.Unit != null && selectedTile.Unit.Army != currentTile.Unit.Army && selectedTile.HighlightEnemy)
                                {
                                    int dealt, received;
                                    Unit attacker = currentTile.Unit;
                                    Unit defender = selectedTile.Unit;
                                    bool isCounterAttack = false;

                                    // MAIN ATTACK
                                    int damageDealt = DamageCalculator.doDamage(attacker, defender, isCounterAttack);
                                    defender.Health -= damageDealt;
                                    dealt = damageDealt;
                                    //updateBox.Update(attacker, defender, damageDealt, isCounterAttack);
                                    //use isCounterAttack as a parameter in updateBox.Update(...)
                                    //if isCounterattack is true, print "attacker did this much damage to defender"
                                    //otherwise, print "defender counterattacked attacker with this much damage"

                                    // COUNTER ATTACK
                                    isCounterAttack = true;
                                    damageDealt = DamageCalculator.doDamage(defender, attacker, isCounterAttack);
                                    attacker.Health -= damageDealt;
                                    received = damageDealt;
                                    //updateBox.Update(defender, attacker, damageDealt, isCounterAttack);

                                    // Game1.UpdateBox.ReportAttack(dealt, received);
                                    Game1.UpdateBox.AddMessage("Player unit attacks, \ndealing " + dealt + " damage!\n\n" + "Enemy unit \ncounterattacks, \ndealing " + received + " damage!");
                                    mapControl = true;
                                    infoBar.Visible = false;
                                    mainSelector.CurrentTile = currentTile;
                                    mainSelector.CurrentGridX = currentGridX;
                                    mainSelector.CurrentGridY = currentGridY;
                                    //Attacker now knows it has attacked this turn
                                    attacker.HasAttacked = true;
                                }
                            }
                            break;
                        case InfoBar.Action.Special:
                            {
                                if (currentTile.Unit.CoolDown == 0) //blocks the use of specials
                                {
                                    if (highlightGone) //Prevent ourselves from highlighting continuously
                                    {
                                        if (infoBar.SelectedSpecialMove.Name == "Frag Grenade")
                                        {
                                            secondarySelector.HighlightSpecial(infoBar.SelectedSpecialMove.Range, true);
                                            highlightGone = false;
                                        }
                                        else
                                        {
                                            secondarySelector.HighlightSpecial(infoBar.SelectedSpecialMove.Range, false);
                                            highlightGone = false;
                                        }
                                    }
                                    int specialDRange = infoBar.SelectedSpecialMove.AreaOfEffect;
                                    selectedTile = secondarySelector.Update(ref actualPos, ref currentGridX, ref currentGridY, ref camera, specialDRange, endTurnButton);
                                    if (infoBar.SelectedSpecialMove.NeedsTarget == false || (selectedTile != null && selectedTile.HighlightEnemy))
                                    {
                                        Unit attacker = currentTile.Unit;
                                        bool isCounterAttack = false;
/*
                                        secondarySelector.HighlightSpecial(infoBar.SelectedSpecialMove.Range, false);
                                        highlightGone = false;
                                    }
                                }
                                int specialDRange = infoBar.SelectedSpecialMove.AreaOfEffect;
                                selectedTile = secondarySelector.Update(ref actualPos, ref currentGridX, ref currentGridY, ref camera, specialDRange/*, endTurnButton);
                                if (infoBar.SelectedSpecialMove.NeedsTarget == false || (selectedTile != null && selectedTile.HighlightEnemy))
                                {
                                    Unit attacker = currentTile.Unit;
                                    bool isCounterAttack = false;
*/
                                        /*
                                         * TODO: Make a special attack happen here:
                                         * get user input for type of attack
                                         * string type = user input
                                         * SpecialMove s1 = new SpecialMove(type);
                                         * Call damage calculator
                                         * Use conditional statements based on s1's properties.
                                         * 
                                         * IN THIS CLASS:
                                         * "Bunker Down" - Boost defense to unit
                                         * "Web Grenade" - Lower enemy move value by 2
                                         * "Frag Grenade" - Secondary selector is on all units, don't allow movement of selector
                                         * "Missile" - Doubles the range of helicoptors, but deals 85% damage
                                         * "Cripple" - Lowers enemy move value by 2 and deals ~80% damage
                                         * "Repair" - Tank attempts to fix some of its parts, returning some lost health
                                         * */

                                        if (infoBar.SelectedSpecialMove != null)
                                        {
                                            SpecialMove s1 = infoBar.SelectedSpecialMove;
                                            Random random = new Random();
                                            //Acts upon what type of special is used
                                            switch (s1.Name)
                                            {
                                                case "Bunker Down":
                                                    if (!attacker.HasAttacked && attacker.Movement == attacker.MaxMovement)
                                                    {
                                                        //Going into bunker
                                                        if (attacker.Bunkered == false)
                                                        {
                                                            Game1.UpdateBox.AddMessage("Used Bunker\nDown! Unit's defense \nincreased!");
                                                            attacker.Defense += 5; // add 5 defense to unit currently using the move
                                                            attacker.Attack /= 2;
                                                            attacker.Movement = 0; // moves all lost! to 0
                                                            attacker.HasAttacked = true; // attacked is true, lose a turn
                                                            attacker.CoolDown = s1.CoolDown; //Sets cooldown for specials
                                                        }
                                                        //Leaving bunker
                                                        else
                                                        {
                                                            Game1.UpdateBox.AddMessage("Unit is no \nlonger Bunkered.");
                                                            attacker.Attack = attacker.OriAttack;
                                                            attacker.Defense = attacker.OriDefense;
                                                            attacker.CoolDown = s1.CoolDown;
                                                            attacker.HasAttacked = true;
                                                        }
                                                    }
                                                    //If conditions aren't met for going or leaving bunker
                                                    else
                                                    {
                                                        Game1.UpdateBox.AddMessage("Can't bunker down\n after moving\n or attacking.");
                                                    }
                                                    break;
                                                case "Web Grenade":
                                                    //If we aren't bunkered
                                                    if (!attacker.Bunkered)
                                                    {
                                                        Unit defender = selectedTile.Unit;
                                                        defender.Movement -= 2;
                                                        Game1.UpdateBox.AddMessage("Used A Web \nGrenade! Enemy \nunit's move\ndecreased by 2!");
                                                        attacker.CoolDown = s1.CoolDown; //Sets cooldown for specials
                                                    }
                                                    //If we are bunkered
                                                    else
                                                    {
                                                        Game1.UpdateBox.AddMessage("Must unbunker unit \nto use this special.");
                                                    }
                                                    break;
                                                case "Frag Grenade":
                                                    //If we aren't bunkered
                                                    if (!attacker.Bunkered)
                                                    {
                                                        Game1.UpdateBox.AddMessage("Used a Frag\ngrenade!\n...Units took\nsome damage!");
                                                        //Each of the five selectable tiles will check if they are not null and if there is a unit present.
                                                        if (tiles[currentGridX, currentGridY].Unit != null)
                                                        {
                                                            // Using Frag Grenade can deal anywhere from 10 to 20 damage.
                                                            tiles[currentGridX, currentGridY].Unit.Health -= random.Next(10, 20);
                                                        }
                                                        if (currentGridX > 0)
                                                        {
                                                            if (tiles[currentGridX - 1, currentGridY].Unit != null)
                                                            {
                                                                // Using Frag Grenade can deal anywhere from 10 to 20 damage.
                                                                tiles[currentGridX - 1, currentGridY].Unit.Health -= random.Next(10, 20);
                                                            }
                                                        }
                                                        if (currentGridY > 0)
                                                        {
                                                            if (tiles[currentGridX, currentGridY - 1].Unit != null)
                                                            {
                                                                // Using Frag Grenade can deal anywhere from 10 to 20 damage.
                                                                tiles[currentGridX, currentGridY - 1].Unit.Health -= random.Next(10, 20);
                                                            }
                                                        }
                                                        if (currentGridX < Math.Sqrt(tiles.Length - 1))
                                                        {
                                                            if (tiles[currentGridX + 1, currentGridY].Unit != null)
                                                            {
                                                                // Using Frag Grenade can deal anywhere from 10 to 20 damage.
                                                                tiles[currentGridX + 1, currentGridY].Unit.Health -= random.Next(10, 20);
                                                            }
                                                        }
                                                        if (currentGridY < Math.Sqrt(tiles.Length - 1))
                                                        {
                                                            if (tiles[currentGridX, currentGridY + 1].Unit != null)
                                                            {
                                                                // Using Frag Grenade can deal anywhere from 10 to 20 damage.
                                                                tiles[currentGridX, currentGridY + 1].Unit.Health -= random.Next(10, 20);
                                                            }
                                                        }
                                                        attacker.CoolDown = s1.CoolDown; //Sets cooldown for specials
                                                    }
                                                    //If we are bunkered
                                                    else
                                                    {
                                                        Game1.UpdateBox.AddMessage("Must unbunker unit \nto use this special.");
                                                    }
                                                    break;
                                                case "Missile":
                                                    Unit defending = selectedTile.Unit;
                                                    defending.Health -= (int).85 * DamageCalculator.doDamage(attacker, defending, false);
                                                    Game1.UpdateBox.AddMessage("Used A Missile \n...Target Unit \n takes damage");
                                                    attacker.CoolDown = s1.CoolDown; //Sets cooldown for specials
                                                    break;
                                                case "Cripple":
                                                    Unit defend = selectedTile.Unit;
                                                    defend.Movement -= 2;
                                                    defend.Health -= (int).806 * DamageCalculator.doDamage(attacker, defend, false);
                                                    Game1.UpdateBox.AddMessage("Used Cripple \n...Target Unit takes \nsome damage \n and unit's move \ndecreased by 2!");
                                                    attacker.CoolDown = s1.CoolDown; //Sets cooldown for specials
                                                    break;
                                                case "Repair":
                                                    int repair = random.Next(0, 10); //How much we heal for
                                                    attacker.Health += repair;
                                                    Game1.UpdateBox.AddMessage("Used Repair \n...Unit healed itself for \n" + repair +" Health.");
                                                    attacker.CoolDown = s1.CoolDown; //Sets cooldown for specials
                                                    break;
                                                default:
                                                    break;
                                            }                                        
                                            infoBar.SelectedSpecialMove = null;
                                        }

                                        //// MAIN ATTACK
                                        //int damageDealt = DamageCalculator.doDamage(attacker, defender, isCounterAttack);
                                        //defender.Health -= damageDealt;
                                        //dealt = damageDealt;
                                        ////updateBox.Update(attacker, defender, damageDealt, isCounterAttack);
                                        ////use isCounterAttack as a parameter in updateBox.Update(...)
                                        ////if isCounterattack is true, print "attacker did this much damage to defender"
                                        ////otherwise, print "defender counterattacked attacker with this much damage"

                                        //// COUNTER ATTACK
                                        //isCounterAttack = true;
                                        //damageDealt = DamageCalculator.doDamage(defender, attacker, isCounterAttack);
                                        //attacker.Health -= damageDealt;
                                        //received = damageDealt;
                                        ////updateBox.Update(defender, attacker, damageDealt, isCounterAttack);

                                        //Game1.UpdateBox.ReportAttack(dealt, received);
                                        mapControl = true;
                                        infoBar.Visible = false;
                                        mainSelector.CurrentTile = currentTile;
                                        mainSelector.CurrentGridX = currentGridX;
                                        mainSelector.CurrentGridY = currentGridY;
                                        //Attacker now knows it has attacked this turn
                                        attacker.HasAttacked = true;
                                    }
                                }
                                else highlightGone = false; //exists so we can q out of a special while we are cooling down. 
                            }
                            break;
                        case InfoBar.Action.Animate:
                            //This is where we animate a unit moving
                            AnimateMovement();
                            break;
                    }
                }
                /*
                if (infoBar.YMod > 105)
                {
                    currentUnit = null;
                    mapControl = true;
                    infoBar.ItemSelected = (int)PlayerStatus.None;
                }
                 * */

            }

            if (actualPos.X > ((GV.TileSize * 4) + rectPos.X))
            {
                rectPos.X += GV.TileSize;
            }
            else if (actualPos.X < rectPos.X - (GV.TileSize * 4))
            {
                rectPos.X -= GV.TileSize;
            }
            if (actualPos.Y > ((GV.TileSize * 4) + rectPos.Y))
            {
                rectPos.Y += GV.TileSize;
            }
            else if (actualPos.Y < rectPos.Y - (GV.TileSize * 4))
            {
                rectPos.Y -= GV.TileSize;
            }

            //rectPos = tiles[currentGridX, currentGridY].Location
            //infoBar.SelectedAction = InfoBar.Action.None;
            if (changeTurn)
            {
                foreach (Unit u in ownArmy.Units)
                    u.CoolDown -= 1;
                changeTurn = false;
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, actualPos, Color.White);
            /*if (mapControl)
                mainSelector.Draw(spriteBatch);
            else
                secondarySelector.Draw(spriteBatch);*/
        }

        #endregion

        #region Methods

        public override void SetTiles(Tile[,] tileArray)
        {
            tiles = tileArray;
            mainSelector.Tiles = tileArray;
            secondarySelector.Tiles = tileArray;
        }


        protected override void AfterAnimate()
        {
            
            beatenPath = null;
            selectedTile.Unit = currentUnit;
            currentTile.Unit = null;

            mapControl = true;
            infoBar.Visible = false;
            mainSelector.CurrentTile = currentTile;
            mainSelector.CurrentGridX = currentGridX;
            mainSelector.CurrentGridY = currentGridY;
        }


        public override void ChangeTurnConditions()
        {
            infoBar.Visible = false;
            mapControl = true;
            secondarySelector.UnitSelected = null;
            mainSelector.PreviousTile.HighlightCursor = true;
            mainSelector.Tiles[currentGridX, currentGridY].HighlightCursor = false;
            ownArmy.Reset();
            changeTurn = true;
        }
        #endregion
    }
}
