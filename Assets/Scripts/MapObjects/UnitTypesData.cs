using System;
using System.Collections.Generic;
using System.IO;

namespace MapObjects
{
    public static class UnitTypesData
    {
        public static Dictionary<Unit.UnitType, int[]> UnitTypeDataDictionary = new Dictionary<Unit.UnitType, int[]>();

        public static int[] GetStats(Unit.UnitType type)
        {
            return UnitTypeDataDictionary[type];
        }
            public static void InitUnitTypeDict()
        {
            StreamReader reader = new StreamReader("Assets/Scripts/MapObjects/UnitTypeData.txt");
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    string[] splitLine = line.Split(" ");
                    int[] stats =
                    {
                        Int32.Parse(splitLine[1]),
                        Int32.Parse(splitLine[2]),
                        Int32.Parse(splitLine[3]),
                        Int32.Parse(splitLine[4])
                    };
                    
                    switch (splitLine[0])
                    {
                        case "Melee":
                            UnitTypeDataDictionary.Add(Unit.UnitType.Melee, stats);
                            break;
                        case "Ranged":
                            UnitTypeDataDictionary.Add(Unit.UnitType.Ranged, stats);
                            break;
                        case "Airship":
                            UnitTypeDataDictionary.Add(Unit.UnitType.Airship, stats);
                            break;
                        case "Settler":
                            UnitTypeDataDictionary.Add(Unit.UnitType.Settler, stats);
                            break;
                    }
                }
            }
        }
    }
}