using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Device;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class reads the unit data from file and puts it into a
    /// dictionary where the key is the unit type and the value is an array of
    /// the damage, health, movement points, and attack radius. It will also
    /// hold the cost of the unit in the future.
    /// </summary> ***********************************************************
    public static class UnitTypesData
    {
        private static readonly Dictionary<Unit.UnitType, int[]> UnitTypeDataDictionary = new Dictionary<Unit.UnitType, int[]>();

        public static int[] GetStats(Unit.UnitType type)
        {
            return UnitTypeDataDictionary[type];
        }
        public static void InitUnitTypeDict()
        { 
            string path = Path.Combine(Application.streamingAssetsPath, "UnitTypeData.txt");
            StreamReader reader = new StreamReader(path);
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
                        Int32.Parse(splitLine[4]),
                        Int32.Parse(splitLine[5]),
                        Int32.Parse(splitLine[6]),
                        Int32.Parse(splitLine[7])
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