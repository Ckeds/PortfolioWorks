using System;

namespace ForeignStars
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// 
        /// Team Dynasty
        /// @author: Rob Husfeldt
        /// @author: Jenny Li
        /// @author: William Powell
        /// @author: Ryan Conrad
        /// @author: Zachary Behrmann
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

