using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // An enemy that chases the player using seek behaviour.
    public class ChaserEnemy : Enemy
    {
        [Header("Enemy/Chaser")]

        // The movement speed of the chaser.
        [Tooltip("The chaser's speed when pursuing the target.")]
        public float pursueSpeed = 5.0F;

        [Tooltip("The chaser's speed when seeking the target.")]
        public float seekSpeed = 7.0F;

        // The maximum speed of the chaser enemy.
        public float maxSpeed = 7.0F;

        // The distance between the target and the chaser for the chaser to pursue them directly.
        public float seekDist = 30.0F;

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // Set the id to the chaser.
            if (id == enemyId.none)
                id = enemyId.chaser;
        }

        // FIXME: have the speed adjust so that the chaser doesn't just run circles around the target.

        // Runs the chaser enemy's behaviour.
        protected override void RunEnemyBehaviour()
        {
            // FIXME: for somee reason this is being called before this rigidbody is set?
            // Quick fix.
            if (rigidbody == null)
                return;

            // Grabs the player as the target.
            Player target = GameplayManager.Instance.player;

            // The target does not exist.
            if (target == null)
                return;

            // Direction of movement.
            Vector3 direc;

            // Distance between the enemy and the target.
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // The target is within hit distance.
            if(distance < searchDistance)
            {
                // Pursue
                if (distance > seekDist)
                {
                    Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.y);
                    direc = (target.transform.position + new Vector3(target.rigidbody.velocity.x, target.rigidbody.velocity.y, 0))
                        - transform.position;

                    rigidbody.AddForce(direc.normalized * pursueSpeed * Time.deltaTime, ForceMode2D.Impulse);
                }
                // Seek
                else
                {
                    direc = target.transform.position - transform.position;

                    rigidbody.AddForce(direc.normalized * seekSpeed * Time.deltaTime, ForceMode2D.Impulse);
                }                
            }
            else
            {
                // Stop enemy.
                // If the enemy is moving.
                if (rigidbody.velocity != Vector2.zero)
                {
                    // Gets the velocity change with friction applied.
                    Vector2 result;

                    // Stop the shooter enemy from having its velocity going continuously.
                    if (currentTiles.Count > 0)
                        result = GetVelocityWithFriction(rigidbody.velocity, rigidbody.mass);
                    else
                        result = GetVelocityWithFriction(rigidbody.velocity, rigidbody.mass, 1.0F);

                    // Set the result.
                    rigidbody.velocity = result;
                }
            }

            // Clamps the speed.
            rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);
        }
    }
}