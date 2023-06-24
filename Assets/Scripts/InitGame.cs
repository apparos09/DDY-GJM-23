using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;

namespace DDY_GJM_23
{
    // Initializes the game.
    public class InitGame : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // Sets the frame rate for the game.
            Application.targetFrameRate = 30;

            // Set the screen size to 720x1280.
            // Do NOT change the game resolution in code when using WebGL. THis bre
            // if(!(Application.platform == RuntimePlatform.WebGLPlayer))
            //     GameSettings.Instance.SetScreenSize1280x720();

            // The game doesn't run in the background.
            // Application.runInBackground = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Loads the title scene.
            SceneManager.LoadScene("TitleScene");
        }
    }
}