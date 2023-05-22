using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The punch weapon.
    public class Punch : Weapon
    {
        // The collider for the punch (enable/disable as part of an animation).
        public new BoxCollider2D collider;

        // The animator for the punch animation.
        public Animator animator;

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            base.Awake();

            weapon = weaponType.punch;
            infiniteUse = true;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Grabs the animator.
            if (animator == null)
                animator = GetComponent<Animator>();
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
        }

        // Called when the punch is finished.
        public void OnPunchFinished()
        {
            // Reset collider rotation (this reset isn't working for some reason, should probably be fixed).
            // This function is being called though.
            collider.transform.rotation = Quaternion.identity;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}