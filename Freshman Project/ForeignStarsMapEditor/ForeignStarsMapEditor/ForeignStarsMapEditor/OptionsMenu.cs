using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ForeignStarsMapEditor
{
    public class OptionsMenu
    {
        public Game1 game;
        public SetUpBox[] SizeBoxes;
        public SaveButton save;
        public LoadButton load;
        public List<String> files;
        public bool saving;
        public bool loading;

        // exit button
        public Rectangle exitButtonRect;

        public OptionsMenu(Game1 g)
        {
            game = g;

            SizeBoxes = new SetUpBox[12];


            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    SizeBoxes[((x * 3) + (y))] = new SetUpBox((10 + (((x * 3) + (y)) * 2)), (game.gameWidth / 2 - 100) + 50 * x, ((game.gameHeight / 2 - 100) + 67 * y), this, game.mouse);
                }
            }

            save = new SaveButton(this);
            load = new LoadButton(this);

            exitButtonRect = new Rectangle(game.gameWidth - 150, game.gameHeight - 75,150,75);
        }

        public void exitTest()
        {

        }

        public void Update()
        {
            if (game.mouse.Rec.Intersects(save.Rec))
            {
                if (game.mouse.IsMouseClicked())
                {
                    saving = true;
                }
            }
            if (game.mouse.Rec.Intersects(load.Rec))
            {
                if (game.mouse.IsMouseClicked())
                {
                    loading = true;
                }
            }
            if (game.mouse.Rec.Intersects(exitButtonRect))
            {
                if (game.mouse.IsMouseClicked())
                {
                    //game.graphics.ToggleFullScreen();
                    //game.graphics.IsFullScreen = false;
                    //Environment.Exit(0);
                    
                    StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
                    sw.AutoFlush = true;
                    Console.SetOut(sw);
                    Console.Out.WriteLine("Exit");
                    //System.Threading.Thread.Sleep(400);
                    game.Exit();
                }
            }

            if (!saving)
            {
                for (int x = 0; x < SizeBoxes.Length; x++)
                {
                    SizeBoxes[x].update();
                }
                if (game.sizeChosen)
                { save.update(); }
                load.update();
            }
            if (saving)
            {
                save.update();
            }

            if (loading)
            {
                load.update();
            }
        }

        public void Draw()
        {
            if (!saving && !loading)
            {
                for (int x = 0; x < SizeBoxes.Length; x++)
                {
                    SizeBoxes[x].Draw(game.spriteBatch);
                }

                if (game.paused)
                {
                    game.spriteBatch.DrawString(game.font, "Warning: Resizing map will reset it", new Vector2(0f), Color.White);
                    save.draw();
                    load.draw();
                }
                else
                {
                    game.spriteBatch.DrawString(game.font, "Please select a map size, or load a saved map", new Vector2(0f), Color.White);
                    load.draw();
                }
            }
            else if (saving)
            {
                game.spriteBatch.DrawString(game.font, "Click the box and type a name, then click 'Save'.", new Vector2(0f), Color.White);
                save.draw();
            }
            else if (loading)
            {
                game.spriteBatch.DrawString(game.font, "Click a name, then click load.", new Vector2(0f), Color.White);
                load.draw();
            }
            game.spriteBatch.DrawString(game.font, "Exit", new Vector2(exitButtonRect.X, exitButtonRect.Y), Color.White);
        }
    }
}
