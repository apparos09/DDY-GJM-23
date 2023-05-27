using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DDY_GJM_23
{
    // Manages the title screen.
    public class TitleManager : MonoBehaviour
    {
        // The TitleManager instance.
        private static TitleManager instance;

        // Gets set to 'true' when the TitleManager is initialized.
        // This isn't needed, but it helps with the clarity.
        private bool initialized = false;

        // The title window for the title scene.
        public GameObject titleWindow;

        // The story subwindow.
        public GameObject storyWindow;

        // The controls subwindow.
        public GameObject controlsWindow;

        // The settings subwindow.
        public GameObject settingsWindow;

        // The liscences subwindow.
        public GameObject liscencesWindow;

        // Constructor
        private TitleManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Run code for initialization.
            if (!initialized)
            {
                initialized = true;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            // Show the title screen.
            OpenTitleWindow();
        }

        // Gets the instance.
        public static TitleManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<TitleManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("TitleManager (singleton)");
                        instance = go.AddComponent<TitleManager>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }

        // NAVIGATION //

        // Closes all the windows.
        public void CloseAllWindows()
        {
            titleWindow.SetActive(false);
            storyWindow.SetActive(false);
            controlsWindow.SetActive(false);
            settingsWindow.SetActive(false);
            liscencesWindow.SetActive(false);
        }


        // Opens the title window.
        public void OpenTitleWindow()
        {
            CloseAllWindows();
            titleWindow.SetActive(true);
        }

        // Activate story window.
        public void OpenStoryWindow()
        {
            CloseAllWindows();
            storyWindow.SetActive(true);
        }

        // Activate controls window.
        public void OpenControlsWindow()
        {
            CloseAllWindows();
            controlsWindow.SetActive(true);
        }

        // Activate settings window.
        public void OpenSettingsWindow()
        {
            CloseAllWindows();
            settingsWindow.SetActive(true);
        }

        // Activate liscences window.
        public void OpenLiscencesWindow()
        {
            CloseAllWindows();
            liscencesWindow.SetActive(true);
        }

        // GAME //
        // Start the game.
        public void ToGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }

        // Quits the game.
        public void QuitGame()
        {
            Application.Quit();   
        }

    }
}