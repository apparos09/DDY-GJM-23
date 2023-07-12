using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DDY_GJM_23
{
    // A combat entity in the game world.
    public abstract class Combatant : AreaEntity
    {
        [Header("Combatant")]

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

        // Used to play the iframe animation after the damage information if set to true.
        private bool startIFrameAnimation = false;

        // Says whether the entity flies or not.
        public bool flying = false;

        // If 'true', the entity is immune to poison damage.
        public bool poisonImmune = false;

        [Header("Combatant/Tiles")]
        // The tile the combat entity is currently on.
        public List<WorldTile> currentTiles = new List<WorldTile>();

        // The rate at which damage is applied by tiles.
        public float tileDamageRate = 0.25F;

        // A timer that counts down to apply damage.
        private float tileDamageTimer = 0.0F;

        [Header("Combatant/Animations")]

        // The empty effect.
        public string emptyEffectAnim = "Empty Effect";

        // The animation name for playing the damage animation.
        public string damageAnim = "Damage";

        // The animation name for playing the invincibility frames animation.
        public string iFrameAnim = "Invincibility Frames";

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

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
        // Gets the number of non-liquid tiles being touched.
        public int GetSolidTileCount()
        {
            // The count.
            int count = 0;

            // Go through each tile.
            foreach (WorldTile tile in currentTiles)
            {
                // Tile exists.
                if (tile != null)
                {
                    // Tile is liquid, so add to count.
                    if (!tile.isLiquid)
                        count++;
                }
            }

            return count;
        }

        // Gets the number of liquid tiles being touched.
        public int GetLiquidTileCount()
        {
            // The count.
            int count = 0;

            // Go through each tile.
            foreach(WorldTile tile in currentTiles)
            {
                // Tile exists.
                if(tile != null)
                {
                    // Tile is liquid, so add to count.
                    if (tile.isLiquid)
                        count++;
                }
            }

            return count;
        }


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


        // HEALTH & DAMAGE //
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
        public void SetHealthToMax()
        {
            health = maxHealth;
        }

        // Gets the health as a percentage.
        public float GetHealthAsPercentage()
        {
            float result = health / maxHealth;
            return result;
        }

        // Reduces the combatant's entity.
        // If 'triggerIFrames' is true, the damage gives the entity invincibility frames.
        public void ApplyDamage(float damage, bool triggerIFrames = true)
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
                // Plays the damage animation.
                PlayDamageAnimation();

                // Sets the invincibility timer if the combatant has invincibility frames.
                if(useIFrames && triggerIFrames)
                {
                    // Set timer.
                    iFrameTimer = I_FRAME_TIME_MAX;

                    // Set so that the iframes trigger after the damage animation finishes.
                    startIFrameAnimation = true;
                }
                    
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


        // ANIMATION //
        // Plays the damage animation.
        public void PlayDamageAnimation()
        {
            // TODO: the damage animation is only getting played once for some reason. Fix that.
            if (animator != null)
                animator.Play(damageAnim);

        }

        // Stops the damage animation.
        public void OnDamageAnimationEnd()
        {
            // Plays the empty effect.
            if (animator != null)
            {
                // Stop the animation.
                animator.Play(emptyEffectAnim);

                // Play the invicibility frames animation.
                if (startIFrameAnimation && iFrameTimer > 0)
                {
                    PlayIFrameAnimation();
                    startIFrameAnimation = false;
                }
            }
        }
        
        // Plays the invincibility frames animation.
        public void PlayIFrameAnimation()
        {
            // Play the animation.
            if (animator != null)
                animator.Play(iFrameAnim);
        }

        // Stops the invincibility frames animation.
        public void StopIFrameAnimation()
        {
            if (animator != null)
                animator.Play("Empty Effect");
        }
        

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
                {
                    // Timer is done.
                    iFrameTimer = 0.0F;

                    // Stop the invincibility frames animation.
                    StopIFrameAnimation();
                }
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
                // FIXME: this is to fix a glitch where the list becomes empty for some reason.
                if (currentTiles.Count == 0)
                    break;

                // Checks if the tile exists.
                if (currentTiles[i] != null)
                {
                    // If it's a damage tile.
                    if (currentTiles[i] is DamageTile)
                    {
                        DamageTile dTile = (DamageTile)currentTiles[i];

                        // Damage should be applied.
                        if (dTile.applyDamage)
                        {
                            // Gets set to 'true' if damagable.
                            bool damagable;

                            // Checks if the combatant can be damaged by the tile.
                            // Poison Immune or Flying Over Pit
                            if (
                                (currentTiles[i].type == WorldTile.tileType.poison && poisonImmune) ||
                                (currentTiles[i].type == WorldTile.tileType.pit && flying)
                                )
                            {
                                damagable = false;
                            }
                            else
                            {
                                damagable = true;
                            }


                            // If the entity can be damaged.
                            if (damagable)
                            {
                                // Tile is instant kill.
                                if (dTile.instantKill)
                                {
                                    // Kill the combatant.
                                    Kill();
                                }
                                // Apply regular damage if it hasn't been applied already.
                                else if (!dTile.instantKill && !damaged)
                                {
                                    // Gets the damage.
                                    float damage = ((DamageTile)currentTiles[i]).GetDamage();

                                    // Applies the damage, but doesn't give invincibility frames.
                                    ApplyDamage(damage, false);

                                    // Mark as damaged, and set the tile damage timer.
                                    damaged = true;
                                    tileDamageTimer = tileDamageRate;
                                }

                            }

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