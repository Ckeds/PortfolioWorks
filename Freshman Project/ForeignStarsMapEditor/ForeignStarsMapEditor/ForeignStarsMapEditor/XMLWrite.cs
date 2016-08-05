using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ForeignStarsMapEditor
{
    class XMLWrite
    {
        private string MapName;
        private EditableSquare[][] toSave;
        private XmlWriter writer;


        public XMLWrite(string name, EditableSquare[][] map)
        {
            MapName = name; toSave = map;
            int x = 0;


            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            while (d.ToString() != "dynasty")
            {
                d = d.Parent;
            }
            string p = d.FullName + @"\GameData\Maps\";

            d = new DirectoryInfo(p);

            string originalname = name;

            FileInfo[] files = d.GetFiles();
            bool fileExists = false;

            for(int j = 0; j < files.Length; j++)
            {
                if(files[j].Name.Equals(MapName + ".xml"))
                {
                    fileExists = true;
                }
            }

            while(fileExists)
            {
                MapName = originalname + "(" + x + ")";
                x++;
                fileExists = false;
                for (int j = 0; j < files.Length; j++)
                {
                    if (files[j].Name.Equals(MapName + ".xml"))
                    {
                        fileExists = true;
                    }
                }

            }


            XmlWriterSettings mySettings = new XmlWriterSettings();
           mySettings.Indent = true;
            mySettings.IndentChars = ("\t");
            mySettings.NewLineHandling = NewLineHandling.Entitize;
            writer = XmlWriter.Create(p + MapName + ".xml",mySettings);
        }

        public void Save()
        {
            
            writer.WriteStartDocument(true);
            
            writer.WriteStartElement("map");
            writer.WriteAttributeString("size", toSave.Length + "");
            writer.WriteAttributeString("id", MapName);
            writer.WriteStartElement("tiles");


            int x = 0;
            int y = 0;
            string tileID;
            int instanceOfID;
            string nextID;

            while (y < toSave.Length)
            {

                tileID = toSave[y][x].TileID;
                instanceOfID = 1;
                if (x != (toSave.Length - 1))
                {
                    nextID = toSave[y][x+1].TileID;
                }
                else if (y != (toSave.Length - 1))
                {
                    nextID = toSave[(y + 1)][x].TileID;
                }
                else
                {
                    nextID = "EndOfFile";
                }

                while (nextID == tileID)
                {
                    x++; instanceOfID++;

                    if (x == toSave.Length)
                    { y++; x = 0; }

                    if (y >= toSave.Length)
                    { break; }

                    if (x != (toSave.Length - 1))
                    {
                        nextID = toSave[y][x + 1].TileID;
                    }
                    else if (y != (toSave.Length - 1))
                    {
                        nextID = toSave[(y + 1)][0].TileID;
                    }
                    else
                    {
                        break;
                    }
                }

                writer.WriteStartElement("tile");
                writer.WriteAttributeString("terrain", tileID);
                writer.WriteAttributeString("length", ""+instanceOfID);
                writer.WriteEndElement();
                x++;
                if (x == toSave.Length)
                { y++; x = 0; }
            }

            writer.WriteEndElement();

            writer.WriteStartElement("spawns");
            writer.WriteStartElement("tile");
            writer.WriteAttributeString("location", "0,0,1," + toSave.Length);
            writer.WriteEndElement();
            writer.WriteStartElement("tile");
            writer.WriteAttributeString("location", "" + (toSave.Length - 1) + ",0," + (toSave.Length) + "," + (toSave.Length));
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
