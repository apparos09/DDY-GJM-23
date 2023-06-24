using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

        // The ideas for the game map. This is used to highlight information on the game map.
        // Note that this is laid out the way it would be viewed on the map itself.
        // So [0, 0] is the top left corner.
        private string[,] mapIds = new string[,]
        {
            { "R00", "R01", "R02", "B00", "B01", "B02" },
            { "R03", "R04", "R05", "B03", "B04", "B05" },
            { "R06", "R07", "W00", "W01", "B06", "B07" },
            { "Y00", "Y01", "W02", "W03", "G00", "G01" },
            { "Y02", "Y03", "Y04", "G02", "G03", "G04" },
            { "Y05", "Y06", "Y07", "G05", "G06", "G07" }
        };

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

        // Gets the area map cell. If the area ID is not in the map, (-1, -1) is returned.
        // (X) is the row, and (Y) is the column.
        public int[] GetAreaMapCell(WorldArea area)
        {
            // The cell to be returned.
            int[] cell = new int[2];

            // The cell location.
            Vector2Int cellLoc = new Vector2Int(-1, -1);

            // Gets set to 'true' when the area has been found.
            bool found = false;

            // Gets the code of the provided area.
            string areaCode = area.GetAreaCode();

            // Row and Col (X, Y)
            for(int r = 0; r < mapIds.GetLength(0) && found; r++)
            {
                for(int c = 0; c < mapIds.GetLength(1) && found; c++)
                {
                    // Checks if the area code matches the code in this cell.
                    if (mapIds[r, c] == areaCode)
                    {
                        cell[0] = r;
                        cell[1] = c;
                        found = true;
                        break;
                    }
                }

            }

            // Returns the cell location.
            return cell;
        }

        // Gets the area map cell. If the index is out of bounds, it returns an empty string.
        public string GetAreaMapCell(int row, int col)
        {
            // The data from the cell.
            string cellData;

            // Checks if the indexes are valid.
            if(row >= 0 && row < mapIds.GetLength(0) &&
                col >= 0 && col < mapIds.GetLength(1))
            {
                cellData = mapIds[row, col];
            }
            else
            {
                cellData = "";
            }

            return cellData;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}