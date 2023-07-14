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

        [Header("TILES")]

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
        public WorldTile bridgeFloorA; // 07
        public WorldTile bridgeWallA; // 08
        public WorldTile pitA; // 09

        // LIQUIDS
        [Header("Tiles/Liquids")]
        public WorldTile waterA; // 10
        public WorldTile poisonA; // 11

        // SAND
        [Header("Tiles/Sand")]
        public WorldTile drySandFloorA; // 12
        public WorldTile drySandWallA; // 13


        // GRAVEL
        [Header("Gravel")]
        public WorldTile gravelFloorA; // 14

        // OBJECTS //
        [Header("OBJECTS")]
        public AreaEntity rockBlock;
        public AreaEntity stoneBlock;
        public AreaEntity lockBox;
        public AreaEntity portal;

        // ENEMIES //
        [Header("ENEMIES")]
        public EnemySpawn chaserSpawnL1;
        public EnemySpawn chaserSpawnL2;
        public EnemySpawn chaserSpawnL3;
        public EnemySpawn shooterSpawnL1;
        public EnemySpawn shooterSpawnL2;
        public EnemySpawn shooterSpawnL3;

        // ITEMS
        [Header("ITEMS")]
        [Header("Items/Scrap")]
        public ScrapSpawn scrapSpawn1;
        public ScrapSpawn scrapSpawn3;
        public ScrapSpawn scrapSpawn5;
        public ScrapSpawn scrapSpawn7;
        public ScrapSpawn scrapSpawn10;
        public ScrapSpawn scrapSpawn15;

        [Header("Items/General")]
        public WorldItem key;
        public WorldItem health;
        public WorldItem weaponRefill;

        [Header("Items/Weapons")]
        public WorldItem gunSlow;
        public WorldItem gunMid;
        public WorldItem gunFast;
        public WorldItem runPower;
        public WorldItem swimPower;

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