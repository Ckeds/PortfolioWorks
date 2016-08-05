USERMANUAL

All of this code was written by Robert Husfeldt, as is this README!

The map editor is fairly simple to use. When first loaded it will present you with a grid of numbers, which will initialize the mapEditor to that size (all maps are square, so it's both width and height).
You will also have the option to hit load, which will bring up a list of files in the save directory, assuming nothing got moved or broken, clicking on a filename will highlight it, clicking load afterwards will
initialize a map in the file. 

The large block of tiles on the left is the editable map space. The line of tiles on the right is a 'tile bank', or like swatches in MS paint.
Clicking a tile in the tilebank 'stores' that tile in the mouse, essentially giving you a painter with that swatch. 

You can click on the editable tiles and they will change to whatever your mouse has stored, you can simply hold left mouse button and drag and you'll paint through the tiles, 
allowing fairly quick editing. For precision, you are able to lock the X or Y cooridinates (or both if you really want to I suppose) by holding the Left and Right shift keys respectively.
The mouse cursor may not show perfectly locked, but the editing mouse rectangle will always be on that axis.

At any point while editing, once a map is initialized regardless of whether you just loaded it or have a blank grid of grass, clicking save will bring up the save dialogue, with the command
"EnterName". Clicking this command will highlight it, and from then on it is editable as simply as typing into the keyboard, with backspace deleting and spacebar spacing up. There are limits, you
can't use casing at present because of the nature of the coding used to get keyboard input, it's all caps and you'll always get full key names when hitting things like the tilde key or trying
to do numbers. At present, best solution, if you're goal is to have lowercase filename, is to simply edit the file since it's easy to find.
 
Rather than adding more complication to the code, currently trying to save with the same filename will simply append an (x) where x is the first digit (starting with 0 naturally!) that has not
been used to append that file name (so repeatedly saving the same file with the samename will result in FileName, FileName(0), FileName(1), and so on).
 
Some logic remains to be written into the xml files themselves, but overall it can be considered to be Complete.
 
Hope you enjoy using the map editor!