using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ForeignStars
{
    public class Scenario
    {
        private string mapFile;
        private string image;
        private string description;
        private string xmlFilePath;
        private string name;
        private int armyAmount;
        private Army[] armies;

        public string Description
        {
            get { return description; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Thumbnail
        {
            get { return image; }
        }

        public Army[] Armies
        {
            get { return armies; }
        }

        public string MapFile
        {
            get { return mapFile; }
        }
        public int ArmyAmount
        {
            get { return armyAmount; }
        }

        public Scenario(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            XmlReader reader = XmlReader.Create(xmlFilePath);
            reader.ReadToFollowing("scenario");
            name = reader.GetAttribute("name");
            armyAmount = int.Parse(reader.GetAttribute("armies"));
            reader.ReadToFollowing("map");
            mapFile = reader.GetAttribute("file");
            image = reader.GetAttribute("thumbnail");
            reader.ReadToFollowing("description");
            description = reader.GetAttribute("text");
            reader.Close();
        }

        public void LoadScenarioArmies()
        {
            XmlReader reader = XmlReader.Create(xmlFilePath);

            armies = new Army[armyAmount];
            int currentArmyNum = 0;
            reader.ReadToFollowing("army");

            do
            {
                int teamNumber = int.Parse(reader.GetAttribute("team"));
                List<Unit> tempUnits = new List<Unit>();
                reader.ReadToFollowing("unit");
                do
                {
                    string type = reader.GetAttribute("type");
                    switch (type)
                    {
                        case "helicopter": tempUnits.Add(
                            new Helicopter(null,
                                Convert.ToInt32(reader.GetAttribute("x")),
                                Convert.ToInt32(reader.GetAttribute("y")),
                                currentArmyNum, reader.GetAttribute("direction")));
                            break;
                        case "infantry": tempUnits.Add(
                            new Infantry(null,
                                Convert.ToInt32(reader.GetAttribute("x")),
                                Convert.ToInt32(reader.GetAttribute("y")),
                                currentArmyNum, reader.GetAttribute("direction")));
                            break;
                        case "tank": tempUnits.Add(
                        new Tank(null,
                            Convert.ToInt32(reader.GetAttribute("x")),
                            Convert.ToInt32(reader.GetAttribute("y")),
                            currentArmyNum, reader.GetAttribute("direction")));
                            break;
                        case "skimmer": tempUnits.Add(
                        new Skimmer(null,
                            Convert.ToInt32(reader.GetAttribute("x")),
                            Convert.ToInt32(reader.GetAttribute("y")),
                            currentArmyNum, reader.GetAttribute("direction")));
                            break;
                        default: break;
                    }
                } while (reader.ReadToNextSibling("unit"));
                armies[currentArmyNum] = new Army(tempUnits, teamNumber, currentArmyNum);
                currentArmyNum++;
            } while (reader.ReadToNextSibling("army"));

        }
    }
}
