using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // An obstruction in the game world.
    public class Obstruction : AreaEntity
    {
        [Header("Obstruction")]

        // The health of the obstruction.
        public float health = 10;

        // The max health of the obstruction.
        public float maxHealth = 10;

        // Can be destroyed by punches.
        public bool punchVulnerable = true;

        // Can be destroyed by bullets.
        public bool bulletVulnerable = true;

        // Restore health to the obstruction.
        public void RestoreHealthToMax()
        {
            health = maxHealth;
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

            // Play the damage animation.
            if (animator != null)
                animator.Play("Damage");
        }

        // Destroys the obstruction.
        public void OnDestruction()
        {
            Destroy(gameObject);
        }
    }
}