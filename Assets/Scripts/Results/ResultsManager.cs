using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DDY_GJM_23
{
    // The results manager.
    public class ResultsManager : MonoBehaviour
    {
        // The ResultsManager instance.
        private static ResultsManager instance;

        // Gets set to 'true' when the ResultsManager is initialized.
        // This isn't needed, but it helps with the clarity.
        private bool initialized = false;

        // The results data.
        public ResultsData results;

        // [Header("UI")]

        // Constructor
        private ResultsManager()
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

        // Start is called before the first frame update
        void Start()
        {
            // Finds the results data.
            if (results == null)
                results = FindObjectOfType<ResultsData>();

            // Results were found, so load them up.
            if (results != null)
                LoadResults();
        }

        // Gets the instance.
        public static ResultsManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<ResultsManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("ResultsManager (ResultsManager)");
                        instance = go.AddComponent<ResultsManager>();
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

        // Loads the results.
        public void LoadResults()
        {
            // ...

            // Destroys the results game object.
            Destroy(results.gameObject);
        }

        // Loads the title scene.
        public void ToTitleScene()
        {
            SceneManager.LoadScene("TitleScene");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}