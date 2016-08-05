using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ForeignStars
{
    /// <summary>
    /// Gets the location of a sprite based off of the texture's name
    /// 
    /// @author: Zachary Behrmann
    /// </summary>
    public static class SpriteSheetLookup
    {
        private static StreamReader reader;
        public static Dictionary<string, string[]> TilesSprites;
        public static Dictionary<string, string[]> UnitSprites;
        public static void Initialize()
        {
            TilesSprites = new Dictionary<string, string[]>();
            reader = new StreamReader("tileDescription.txt");
            string fullLine;
            do
            {
                fullLine = reader.ReadLine();
                string[] splitLine = fullLine.Split('=');
                TilesSprites.Add(splitLine[0], splitLine[1].Split(','));
            } while (!reader.EndOfStream);

            reader.Close();

            UnitSprites = new Dictionary<string, string[]>();
            reader = new StreamReader("unitDescription.txt");
            do
            {
                fullLine = reader.ReadLine();
                string[] splitLine = fullLine.Split('=');
                    UnitSprites.Add(splitLine[0], splitLine[1].Split(','));
            } while (!reader.EndOfStream);

            reader.Close();
            
        }

    }
}
