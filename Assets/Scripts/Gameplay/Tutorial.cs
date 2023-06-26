using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The game notifications.
    public class Tutorial : MonoBehaviour
    {
        // Variables for getting notifications.
        public bool usedOpening = true;

        // Gets the debug text.
        public List<string> GetDebugText()
        {
            List<string> pages = new List<string>();

            pages.Add("This is a test.");
            pages.Add("This is only a test.");

            return pages;
        }

        // Gets the opening text.
        public List<string> GetOpeningText()
        {
            List<string> pages = new List<string>();

            pages.Add("This is the opening from the game.");

            return pages;
        }
    }
}