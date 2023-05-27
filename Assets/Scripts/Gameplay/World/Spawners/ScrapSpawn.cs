using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // Spawns scrap.
    public class ScrapSpawn : ItemSpawn
    {
        // The value of the scrap.
        public int scrapAmount = 1;

        // If the scrap amount should be changed upon being spawned.
        public bool changeScrapAmount = true;

        public override void Spawn()
        {
            // The item prefab has not been set.
            if (itemPrefab == null)
                return;

            // If the item prefab is not a scrap item.
            if (!(itemPrefab is ScrapItem))
                return;

            // Generates the scrap item.
            ScrapItem scrap = Instantiate(itemPrefab as ScrapItem);

            // Activate item.
            scrap.gameObject.SetActive(true);

            if (changeScrapAmount)
                scrap.scrapAmount = scrapAmount;

            // Sets the position.
            scrap.transform.position = GetSpawnPosition();

            // Set the area.
            if (area != null)
                area.AddItemToArea(scrap);
            
        }
    }
}