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
    public class EditableSquare : EditorTile
    {
        

        /// <summary>
        /// Constructor for editable grid of tiles, these tiles take textures from the mouse
        /// rather than giving them.
        /// </summary>
        /// <param name="x">X value of rectangle</param>
        /// <param name="y">Y value of rectangle</param>
        /// <param name="width">rectangle's width</param>
        /// <param name="height">rectangle's height</param>
        /// <param name="s">Source Rectangle to be loaded</param>
        /// <param name="m">MouseRectangle used by the program</param>
        public EditableSquare(int x, int y, int width, int height, Rectangle s, MouseRectangle m, string ID)
        { rec = new Rectangle(x, y, width, height); sourceRec = s; mouse = m; tileID = ID; spriteSheet = mouse.SpriteSheet; }

        /// <summary>
        /// Draw simply draws the rectangle and its Texture.
        /// </summary>
        /// <param name="sp"></param>
        public void draw(SpriteBatch sp)
        {
            sp.Draw(spriteSheet, rec, sourceRec, Color.White);
        }

        /// <summary>
        /// Checks if the mouse is intersecting EditableSquare and tries to take its texture.
        /// If mouse does not have one, it's GiveTexture() method will not break so that's okay.
        /// </summary>
        public void update()
        {
            if (mouse.Rec.Intersects(rec))
            {
                if (mouse.IsMouseClicked())
                {
                    mouse.GiveTexture(this);
                }
            }
        }
    }
}
