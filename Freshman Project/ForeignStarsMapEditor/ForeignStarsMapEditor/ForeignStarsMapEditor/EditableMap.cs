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
    public class EditableMap
    {
        TextureHolder[] options;
        public EditableSquare[][] mapGrid;
        MouseRectangle mouse;
        int side;

        public EditableMap(int side, MouseRectangle mouse, List<Rectangle> tiles, List<string> tileIDs, int windowWidth, int windowHeight, bool XML, EditableSquare[][] grid)
        {
            this.side = side;
            this.mouse = mouse;
            mapGrid = new EditableSquare[side][];


            if (!XML)
            {
                for (int y = 0; y < side; y++)
                {
                    mapGrid[y] = new EditableSquare[side];
                    for (int x = 0; x < side; x++)
                    {
                        //mapGrid[y][x] = new EditableSquare(x * 32, y * 32, 32, 32, tiles[0], mouse, tileIDs[0]);
                        mapGrid[y][x] = new EditableSquare(x * 32, y * 32, 32, 32, tiles[0], mouse, tileIDs[0]);
                    }
                }

            }
            else
            {
                mapGrid = grid;
            }
            options = new TextureHolder[tiles.Count];

            for (int y = 0; y < tiles.Count; y++)
            {
                options[y] = new TextureHolder(windowWidth - 32, y * 32, 32, 32, tiles[y], mouse, tileIDs[y]);
            }
        }

        public void Update()
        {
            for (int x = 0; x < side; x++)
            {
                for (int y = 0; y < side; y++)
                {
                    mapGrid[y][x].update();
                }
            }

            for (int x = 0; x < options.Length; x++)
            {
                options[x].update();
            }
        }

        public void Draw(SpriteBatch sp)
        {
            for (int x = 0; x < side; x++)
            {
                for (int y = 0; y < side; y++)
                {
                    mapGrid[y][x].draw(sp);
                }
            }

            for (int x = 0; x < options.Length; x++)
            {
                options[x].draw(sp);
            }

        }
    }
}
