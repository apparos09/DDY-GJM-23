using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DDY_GJM_23
{
    // A list of enemy prefabs to reference for enemy spawning.
    public class EnemyPrefabs : MonoBehaviour
    {
        // The EnemyPrefabs instance.
        private static EnemyPrefabs instance;

        // Gets set to 'true' when the EnemyPrefabs is initialized.
        // This isn't needed, but it helps with the clarity.
        private bool initialized = false;

        // TODO: implement enemies.

        // Constructor
        private EnemyPrefabs()
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

        // Gets the instance.
        public static EnemyPrefabs Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<EnemyPrefabs>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("EnemyPrefabs (singleton)");
                        instance = go.AddComponent<EnemyPrefabs>();
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

        // Instantiates an enemy based on the provided type.
        public Enemy InstantiateEnemyByType(Enemy.enemyId type)
        {
            // switch(type)
            // {
            // 
            // }

            return null;
        }
    }
}