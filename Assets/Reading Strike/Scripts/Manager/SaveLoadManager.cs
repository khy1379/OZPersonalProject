using System.IO;
using UnityEngine;

namespace ReadingStrike.Manager
{
    public static class SaveLoadManager
    {
        public static void SaveDataPlF(string dataName, float fValue) { PlayerPrefs.SetFloat(dataName, fValue); }
        public static float LoadDataPlF(string dataName, float defaultFValue) { return PlayerPrefs.GetFloat(dataName, defaultFValue); }

        public static void SaveDataPlB(string dataName, bool bValue) { PlayerPrefs.SetInt(dataName, bValue ? 1 : 0); }
        public static bool LoadDataPlB(string dataName, bool defaultBValue) { return PlayerPrefs.GetInt(dataName, defaultBValue ? 1 : 0) == 1; }
        public static bool TrySaveDataJson<T>(string fileName, in T data) where T : new()
        {
            if (data == null)
            {
                Debug.LogWarning("Data 없음");
                return false;
            }
            try
            {
                string directoryPath = Path.Combine(Application.persistentDataPath, "SaveData");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string path = Path.Combine(directoryPath, $"{fileName}.json");
                string saveJson = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, saveJson);
                return true;
            }
            catch
            {
                Debug.LogWarning("Save 실패");
                return false;
            }
        }
        public static bool TryLoadDataJson<T>(string fileName, out T data) where T : new()
        {
            string path = Path.Combine(Application.persistentDataPath, $"SaveData/{fileName}.json");
            if (!File.Exists(path))
            {
                Debug.LogWarning("Save 파일 없음, 신규 Data 반환");
                data = new T();
                return false;
            }
            try
            {
                string loadJson = File.ReadAllText(path);
                data = JsonUtility.FromJson<T>(loadJson);
                if(data == null)
                {
                    Debug.LogWarning("Data 없음, 신규 Data 반환");
                    data = new T();
                    return false;
                }
                return true;
            }
            catch
            {
                Debug.LogWarning("Load 실패, 신규 Data 반환");
                data = new T();
                return false;
            }
        }
    }
}