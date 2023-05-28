using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // World
    public class World : MonoBehaviour
    {
        // The gravity acceleration for the game worlds.
        public static float GRAVITY_ACCEL = 9.8F;

        // The current area the player is in.
        private WorldArea currentArea;

        // TODO: the game no longer accounts for items that escape the world, but I don't have time to fix it.

        // // The collider for the game world, which is used to destroy objects outside of this range.
        // public new Collider collider;

        // TODO: maybe have an area list so that each area can be numbered.
        // // The list of areas.
        // public List<WorldArea> areas;

        // // If 'true', objects outside of the game world get destoryed.
        // [Tooltip("If true, objects outside of the game world get destroyed, as determined by a collider.")]
        // public bool destroyObjectsOutside = true;



        // Start is called before the first frame update
        void Start()
        {
            // // Gets the world collider.
            // if (collider == null)
            //     collider = GetComponent<Collider>();
        }

        //// OnTriggerExit2D - destroys objects outside of this collider.
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    // Checks if object's outside of the game wolrd should be destroyed.
        //    if (!destroyObjectsOutside)
        //        return;

        //    // Checks if it's a combatant.
        //    Combatant combatant = null;

        //    // Tries to grab component.
        //    if(collision.TryGetComponent(out combatant))
        //    {
        //        // Kills the combantant instead of destroying the object.
        //        combatant.Kill();
        //    }
        //    else
        //    {
        //        // FIXME: this deletes any object inside the world that has its collider turned off, so it was removed.
        //        // Destroy the object.
        //        Destroy(collision.gameObject);
        //    }
        //}


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