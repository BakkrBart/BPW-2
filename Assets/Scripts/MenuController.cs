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

        bool isPaused = false;

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
