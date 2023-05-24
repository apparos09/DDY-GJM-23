using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A bullet.
    public class Bullet : MonoBehaviour
    {
        // A rigidbody for the bullet.
        // public new Rigidbody2D rigidbody;

        // The tags of valid target for the bullet. If no tags are listed, then the bullet hits all targets.
        public List<string> targetTags = new List<string>();

        // The movement speed of the bullet. Bullets travel at a FIXED speed.
        public float speed = 1.0F;

        // The movement direction of the bullet.
        public Vector2 direc = Vector2.right;

        // The amount of damage the bullet does upon contact.
        public float power = 1.0F;

        // If set to 'true', bullets pass through targets.
        [Tooltip("If 'true', the bullet passes through targets.")]
        public bool passThrough = false;

        [Header("Life Time")]

        // The life time timer for the bullet.
        public float lifeTimeTimer = 0.0F;

        // Pauses the bullet life time timer.
        public bool pausedTimer = false;

        // Start is called before the first frame update
        void Start()
        {

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
        

        // Moves the bullet.
        public virtual void MoveBullet()
        {
            // TODO: maybe use rigidbody.
            transform.Translate(direc * speed * Time.deltaTime);
        }

        // Kills the bullet by destroying the game object.
        public virtual void Kill()
        {
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            // Lifetime timer.
            if(!pausedTimer && lifeTimeTimer > 0.0F)
            {
                lifeTimeTimer -= Time.deltaTime;

                // Time is over.
                if(lifeTimeTimer <= 0.0F)
                {
                    lifeTimeTimer = 0.0F;
                    Kill();
                }
            }

            // Moves the bullet.
            MoveBullet();
        }
    }
}