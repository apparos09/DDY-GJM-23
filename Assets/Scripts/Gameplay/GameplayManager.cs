using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Constructor
        private GameplayManager()
        {

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
                player = FindObjectOfType<Player>();

            // Finds the world component.
            if (world == null)
                world = FindObjectOfType<World>();

            // Find the world camera.
            if (worldCamera == null)
                worldCamera = FindObjectOfType<WorldCamera>();
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}