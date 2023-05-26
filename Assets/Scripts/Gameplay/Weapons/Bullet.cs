using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A bullet.
    public class Bullet : MonoBehaviour
    {
        // A rigidbody for the bullet.
        public new Rigidbody2D rigidbody;

        // The tags of valid target for the bullet. If no tags are listed, then the bullet hits all targets.
        public List<string> targetTags = new List<string>();

        // The movement speed of the bullet. Bullets travel at a FIXED speed.
        public float speed = 1.0F;

        // The maximum speed of the bullet.
        public float maxSpeed = 10.0F;

        // The amount of damage the bullet does upon contact.
        public float power = 1.0F;

        // If set to 'true', bullets pass through targets.
        [Tooltip("If 'true', the bullet passes through targets.")]
        public bool passThrough = false;

        // The movement direction of the bullet.
        public Vector2 moveDirec = Vector2.right;

        [Header("Life Time")]

        // The life time timer for the bullet.
        public float lifeTimeTimer = 5.0F;

        // Pauses the bullet life time timer.
        public bool pausedTimer = false;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs rigidbody 2D.
            if(rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();
        }

        // OnCollisionEnter2D
        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnColliderInteract(collision.gameObject);
        }

        // OnTriggerEnter2D
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnColliderInteract(collision.gameObject);
        }

        // Try to damage the hit entity.
        private void OnColliderInteract(GameObject other)
        {
            Combatant combat;

            // Tries to grab a combatant component.
            if(other.TryGetComponent(out combat))
            {
                combat.ApplyDamage(power);
            }
        }
        
        // Sets the bullet's direction.
        public void SetBulletDirection(Vector2 newDirec)
        {
            //// Changing the forward was iffy, so I'm doing this now.
            //// TODO: find a way to make the forward work.

            //// No new direction given, so just keep going forward.
            //if (newDirec == Vector2.zero)
            //    return;

            //// Normalize the direction.
            //newDirec.Normalize();

            //// FIXME: there has to be a better way of doing this...

            //// The bullet rotation.
            //float theta = 0.0F;

            //// Relative to quadrant the direction would fall in.
            //if (newDirec.x <= 0 && newDirec.y > 0) // Quadrant 2 (Top Left)
            //{
            //    theta += 90.0F;
            //}
            //else if(newDirec.x < 0 && newDirec.y <= 0) // Quadrant 3 (Bottom Left)
            //{
            //    theta += 180.0F;
            //}
            //else if(newDirec.x >= 0 && newDirec.y < 0) // Quadrant 4 (Bottom Right)
            //{
            //    theta += 270.0F;
            //}

            //// If both values are not equal to 0, do a calculation.
            //if(newDirec.x != 0 && newDirec.y != 0)
            //{
            //    // Get relative rotation.
            //    theta += Mathf.Rad2Deg * Mathf.Atan2(newDirec.y, newDirec.x);
            //}

            //// Get the rotation.
            //Vector3 eulers = transform.eulerAngles;
            //eulers.z = theta;

            //// Apply the rotation.
            //transform.eulerAngles = eulers;

            // Rotating didn't work, so I'm just hardcoding the direction.
            moveDirec = newDirec.normalized;
        }


        // Moves the bullet.
        public virtual void TransformBullet()
        {
            rigidbody.AddForce(moveDirec.normalized * speed * Time.deltaTime);
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);

        }

        // Kills the bullet by destroying the game object.
        public virtual void Kill()
        {
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            // Moves the bullet.
            TransformBullet();

            // Lifetime timer.
            if (!pausedTimer && lifeTimeTimer > 0.0F)
            {
                lifeTimeTimer -= Time.deltaTime;

                // Time is over.
                if(lifeTimeTimer <= 0.0F)
                {
                    lifeTimeTimer = 0.0F;
                    Kill();
                }
            }
        }
    }
}