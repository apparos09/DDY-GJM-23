using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A locked object in the game world.
    public class LockedObject : AreaEntity
    {
        [Header("LockedObject")]

        // The game manager for the locked objecrt.
        public GameplayManager gameManager;


        // Start is called just before any Update methods is called the first time.
        protected override void Start()
        {
            base.Start();

            if (gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        // OnCollisionEnter2D
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // If the collided object has the player tag.
            if (collision.gameObject.tag == Player.PLAYER_TAG)
            {
                // Activate the lock block tutorial.
                gameManager.ActivateTutorial(Tutorial.trlType.lockBlock);
            }

            // Try unlock.
            TryUnlockByCollision(collision.gameObject);   
        }

        // OnTriggerEnter2D
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Checks if tutorials are active.
            if (gameManager.GetUseTutorial())
            {
                // If the collided object has the player tag.
                if (collision.gameObject.tag == Player.PLAYER_TAG)
                {
                    // Activate the lock block tutorial.
                    gameManager.ActivateTutorial(Tutorial.trlType.lockBlock);
                }
            }

            TryUnlockByCollision(collision.gameObject);
        }

        // Tries to unlock the object by collision.
        private void TryUnlockByCollision(GameObject colObject)
        {
            // Player.
            Player player;

            // Tries to grab the player.
            if(colObject.TryGetComponent(out player))
            {
                // The player has a key.
                if(player.keyCount > 0)
                {
                    // Reduce the key count.
                    player.keyCount--;

                    // The player has used a key.
                    player.keysUsed++;

                    // Unlock the object.
                    Unlock();
                }
            }
        }

        // Destroys the locked object.
        public void Unlock()
        {
            // Destroy this object.
            Destroy(gameObject);
        }
    }
}