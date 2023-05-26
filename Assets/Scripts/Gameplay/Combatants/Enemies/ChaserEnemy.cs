using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        // FIXME: have the speed adjust so that the chaser doesn't just run circles around the target.

        // Runs the chaser enemy's behaviour.
        protected override void RunEnemyBehaviour()
        {
            // Grabs the player as the target.
            Player target = GameplayManager.Instance.player;

            // The target does not exist.
            if (target == null)
                return;

            // Direction of movement.
            Vector3 direc;

            // Pursue
            if(Vector3.Distance(transform.position, target.transform.position) > seekDist)
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

            // CLamps the speed.
            rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);
        }
    }
}