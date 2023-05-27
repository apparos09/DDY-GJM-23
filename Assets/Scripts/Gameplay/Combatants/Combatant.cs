using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        [Header("Tiles")]
        // The tile the combat entity is currently on.
        public List<WorldTile> currentTiles = new List<WorldTile>();

        // The rate at which damage is applied by tiles.
        public float tileDamageRate = 1.0F;

        // A timer that counts down to apply damage.
        private float tileDamageTimer = 0.0F;

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

        // TILES
        // Gets the friction of the tiles the combatant is on, averaged.
        // If the entity is on no tiles, the friction is returned as 0.
        public float GetAveragedFrictionFactor()
        {
            // World friction.
            float friction = 0.0F;

            // Checks if there are tiles.
            if(currentTiles.Count == 0)
            {
                friction = 0.0F;
            }
            else
            {
                // Goes from back to forwards.
                for(int i = currentTiles.Count - 1;  i >= 0; i--)
                {
                    // Checks if the tile exists.
                    if(currentTiles[i] != null) // Exists.
                    {
                        friction += currentTiles[i].GetFriction();
                    }
                    else // Does not exist.
                    {
                        // Remove the current index.
                        currentTiles.RemoveAt(i);
                    }
                }

                // If there are tiles, average out the friction.
                if(currentTiles.Count > 0)
                {
                    friction /= currentTiles.Count;
                }
                else // No tiles, so set friction to 1.0F.
                {
                    friction = 1.0F;
                }
            }

            return friction;
        }

        // Gets the velocity with friction applied.
        public Vector2 GetVelocityWithFriction(Vector2 inVelocity, float mass, float frictionFactor)
        {
            // The velocity, mass or fricton are equal to 0, meaning there's nothing to slow down.
            if (mass == 0 || frictionFactor == 0.0F || inVelocity == Vector2.zero)
                return inVelocity;

            // This is modeled after the actual physics calculation.
            Vector2 outVelocity = inVelocity;

            // The object's mass and the gravitational force.
            float objectMass = Mathf.Abs(mass);
            float gravity = Mathf.Abs(World.GRAVITY_ACCEL);

            // Calculates the normal force.
            // Normal Force: mass * gravity.
            float normalForce = objectMass * gravity;

            // The friction being applied to the velocity.
            // Calclation: friction = friction_coefficient * normal_force.
            float veloFriction = Mathf.Abs(frictionFactor) * normalForce;

            // Calculate the counter velocity/friction velocity.
            Vector2 counterVelocity = -inVelocity.normalized * veloFriction;

            // Checks if the velocity should now be zero.
            if(counterVelocity.magnitude >= inVelocity.magnitude) // Set to zero.
            {
                outVelocity = Vector2.zero;
            }
            else // Don't set to zero.
            {
                outVelocity += counterVelocity;
            }

            // Return result.
            return outVelocity;
        }

        // Calculates the velocity with friction. Uses the friction of the touching tiles.
        public Vector3 GetVelocityWithFriction(Vector2 inVelocity, float mass)
        {
            return GetVelocityWithFriction(inVelocity, mass, GetAveragedFrictionFactor());

        }


        // DAMAGE //
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

        // Set the combatant to max health.
        public void SetToMaxHealth()
        {
            health = maxHealth;
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
            // If the entity's health is 0, kill them. Do not run any other behaviours. 
            if (health <= 0)
            {
                health = 0;
                OnDeath();
            }



            // Checks if there's time left for invincibility frames.
            if (iFrameTimer > 0)
            {
                // Reduce timer.
                iFrameTimer -= Time.deltaTime;

                // Take care of negative numbers.
                if (iFrameTimer < 0.0F)
                    iFrameTimer = 0.0F;
            }


            // The tile damage timer is greater than 0.
            if(tileDamageTimer > 0)
            {
                tileDamageTimer -= Time.deltaTime;

                if (tileDamageTimer < 0)
                    tileDamageTimer = 0.0F;
            }


            // Makes it so that an entity can only be damaged by one tile at a time.
            // If the tileDamageTimer is greater than 0, then the entity has already been damaged.
            bool damaged = (tileDamageTimer > 0);

            // Goes through the tiles being touched (from last to first).
            for (int i = currentTiles.Count - 1; i >= 0; i--)
            {
                // Checks if the tile exists.
                if (currentTiles[i] != null)
                {
                    // The entity can only be damaged once per frame.
                    if (!damaged)
                    {
                        // If it's a damage tile.
                        if (currentTiles[i] is DamageTile)
                        {
                            // Gets the damage.
                            float damage = ((DamageTile)currentTiles[i]).GetDamage();

                            // Applies the damage.
                            ApplyDamage(damage);

                            // Mark as damaged, and set the tile damage timer.
                            damaged = true;
                            tileDamageTimer = tileDamageRate;
                        }
                    }
                }
                else
                {
                    // Remove blank index.
                    currentTiles.RemoveAt(i);
                }
            }
        }
    }
}