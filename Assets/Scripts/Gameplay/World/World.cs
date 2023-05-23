using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // World
    public class World : MonoBehaviour
    {
        // The current area the player is in.
        private WorldArea currentArea;

        // TODO: maybe have an area list so that each area can be numbered.
        // // The list of areas.
        // public List<WorldArea> areas;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Checks if this is the current area.
        public bool IsCurrentArea(WorldArea area)
        {
            bool result = currentArea == area;
            return result;
        }

        // Gets the current area.
        public WorldArea GetCurrentArea()
        {
            return currentArea;
        }

        // Sets the current area.
        public void SetCurrentArea(WorldArea newArea)
        {
            // Exited the current area.
            if (currentArea != null)
                currentArea.OnAreaExit();

            // Set area.
            currentArea = newArea;

            // Entered the new area.
            if (newArea != null)
                newArea.OnAreaEnter();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}