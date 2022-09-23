using System.IO;
using UnityEngine;

namespace FileManagement
{
    public static class SaveManager
    {
        public readonly static string AppDataPath = Application.dataPath;

        private static string SavesDir
        {
            get
            {
                var dir = AppDataPath + "\\Maps\\";
                
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }

                return dir;
            }
        }

        private const string FileExt = ".json";


        public static bool SaveLevel(TileData tileData)
        {
            var data = JsonUtility.ToJson(tileData);

            if (data == null)
                return false;
            else
                File.WriteAllText(SavesDir + $"Map" + FileExt, data);

            return true;
        }

        public static TileData LoadLevel()
        {
            var data = File.ReadAllText(SavesDir + $"Map" + FileExt);
            var toReturn = JsonUtility.FromJson<TileData>(data);

            return toReturn;
        }
    }

    public class TileData
    {
        public string tileName;
        public Vector2Int[] positions;
    }
}
