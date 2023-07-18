using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace DDY_GJM_23
{
    // World
    public class World : MonoBehaviour
    {
        // The world rows and columns.
        public const int WORLD_ROWS = 6;
        public const int WORLD_COLS = 6;

        // If set to 'true', the game world gets optimized.
        // NOTE: deactivated unneeded areas causes a stutter when using the portal or having a player death.
        // Since you intend to have a death animation, this probably won't be a problem.
        public const bool OPTIMIZE_WORLD = true;

        // The gravity acceleration for the game worlds.
        public static float GRAVITY_ACCEL = 9.8F;

        // The current area the player is in.
        private WorldArea currentArea;

        // The ideas for the game map. This is used to highlight information on the game map.
        // Note that this is laid out the way it would be viewed on the map itself.
        // So [0, 0] is the top left corner.
        private string[,] worldMap = new string[WORLD_ROWS, WORLD_COLS]
        {
            { "R00", "R01", "R02", "B00", "B01", "B02" },
            { "R03", "R04", "R05", "B03", "B04", "B05" },
            { "R06", "R07", "W00", "W01", "B06", "B07" },
            { "Y00", "Y01", "W02", "W03", "G00", "G01" },
            { "Y02", "Y03", "Y04", "G02", "G03", "G04" },
            { "Y05", "Y06", "Y07", "G05", "G06", "G07" }
        };

        // The areas in the game world, arranged in accordance with the game map.
        private WorldArea[,] worldAreas = new WorldArea[WORLD_ROWS, WORLD_COLS];

        // TODO: the game no longer accounts for items that escape the world, but I don't have time to fix it.

        // // The collider for the game world, which is used to destroy objects outside of this range.
        // public new Collider collider;

        // // If 'true', objects outside of the game world get destoryed.
        // [Tooltip("If true, objects outside of the game world get destroyed, as determined by a collider.")]
        // public bool destroyObjectsOutside = true;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            LoadAreasInto2dArray();
        }

        //// Start is called before the first frame update
        //void Start()
        //{
        //    // // Gets the world collider.
        //    // if (collider == null)
        //    //     collider = GetComponent<Collider>();
        //}

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

        // Loads the areas into the 2D array.
        public void LoadAreasInto2dArray()
        {
            // Empty out the array.
            worldAreas = new WorldArea[WORLD_ROWS, WORLD_COLS];

            // Finds all areas in the game world (all areas should already exist).
            WorldArea[] areaArr = FindObjectsOfType<WorldArea>(true);

            // Goes through each area.
            foreach (WorldArea area in areaArr)
            {
                // Gets set to 'true' when the area is stored. 
                bool stored = false;

                // Gets the area code.
                string areaCode = area.GetAreaCode();

                // Goes through each row and column.
                for (int r = 0; r < worldAreas.GetLength(0) && !stored; r++)
                {
                    for (int c = 0; c < worldAreas.GetLength(1) && !stored; c++)
                    {
                        // If the area code matches, and the spot is empty, put it in the list.
                        if (worldAreas[r, c] == null && worldMap[r, c] == areaCode)
                        {
                            // Store the area and break out of the loop.
                            worldAreas[r, c] = area;
                            stored = true;
                            break;
                        }
                    }
                }
            }

        }


        // CELLS //

        // Gets the world map cell. If the area ID is not in the map, (-1, -1) is returned.
        // (X) is the row, and (Y) is the column.
        public int[] GetWorldMapCell(WorldArea area)
        {
            // The cell to be returned.
            int[] cell = new int[2] { -1, -1 };

            // Gets set to 'true' when the area has been found.
            bool found = false;

            // OLD - goes by area code.
            //// Gets the code of the provided area.
            //string areaCode = area.GetAreaCode();

            //// Row and Col (X, Y)
            //for(int r = 0; r < worldMap.GetLength(0) && !found; r++)
            //{
            //    for(int c = 0; c < worldMap.GetLength(1) && !found; c++)
            //    {
            //        // Checks if the area code matches the code in this cell.
            //        if (worldMap[r, c] == areaCode)
            //        {
            //            cell[0] = r;
            //            cell[1] = c;
            //            found = true;
            //            break;
            //        }
            //    }

            //}

            // NEW - goes by area grid.
            // Row and Col (X, Y)
            for (int r = 0; r < worldAreas.GetLength(0) && !found; r++)
            {
                for (int c = 0; c < worldAreas.GetLength(1) && !found; c++)
                {
                    // Checks if the area code matches the code in this cell.
                    if (worldAreas[r, c] == area)
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

        // Gets the world map cell. If the index is out of bounds, it returns an empty string.
        public string GetWorldMapCell(int row, int col)
        {
            // The data from the cell.
            string cellData;

            // Checks if the indexes are valid.
            if(row >= 0 && row < worldMap.GetLength(0) &&
                col >= 0 && col < worldMap.GetLength(1))
            {
                cellData = worldMap[row, col];
            }
            else
            {
                cellData = "";
            }

            return cellData;
        }

        // Gets the current world map cell.
        public int[] GetCurrentWorldMapCell()
        {
            return GetWorldMapCell(GetCurrentArea());
        }

        // Gets the cell of the provided area.
        public int[] GetWorldAreaCell(WorldArea area)
        {
            // The cell to be returned.
            int[] cell = new int[2] { -1, -1 };

            // Gets set to 'true' when the area has been found.
            bool found = false;

            // Row and Col (X, Y)
            for (int r = 0; r < worldAreas.GetLength(0) && !found; r++)
            {
                for (int c = 0; c < worldAreas.GetLength(1) && !found; c++)
                {
                    // Checks if the area code matches the code in this cell.
                    if (worldAreas[r, c] == area)
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

        // Returns the area at the provided cell.
        public WorldArea GetWorldAreaAtCell(int row, int col)
        {
            // The data from the cell.
            WorldArea cellData;

            // Checks if the indexes are valid.
            if (row >= 0 && row < worldMap.GetLength(0) &&
                col >= 0 && col < worldMap.GetLength(1))
            {
                cellData = worldAreas[row, col];
            }
            else
            {
                cellData = null;
            }

            return cellData;
        }

        // Laods all areas. Use this if the player is sent to an area that isn't adjacent to their last area.
        public void ActivateAllAreas()
        {
            // Goes through each area.
            foreach(WorldArea area in worldAreas)
            {
                // If the area is not equal to null...
                if(area != null)
                {
                    // Activates the area if it is currently inactive.
                    if (!area.gameObject.activeSelf)
                        area.gameObject.SetActive(true);
                }
            }
        }

        // Optmizes the world by unloading areas that aren't currently needed.
        public void DeactivateUnneededAreas()
        {
            // Gets the current area's world map cell.
            int[] currCell = GetCurrentWorldMapCell();

            // Checks if the cell is valid.
            if (currCell[0] == -1 || currCell[1] == -1)
            {
                Debug.LogWarning("Area not included in world map. Unload failed.");
            }


            // The active areas.
            List<WorldArea> activeAreas = new List<WorldArea>();

            // Finds the areas that should remain on.
            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    // Checks to make sure that the index is valid.
                    if(
                        currCell[0] + r >= 0 && currCell[0] + r < worldAreas.GetLength(0) &&
                        currCell[1] + c >= 0 && currCell[1] + c < worldAreas.GetLength(1)
                        )
                    {
                        // The row and column of the chosen cell.
                        int rx = currCell[0] + r;
                        int cx = currCell[1] + c;

                        // If the world area is set at this index.
                        if (worldAreas[rx, cx] != null)
                            activeAreas.Add(worldAreas[rx, cx]);
                    }
                }
            }


            // If there are areas to activate.
            if(activeAreas.Count != 0)
            {
                // Goes through each index, turning off each area.
                for (int r = 0; r < worldAreas.GetLength(0); r++) // Rows
                {
                    for (int c = 0; c < worldAreas.GetLength(1); c++) // Cols
                    {
                        // Deactivates the world area.
                        if (worldAreas[r, c] != null)
                        {
                            // If the active area list contains the area at index [r, c], turn it on.
                            // If it isn't in the list, turn it off.
                            if (activeAreas.Contains(worldAreas[r, c]))
                            {
                                // If the area is deactivated, activate it.
                                if(!worldAreas[r, c].gameObject.activeSelf)
                                    worldAreas[r, c].gameObject.SetActive(true);
                            }
                            else
                            {
                                // If the area is active, deactivate it.
                                if (worldAreas[r, c].gameObject.activeSelf)
                                    worldAreas[r, c].gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
            
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
            {
                // Enters the new scene.
                newArea.OnAreaEnter();

                // Checks if areas should be deactivated.
                bool deactivateAreas = OPTIMIZE_WORLD;

                // Unloads unneeded areas for the game.
                if (deactivateAreas)
                    DeactivateUnneededAreas();
            }
        }



        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}