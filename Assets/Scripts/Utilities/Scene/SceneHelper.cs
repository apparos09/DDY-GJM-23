using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace util
{
    // Scene assist script, which was imported from an existing project of mine.
    public class SceneHelper : MonoBehaviour
    {

        // Changes the scene using the scene number.
        public static void ChangeScene(int scene)
        {
            SceneManager.LoadScene(scene);

        }

        // Changes the scene using the scene name.
        public static void ChangeScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        // Returns the skybox of the scene.
        public static Material GetSkybox()
        {
            return RenderSettings.skybox;
        }

        // Sets the skybox of the scene.
        public static void SetSkybox(Material newSkybox)
        {
            RenderSettings.skybox = newSkybox;
        }



        // Called to change the screen size.
        public static void ChangeScreenSize(int width, int height, FullScreenMode mode)
        {
            Screen.SetResolution(width, height, mode);
        }

        // Called to change the screen size.
        public static void ChangeScreenSize(int width, int height, FullScreenMode mode, bool fullScreen)
        {
            ChangeScreenSize(width, height, mode);
            Screen.fullScreen = fullScreen;
        }



        // Returns 'true' if the game is full screen.
        public static bool IsFullScreen()
        {
            return Screen.fullScreen;
        }

        // Sets 'full screen' mode
        public static void SetFullScreen(bool fullScreen, FullScreenMode mode = FullScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = mode;
            Screen.fullScreen = fullScreen;
        }

        // Toggles the full screen.
        public static void FullScreenToggle()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        // Set Screen Size (1080 Resolution - 16:9 - Full HD (FHD))
        public static void SetScreenSize1920x1080(FullScreenMode mode = FullScreenMode.MaximizedWindow)
        {
            ChangeScreenSize(1920, 1080, mode, false);
        }

        // Set Screen Size (1080 Resolution - 4:3)
        public static void SetScreenSize1440x1080(FullScreenMode mode = FullScreenMode.Windowed)
        {
            ChangeScreenSize(1440, 1080, mode, false);
        }

        // Set Screen Size (720 Resolution - 16:9 - HD (High Definition))
        public static void SetScreenSize1280x720(FullScreenMode mode = FullScreenMode.Windowed)
        {
            ChangeScreenSize(1280, 720, mode, false);
        }

        // Set Screen Size (720 Resolution - 4:3)
        public static void SetScreenSize960x720(FullScreenMode mode = FullScreenMode.Windowed)
        {
            ChangeScreenSize(960, 720, mode, false);
        }

        // Set Screen Size (16:9 - WSVGA (Wide Super Video Graphics Array))
        public static void SetScreenSize1024x576(FullScreenMode mode = FullScreenMode.Windowed)
        {
            ChangeScreenSize(1024, 576, mode, false);
        }

        // Set Screen Size (4:3 - XGA (Extended Graphics Array))
        public static void SetScreenSize1024x768(FullScreenMode mode = FullScreenMode.Windowed)
        {
            ChangeScreenSize(1024, 768, mode, false);
        }



        // Exits the game
        public static void ExitApplication()
        {
            Application.Quit();
        }
    }
}