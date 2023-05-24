using DDY_GJM_23;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // A generic singleton class.
    public class Singleton : MonoBehaviour
    {
        // The singleton instance.
        private static Singleton instance;

        // Gets set to 'true' when the singleton is initialized.
        // This isn't needed, but it helps with the clarity.
        private bool initialized = false;

        // Constructor
        private Singleton()
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
        public static Singleton Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<Singleton>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Singleton (singleton)");
                        instance = go.AddComponent<Singleton>();
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
    }
}