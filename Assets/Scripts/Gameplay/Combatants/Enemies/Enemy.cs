using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // An enemy in the game world.
    public abstract class Enemy : Combatant
    {
        // The enemy id.
        public enum enemyId { none, chaser, shooter };

        [Header("Enemy")]

        // The id of the enemy.
        [Tooltip("The ID of the enemy.")]
        public enemyId id = enemyId.none;

        // If 'true', the enemy causes damage upon making contact with the player.
        public bool contactDamage = true;

        // The power for contact damage.
        public float contactDamagePower = 10.0F;

        // The search distance for the shooter.
        public float searchDistance = 100.0F;

        // If 'true', the game runs the enemy's behaviour.
        [Tooltip("Runs the behaviour of the enemy if this is true.")]
        public bool runBehaviour = true;

        [Header("Enemy/Animations")]
        // The idle animation for the enemy.
        public string idleAnimation = "";

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Plays the idle animation by default.
            PlayIdleAnimation();
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

        // PROGRAMMING

        // Runs the enemy's behaviour (AI)
        protected abstract void RunEnemyBehaviour();


        // On the death of the player.
        protected override void OnDeath()
        {
            Destroy(gameObject);
        }

        // ANIMATIONS //

        // Plays an animation based on the current level.
        protected virtual void PlayIdleAnimation()
        {
            animator.Play(idleAnimation);
        }


        // AUDIO //

        // UPDATE

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Runs the enemy behaviour.
            RunEnemyBehaviour();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // Area has been set.
            if(area != null)
            {
                // Remove from enemy list.
                if(area.enemies.Contains(this))
                {
                    area.enemies.Remove(this);
                }
            }
        }
    }
}