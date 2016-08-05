using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ForeignStarsMapEditor
{
    class XMLRead
    {
        public OptionsMenu menu;

        public XMLRead(OptionsMenu op)
        {
            menu = op;
        }

        public EditableSquare[][] Load(string xmlFile)
        {
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            while (d.ToString() != "dynasty")
            {
                d = d.Parent;
            }
            string p = d.FullName + @"\GameData\Maps\";
            XmlReader mapReader = XmlReader.Create(p + xmlFile);
            int currentRow = 0;
            int currentCol = 0;

            EditableSquare[][] mapGrid;

            mapReader.ReadToFollowing("map"); //read to the first map element available
            int mapSize = Convert.ToInt32(mapReader.GetAttribute("size"));
            mapGrid = new EditableSquare[mapSize][];
            string tileID;

            for (int x = 0; x < mapSize; x++)
            {
                mapGrid[x] = new EditableSquare[mapSize];
            }
            currentRow = 0;
            currentCol = 0;
            mapReader.ReadToFollowing("tile");
            do
            {
                int length = Convert.ToInt32(mapReader.GetAttribute("length"));
                tileID = mapReader.GetAttribute("terrain");
                int sourceRecIndex = 0;

                for (int x = 0; x < menu.game.tileIDs.Count; x++)
                {
                    if (menu.game.tileIDs[x].Equals(tileID))
                    { sourceRecIndex = x; }
                }

                for (int i = 0; i < length; i++)
                {

                    mapGrid[currentRow][currentCol] = new EditableSquare(currentCol * 32, currentRow * 32, 32, 32, menu.game.tiles[sourceRecIndex], menu.game.mouse, tileID);

                    if (currentCol != mapSize - 1)
                    {
                        currentCol++;
                    }
                    else
                    {
                        currentRow++;
                        currentCol = 0;
                    }
                }
            } while (mapReader.ReadToNextSibling("tile"));


            mapReader.Close();

            return mapGrid;
        }

    }
}
