using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A locked object in the game world.
    public class LockedObject : AreaEntity
    {
        // OnCollisionEnter2D
        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryUnlockByCollision(collision.gameObject);   
        }

        // OnTriggerEnter2D
        private void OnTriggerEnter2D(Collider2D collision)
        {
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