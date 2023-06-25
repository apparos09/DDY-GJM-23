using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DDY_GJM_23
{
    // A scrap item.
    public class ScrapItem : WorldItem
    {
        [Header("Scrap")]

        // The value of this scrap item.
        public int scrapAmount = 1;

        // The spawner the scrap belongs to.
        public ScrapSpawn spawner;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Auto-set id.
            if(id == itemId.none)
                id = itemId.scrap;
        }

        // On the item get.
        protected override void GiveItem()
        {
            // Increases the player's scrap value.
            GameplayManager.Instance.player.scrapCount += scrapAmount;

            // If the spawner is not equal to null, tell it this scrap has been collected.
            if(spawner != null)
            {
                spawner.OnScrapCollected();
            }

            // Item has been gotten.
            OnItemGet();
        }

        //// Update is called once per frame
        //protected override void Update()
        //{
        //    base.Update();
        //}
    }
}