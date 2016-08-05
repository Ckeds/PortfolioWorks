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
using System.IO;

namespace ForeignStarsMapEditor
{
    public class LoadButton : EditorTile
    {
        private OptionsMenu menu;
        private Vector2 drawString;
        private FileNameEntry[] filenames;
        private XMLRead reader;

        public LoadButton(OptionsMenu op) 
        {
            menu = op;
            rec = new Rectangle((menu.game.gameWidth / 2) - 100, menu.game.gameHeight - 100, 100, 20);
            drawString = new Vector2((float)(rec.X), (float)(rec.Y));
            mouse = menu.game.mouse;

            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            while (d.ToString() != "dynasty")
            {
                d = d.Parent;
            }
            string p = d.FullName + @"\GameData\Maps\";
            d = new DirectoryInfo(p);

            FileInfo[] files = d.GetFiles();
            int x = 0;
            filenames = new FileNameEntry[files.Length];
            foreach(FileInfo file in files)
            {
                Rectangle rect = new Rectangle(menu.game.gameWidth/2, x*30, 100, 30);
                filenames[x] = new FileNameEntry(menu.game.mouse, menu, rect , file.Name);
                x++;
            }

        }

        public void Unhighlight(int indexofException)
        {
            for (int x = 0; x < filenames.Length; x++)
            {
                if (x != indexofException)
                {
                    filenames[x].highlighted = false;
                    filenames[x].lasthighLighted = false;
                }
                if (indexofException != -1)
                {
                    filenames[indexofException].lasthighLighted = true;
                }
            }
        }

        public void updateList()
        {
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            while (d.ToString() != "dynasty")
            {
                d = d.Parent;
            }
            string p = d.FullName + @"\GameData\Maps\";
            d = new DirectoryInfo(p);

            FileInfo[] files = d.GetFiles();
            int x = 0;
            filenames = new FileNameEntry[files.Length];
            foreach (FileInfo file in files)
            {
                Rectangle rect = new Rectangle(menu.game.gameWidth / 2, x * 30, 100, 30);
                filenames[x] = new FileNameEntry(menu.game.mouse, menu, rect, file.Name);
                x++;
            }
        }

        public void update()
        {
            if (menu.loading)
            {
                foreach (FileNameEntry f in filenames)
                {
                    f.update();
                }
                for (int y = 0; y < filenames.Length; y++)
                {
                    if (filenames[y].highlighted && !filenames[y].lasthighLighted)
                    { Unhighlight(y); }

                }

                if(mouse.Rec.Intersects(rec) && mouse.IsMouseClicked())
                {
                    foreach(FileNameEntry f in filenames)
                    {
                        if(f.highlighted)
                        {
                            reader = new XMLRead(menu);
                            EditableSquare[][] mapGrid = reader.Load(f.name);
                            menu.game.paused = false;
                            menu.loading = false;
                            menu.saving = false;
                            menu.game.sizeChosen = true;
                            menu.game.map = new EditableMap(mapGrid.Length, menu.game.mouse, menu.game.tiles, menu.game.tileIDs, menu.game.gameWidth, menu.game.gameHeight, true, mapGrid);
                            this.Unhighlight(-1);
                        }
                    }
                }

            }
            else
            {
                if(mouse.Rec.Intersects(rec) && mouse.IsMouseClicked())
                {
                    menu.loading = true;
                }
            }

        }

        public void draw()
        {
            menu.game.spriteBatch.DrawString(menu.game.font, "Load", drawString, Color.White);

            if(menu.loading)
            {
            foreach (FileNameEntry f in filenames)
            {
                f.draw();
            }
        }
        }

        }
    }
