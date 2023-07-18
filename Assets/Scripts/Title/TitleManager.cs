using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static util.AudioCredits;
using util;

namespace DDY_GJM_23
{
    // Manages the title screen.
    public class TitleManager : MonoBehaviour
    {
        // The TitleManager instance.
        private static TitleManager instance;

        // Gets set to 'true' when the TitleManager is initialized.
        // This isn't needed, but it helps with the clarity.
        private bool instantiated = false;

        // Checks if the game should asynchronously load or not.
        public const bool ASYNC_LOAD_GAME_SCENE = true;

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

        // The quit button for the game.
        public Button quitButton;

        [Header("Other")]

        // The title audio for the manager.
        public TitleAudio titleAudio;

        // The credits interface.
        public AudioCreditsInterface credits;

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
            if (!instantiated)
            {
                // Game has been instantiated.
                instantiated = true;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            // Show the title screen.
            OpenTitleWindow();

            // Disable the quit button since this is WebGL.
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                quitButton.interactable = false;

            // AUDIO //
            // If title audio isn't set, try to find the object.
            if(titleAudio == null)
            {
                titleAudio = FindObjectOfType<TitleAudio>();
            }
                

            // AUDIO CREDITS //

            // The audio credits for the game.
            List<AudioCredit> audioCredits = new List<AudioCredit>();

            // Loading Audio Credits
            // Title 
            AudioCredit ac = new AudioCredit();
            ac.title = "Dawn of the Apocaylse";
            ac.artists = "Rafael Krux";
            ac.collection = "FreePD/Horror";
            ac.source = "FreePD";
            ac.link1 = "https://freepd.com/horror.php";
            ac.link2 = "https://music.orchestralis.net/track/28566414";

            ac.copyright = "\"Dawn of the Apocalypse\" by Rafael Krux (orchestralis.net)" +
                "\nLicensed under Creative Commons: By Attribution 4.0 International (CC BY 4.0)" +
                "\nhttps://creativecommons.org/licenses/by/4.0/";

            audioCredits.Add(ac);

            // Loading
            ac = new AudioCredit();
            ac.title = "Rapture";
            ac.artists = "Ross Bugden";
            ac.collection = "Music - Ross Bugden";
            ac.source = "GameSounds.xyz, YouTube";
            ac.link1 = "https://gamesounds.xyz/?dir=Music%20-%20Ross%20Bugden";
            ac.link2 = "https://www.youtube.com/watch?v=vja87ZXejyk";

            ac.copyright = "\"Rapture\" by Ross Bugden (https://youtu.be/vja87ZXejyk)" +
                "\nConfirmed to be free to copy, modify, distribute, and perform for work, even for commercial purposes, all without asking permission.";

            audioCredits.Add(ac);

            // Gameplay 
            ac = new AudioCredit();
            ac.title = "Mysterious Lights";
            ac.artists = "Bryan Teoh";
            ac.collection = "FreePD/Horror";
            ac.source = "FreePD";
            ac.link1 = "https://freepd.com/horror.php";
            ac.link2 = "https://www.bryanteoh.com/";

            ac.copyright = "Mysterious Lights\" by Bryan Teoh" +
                "\nLicensed under Creative Commons: CC0 1.0 Universal (CC0 1.0) Public Domain Dedication" +
                "\nhttps://creativecommons.org/publicdomain/zero/1.0/";

            audioCredits.Add(ac);

            // Results 
            ac = new AudioCredit();
            ac.title = "No Winners";
            ac.artists = "Ross Bugden";
            ac.collection = "Music - Ross Bugden";
            ac.source = "GameSounds.xyz, YouTube";
            ac.link1 = "https://gamesounds.xyz/?dir=Music%20-%20Ross%20Bugden";
            ac.link2 = "https://www.youtube.com/watch?v=9qk-vZ1qicI";

            ac.copyright = "\"No Winners\" by Ross Bugden (https://youtu.be/9qk-vZ1qicI)" +
                "\nConfirmed to be free to copy, modify, distribute, and perform for work, even for commercial purposes, all without asking permission.";

            audioCredits.Add(ac);

            // Set the audio credits.
            credits.audioCredits.audioCredits = audioCredits;


            // SFXs //
            // Sound Effects
            ac = new AudioCredit();
            ac.title = "Sound Effects (Sourced)";
            ac.artists = "Unknown Artists";
            ac.collection = "Sound Effects";
            ac.source = "GameSounds.xyz";
            ac.link1 = "https://gamesounds.xyz/?dir=Sound%20Effects";
            ac.link2 = "";

            ac.copyright = "Confirmed to be copyright free by GameSounds.xyz, even for commercial purposes.";

            audioCredits.Add(ac);

            // Set the audio credits.
            credits.audioCredits.audioCredits = audioCredits;
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
                return instantiated;
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
            // The game scene.
            string gameScene = "GameScene";

            // Gets the loading type.
            bool asyncLoad = ASYNC_LOAD_GAME_SCENE;

            // Checks if the game should be loaded asynchronously or not.
            if(asyncLoad) // Async Load
            {
                // Creates then next scene object, and has it not be destroyed on load.
                GameObject newObject = new GameObject("Next Scene Load");
                DontDestroyOnLoad(newObject);

                // Goes to the next scene.
                NextSceneLoad nextScene = newObject.AddComponent<NextSceneLoad>();
                nextScene.nextScene = gameScene;

                // Goes to the loading scene.
                SceneManager.LoadScene("LoadingScene");
            }
            else // Normal Load
            {
                SceneManager.LoadScene(gameScene);
            }
          
        }

        // Quits the game.
        public void QuitGame()
        {
            Application.Quit();   
        }

    }
}