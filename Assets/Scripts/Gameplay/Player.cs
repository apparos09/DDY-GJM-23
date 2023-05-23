using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The namespace for the Doomsday Game Jam (2023).
namespace DDY_GJM_23
{
    // The player for the game.
    public class Player : Combatant
    {
        // // The amoutn of lives the player has.
        // public int lives = 0;

        public const string PLAYER_TAG = "Player";

        [Header("Movement")]

        // The direction the player is facing.
        public Vector2 facingDirec = Vector2.down;

        // The movement speed of the player.
        public float speed = 10.0F;

        // The maximum movement speed.
        public float maxSpeed = 10.0F;

        // The movement keys.
        public KeyCode moveLeftKey = KeyCode.A;
        public KeyCode moveRightKey = KeyCode.D;
        public KeyCode moveUpKey = KeyCode.W;
        public KeyCode moveDownKey = KeyCode.S;

        // The attack key.
        public KeyCode attackKey = KeyCode.Space;

        // Weapons
        [Header("Player Weapons")]

        // The current weapon of the player.
        public Weapon currWeapon;

        // The list of weapons.
        public List<Weapon> weapons;

        // The punch weapon.
        public Punch punchWeapon;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Gives the player the punch weapon.
            currWeapon = punchWeapon;

            // The punch.
            punchWeapon.owner = this;
            weapons.Add(punchWeapon);
        }

        // Gets the forward direction.
        // The values will either be [0] or [1].
        public Vector2 FacingDirection
        {
            get { return facingDirec; }
        }

        // Updates the inputs for the player.
        private void UpdateInputs()
        {
            // MOVEMENT
            // The movement directions.
            Vector2 movement = Vector2.zero;

            // TODO: find a way to have gradual slow down for tile movement instead of stopping instantly.

            // Movement - Horizontal (Left, Right)
            if(Input.GetKey(moveLeftKey)) // Left
            {
                movement.x = -1;
            }
            else if(Input.GetKey(moveRightKey)) // Right
            {
                movement.x = 1;
            }
            else if(Input.GetKeyUp(moveLeftKey) || Input.GetKeyUp(moveRightKey))
            {
                // Stop horizontal velocity.
                Vector2 velocity = new Vector2(0, rigidbody.velocity.y);
                rigidbody.velocity = velocity;
            }


            // Movement - Vertical (Up, Down)
            if (Input.GetKey(moveUpKey)) // Up
            {
                movement.y = 1;
            }
            else if (Input.GetKey(moveDownKey)) // Down
            {
                movement.y = -1;
            }
            else if (Input.GetKeyUp(moveUpKey) || Input.GetKeyUp(moveDownKey))
            {
                // Stop vertical velocity.
                Vector2 velocity = new Vector2(rigidbody.velocity.x, 0);
                rigidbody.velocity = velocity;
            }


            // There is movement, so apply it.
            if (movement != Vector2.zero)
            {
                // Set the face direction.
                facingDirec = movement.normalized;

                // Calculates the force.
                Vector2 force = movement * speed * Time.deltaTime;
                
                // Uses rigidbody for movement (original) (not needed) [OLD]
                // rigidbody.AddForce(force, ForceMode2D.Impulse);

                // TODO: add factor for slowing down by material.

                // Set velocity.
                rigidbody.velocity = Vector2.ClampMagnitude(force.normalized * 10.0F, maxSpeed);

                // Uses transform for movement.
                // transform.Translate(force);
            }

            // ATTACK
            // The user is attacking.
            if(Input.GetKeyDown(attackKey))
            {
                // Uses the weapon.
                if(currWeapon != null)
                {
                    if (currWeapon.IsUsable())
                        currWeapon.UseWeapon();
                }
            }
            
        }

        // On the death of the player.
        protected override void OnDeath()
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Updates the player's inputs.
            UpdateInputs();
        }
    }
}