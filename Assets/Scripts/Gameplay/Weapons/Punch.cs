using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The punch weapon.
    public class Punch : Weapon
    {
        [Header("Punch")]

        // The collider for the punch (enable/disable as part of an animation).
        public new Collider2D collider;

        // The animator for the punch animation.
        public Animator animator;

        // The punch's effect.
        public GameObject effect;

        // If 'true', the punch's effect is shown when the punch is active.
        public bool useEffect = true;

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Collider
            if (collider == null)
                collider = GetComponent<Collider2D>();

            // Grabs the animator.
            if (animator == null)
                animator = GetComponent<Animator>();

            // Hide the effect.
            if (useEffect)
                effect.gameObject.SetActive(false);


            // Turn off punch collider.
            collider.enabled = false;
        }

        // OnCollisionEnter2D - used to damage an enemy.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            HitTarget(collision.gameObject);
        }

        // OnTriggerEnter2D - used to damage an enemy.

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitTarget(collision.gameObject);
        }


        // Hits the provided target.
        public void HitTarget(GameObject target)
        {
            // The owner can't damage themselves.
            if (target == owner.gameObject)
                return;

            // Enemy component.
            Enemy enemy;

            // Obstruction component.
            Obstruction obstruc;

            // Tries to get the enemy component.
            if (target.TryGetComponent(out enemy))
            {
                // If the enemy is vulnerable.
                if(enemy.IsVulnerable())
                {
                    // TODO: trigger animation and invincibility frames.
                    enemy.ApplyDamage(power);

                    // If push force should be used, apply it to the enemy.
                    if (useKnockback)
                    {
                        ApplyKnockback(enemy);
                    }
                }
                
            }

            // The target is ab obstruction.
            if(target.TryGetComponent(out obstruc))
            {
                // If the obstruction is vulnerable to being destroyed by a punch.
                if(obstruc.punchVulnerable)
                {
                    // Apply damage to the obstruction.
                    obstruc.ApplyDamage(power);
                }
            }
        }

        // Use the weapon.
        public override void UseWeapon()
        {
            // Make sure the punch is finished.
            OnPunchFinished();

            // The direction of the punch.
            Vector2 direc = owner.FacingDirection;

            // Calcuate the punch angle (in degrees).
            float angle = 0.0F;

            // Checks the movement direction.
            if(direc.x != 0.0F && direc.y == 0.0F)
            {
                angle = (direc.x > 0) ? 0.0F : 180.0F;
            }
            else if(direc.x == 0.0F && direc.y != 0.0F)
            {
                angle = (direc.y > 0) ? 90.0F : 270.0F;
            }
            else
            {
                // Calculate the angle, and convert it to degrees.
                angle = Mathf.Rad2Deg * Mathf.Atan2(direc.y, direc.x);

                // // Manually set the angle (in degrees).
                // if (direc.x > 0 && direc.y > 0) // Top Right
                // {
                //     angle = 45.0F;
                // }
                // else if(direc.x > 0 && direc.y < 0) // Bottom Right
                // {
                //     angle = 315.0F;
                // }
                // else if(direc.x < 0 && direc.y > 0) // Top Left
                // {
                //     angle = 135.0F;
                // }
                // else if(direc.x < 0 && direc.y < 0) // Bottom Left
                // {
                //     angle = 225.0F;
                // }
                    

            }

            // Set the rotation of the hitbox.
            collider.transform.eulerAngles = new Vector3(0, 0, angle);

            // Plays the punch animation.
            animator.Play("Punch");

            // Show the effect.
            if (useEffect && effect != null)
                effect.gameObject.SetActive(true);

            // Called when the weapon was used.
            OnUseWeapon(0);


            // SFX
            // Grabs the game audio and plays the punch SFX.
            GameplayAudio gameAudio = owner.gameManager.gameAudio;
            gameAudio.PlayPlayerPunchSfx();
        }

        // Called when the punch is finished.
        public void OnPunchFinished()
        {
            // Reset collider rotation (this reset isn't working for some reason, should probably be fixed).
            // This function is being called though.
            collider.transform.rotation = Quaternion.identity;

            // Play the empty animation so that the animation switches off and can be replayed properly.
            animator.Play("Empty");

            // Hide the effect.
            if (useEffect && effect != null)
                effect.gameObject.SetActive(false);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}