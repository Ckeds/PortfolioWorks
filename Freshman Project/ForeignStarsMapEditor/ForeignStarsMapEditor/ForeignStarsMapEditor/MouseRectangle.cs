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
    public class MouseRectangle : EditorTile
    {
        protected bool takeSprite = true;
        public int lockedX;
        public int lockedY;
        public Rectangle defaultRec;

        /// <summary>
        /// Returns a bool of whether or not the left mouse button is pressed down.
        /// </summary>
        /// <returns></returns>
        public bool IsMouseClicked()
        {
            MouseState m = Mouse.GetState();
            return (m.LeftButton == ButtonState.Pressed);
        }


        /// <summary>
        /// Initializes mouseRectangle. locked values are set to 0, considered 'default', 
        /// and Rectangle is set to 0,0 with only 1x1 size. Will immedietally follow the mouse on first
        /// update, before any intersection updates are called.
        /// </summary>
        public MouseRectangle(Texture2D s)
        { 
            rec = new Rectangle(0, 0, 1, 1); lockedX = 0; lockedY = 0;
            spriteSheet = s;
            defaultRec = new Rectangle(0, 0, 0, 0);
            sourceRec = defaultRec;
        }

        /// <summary>
        /// Causes rectangle follows the mouse cursor.
        /// </summary>
        public void FollowMouse()
        { 
            MouseState ms = Mouse.GetState();

                int x;
                int y;

                x = ms.X;
                y = ms.Y;

                rec.Location = new Point(x, y);
        }


        /// <summary>
        /// Mouse takes the texture of a TextureHolder that it's given.
        /// Should be called by textureholder if intersected by mouse.
        /// </summary>
        /// <param name="t"></param>
        public void TakeTexture(TextureHolder t)
        {
                sourceRec = t.SourceRec;
                tileID = t.TileID;
        }

        /// <summary>
        /// Mouse passes its texture (if any) to editablesquare that it's given.
        /// Should only be called by editable square.
        /// </summary>
        /// <param name="e"></param>
        public void GiveTexture(EditableSquare e)
        {
            if (sourceRec != defaultRec )
            {
                e.SourceRec = sourceRec;
                e.TileID = tileID;
            }
        }



    }
}
