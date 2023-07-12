using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A health item.
    public class HealthItem : WorldItem
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Id not set.
            if(id == itemId.none)
                id = itemId.health;
        }

        // Give the player the key.
        protected override void GiveItem()
        {
            // Get the player.
            GameplayManager manager = GameplayManager.Instance;
            Player player = manager.player;

            // Give the player the key.
            player.healCount++;

            // Attempts to activate the tutorial.
            manager.ActivateTutorial(Tutorial.trlType.healthItem);

            // Destroy the item.
            OnItemGet();
        }
    }
}
