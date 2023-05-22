using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The namespace for the Doomsday Game Jam (2023).
namespace DDY_GJM_23
{
    // The player for the game.
    public class Player : MonoBehaviour
    {
        // The rigidbody for the player.
        public new Rigidbody2D rigidbody;

        // The player's collider.
        public new BoxCollider2D collider;

        // The direction the player is facing.
        private Vector2 forwardDirec = Vector2.down;

        // The movement speed of the player.
        private float speed = 10.0F;

        // The movement keys.
        private KeyCode moveLeftKey = KeyCode.A;
        private KeyCode moveRightKey = KeyCode.D;
        private KeyCode moveUpKey = KeyCode.W;
        private KeyCode moveDownKey = KeyCode.S;

        // The attack key.
        private KeyCode attackKey = KeyCode.Space;

        // Weapons
        [Header("Weapons")]

        // The current weapon of the player.
        public Weapon currWeapon;

        // The list of weapons.
        public List<Weapon> weapons;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the 2D rigidbody.
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody2D>();
            }

            // Gets the player's box collider.
            if (collider == null)
            {
                collider = GetComponent<BoxCollider2D>();
            }

            // Gives the player the punch weapon.
            currWeapon = new Punch(this, null);
            weapons.Add(currWeapon);
        }

        // Gets the forward direction.
        // The values will either be [0] or [1].
        public Vector2 ForwardDirection
        {
            get { return forwardDirec; }
        }

        // Updates the inputs for the player.
        private void UpdateInputs()
        {
            // MOVEMENT
            // The movement directions.
            Vector2 movement = Vector2.zero;

            // Movement - Horizontal (Left, Right)
            if(Input.GetKey(moveLeftKey))
            {
                movement.x = -1;
            }
            else if(Input.GetKey(moveRightKey))
            {
                movement.x = 1;
            }

            // Movement - Vertical (Up, Down)
            if (Input.GetKey(moveUpKey))
            {
                movement.y = 1;
            }
            else if (Input.GetKey(moveDownKey))
            {
                movement.y = -1;
            }

            // There is movement, so apply it.
            if(movement != Vector2.zero)
            {
                // Calculates the force.
                Vector2 force = movement * speed * Time.deltaTime;
                
                // Uses rigidbody for movement.
                // rigidbody.AddForce(force, ForceMode2D.Impulse);

                // Uses transform for movement.
                transform.Translate(force);
            }

            // ATTACK
            // The user is attacking.
            if(Input.GetKeyDown(attackKey) && currWeapon != null)
            {
                // Uses the weapon.
                if (currWeapon.IsUsable())
                    currWeapon.UseWeapon();
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            // Updates the player's inputs.
            UpdateInputs();
        }
    }
}