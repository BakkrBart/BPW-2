using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace roguelike
{
    public class DungeonCompletion : MonoBehaviour
    {
        public float enemiesInScene;

        public DungeonSettings settings;
        public static int dungeonNumber;

        public GameObject victoryScreen;
        public GameObject ingameUI;
        public GameObject gameController;

        public Text dungeonsComplete;

        private void Start()
        {
            if (SaveData.loaded == 1)
            {
                dungeonNumber = settings.dungeonNumber;
            }
            else if (SaveData.fresh_loaded == 1)
            {
                dungeonNumber = 1;
            }
            else 
            {
                dungeonNumber += 1;
            }
            dungeonsComplete.text = dungeonNumber.ToString();

            Time.timeScale = 1;
        }

        private void Update()
        {
            if (enemiesInScene == 0)
            {
                victoryScreen.gameObject.SetActive(true);
                ingameUI.gameObject.SetActive(false);
                Time.timeScale = 0;
            }

        }

        public void Restart()
        {
            SaveData.reloaded = 1;
            SaveData.loaded = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
