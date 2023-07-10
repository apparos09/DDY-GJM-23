using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;

namespace DDY_GJM_23
{
    // Loads an area from a file.
    public class AreaLoader : MonoBehaviour
    {
        // Loads the data on start.
        public bool loadOnStart = true;

        // Gives the area the values if this component is set.
        [Tooltip("Set the area if you want the program to fill in the values.")]
        public WorldArea area;

        // The world sector.
        public WorldArea.worldSector sector = WorldArea.worldSector.unknown;

        // The number of the area.
        public int areaNumber = -1;

        [Header("Files")]

        // The file name (include the file extension).
        [Tooltip("The file name. Make sure to include the file extension (.csv).")]
        public string fileName = "";

        // The three folder paths that are used to check for files.
        // Do NOT have them end in a slash.
        [Tooltip("The primary search location. Include the slash at the end of the path.")]
        public string folderPath1 = "Assets/Resources/Data/Areas/";

        [Tooltip("The secondary search location. Include the slash at the end of the path.")]
        public string folderPath2 = "Resources/Data/Areas/";

        [Tooltip("The tertiary search location. Include the slash at the end of the path.")]
        public string folderPath3 = "Data/Areas/";

        // Gets set to 'true' when the file loaded.
        [Tooltip("If 'true', the file has been loaded successfully.")]
        public bool fileLoaded = false;

        [Header("Entities")]
        // The parent for the loaded entities.
        public Transform tileParent = null;

        // The parent for the loaded objects.
        public Transform objectParent = null;

        // The parent for the loaded enemies.
        public Transform enemyParent = null;

        // The parent for the loaded items.
        public Transform itemParent = null;


        [Header("Entities/Other")]
        // A reference object for placing the tiles. This can be used to adjust posOffset.
        [Tooltip("An optional object to apply posOffset to for determing the base tile position (origin's world space pos used).")]
        public Transform originTransform;

        // The reference position for loaded content.
        [Tooltip("The offset of the tile positions.")]
        public Vector3 posOffset = new Vector3(-13.5F, 6.5F, 0);

        // The spacing for placing tiles.
        [Tooltip("Spacing for tiles [x = col, y = row]")]
        public Vector2 spacing = new Vector2(1, -1);

        // Sets the local position of the tile if true, world position if false.
        [Tooltip("If true, the local position of the tile is set, not the world position.")]
        public bool setLocalPos = true;

        [Header("Data")]

        // The prefabs for the loader.
        public AreaLoaderPrefabs loaderPrefabs;


        // Start is called before the first frame update
        void Start()
        {
            // Gets the componnet if it's not set.
            if (area == null)
                area = GetComponent<WorldArea>();

            // Grabs the instance of the loader.
            if (loaderPrefabs == null)
                loaderPrefabs = AreaLoaderPrefabs.Instance;

            // Load the data on start.
            if (loadOnStart)
                LoadData();
        }

        // Loads the data from the set file.
        public bool LoadData()
        {
            // The file is not loaded yet.
            fileLoaded = false;

            // The data file being loaded.
            string file = "";
            string filePath = "";

            // Gets set to 'true' if the file exists at the provided path.
            bool exists = false;

            // Attempt 1 (Folder Path 1)
            if(File.Exists(folderPath1 + fileName))
            {
                filePath = folderPath1;
                exists = true;
            }
            else if (File.Exists(folderPath2 + fileName))
            {
                filePath = folderPath2;
                exists = true;
            }
            else if (File.Exists(folderPath3 + fileName))
            {
                filePath = folderPath3;
                exists = true;
            }

            // File doesn't exist.
            if(!exists)
            {
                Debug.LogError("The file does not exist at the specified path.");
                return false;
            }

            // Form the file.
            file = filePath + fileName;

            // Reads all lines in the provided file.
            string[] lines = File.ReadAllLines(@file);

            // The character for splitting elements.
            const char SPLIT_CHAR = ',';

            // The character used for seperating sections.
            const string SECTION_CHAR = "-";

            // The row for the generated entity.
            // This gets reset to '0' when a new section starts.
            int row = 0;

            // Goes through each line.
            for (int i = 0; i < lines.Length; i++)
            {
                // Grabs the current line and splits it by the split character.
                string[] entries = lines[i].Split(SPLIT_CHAR);

                // 0 - Area Code
                if (i == 0)
                {
                    // The area code.
                    string areaCode = entries[0];

                    // The sector and number.
                    string areaSectorStr = "";
                    string areaNumberStr = "";

                    // The sector and the number string.
                    if(areaCode.Length == 3) // Correct length.
                    {
                        // Gets the sector.
                        areaSectorStr = areaCode.Substring(0, 1);
                        sector = WorldArea.GetWorldSectorByLetter(areaSectorStr[0]);

                        // Gets the area number.
                        areaNumberStr = areaCode.Substring(1);
                        areaNumber = int.Parse(areaNumberStr);

                        // Tries to parse to convert the string to an integer.
                        if(!int.TryParse(areaNumberStr, out areaNumber))
                        {
                            Debug.LogAssertion("Area number could not be read.");
                            areaNumber = 0;
                        }
                    }
                    else
                    {
                        sector = WorldArea.worldSector.unknown;
                        areaNumber = 0;
                    }
                }
                // Entity or Section Char
                else
                {
                    // If this is a section character.
                    if (entries[0] == SECTION_CHAR)
                    {
                        row = 0;
                    }
                    else
                    {
                        // Goes through each entry.
                        for (int j = 0; j < entries.Length; j++)
                        {
                            // Gets the tile code.
                            string tc = entries[j];

                            // If the tile code is empty, is set to "0" or "00A", nothing should be put here.
                            // So go onto the next entry.
                            if (tc.Length != 4 || tc.Substring(1) == "00A")
                                continue;

                            // Gets the type from the first letter.
                            // T = Tile, O = Object, E = Enemy, I = Item
                            string idLetter = tc.Substring(0, 1);

                            // The first character (letter) is the id.
                            char type;
                            // The second two characters (numbers) are the type.
                            int number;
                            // The third character (letter) is the variant.
                            string variant;

                            // Gets the values.
                            // Set the type.
                            type = tc.Substring(0, 1)[0];

                            // Set the number - Converts the tile number from a string to an int.
                            int.TryParse(tc.Substring(1, 2), out number);

                            // Set the variant - gets the type as a string.
                            variant = tc.Substring(3, 1);

                            // The area entity.
                            AreaEntity areaEntity = null;
                            Transform entityParent = null;

                            // ELEMENT LOADING //
                            switch(type)
                            {
                                case 'T': // Tile
                                case 't':
                                    areaEntity = InstantiateTile(number, variant[0]);
                                    entityParent = tileParent;
                                    break;
                                    
                                case 'O': // Object
                                case 'o':
                                    areaEntity = InstantiateObject(number, variant[0]);
                                    entityParent = objectParent;
                                    break;

                                case 'E': // Enemy
                                case 'e':
                                    areaEntity = InstantiateEnemy(number, variant[0]);
                                    entityParent = enemyParent;
                                    break;

                                case 'I': // Item
                                case 'i':
                                    areaEntity = InstantiateItem(number, variant[0]);
                                    entityParent = itemParent;
                                    break;

                                default:
                                    areaEntity = null;
                                    break;

                            }

                            // No entity was generated, so continue.
                            if (areaEntity == null)
                                continue;


                            // POSITIONING //

                            // The entity position.
                            Vector3 entityPos;


                            // Checks if an origin object has been set.
                            if (originTransform != null) // Set
                                entityPos = originTransform.position + posOffset;
                            else // Not set
                                entityPos = posOffset;

                            // Gets the column position.
                            int col = j;

                            // Calculate the tile position.
                            entityPos.x += spacing.x * col;
                            entityPos.y += spacing.y * row;

                            // Set the tile's position.
                            if (setLocalPos)
                            {
                                areaEntity.transform.localPosition = entityPos;
                            }
                            else
                            {
                                areaEntity.transform.position = entityPos;
                            }

                            // Give it the tiles parent.
                            areaEntity.transform.parent = entityParent;
                        }

                        // Increase the row number.
                        row++;
                    } 
                }
            }

            // Set the area information.
            // This doesn't appear to work if this function is called in the Start() function, as it...
            // Likely gets overwritten by other things.
            if(area != null)
            {
                area.sector = sector;
                area.areaNumber = areaNumber;
            }

            // The file has been loaded successfully.
            fileLoaded = true;

            // Data loaded successfully.
            return true;
        }

        // ELEMENT INSTANTIONS //

        // Instantiates a tile.
        public WorldTile InstantiateTile(int number, char type)
        {
            // The original tile.
            WorldTile origTile = null;

            // The instantiated tile.
            WorldTile copyTile;

            // Checking the tile number.
            switch(number)
            {
                case 1: // Grass Floor (01)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.grassFloorA;
                            break;
                    }

                    break;

                case 2: // Grass Wall (02)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.grassWallA;
                            break;
                    }

                    break;

                case 3: // Metal Floor (03)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.metalFloorA;
                            break;
                    }
                    break;

                case 4: // Metal Wall (04)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.metalWallA;
                            break;
                    }
                    break;

                case 5: // Pavement Floor (05)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.pavementFloorA;
                            break;
                    }

                    break;

                case 6: // Brick Wall (06)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.brickWallA;
                            break;
                    }

                    break;

                case 7: // Bridge (07)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.bridgeFloorA;
                            break;
                    }

                    break;

                case 8: // Bottomless Pit (08)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.pitA;
                            break;
                    }

                    break;

                case 9: // Water (09)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.waterA;
                            break;
                    }

                    break;

                case 10: // Poison (10)
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = loaderPrefabs.poisonA;
                            break;
                    }

                    break;
            }

            // Checks if the original tile was set or not.
            if(origTile != null) // Set
            {
                // Copies the tile and returns it.
                copyTile = Instantiate(origTile);
                return copyTile;
            }
            else // Not set.
            {
                return null;
            }
        }

        // Instantiates an object.
        public AreaEntity InstantiateObject(int number, char type)
        {
            AreaEntity entity = null;

            switch (number)
            {
                case 0: // None (0)
                default:
                    entity = null;
                    break;

                case 1: // Rock (1)
                    entity = Instantiate(loaderPrefabs.rockBlock);
                    break;

                case 2: // Stone (2)
                    entity = Instantiate(loaderPrefabs.stoneBlock);
                    break;

                case 3: // Lock Box (3)
                    entity = Instantiate(loaderPrefabs.lockBox);
                    break;

                case 4: // Portal (4)
                    entity = Instantiate(loaderPrefabs.portal);

                    // The entity is set.
                    if(entity != null)
                    {
                        // Portal.
                        Portal portal;
                        
                        // Tries to get the portal component.
                        if(entity.TryGetComponent(out portal))
                        {
                            switch(type)
                            {
                                case 'A':
                                case 'a':
                                    portal.sprite.color = Color.white;
                                    break;

                                case 'B':
                                case 'b':
                                    portal.sprite.color = Color.red;
                                    break;

                                case 'C':
                                case 'c':
                                    portal.sprite.color = Color.blue;
                                    break;

                                case 'D':
                                case 'd':
                                    portal.sprite.color = Color.yellow;
                                    break;

                                case 'E':
                                case 'e':
                                    portal.sprite.color = Color.green;
                                    break;
                            }
                        }
                    }

                    break;
            }

            return entity;
        }

        // Instantiates an enemy spawner.
        public AreaEntity InstantiateEnemy(int number, char type)
        {
            // The enemy spawn.
            EnemySpawn enemySpawn = null;

            switch(number)
            {
                case 0: // None (0)
                    enemySpawn = null;
                    break;

                case 1: // Chaser (1)
                    enemySpawn = Instantiate(loaderPrefabs.chaserSpawn);
                    break;

                case 2: // Shooter (2)
                    enemySpawn = Instantiate(loaderPrefabs.shooterSpawn);
                    break;
            }

            return enemySpawn;
        }

        // Instantiates an object.
        public AreaEntity InstantiateItem(int number, char type)
        {
            // The item.
            AreaEntity entity = null;

            switch (number)
            {
                case 0: // None (0)
                    entity = null;
                    break;

                case 1: // Scrap Spawn (1)
                    switch(type)
                    {
                        case 'A': // Scrap = 1
                        case 'a':
                        default:
                            entity = Instantiate(loaderPrefabs.scrapSpawn1);
                            break;

                        case 'B': // Scrap = 3
                        case 'b':
                            entity = Instantiate(loaderPrefabs.scrapSpawn3);
                            break;

                        case 'C': // Scrap = 5
                        case 'c':
                            entity = Instantiate(loaderPrefabs.scrapSpawn5);
                            break;

                        case 'D': // Scrap = 7
                        case 'd':
                            entity = Instantiate(loaderPrefabs.scrapSpawn7);
                            break;

                        case 'E': // Scrap = 10
                        case 'e':
                            entity = Instantiate(loaderPrefabs.scrapSpawn10);
                            break;

                        case 'F': // Scrap = 15
                        case 'f':
                            entity = Instantiate(loaderPrefabs.scrapSpawn15);
                            break;
                    }
                    break;

                case 2: // Key (2)
                    entity = Instantiate(loaderPrefabs.key);
                    break;

                case 3: // Health (3)
                    entity = Instantiate(loaderPrefabs.health);
                    break;

                case 4: // Weapon Refill (4)
                    entity = Instantiate(loaderPrefabs.weaponRefill);
                    break;

                case 5: // Gun Slow (5)
                    entity = Instantiate(loaderPrefabs.gunSlow);
                    break;

                case 6: // Gun Mid (6)
                    entity = Instantiate(loaderPrefabs.gunMid);
                    break;

                case 7: // Gun Fast (7)
                    entity = Instantiate(loaderPrefabs.gunFast);
                    break;

                case 8: // Run Up (8)
                    entity = Instantiate(loaderPrefabs.runPower);
                    break;

                case 9: // Swim Up (9)
                    entity = Instantiate(loaderPrefabs.swimPower);
                    break;

            }

            return entity;
        }
    }
}