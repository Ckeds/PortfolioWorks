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
    public class TextureHolder : EditorTile
    {

        /// <summary>
        /// Constructs the textureHolder. 
        /// </summary>
        /// <param name="x">X value of holder's rectangle.</param>
        /// <param name="y">Y value of the holder's rectangle.</param>
        /// <param name="width">Width of holder's rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="s">Texture to use.</param>
        /// <param name="m">MouseRectangle used in this program.</param>
        /// <param name="tile">TileID of sprite in this textureholder. Important.</param>
        public TextureHolder(int x, int y, int width, int height, Rectangle s, MouseRectangle m, String tile)
        { rec = new Rectangle(x, y, width, height); sourceRec = s; mouse = m; tileID = tile; spriteSheet = mouse.SpriteSheet; }


        /// <summary>
        /// Self explanatory I think.
        /// </summary>
        /// <param name="sp"></param>
        public void draw(SpriteBatch sp)
        {
            sp.Draw(spriteSheet, rec, sourceRec, Color.White);
        }

        /// <summary>
        /// checks if the mouse is intersecting (based on the 1x1 rectangle at the mouse's point)
        /// and, only if it is, gives the mouse a copy of its texture to store.
        /// </summary>
        public void update()
        {
            if (mouse.Rec.Intersects(rec))
            {
                if (mouse.IsMouseClicked())
                {
                    mouse.TakeTexture(this);
                }
            }
        }


    }
}
