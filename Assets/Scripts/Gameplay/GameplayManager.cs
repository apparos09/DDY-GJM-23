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
        private bool ASYNC_LOAD_RESULTS_SCENE = true;

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
        public WeaponUsesItem weaponUsesItemPrefab;


        [Header("Canvas")]

        // The gameplay UI.
        public GameplayUI gameUI;

        // The dialogue for the game.
        public Tutorial tutorial;

        [Header("Other")]

        // The game timer (in seconds) - 5 Minutes (300 Seconds)
        public float timer = 300.0F;

        // The amoutn of time that has passed since the game has started.
        public float elapsedGameTime = 0.0F;

        // Pauses the timers if set to 'true'.
        public bool pausedTimers = false;

        // Pauses the game if set to 'true'.
        private bool pausedGame = false;

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


            // Dialogue not set.
            if (tutorial == null)
            {
                tutorial = FindObjectOfType<Tutorial>(true);

                // Adds the component to the game manager.
                if (tutorial == null)
                    tutorial = gameObject.AddComponent<Tutorial>();
            }


            //// If the tutorial should be used.
            //if(GameSettings.Instance.useTutorial)
            //{
            //    OpenTextBox(tutorial.GetDebugText());
            //}
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

        // GAME WORLD
        // Spawns an item at the provided position.
        public void SpawnItem(Vector3 itemPos)
        {
            // TODO: implement chance rate.

            // If the weapon item prefab isn't set.
            if (weaponUsesItemPrefab != null)
            {
                WeaponUsesItem item = Instantiate(weaponUsesItemPrefab);
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


            // LOADING THE RESULTS SCENE //
            // The game scene.
            string resultsScene = "ResultsScene";

            // Gets the loading type.
            bool asyncLoad = ASYNC_LOAD_RESULTS_SCENE;

            // Checks if the game should be loaded asynchronously or not.
            if (asyncLoad) // Async Load
            {
                // Creates then next scene object, and has it not be destroyed on load.
                GameObject newObject = new GameObject("Next Scene Load");
                DontDestroyOnLoad(newObject);

                // Goes to the next scene.
                NextSceneLoad nextScene = newObject.AddComponent<NextSceneLoad>();
                nextScene.nextScene = resultsScene;

                // Goes to the loading scene.
                SceneManager.LoadScene("LoadingScene");
            }
            else // Normal Load
            {
                SceneManager.LoadScene(resultsScene);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // The timer isn't paused.
            if(!pausedTimers)
            {
                // Time remaining.
                if(timer > 0.0F)
                {
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
                }

                // Add to the game time.
                elapsedGameTime += Time.deltaTime;
            }
        }
    }
}