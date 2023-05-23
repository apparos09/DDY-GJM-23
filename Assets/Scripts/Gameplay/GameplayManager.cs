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
        private bool initialized = false;

        // The player.
        public Player player;

        // The total number of scraps the player has created.
        public int scrapsTotal;

        [Header("World")]

        // The game world.
        public World world;

        // The world camera.
        public WorldCamera worldCamera;

        // The home base of the game.
        public HomeBase homeBase;

        [Header("Other")]

        // The gameplay UI.
        public GameplayUI gameUI;

        // The game timer (in seconds) - 5 Minutes (300 Seconds)
        public float timer = 300.0F;

        // Pauses the timer if set to 'true'.
        public bool pausedTimer = false;

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
            else if(instance != this)
            {
                Destroy(gameObject);
            }
                
            // Run code for initialization.
            if(!initialized)
            {
                initialized = true;
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

            // OTHER
            // The game UI.
            if (gameUI == null)
                gameUI = FindObjectOfType<GameplayUI>(true);
        }

        // Gets the instance.
        public static GameplayManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if(instance == null)
                {
                    // Generate the instance.
                    GameObject go = new GameObject("Gameplay Manager (singleton)");
                    instance = go.AddComponent<GameplayManager>();
                }

                // Return the instance.
                return instance;
            }
        }

        // Returns the game timer, formatted.
        public string GetTimerFormatted(bool roundUp = true)
        {
            // Gets the time and rounds it up to the nearest whole number.
            float time = (roundUp) ? Mathf.Ceil(timer) : timer;

            // Formats the time.
            string formatted = StringFormatter.FormatTime(time, false);

            // Returns the formatted time.
            return formatted;
        }


        // Called to end the game.
        protected void OnTimeOver()
        {
            // Checks if the player is in the base.
            bool inBase = homeBase.IsPlayerInBase();

            // NOTE: if the player is in a base area when the timer is over, have it count.
            // TODO: implement time over effects.

            EndGame();
        }

        // Called to end the game.
        public void EndGame()
        {
            // ...

            // Loads the results scene.
            // SceneManager.LoadScene("ResultsScene");
        }

        // Update is called once per frame
        void Update()
        {
            // The timer isn't paused.
            if(!pausedTimer)
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
            }
        }
    }
}