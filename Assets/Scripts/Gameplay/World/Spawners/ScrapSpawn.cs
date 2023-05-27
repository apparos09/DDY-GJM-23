using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
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

        // The amount of time needed for the scraps to refresh.
        [Tooltip("The time needed for the spawner to refersh. This sotps it from spawning scrap everytime the window is entered.")]
        public float refreshTime = 30.0F;

        // The game time when the item was received.
        // This is compared to the elapsed game time to know if the scrap should spawn again.
        public float timeOnGet = -1.0F;

        // If 'true', the refresh time is used to determine if the scrap should spawn.
        // If 'false', the scrap refreshes when the area is entered.
        public bool useRefreshTime = true;

        // Returns 'true' if scrap can spawn.
        public bool CanSpawn()
        {
            // If the time on get is negative, it will always spawn.
            if(timeOnGet < 0 || !useRefreshTime)
            {
                return true;
            }
            else
            {
                // The elapsed game time.
                float elapsedTime = GameplayManager.Instance.elapsedGameTime;

                // If enough time has passed to spawn more scrap.
                if(elapsedTime - timeOnGet >= refreshTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Spawn the scrap.
        public override void Spawn()
        {
            // The item prefab has not been set.
            if (itemPrefab == null)
                return;

            // If the item prefab is not a scrap item.
            if (!(itemPrefab is ScrapItem))
                return;

            // If the element can't spawn, don't do anything.
            if (!CanSpawn())
                return;

            // Generates the scrap item.
            ScrapItem scrap = Instantiate(itemPrefab as ScrapItem);

            // Remember the spawner.
            scrap.spawner = this;

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

        // Called when the spawned scrap has been collected.
        public void OnScrapCollected()
        {
            // If refresh time should be used, grap the elapsed game time.
            if (useRefreshTime)
                timeOnGet = GameplayManager.Instance.elapsedGameTime;
        }
    }
}