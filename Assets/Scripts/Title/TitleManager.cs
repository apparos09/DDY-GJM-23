using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}