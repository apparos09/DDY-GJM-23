using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace util
{
    // Loads a scene asynchronously, which allows there to be a loading screen.
    public class AsyncSceneLoader : MonoBehaviour
    {
        // The loading coroutine.
        private Coroutine coroutine = null;

        // the scene that's being loaded.
        private string loadingScene = "";

        // Set to 'true' if the scene loader is loading.
        private bool isLoading = false;

        // How much has already been loaded.
        private float progress = 0.0F;

        // Returns the scene that's being loaded. If no scene is being loaded then the string will be blank ("").
        public string LoadingScene
        {
            get { return loadingScene; }
        }

        // Returns 'true' if the scene is now loading.
        public bool IsLoading
        {
            get { return isLoading; }
        }

        // Returns normal progress, which goes from 0.0 to 1.0.
        // [0.0, 0.9] is the loading phase, and (0.9, 1.0] is the activation phase.
        public float GetProgress()
        {
            return progress;
        }

        // Returns loading progress, which is progress out of 0.9F.
        // [0.0, 0.9] is the loading phase, while (0.9, 1.0] is the activation phase.
        // This looks at only the loading phase, and puts it in a 0.0 - 1.0 range.
        public float GetProgressLoading()
        {
            // Clamps the value so that it's in a 0-1 range.
            float result = Mathf.Clamp01(progress / 0.9F);
            return result;
        }

        // Gets the activation progress, which is the (0.9F, 1.0F] phase of the loading progress.
        // [0.0, 0.9] is the loading phase, while (0.9, 1.0] is the activation phase.
        public float GetProgressActivation()
        {
            // Clamps the value to a 0.9 - 1.0 range, then uses inverse lerp to get the result's range...
            // Between 0.9 and 1.0 as a lerp operation.
            float result = Mathf.InverseLerp(0.9F, 1.0F, Mathf.Clamp(progress, 0.9F, 1.0F));
            return result;
        }

        // Public function to call for scene loading.
        public void LoadScene(string sceneName)
        {
            // TODO: check to see if a scene exists.

            // If a coroutine is running, stop it.
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            // Spreads an operation across multiple frames.
            coroutine = StartCoroutine(LoadSceneAsync(sceneName));
        }

        // Loads a scene asynchonously.
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // The asynchonrous operation.
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            // the operation is now loading.
            isLoading = true;
            loadingScene = sceneName;

            // While the operation is going.
            while (!operation.isDone)
            {
                // Gets the current progress of the operation.
                // Unity starts off by loading the new assets, which goes from a (0 - 0.9) range.
                // Then Unity does activation, deleting the old content and loading the new content, which is the (0.9 - 1) range.
                // The activation phase goes straight to 1.0F instead of doing anything in-between.
                // As such, we divide by 0.9F and use Clamp01 so that we get the actual loading phase percentage.

                // Loading Phase: [0.0, 0.9]
                // Activation Phase: (0.9, 1.0]
                progress = operation.progress;

                // Tells the program to stall the operation and return controls back to Unity.
                yield return null;
            }

            // The scene has finished loading, so change these values.
            isLoading = false;
            loadingScene = "";

            // Set the coroutine to null to show that it's finished.
            coroutine = null;
        }



    }
}