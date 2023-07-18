using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // An obstruction in the game world.
    public class Obstruction : AreaEntity
    {
        [Header("Obstruction")]

        // Gets the gameplay manager.
        public GameplayManager gameManager;

        // The health of the obstruction.
        public float health = 10;

        // The max health of the obstruction.
        public float maxHealth = 10;

        // Can be destroyed by punches.
        public bool punchVulnerable = true;

        // Can be destroyed by bullets.
        public bool bulletVulnerable = true;

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // Gets the gameplay manager if it's not set.
            if (gameManager == null)
                gameManager = GameplayManager.Instance;
        }

        // Restore health to the obstruction.
        public void RestoreHealthToMax()
        {
            health = maxHealth;
        }

        // Called when the collision is started.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Checks if the tutorial is being used.
            if(gameManager.GetUseTutorial())
            {
                // If the collided object has the player tag.
                if (collision.gameObject.tag == Player.PLAYER_TAG)
                {
                    // Checks what kind of tutorial to load.
                    if (punchVulnerable && bulletVulnerable) // Punch and Bullet
                    {
                        gameManager.ActivateTutorial(Tutorial.trlType.softBlock);
                    }
                    else if (!punchVulnerable && bulletVulnerable) // Bullet Only
                    {
                        gameManager.ActivateTutorial(Tutorial.trlType.hardBlock);
                    }

                }
            }

        }


        // Apply damage to the obstruction.
        public void ApplyDamage(float power)
        {
            health -= power;

            // Checks if obstruction should be destroyed.
            if(health <= 0)
            {
                health = 0;
                OnDestruction();
            }
            else // Play the hurt SFX.
            {
                gameManager.gameAudio.PlayEnemyHurtSfx();
            }

            // Plays the damage animation.
            PlayDamageAnimation();
        }

        // ANIMATION //
        // Plays the damage animation.
        public void PlayDamageAnimation()
        {
            // Play the damage animation.
            if (animator != null)
                animator.Play("Damage");

        }

        // Stops the damage animation.
        public void OnDamageAnimationEnd()
        {
            // Plays the empty effect.
            if (animator != null)
            {
                // Stop the animation.
                animator.Play("Empty");
            }
        }

        // DESTRUCTION
        // Destroys the obstruction.
        public void OnDestruction()
        {
            // Block broken.
            gameManager.gameAudio.PlayBlockBreakSfx();

            Destroy(gameObject);
        }
    }
}