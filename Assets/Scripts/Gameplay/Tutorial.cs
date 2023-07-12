using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The game notifications.
    public class Tutorial : MonoBehaviour
    {
        // The enum for the tutorials.
        public enum trlType { none, debug, opening, scrapItem, keyItem, healthItem, 
            punch, gunSlow, gunMid, gunFast, runUp, swimUp  }

        // Variables for getting notifications.
        public bool usedOpening = false;

        public bool usedScrapItem = false;
        public bool usedKeyItem = false;
        public bool usedHealthItem = false;

        // Gets the debug text.
        public List<string> GetDebugText()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "This is a test.",
                "This is only a test."
            };

            // Returns the pages.
            return pages;
        }

        // Gets the opening text.
        public List<string> GetOpeningText()
        {
            // The page to be returned.
            List<string> pages = new List<string>()
            {
                "This is the opening from the game."
            };

            // The opening has been used.
            usedOpening = true;

            return pages;
        }

        // Gets the key text.
        public List<string> GetScrapItemTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "You found some scrap! If you die, the scrap you're holding will be lost, so make sure to bring it to the base.",
            };

            usedScrapItem = true;

            // Returns the pages.
            return pages;
        }

        public List<string> GetKeyItemTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "This is a key! You can use it to unlock lock boxes to access more areas! " +
                "Keys are used automatically upon touching a locked object.",
            };

            usedKeyItem = true;

            // Returns the pages.
            return pages;
        }

        // Health item tutorial.
        public List<string> GetHealthItemTutorial()
        {
            // Loads the pages.
            List<string> pages = new List<string>()
            {
                "This is a health pack, which restores health. To use the health pack, use the (" 
                + GameplayManager.Instance.player.healKey.ToString() + ") key.",
            };

            usedHealthItem = true;

            // Returns the pages.
            return pages;
        }

    }
}