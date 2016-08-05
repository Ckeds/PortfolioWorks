using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForeignStars
{
    /// <summary>
    /// The GlobalVariables Class
    /// Holds variables that all classes should have access to, such as our current tile size
    /// </summary>
    static class GV
    {
        static private int tileSize = 32;

        static public int TileSize
        {
            get { return tileSize; }
        }


    }
}
