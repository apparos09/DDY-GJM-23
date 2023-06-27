using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace DDY_GJM_23
{
    // A world area.
    public class WorldArea : MonoBehaviour
    {
        // Shows what sector this world area belongs to.
        // Sector 'white' doubles as the 'homeBase' sector.
        public enum worldSector { unknown, white, red, blue, yellow, green}

        // The gameplay manager.
        public GameplayManager gameManager;

        // The collider for entering and exiting the area.
        public BoxCollider2D areaCollider;

        [Header("Area Info")]
        // The world this area is part of.
        public World world;

        // The world cell for this area.
        private int[] worldCell = new int[2] { -1, -1 };

        // The sector of this world area.
        public worldSector sector = worldSector.unknown;

        // The area number.
        public int areaNumber = 0;

        [Header("Camera")]
        // Determins if the camera is fixed mves.
        public bool fixedCamera = true;

        // The area target for the camera. If not set, the camera goes with the object's position.
        public Transform camTransformPos = null;

        // The camera view target when focusing on the area (relative to world size).
        // (0.5, 0.5) means the middle of the area collider. 
        public Vector2 camViewTarget = new Vector2(0.5F, 0.5F);

        // The limits for the camera.
        public Vector2 cameraLimitsMin;
        public Vector2 cameraLimitsMax;

        [Header("Spawners")]

        // The parent that all enemies are parented to.
        public GameObject enemyParent;

        // The list of enemies.
        public List<Enemy> enemies;

        // The parent that all items are parented to.
        public GameObject itemParent;

        // The list of scraps.
        public List<WorldItem> items;

        // The list of area spawners.
        public List<AreaSpawn> spawners = new List<AreaSpawn>();

        // Start is called before the first frame update
        void Start()
        {
            // Save the instance.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;

            // Grabs the world component from the parent.
            if(world == null)
                world = GetComponentInParent<World>();

            // Sets the world cell.
            GetWorldCell();

            // Gets the box collider 2D.
            if (areaCollider == null)
                areaCollider = GetComponent<BoxCollider2D>();


            // Find spawners in children if this is not set.
            if (spawners.Count == 0)
            {
                // Grab all the spawners and put them in the list.
                GetComponentsInChildren(true, spawners);

                // Set the areas.
                foreach (AreaSpawn spawn in spawners)
                    spawn.area = this;
            }
        }

        // Called when entering the area trigger.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Gets the component.
            if(collision.gameObject.tag == Player.PLAYER_TAG)
            {
                // Set current area.
                if(!gameManager.world.IsCurrentArea(this))
                {
                    // Set the current area.
                    gameManager.world.SetCurrentArea(this);
                }
            }
        }


        // AREA INFO

        // Gets the cell this world belongs to. If it's negative, then that means this cell isn't considered in the world.
        public int[] GetWorldCell()
        {
            // Checks if the world cell has been set yet.
            if (world != null && (worldCell[0] == -1 || worldCell[1] == -1))
                worldCell = world.GetWorldAreaCell(this);

            return worldCell;
        }


        // Returns the world sector tied to the specific letter.
        public static worldSector GetWorldSectorByLetter(char letter)
        {
            // The world sector
            worldSector ws;

            // Checks the letter.
            switch (letter)
            {
                case 'W':
                case 'w':
                    ws = worldSector.white;
                    break;

                case 'R':
                case 'r':
                    ws = worldSector.red;
                    break;

                case 'B':
                case 'b':
                    ws = worldSector.blue;
                    break;

                case 'Y':
                case 'y':
                    ws = worldSector.yellow;
                    break;

                case 'G':
                case 'g':
                    ws = worldSector.green;
                    break;

                default:
                    ws = worldSector.unknown;
                    break;
            }

            return ws;
        }

        // Gets the world sector as a letter.
        public char GetWorldSectorAsLetter(worldSector ws)
        {
            // THe letter being returned.
            char letter;

            // Checks the letter.
            switch (ws)
            {
                case worldSector.white:
                    letter = 'W';
                    break;

                case worldSector.red:
                    letter = 'R';
                    break;

                case worldSector.blue:
                    letter = 'B';
                    break;

                case worldSector.yellow:
                    letter = 'Y';
                    break;

                case worldSector.green:
                    letter = 'G';
                    break;

                case worldSector.unknown:
                default:
                    letter = 'X';
                    break;
            }

            return letter;
        }



        // Gets the area code, which is the sector plus the area number.
        public string GetAreaCode()
        {
            // The area code.
            string areaCode = "";

            // Gets the world sector as a letter.
            areaCode += GetWorldSectorAsLetter(sector);

            // Add the number to the area code.
            areaCode += areaNumber.ToString("D2");

            return areaCode;
        }  


        // AREA DIMENSIONS //

        // Gets the minimum of the area.
        public Vector3 GetAreaMin()
        {
            // Gets the min of the area in world space.
            Vector3 areaMin = transform.TransformPoint(areaCollider.bounds.min);

            return areaMin;
        }

        // Gets the area max.
        public Vector3 GetAreaMax()
        {
            // Gets the max of the area in world space.
            Vector3 areaMax = transform.TransformPoint(areaCollider.bounds.max);

            return areaMax;
        }


        // Get the camera position based on the world area.
        public Vector3 GetAreaCenter()
        {
            // Gets the center of the area in world space.
            Vector3 areaCenter = transform.TransformPoint(areaCollider.bounds.center);

            return areaCenter;
        }

        // Calculates the camera limits.
        public void CalculateCameraLimits()
        {
            // TODO: check if the calculations are accurate.

            // The area center.
            // Using area max and min seems to give weird numbers, so we're using center for now.
            Vector3 areaMin = GetAreaMin();
            Vector3 areaMax = GetAreaMax();
            
            // Calculates the camera target (which normally is the area center).
            Vector2 camTargetPos = new Vector2(
                Mathf.Lerp(areaMin.x, areaMax.x, camViewTarget.x),
                Mathf.Lerp(areaMin.y, areaMax.y, camViewTarget.y)
                );

            // OLD - maybe change to use viewport instead?
            // Calculates the camera points (relative to world origin).
            // Camera center (in world space).
            Vector3 camMin = gameManager.worldCamera.GetWorldCameraViewSizeMinWorld();
            Vector3 camMax = gameManager.worldCamera.GetWorldCameraViewSizeMaxWorld();
            Vector3 camCenter = gameManager.worldCamera.GetWorldCameraCenterWorld();


            // // NEW
            // // The orthographic size is the camera height divided by 2.
            // float camHeight = gameManager.camera.orthographicSize * 2.0F;
            // 
            // // The camera width is the aspect ratio times the half height.
            // float camWidth = gameManager.camera.orthographicSize * gameManager.camera.aspect * 2.0F;
            // 
            // // TODO: maybe put the camera height and width functions in a seperate script.
            // Vector3 camCenter = new Vector3(camWidth / 2, camHeight / 2, 0.0F);

            // // Camera maximum (in world space).
            // Vector3 camMax = gameManager.camera.ScreenToWorldPoint(
            //     new Vector3(gameManager.camera.pixelWidth , gameManager.camera.pixelHeight, 0));
            // 
            // // Camera minimum (in world space).
            // Vector3 camMin = gameManager.camera.ScreenToWorldPoint(Vector3.zero);


            // NOTE: make sure that the hitbox is big enough so that the camera limits work properly.

            // Calculates the camera minimum and maximum.
            Vector3 temp;
            
            // temp = areaMin + (camCenter - camMin); // OLD
            temp = new Vector3(camTargetPos.x, camTargetPos.y, 0) - (camCenter - camMin); // NEW
            cameraLimitsMin = new Vector2(temp.x, temp.y);

            // temp = areaMax - (camMax - camCenter); // OLD
            temp = new Vector3(camTargetPos.x, camTargetPos.y, 0) + (camMax - camCenter); // NEW
            cameraLimitsMax = new Vector2(temp.x, temp.y);
        }


        // Called when an area is being opened.
        public void OnAreaEnter()
        {
            CalculateCameraLimits();

            // Sets the camera position (don't change z).
            Vector3 camPos = gameManager.worldCamera.transform.position;

            // Checks if the camera transform position is set.
            if (camTransformPos != null)
            {
                camPos.x = camTransformPos.position.x;
                camPos.y = camTransformPos.position.y;
                gameManager.worldCamera.anchor = camTransformPos.gameObject;
            }
            else
            {
                camPos.x = transform.position.x;
                camPos.y = transform.position.y;
                gameManager.worldCamera.anchor = gameManager.player.gameObject;
            }

            // Sets the camera position. (TODO: change to game object instead of transform.)
            gameManager.worldCamera.SetCameraPosition(camPos.x, camPos.y);

            // Spawns dynamic entities.
            SpawnEntities();
        }

        // Called when an area is being closed.
        public void OnAreaExit()
        {
            DestroyAllEnemies();
            DestroyAllItems();

            // Clears out the tiles the player is on.
            gameManager.player.currentTiles.Clear();
        }

        // Destroys all entities in the provided list.
        public static void DestroyAllAreaEntitiesInList(List<AreaEntity> list)
        {
            // Destroys all enemies.
            for (int i = list.Count - 1; i >= 0; i--)
            {
                // The item still exists.
                // List will be cleared when the check is finished.
                if (list[i] != null)
                {
                    Destroy(list[i].gameObject);
                }
            }

            // Clear the list.
            list.Clear();
        }


        // ENEMIES //
        // Adds the enemy to the area.
        public void AddEnemyToArea(Enemy enemy)
        {
            // Area has been set.
            if (enemy.area != null)
            {
                // Remove from enemy list.
                if (enemy.area.enemies.Contains(enemy))
                {
                    enemy.area.enemies.Remove(enemy);
                }
            }

            // Setting the enemy area.
            enemy.area = this;

            // Add the enemy to the list.
            if(!enemies.Contains(enemy)) 
            {
                // Add the enemy to the list.
                enemies.Add(enemy);

                // If the enemy parent exists.
                if(enemyParent != null)
                    enemy.transform.parent = enemyParent.transform;
            }

        }

        // Destroy all enemies tied to this area.
        public void DestroyAllEnemies()
        {
            DestroyAllAreaEntitiesInList(new List<AreaEntity>(enemies));
            enemies.Clear();
        }


        // ITEM //
        // Adds the item to the area.
        public void AddItemToArea(WorldItem item)
        {
            // Area has been set.
            if (item.area != null)
            {
                // Remove from item list.
                if (item.area.items.Contains(item))
                {
                    item.area.items.Remove(item);
                }
            }

            // Setting the enemy area.
            item.area = this;

            // Add the enemy to the list.
            if (!items.Contains(item))
            {
                // Add the item to the list.
                items.Add(item);

                // If the item parent exists, give it to the spawned item.
                if (itemParent != null)
                    item.transform.parent = itemParent.transform;
            }

        }

        // Destroy all enemies tied to this area.
        public void DestroyAllItems()
        {
            DestroyAllAreaEntitiesInList(new List<AreaEntity>(items));
            items.Clear();
        }


        // SPAWNS
        // Spawns the enemies.
        public void SpawnEntities()
        {
            // Calls the spawns.
            foreach (AreaSpawn spawn in spawners)
            {
                spawn.Spawn();
            }
                
        }


        // Update is called once per frame
        void Update()
        {
            // The camera should track the player.
            if(!fixedCamera)
            {
                // Updates the world camera.
                WorldCamera worldCam = gameManager.worldCamera;

                if(!(worldCam.transform.position.x == gameManager.player.transform.position.x) && 
                    (worldCam.transform.position.y == gameManager.player.transform.position.y))
                {
                    // Sets the camera to the player's position (ignores the z-axis).
                    worldCam.SetCameraToPlayerPositionXY();

                    // Calculates the new position.
                    Vector3 newPos = new Vector3(
                        Mathf.Clamp(worldCam.transform.position.x, cameraLimitsMin.x, cameraLimitsMax.x),
                        Mathf.Clamp(worldCam.transform.position.y, cameraLimitsMin.y, cameraLimitsMax.y),
                        worldCam.transform.position.z);

                    // Sets the camera position.
                    worldCam.SetCameraPosition(newPos);
                }
                
            }
        }
    }
}