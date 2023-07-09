using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DDY_GJM_23
{
    // An object for providing area loader entities.
    public class AreaLoaderPrefabs : MonoBehaviour
    {
        // The gameplay manager instance.
        private static AreaLoaderPrefabs instance;

        // Gets set to 'true' when the singleton is initialized.
        private bool instantiated = false;

        // GRASS
        [Header("Tiles/Grass")]
        public WorldTile grassFloorA; // 01
        public WorldTile grassWallA; // 02

        // METAL
        [Header("Tiles/Metal")]
        public WorldTile metalFloorA; // 03
        public WorldTile metalWallA; // 04

        // PAVEMENT AND BRICK
        [Header("Tiles/Pavement, Brick")]
        public WorldTile pavementFloorA; // 05
        public WorldTile brickWallA; // 06

        // BRIDGE AND PIT
        [Header("Tiles/Bridge, Pit")]
        public WorldTile bridgeA; // 07
        public WorldTile pitA; // 08

        // LIQUIDS
        [Header("Tiles/Liquids")]
        public WorldTile waterA; // 09
        public WorldTile poisonA; // 10

        // Constructor
        private AreaLoaderPrefabs()
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

        //// Start is called just before any of the Update methods is called the first time
        //private void Start()
        //{
        //    // ...
        //}

        // Gets the instance.
        public static AreaLoaderPrefabs Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<AreaLoaderPrefabs>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Area Loader Prefabs (singleton)");
                        instance = go.AddComponent<AreaLoaderPrefabs>();
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
    }
}