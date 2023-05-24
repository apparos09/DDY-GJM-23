using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DDY_GJM_23
{
    // A scrap item.
    public class ScrapItem : WorldItem
    {
        [Header("Scrap")]

        // The value of this scrap item.
        public int scrapAmount = 1;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            id = itemId.scrap;
            destroyOnGet = false;
        }

        // On the item get.
        protected override void GiveItem()
        {
            // Increases the player's scrap value.
            GameplayManager.Instance.player.scrapCount += scrapAmount;

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