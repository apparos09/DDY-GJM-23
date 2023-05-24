using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // An enemy in the game world.
    public class Enemy : Combatant
    {
        // The enemy id.
        public enum enemyId { none };

        // The spawner the enemy came from.
        public EnemySpawn spawn;

        // If 'true', the enemy causes damage upon making contact with the player.
        public bool contactDamage = true;

        // The power for contact damage.
        public float contactDamagePower = 10.0F;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Enemies don't have invincibility frames.
            useIFrames = false;
        }

        // OnCollisionStay2D - used to damage the player if contact damage is enabled.
        private void OnCollisionStay2D(Collision2D collision)
        {
            // If the enemy causes contact damage.
            if(contactDamage)
            {
                Player player;

                // Tries to get the player component.
                if(collision.gameObject.TryGetComponent(out player))
                {
                    player.ApplyDamage(contactDamagePower);
                }
            }
        }

        // OnTriggerStay2D - damages player if contact damage is enabled.
        private void OnTriggerStay2D(Collider2D collision)
        {
            // If the enemy causes contact damage.
            if (contactDamage)
            {
                Player player;

                // Tries to get the player component.
                if (collision.gameObject.TryGetComponent(out player))
                {
                    player.ApplyDamage(contactDamagePower);
                }
            }
        }

        // Adds the enemy to the spawn.
        public void AddToSpawn(EnemySpawn newSpawn)
        {
            // Removes from spawn.
            if (spawn != null)
                RemoveFromSpawn();

            // Sets spawn.
            spawn = newSpawn;

            // Adds the enemy to the list.
            if (!spawn.spawnedEnemies.Contains(this))
                spawn.spawnedEnemies.Add(this);
        }

        // Removes the enemy from the spawner if one is set.
        // Returns false if no spawn was set, or if the enemy wasn't in the spawn's list.
        public bool RemoveFromSpawn()
        {
            bool result;

            // A spawner was set.
            if (spawn != null)
            {
                // Remove from the spawn list.
                if (spawn.spawnedEnemies.Contains(this))
                {
                    spawn.spawnedEnemies.Remove(this);
                    result = true;
                }
                else
                {
                    result = false;
                }

                // Not part of any spawn now.
                spawn = null;
            }
            else
            {
                result = false;
            }

            return result;
        }

        // On the death of the player.
        protected override void OnDeath()
        {
            Destroy(gameObject);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            RemoveFromSpawn();
        }
    }
}