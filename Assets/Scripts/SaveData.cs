using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace roguelike
{
    [System.Serializable]
    public class SaveData : MonoBehaviour
    {
        public string fileName;
        private string fullPath;
        public static int loaded;
        public static int fresh_loaded;
        public static int reloaded;
        public static int saved;
        public static int loadedMenu;

        public GameObject gameController;
        public DungeonSettings settings;
        public GameObject noSaveFile;

        void Start()
        {
            noSaveFile.SetActive(false);
            fullPath = Application.persistentDataPath + fileName + ".json";
            
            if (loadedMenu == 1)
            {
                loadedMenu = 0;
                Load();
            }
        }

        public void Save()
        {
            settings.seed = gameController.GetComponent<DungeonGenerator>().seed;
            settings.dungeonNumber = DungeonCompletion.dungeonNumber;
            StreamWriter writer = new StreamWriter(fullPath, false);
            writer.WriteLine(JsonUtility.ToJson(settings, true));
            writer.Close();
            writer.Dispose();
            saved = 1;
        }

        public void Load()
        {
            if (File.Exists(fullPath))
            {
                loaded = 1;
                reloaded = 0;
                fresh_loaded = 0;
                StreamReader reader = new StreamReader(fullPath);
                DungeonSettings saveSettings = new DungeonSettings();
                JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), saveSettings);
                settings.dungeonNumber = saveSettings.dungeonNumber;
                settings.seed = saveSettings.seed;
                reader.Close();
                reader.Dispose();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } else
            {
                noSaveFile.SetActive(true);
            }
        }

        public void Restart()
        {
            reloaded = 1;
            loaded = 0;
            SaveData.fresh_loaded = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
