using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class SaveButton : EditorTile
    {
        private OptionsMenu menu;
        public FileNameEntry psuedotextBox;
        XMLWrite writer;

        public SaveButton(OptionsMenu op)
        {
            menu = op;
            rec = new Rectangle(menu.game.gameWidth / 2, menu.game.gameHeight - 100, 100, 20);
            psuedotextBox = new FileNameEntry(mouse, menu,new Rectangle(menu.game.gameWidth/2, menu.game.gameHeight/2, 100,100),  "EnterName");

        }


        public void update()
        {
            if (menu.game.paused)
            {
                if (menu.saving)
                {
                    psuedotextBox.update();

                    if (psuedotextBox.highlighted)
                    {
                        if (menu.game.mouse.Rec.Intersects(rec) && menu.game.mouse.IsMouseClicked())
                        {
                            writer = new XMLWrite(psuedotextBox.name, menu.game.map.mapGrid);
                            writer.Save();
                            menu.saving = false;
                            psuedotextBox.highlighted = false;
                            menu.game.paused = false;
                            menu.loading = false;
                            menu.load.updateList();

                        }
                    }
                }
            }
        }

        public void draw()
        {
            if (menu.saving)
            {
                psuedotextBox.draw();
            }
            menu.game.spriteBatch.DrawString(menu.game.font, "Save", new Vector2((float)(rec.X), (float)(rec.Y)), Color.White);
        }
    }
}
