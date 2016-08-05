using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
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
    /// A scenario menu used to customize the main game.
    /// 
    /// @author: Ryan Conrad
    /// </summary>
    class ScenarioMenu : Menu
    {
        // base.CurrMenuItem refers to the scenario chosen in this case.
        // numMenuItems refers to how many scenarios there are in this case.'

        /// <summary>
        /// Enum representing the scenario chosen.
        /// 
        /// Will be removed at the inclusion of loading XML files
        /// </summary>

        #region fields

        // Graphics
        private Texture2D arrowLeft;
        private Texture2D arrowRight;
        private Texture2D startButton;
        private Texture2D mapPreview;

        // Rectangles/Vector2 for the items on the menu
        private Rectangle gameType;
        private Rectangle playerFaction;
        private Rectangle enemyFaction;
        private Rectangle mapLeft;
        private Rectangle mapRight;
        private Rectangle teamLeft;
        private Rectangle teamRight;
        private Rectangle start;
        private Rectangle mapRect;
        private Vector2 scenTextVect;
        private Vector2 teamTextVect;


        private string path;

        private Texture2D terrainSheet;
        private Texture2D infantryTexture;

        // XML File Stuff Goes Here
        private List<string> Files;
        private List<Scenario> scenarios;

        private int currentIndex = 0;

        private bool imageChange;

        private SoundEffect startGameSound;

        #endregion

        public ScenarioMenu()
            : base(3, int.MaxValue, int.MaxValue)
        {
            imageChange = true;
            scenarios = new List<Scenario>();
            arrowLeft = Game1.GameContent.Load<Texture2D>("arrow_left");
            arrowRight = Game1.GameContent.Load<Texture2D>("arrow_right");
            startButton = Game1.GameContent.Load<Texture2D>("start");

            //mapPreview = Game1.GameContent.Load<Texture2D>("DefaultScenarioPreview");
            mapLeft = new Rectangle(500, 50, 64, 32);
            mapRight = new Rectangle(628, 50, 64, 32);
            teamLeft = new Rectangle(100, 50, 64, 32);
            teamRight = new Rectangle(228, 50, 64, 32);
            start = new Rectangle(175, 275, 64, 32);
            mapRect = new Rectangle(400, 150, 309, 212);
            scenTextVect = new Vector2(530, 100);
            teamTextVect = new Vector2(140, 100);
            CurrMenuState = 0;
            terrainSheet = Game1.GameContent.Load<Texture2D>("grass");
            infantryTexture = Game1.GameContent.Load<Texture2D>("generic_unit");

            Background = Game1.GameContent.Load<Texture2D>("scenarioBG");

            // Load XML Files here
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            Console.WriteLine(d);
            while (d.ToString() != "dynasty")
                d = d.Parent;
            path = d.FullName + @"\GameData\Scenarios\";

            Files = System.IO.Directory.EnumerateFiles(path, "*.xml").ToList();
            foreach (string file in Files)
            {
                scenarios.Add(new Scenario(file));
            }

            startGameSound = Game1.GameContent.Load<SoundEffect>("StartGameSound");
        }
        public Scenario Update(Game1 game)
        {
            if (InputManager.KeyButtonReady(Keys.Escape, Buttons.A, 1, false))
            {
                Game1.Menus.Push(Game1.TitleMenu);
            }
            if (InputManager.MouseReleased())
            {
                if (start.Contains(InputManager.MousePos))
                {
                    startGameSound.Play();
                    Game1.Menus.Pop();
                    scenarios[currentIndex].LoadScenarioArmies();
                    return scenarios[currentIndex];
                }
                else if (mapRight.Contains(InputManager.MousePos))
                {
                    Game1.ClickSound.Play();
                    currentIndex++;
                    if (currentIndex >= scenarios.Count)
                    {
                        currentIndex = 0;
                    }
                    imageChange = true;
                }
                else if (mapLeft.Contains(InputManager.MousePos))
                {
                    Game1.ClickSound.Play();
                    currentIndex--;
                    if (currentIndex < 0)
                    {
                        currentIndex = scenarios.Count - 1;
                    }

                    imageChange = true;
                }
                else if (teamLeft.Contains(InputManager.MousePos))
                {
                    Game1.ClickSound.Play();
                    Game1.Team--;
                }
                else if (teamRight.Contains(InputManager.MousePos))
                {
                    Game1.ClickSound.Play();
                    Game1.Team++;
                }
            }
            if (imageChange)
            {
                try
                {

                    FileStream blah = System.IO.File.Open(path + @"thumbnails\" + scenarios[currentIndex].Thumbnail, FileMode.Open);
                    mapPreview = Texture2D.FromStream(game.GraphicsDevice, blah);
                    blah.Close();
                }
                catch
                {
                    FileStream blah = System.IO.File.Open(path + @"thumbnails\default.png", FileMode.Open);
                    mapPreview = Texture2D.FromStream(game.GraphicsDevice, blah, 240, 240, false);
                    blah.Close();
                }
                finally
                {
                    imageChange = false;
                }
            }
            base.Update();
            return null;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(arrowLeft, mapLeft, Color.White);
            spriteBatch.Draw(arrowRight, mapRight, Color.White);
            spriteBatch.Draw(arrowLeft, teamLeft, Color.White);
            spriteBatch.Draw(arrowRight, teamRight, Color.White);
            spriteBatch.Draw(startButton, start, Color.White);


            spriteBatch.DrawString(Game1.DefFont, scenarios[currentIndex].Name, scenTextVect, Color.White);

            if (mapPreview != null)
                spriteBatch.Draw(mapPreview, new Rectangle(400, 170, 240, 240), Color.White);
            spriteBatch.DrawString(Game1.DefFont, scenarios[currentIndex].Description, new Vector2(30, 420), Color.White);


            switch (Game1.Team)
            {
                case 0:
                    spriteBatch.DrawString(Game1.DefFont, "Red Team", teamTextVect, Color.White);
                    break;
                case 1:
                    spriteBatch.DrawString(Game1.DefFont, "Blue Team", teamTextVect, Color.White);
                    break;
            }
            //spriteBatch.Draw(mapPreview, mapRect, Color.White);
        }
    }
}
