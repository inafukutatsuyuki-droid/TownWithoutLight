using System.IO;
using UnityEngine;

namespace TownWithoutLight.Systems
{
    /// <summary>
    /// Serializes lightweight save data to JSON in Application.persistentDataPath.
    /// </summary>
    public static class SaveSystem
    {
        private const string SaveFileName = "savegame.json";

        public static string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

        public static void Save(GameSaveData data)
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(SavePath, json);
        }

        public static bool TryLoad(out GameSaveData data)
        {
            data = default;
            if (!File.Exists(SavePath))
            {
                return false;
            }

            string json = File.ReadAllText(SavePath);
            data = JsonUtility.FromJson<GameSaveData>(json);
            return true;
        }

        public static void Delete()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }
    }

    [System.Serializable]
    public struct GameSaveData
    {
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        public string checkpointId;
    }
}
