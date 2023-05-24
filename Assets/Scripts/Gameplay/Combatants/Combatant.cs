using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A combat entity in the game world.
    public abstract class Combatant : AreaEntity
    {
        // The rigidbody for the player.
        public new Rigidbody2D rigidbody;

        // The player's collider.
        public new BoxCollider2D collider;

        // The player's health.
        public float health = 100.0F;

        // The player's max health.
        public float maxHealth = 100.0F;

        // Gets set to 'true' if the entity is vulnerable (can be damaged).
        public bool vulnerable = true;

        // If 'true', the entity is able to use iFrames (invincibility frames).
        public bool useIFrames = true;

        // The amount of time for invincibility frames.
        public const float I_FRAME_TIME_MAX = 5.0F;

        // The timer for invincibility frames.
        private float iFrameTimer;

        // The tile the combat entity is currently on.
        public List<FloorTile> currentTiles;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Grabs the 2D rigidbody.
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody2D>();
            }

            // Gets the player's box collider.
            if (collider == null)
            {
                collider = GetComponent<BoxCollider2D>();
            }
        }

        // Returns 'true' if the combatant is invincible/invulnerable.
        public bool IsVulnerable()
        {
            // The result object.
            bool result = false;

            // Checks base variable.
            if(vulnerable) // Can be harmed.
            {
                // If the iFrameTimer is not 0, then the entity has invincibility frames.
                // This means they can't be harmed.
                result = iFrameTimer <= 0.0F;
            }
            else // Cannot be harmed.
            {
                result = false;
            }

            return result;
        }

        // Reduces the combatant's entity.
        public void ApplyDamage(float damage)
        {
            // The entity can't be damaged because they're invincible.
            if (!IsVulnerable())
                return;

            // Reduces health.
            health -= damage;

            // Prevent negative health.
            if (health < 0)
                health = 0;

            // Activate invincibility timer.
            if(health > 0)
            {
                // Sets the invincibility timer if the combatant has invincibility frames.
                if(useIFrames)
                    iFrameTimer = I_FRAME_TIME_MAX;
            }
            // Called to handle death mechanics.
            else if (health <= 0.0F)
            {
                OnDeath();
            }
        }

        // Kills the entity. This ignores the value of 'killable'.
        public void Kill()
        {
            health = 0.0F;

            // Called to handle death mechanics.
            OnDeath();
        }

        // Called when an entity has died.
        protected abstract void OnDeath();

        // Update is called once per frame
        protected virtual void Update()
        {
            // Checks if there's time left for invincibility frames.
            if(iFrameTimer > 0)
            {
                // Reduce timer.
                iFrameTimer -= Time.deltaTime;

                // Take care of negative numbers.
                if (iFrameTimer < 0.0F)
                    iFrameTimer = 0.0F;
            }
        }
    }
}