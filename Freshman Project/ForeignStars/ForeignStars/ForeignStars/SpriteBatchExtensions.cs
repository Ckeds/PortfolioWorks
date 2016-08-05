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

namespace ForeignStars
{
    /// <summary>
    /// A static class that currently only has one method.
    /// This class will contain enhancements to the original SpriteBatch class.
    /// It doesn't inherit from SpriteBatch because it passes in a spriteBatch parameter to its only current method and uses spriteBatch's methods.
    /// We may make SpriteBatchExtensions inherit from SpriteBatch, then change all SpriteBatch references to the object type SpriteBatchExtensions.
    /// 
    /// @author: Ryan Conrad
    /// </summary>
    public static class SpriteBatchExtensions
    {
        /// <summary>
        /// Takes in a string, splits it into a list of strings, and prints each element of that list to a separate line by incrementing y with each element centered about the x axis.
        /// The string is measured with spriteFont.MeasureString.
        /// spriteFont.MeasureString may be used in other areas of code in the future involving drawing strings with the spriteBatch to increase efficiency.
        /// </summary>
        /// <param name="spriteBatch">Current spriteBatch object</param>
        /// <param name="spriteFont">Current spritefont</param>
        /// <param name="text">The text to be split</param>
        /// <param name="y">The y position of the string (increments by 25 with each new line)</param>
        /// <param name="color">Color of the text</param>
        public static void DrawStringCentered(this SpriteBatch spriteBatch, SpriteFont spriteFont, String text, Single y, Color color)
        {
            List<String> lines = SplitLines(text);
            foreach (String line in lines)
            {
                Vector2 textBounds = spriteFont.MeasureString(line);
                Single centerX = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth * 0.5f - textBounds.X * 0.5f;

                spriteBatch.DrawString(spriteFont, line, new Vector2(centerX, y), color);
                y += 25;
            }
        }
        /// <summary>
        /// A private method that parses the passed in string from the DrawStringCentered method.
        /// The delimiter will most likely change.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static List<String> SplitLines(String text)
        {
            List<String> lines;
            lines = new List<String>(text.Split('-'));
            return lines;
        }
    }
}
