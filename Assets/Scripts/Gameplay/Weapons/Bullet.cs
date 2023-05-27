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
            // The combatant.
            Combatant combatant;

            // Tries to grab a combatant component.
            if(other.TryGetComponent(out combatant))
            {
                // Applies the damage.
                combatant.ApplyDamage(power);

                // If the bullet shouldn't pass through the combatant, destroy it.
                if (!passThrough)
                    Kill();
            }
        }
        
        // Returns the bullet direction.
        public Vector2 GetBulletDirection()
        {
            Vector3 right = transform.right;
            Vector2 direc = new Vector2(right.x, right.y);

            return direc;
        }

        // Sets the bullet's direction. This sets the direction by changing the 'right' parameter of the bullet.
        public void SetBulletDirection(Vector2 newDirec)
        {     
            // Rotate the bullet so that it goes in the intended direction.
            transform.right = newDirec.normalized;
        }


        // Moves the bullet.
        public virtual void TransformBullet()
        {
            rigidbody.AddForce(transform.right * speed * Time.deltaTime);
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