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
    class MapSelectTile
    {
        public String widthValue;
        public int width;
        public Rectangle rec;
        public MouseRectangle mouse;
        public Texture2D placeholder;
        public Vector2 valuestringLocation;

        public MapSelectTile(int x, int y, int w, String sw, MouseRectangle m, GraphicsDevice graphicsDevice)
        { 
            rec = new Rectangle(x, y, 32, 32); mouse = m; width = w; 
            widthValue = sw; placeholder = new Texture2D(graphicsDevice, 30, 30);
            valuestringLocation = new Vector2(x, y);
        }

        public void draw(SpriteBatch sp, SpriteFont font)
        {
            sp.Draw(placeholder, rec, Color.OrangeRed);
            sp.DrawString(font, widthValue, valuestringLocation, Color.White);
        }

        public int getmapValue()
        {
            if (mouse.Rec.Intersects(rec))
            {
                if (mouse.IsMouseClicked())
                {
                    return width;
                }
            }

            return -1;
        }


    }
}
