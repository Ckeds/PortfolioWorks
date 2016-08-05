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

namespace ForeignStarsMapEditor
{
    public class SetUpBox : EditorTile
    {
            int size;
            OptionsMenu options;

            public SetUpBox(int size, int x, int y, OptionsMenu o, MouseRectangle m)
            {
                options = o;
                this.size = size;
                rec = new Rectangle(x, y, 32, 32);
                mouse = m;
                TileID = size + "";
                options = o;
            }

            public void Draw(SpriteBatch sp)
            {
                sp.DrawString(options.game.font, TileID, new Vector2((float)(rec.X), (float)(rec.Y)), Color.White);
            }

            public void update()
            {
                if (mouse.Rec.Intersects(rec))
                {
                    if (mouse.IsMouseClicked())
                    {
                        if (options.game.sizeChosen)
                        {
                            options.game.paused = false;
                        }
                        else
                        {
                            options.game.sizeChosen = true;
                        }
                        
                        options.game.map = new EditableMap(size, mouse, options.game.tiles, options.game.tileIDs, options.game.gameWidth,options.game.gameHeight, false, null);
                        mouse.SourceRec = new Rectangle(0,0,0,0);

                    }
                }
            }

    }
}
