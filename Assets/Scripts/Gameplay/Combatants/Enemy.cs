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
    }
}