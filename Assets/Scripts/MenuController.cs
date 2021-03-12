using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace roguelike
{
    public class MenuController : MonoBehaviour
    {
        public GameObject MainMenu;
        public GameObject PauseScreen;
        public GameObject IngameUI;
        public GameObject noSaveFile;

        bool isPaused = false;

        private void start()
        {
            noSaveFile.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1;
            SaveData.loaded = 0;
            SaveData.reloaded = 1;
            SaveData.fresh_loaded = 1;
        }

        public void LoadGame()
        {
            if (SaveData.saved == 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Time.timeScale = 1;
                SaveData.loaded = 1;
                SaveData.reloaded = 0;
                SaveData.fresh_loaded = 0;
                SaveData.loadedMenu = 1;
            } else
            {
                noSaveFile.SetActive(true);
            }
        }

        public void TogglePause()
        {
            if (isPaused)
            {
                PauseScreen.gameObject.SetActive(false);
                IngameUI.gameObject.SetActive(true);
                Time.timeScale = 1;
                isPaused = false;
            }
            else
            {
                PauseScreen.gameObject.SetActive(true);
                IngameUI.gameObject.SetActive(false);
                Time.timeScale = 0;
                isPaused = true;
            }
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
