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
    public class EditorTile
    {
        protected Texture2D spriteSheet;
        protected Rectangle sourceRec;
        protected Rectangle rec;
        protected MouseRectangle mouse;
        protected String tileID;

        public String TileID
        {
            get { return tileID; }
            set { tileID = value; }
        }

        public Rectangle Rec
        { get { return rec; } }

        public Rectangle SourceRec
        {
            set { sourceRec = value; }
            get { return sourceRec; }
        }

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
        }

    }
}
