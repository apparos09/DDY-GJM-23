using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using util;

namespace DDY_GJM_23
{
    // Loads a new scene.
    public class LoadingManager : MonoBehaviour
    {
        // Gets set to 'true' when the loading process has started.
        private bool loadStarted = false;

        // The number of frames delayed for the loading.
        private int loadDelay = 2;

        // The asynchronous scene loader.
        public AsyncSceneLoader sceneLoader;

        // The next scene load.
        public NextSceneLoad nextSceneLoad;

        // The debug text for showing the progess.
        public TMP_Text debugText;

        // Start is called before the first frame update
        void Start()
        {
            // If not set, find the scene loader.
            if (sceneLoader == null)
                sceneLoader = FindObjectOfType<AsyncSceneLoader>();

            // Finds the next scene.
            if(nextSceneLoad == null)
                nextSceneLoad = FindObjectOfType<NextSceneLoad>(true);
        }

        // Starts the loading process.
        private void StartLoading()
        {
            // If the next scene object was found.
            if (nextSceneLoad != null)
            {
                // Checks if the nextScene is set.
                if (nextSceneLoad.nextScene != string.Empty)
                {
                    // Sets the loading scene.
                    sceneLoader.LoadScene(nextSceneLoad.nextScene);
                }
                else
                {
                    Debug.LogError("No next scene has been set.");
                }

                // Destroys the nextScene object.
                Destroy(nextSceneLoad.gameObject);
            }
            else
            {
                Debug.LogError("No NextScene object found.");
            }

            // The loading has started.
            loadStarted = true;
        }

        // Update is called once per frame
        void Update()
        {
            // Checks how long the loading process should be delayed for.
            if(loadDelay <= 0)
            {
                // If the load started function has not been called yet.
                if (!loadStarted)
                {
                    StartLoading();
                }
            }
            else
            {
                loadDelay--;
            }

            // Displays the loading amount.
            if(debugText != null && sceneLoader.IsLoading)
            {
                debugText.text = (sceneLoader.GetProgressLoading() * 100.0F).ToString() + "%";
            }    
        }
    }
}