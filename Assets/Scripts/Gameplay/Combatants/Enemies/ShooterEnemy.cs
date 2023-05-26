using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // Shoots at the player.
    public class ShooterEnemy : Enemy
    {
        [Header("Enemy/Shooter")]

        // The prefab for the bullet to be fired.
        public Bullet bulletPrefab;

        // The fire rate of the enemy (seconds between shots)
        public float fireRate = 3.0F;

        // The timer for firing a projectile.
        public float fireTimer = 0.0F;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // The shooter shouldn't fire the moment the player enters the screen.
            if (fireTimer <= 0)
                fireTimer = fireRate;
        }

        // Runs the enemy behaviour.
        protected override void RunEnemyBehaviour()
        {
            // Checks if the enemy should fire or not.
            if(fireTimer <= 0) // Time to fire.
            {
                // Grab the player from the gameplay manager.
                Player player = GameplayManager.Instance.player;


                // The bullet prefab has been set, and so has the player.
                if (bulletPrefab != null && player != null)
                {
                    // Instantiates the bullet.
                    Bullet bullet = Instantiate(bulletPrefab);

                    // Give the bullet the enemy's position.
                    bullet.transform.position = transform.position;

                    // Get the direction.
                    Vector3 direc = player.transform.position - transform.position;

                    // Normalize the direction.
                    direc.Normalize();

                    // Sets the bullet direction.
                    bullet.SetBulletDirection(new Vector2(direc.normalized.x, direc.normalized.y));

                    // Reset the timer.
                    fireTimer = fireRate;
                }

            }
            else // Reduce timer
            {
                // Reduce timer.
                fireTimer -= Time.deltaTime;

                // Stop at 0.
                if(fireTimer < 0)
                    fireTimer = 0;
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}