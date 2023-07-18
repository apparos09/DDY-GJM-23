using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DDY_GJM_23
{
    // The gameplay manager.
    public class GameplayManager : MonoBehaviour
    {
        // The gameplay manager instance.
        private static GameplayManager instance;

        // Gets set to 'true' when the singleton is initialized.
        private bool instantiated = false;

        // If 'true', the results scene is loaded asynchronously.
        private bool ASYNC_LOAD_SCENES = true;

        // Gets set to 'true', when the first update has been called.
        private bool calledPostStart = false;

        // A countdown for the post start delay.
        private int postStartDelayTimer = 3;

        // THe game settings.
        public GameSettings settings;

        // The player.
        public Player player;

        // The total number of scraps the player has created.
        public int scrapTotal = 0;

        // The current scrap goal for the player. 
        // TODO: have this increase as the palyer keeps meeeting the goal.
        // TODO: have the game take this into account for grading.
        public int scrapGoal = 100;

        [Header("World")]

        // The game world.
        public World world;

        // The world camera.
        public WorldCamera worldCamera;

        // The home base of the game.
        public HomeBase homeBase;

        [Header("World/Item Drops")]

        // The weapon uses item prebab.
        public WeaponRefillItem weaponUsesItemPrefab;


        [Header("Canvas")]

        // The gameplay UI.
        public GameplayUI gameUI;

        // The dialogue for the game.
        public Tutorial tutorial;

        [Header("Audio")]

        // The game audio.
        public GameplayAudio gameAudio;

        [Header("Time")]

        // The maximum time for the game (in seconds)
        // TODO: 5 Minutes (300 seconds) or 7 Minutes (420 Seconds)
        public float timerMax = 300.0F;

        // The game timer (in seconds)
        public float timer = 300.0F;

        // The amount of time that has passed since the game has started.
        public float elapsedGameTime = 0.0F;

        // Pauses the timers if set to 'true'.
        public bool pausedTimers = false;

        // Pauses the game if set to 'true'.
        private bool pausedGame = false;

        // The rate the time chime goes off at.
        public const float TIME_CHIME_RATE = 60; // Time in seconds.

        // The time when the BGM starts to speed up.
        public const float BGM_SPEED_UP_TIME = 30; // Time in seconds.

        // The point in time where the 'seconds left' SFX starts to play.
        public const float TIME_SECONDS_LEFT_START = 5; // Time in seconds.

        // Constructor
        private GameplayManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        void Awake()
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
                instantiated = true;
            }
        }

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            // Grab the instance.
            if (settings == null)
                settings = GameSettings.Instance;

            // Find the player.
            if (player == null)
                player = FindObjectOfType<Player>(true);


            // WORLD
            // Finds the world component.
            if (world == null)
                world = FindObjectOfType<World>(true);

            // Find the world camera.
            if (worldCamera == null)
                worldCamera = FindObjectOfType<WorldCamera>(true);

            // Find the home base.
            if (homeBase == null)
                homeBase = FindObjectOfType<HomeBase>(true);

            // UI
            // The game UI.
            if (gameUI == null)
                gameUI = FindObjectOfType<GameplayUI>(true);


            // The game audio.
            if (gameAudio == null)
                gameAudio = FindObjectOfType<GameplayAudio>(true);


            // Dialogue not set.
            if (tutorial == null)
            {
                tutorial = FindObjectOfType<Tutorial>(true);

                // Adds the component to the game manager.
                if (tutorial == null)
                    tutorial = gameObject.AddComponent<Tutorial>();
            }

            timer = timerMax;
        }

        // Gets the instance.
        public static GameplayManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<GameplayManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Gameplay Manager (singleton)");
                        instance = go.AddComponent<GameplayManager>();
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

        // Called on the first update loop.
        private void PostStart()
        {
            // If the tutorial should be used.
            if (settings.useTutorial)
            {
                ActivateTutorial(Tutorial.trlType.opening);
            }

            // FIrst update has been called.
            calledPostStart = true;
        }

        // GAME WORLD
        // Spawns an item at the provided position.
        public void SpawnItem(Vector3 itemPos)
        {
            // TODO: implement chance rate.

            // If the weapon item prefab isn't set.
            if (weaponUsesItemPrefab != null)
            {
                WeaponRefillItem item = Instantiate(weaponUsesItemPrefab);
                item.transform.position = itemPos;
            }
        }


        // WINDOWS

        // MAP
        // Checks if the map is open.
        public bool IsMapOpen()
        {
            return gameUI.IsMapOpen();
        }

        // Opens the game map.
        public void OpenMap()
        {
            gameUI.OpenMap();
        }

        // Closes the game map.
        public void CloseMap()
        {
            gameUI.CloseMap();
        }

        // Toggles the map.
        public void ToggleMap()
        {
            gameUI.ToggleMap();
        }

        // TUTORIAL //
        // Checks if the text box is open.
        public bool IsTextBoxOpen()
        {
            bool result = gameUI.IsTextBoxOpen();
            return result;
        }

        // Opens the text box.
        public void OpenTextBox(List<string> pages)
        {
            gameUI.OpenTextBox(pages);
        }

        // Closes the text box.
        // If 'clearPages' is true, the text box elements are cleared out.
        public void CloseTextBox(bool clearPages = false)
        {
            gameUI.CloseTextBox(clearPages);
        }


        // SETTINGS //
        // Opens the game settings.
        public void OpenSettings()
        {
            gameUI.OpenSettings();
        }

        // Close the game settings.
        public void CloseSettings()
        {
            gameUI.CloseSettings();
        }

        // OTHER //
        // TUTORIAL //

        // Checks if the tutorial should be used.
        public bool GetUseTutorial()
        {
            // Get the settings instance if it's not set.
            if (settings == null)
                settings = GameSettings.Instance;

            return settings.useTutorial;
        }

        // Starts the game tutorial.
        public void ActivateTutorial(Tutorial.trlType type)
        {
            // If the tutorial shouldn't be used, do nothing.
            if (!GetUseTutorial() || tutorial == null)
                return;

            // Gets the pages.
            // If no pages are returned, then the tutorial either doesn't exist, or has been cleared already.
            List<string> pages = tutorial.GetTutorialByType(type, true);
            
            // Open the text box.
            if (pages.Count != 0)
                OpenTextBox(pages);
        }


        // SCRAP GOAL REACHED
        // Returns 'true' if the scrap goal is reached.
        public bool GetScrapGoalReached()
        {
            bool result = scrapGoal <= scrapTotal;
            return result;
        }



        // Returns value to see if the game is currently paused.
        public bool GetPausedGame()
        {
            return pausedGame;
        }

        // Sets the game to be paused.
        public void SetPausedGame(bool paused)
        {
            pausedGame = paused;

            // TODO: check for animations not bound by time scale.
            // Makes changes based on if the game is paused or not.
            if (pausedGame)
            {
                Time.timeScale = 0; // Paused
                player.enableInputs = false;

            }
            else
            {
                Time.timeScale = 1; // Normal
                player.enableInputs = true;
            }
        }


        // TIME
        // Returns the game timer, formatted.
        public string GetTimerFormatted(bool roundUp = true)
        {
            // Gets the time and rounds it up to the nearest whole number.
            float time = (roundUp) ? Mathf.Ceil(timer) : timer;

            // Formats the time.
            string formatted = StringFormatter.FormatTime(time, false, true, false);

            // Returns the formatted time.
            return formatted;
        }




        // Called to end the game.
        protected void OnTimeOver()
        {
            // NOTE: if the player is in a base area when the timer is over, have it count.
            // TODO: implement time over effects.

            ToResultsScene();
        }


        // SCENES //
        // Goes to the provided scene. Checks ASYNC_LOAD_SCENES to see what load time to use.
        public void ToScene(string sceneName)
        {
            ToScene(sceneName, ASYNC_LOAD_SCENES);
        }

        // Go to the provided scene.
        public void ToScene(string sceneName, bool asyncLoad)
        {
            // Checks if the next scene should be loaded asynchronously or not.
            if (asyncLoad) // Async Load
            {
                // Creates then next scene object, and has it not be destroyed on load.
                GameObject newObject = new GameObject("Next Scene Load");
                DontDestroyOnLoad(newObject);

                // Goes to the next scene.
                NextSceneLoad nextScene = newObject.AddComponent<NextSceneLoad>();
                nextScene.nextScene = sceneName;

                // Goes to the loading scene.
                SceneManager.LoadScene("LoadingScene");
            }
            else // Normal Load
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        // Goes to the title scene.
        public void ToTitleScene()
        {
            ToScene("TitleScene");
        }


        // Called to end the game.
        public void ToResultsScene()
        {
            // ...

            // Checks if the player is in the base.
            bool inBase = homeBase.IsPlayerInBase();

            // If the player isn't in the base.
            if(!inBase)
            {
                // Grabs the current area.
                WorldArea currArea = world.GetCurrentArea();

                // Area was set.
                if (currArea != null)
                {
                    // If the player is in a white sector, then it counts as being safe.
                    inBase = currArea.sector == WorldArea.worldSector.white;
                }
            }

            // If the player is in the base, count the scraps they have on hand. 
            if (inBase)
            {
                scrapTotal += player.scrapCount;
                player.scrapCount = 0;
            }
                

            // Creates the results data to be read in on the results screen.
            GameObject resultsObject = new GameObject("Results Data");
            DontDestroyOnLoad(resultsObject);
            ResultsData results = resultsObject.AddComponent<ResultsData>();

            // Sets if the player was alive or not.
            results.survived = inBase;

            // Set the number of deaths.
            results.deaths = player.deaths;

            // Scrap count.
            results.scrapsTotal = scrapTotal;

            // Visits.
            results.baseVisits = homeBase.visits;

            // Keys used.
            results.keysUsed = player.keysUsed;

            // Heals used.
            results.healthItemsUsed = player.healsUsed;

            // Game length.
            results.gameLength = elapsedGameTime;

            // Goals
            results.scrapGoalReached = GetScrapGoalReached();

            // Weapons
            results.gotGunSlow = player.HasWeapon(player.gunSlow);
            results.gotGunMid = player.HasWeapon(player.gunMid);
            results.gotGunFast = player.HasWeapon(player.gunFast);
            results.gotRunPower = player.HasWeapon(player.runPower);
            results.gotSwimPower = player.HasWeapon(player.swimPower);


            // Loads the results scene.
            ToScene("ResultsScene");
        }


        // Update is called once per frame
        void Update()
        {
            // Call the function for the first update.
            if (!calledPostStart)
            {
                // Checks if the timer has reached 0.
                if(postStartDelayTimer <= 0)
                {
                    PostStart();
                    postStartDelayTimer = 0;
                }
                else
                {
                    postStartDelayTimer--;
                }
            }


            // The timer isn't paused.
            if(!pausedTimers)
            {
                // Time remaining.
                if(timer > 0.0F)
                {
                    // Saves the old time before the timer is adjusted.
                    float oldTime = timer;

                    // Reduce the timer.
                    timer -= Time.deltaTime;

                    // Time over.
                    if(timer <= 0.0F)
                    {
                        // Keep timer at 0.
                        timer = 0.0F;

                        // Call time over function.
                        OnTimeOver();
                    }
                    else
                    {
                        // If a minute has passed, play the minute chime.
                        // Only do so if it's not the start of the game.
                        if (oldTime != timerMax && 
                            Mathf.Floor(oldTime / TIME_CHIME_RATE) != Mathf.Floor(timer / TIME_CHIME_RATE))
                        {
                            gameAudio.PlayTimeChimeSfx();
                        }
                        
                        // Play the seconds left time sounds if the time threshold has been passed.
                        if(timer <= TIME_SECONDS_LEFT_START && !gameAudio.sfxLoopSource.isPlaying)
                        {
                            gameAudio.PlaySecondsLeftSfx();
                        }


                        // Speeds up the music at the specified time to indicate the game is almost over.  
                        if(timer <= BGM_SPEED_UP_TIME && !gameAudio.IsBgmFastSpeed())
                        {
                            gameAudio.SetBgmFastSpeed();
                        }
                    }
                }

                // Add to the game time.
                elapsedGameTime += Time.deltaTime;
            }
        }
    }
}