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
        private Vector2 facingDirec = Vector2.down;

        // The movement speed of the player.
        public float speed = 75.0F;

        // The maximum movement speed.
        public float maxSpeed = 12.0F;

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
        public Weapon currentWeapon;

        // TODO: maybe make it a fixed size and replace the null entries with the proper weapon (set order)?
        // The list of weapons.
        public List<Weapon> weapons;

        // The punch weapon.
        public Punch punch;
        public GunSingle gunSingle;

        // The left weapon key and the right weapon key.
        public KeyCode leftWeaponKey = KeyCode.LeftArrow;
        public KeyCode rightWeaponKey = KeyCode.RightArrow;

        [Header("Other")]

        // The amount of scrap the player has on hand.
        public int scrapCount = 0;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();


            // Finding weapons
            // Punch
            if (punch == null)
                punch = FindObjectOfType<Punch>(true);

            // Gun (Single Shot)
            if (gunSingle == null)
                gunSingle = FindObjectOfType<GunSingle>(true);


            // Adds the punch weapon to the list.
            AddWeapon(Weapon.weaponType.punch);

            // Gives the player the punch weapon.
            currentWeapon = punch;
        }

        // Gets the forward direction.
        // The values will either be [0] or [1].
        public Vector2 FacingDirection
        {
            get { return facingDirec; }
        }

        // Gets facing orientation in degrees (z-axis). Facing right means an angle of 0.0.
        public float GetFacingDirectionAsRotation()
        {
            float angle = 0.0F;

            // Gets the facing direction.
            if(facingDirec.x != 0 && facingDirec.y == 0) // Left/Right
            {
                angle = (facingDirec.x > 0) ? 0.0F : 180.0F;
            }
            else if (facingDirec.x == 0 && facingDirec.y != 0) // Up/Down
            {
                angle = (facingDirec.y > 0) ? 90.0F : 270.0F;
            }
            else if(facingDirec == Vector2.zero) // No facing direction set (defaults right).
            {
                angle = 0.0F;
            }
            else // Diagonals.
            {
                // This is propbably really bad, but it works.
                float x = facingDirec.x;
                float y = facingDirec.y;

                // Checks the direction.
                if(x > 0 && y > 0) // Top Right
                {
                    angle = 45.0F;
                }
                else if (x < 0 && y > 0) // Top Left
                {
                    angle = 135.0F;
                }
                else if(x > 0 && y < 0) // Bottom Right
                {
                    angle = 225.0F;
                }
                else if(x < 0 && y < 0) // Bottom Left
                {
                    angle = 315.0F;
                }
            }

            return angle;
        }

        // TODO: maybe have the weapons stay in fixed slots?
        // Adds a weapon to the player.
        public void AddWeapon(Weapon.weaponType type)
        {
            // Checks the type of weapon to add.
            switch(type)
            {
                case Weapon.weaponType.punch:

                    // Put the gun single in the list.
                    if (!weapons.Contains(punch))
                    {
                        weapons.Add(punch);
                        punch.owner = this;
                    }
                        
                    
                    break;

                case Weapon.weaponType.gunSingle:

                    // Put the gun single in the list.
                    if (!weapons.Contains(gunSingle))
                    {
                        weapons.Add(gunSingle);
                        gunSingle.owner = this;
                    }
                    else
                    {
                        gunSingle.RestoreUsesToMax();
                    }

                    break;
            }
        }

        // Removes a weapon from the player.
        public void RemoveWeapon(Weapon.weaponType type)
        {
            // Checks the type of weapon to remove.
            switch (type)
            {
                case Weapon.weaponType.gunSingle:
                    // Remove gun single from the list.
                    if (weapons.Contains(gunSingle))
                        weapons.Remove(gunSingle);

                    break;
            }
        }


        // On the death of the player.
        protected override void OnDeath()
        {
            throw new System.NotImplementedException();
        }

        // Updates the inputs for the player.
        private void UpdateInputs()
        {
            // MOVEMENT INPUT
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


            // ATTACK
            // The user is attacking.
            if (Input.GetKeyDown(attackKey))
            {
                // Uses the weapon.
                if (currentWeapon != null)
                {
                    
                    // Checks if the weapon is usable.
                    if (currentWeapon.IsUsable())
                    {
                        // Uses the weapon.
                        currentWeapon.UseWeapon();

                        // TODO: this implementation isn't the best, so try to improve it.

                        // If the player can't move and attack at the same time, stop their movement.
                        if (!currentWeapon.canMoveAndAttack)
                            movement = Vector2.zero;
                    }
                        
                }
            }

            // MOVEMENT CALCULATION
            // There is movement, so apply it.
            if (movement != Vector2.zero)
            {
                // Set the face direction.
                facingDirec = movement.normalized;

                // Calculates the force.
                Vector2 force = movement * speed * Time.deltaTime;
                
                // Uses rigidbody for movement (original) (not needed) [OLD]
                rigidbody.AddForce(force, ForceMode2D.Impulse);

                // TODO: add factor for slowing down by material.

                // Clamp velocity.
                rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);

                // Uses transform for movement.
                // transform.Translate(force);
            }

            // WEAPON SWITCH
            // The player has weapons to switch to.
            if (weapons.Count > 1)
            {
                // Weapon button should be switched.
                if (Input.GetKeyDown(leftWeaponKey) || Input.GetKeyDown(rightWeaponKey))
                {
                    // Determines whether to go left or right.
                    bool left = Input.GetKeyDown(leftWeaponKey) ? true : false;

                    // The index of the new weapon.
                    int newIndex;

                    // Gets the index of the current weapon.
                    // Current weapon does not exist.
                    if (currentWeapon == null)
                    {
                        newIndex = 0;
                    }
                    else
                    {
                        // If weapon is in list, get the index.
                        if (weapons.Contains(currentWeapon))
                        {
                            // Index of the current weapon.
                            newIndex = weapons.IndexOf(currentWeapon);

                            // Adjust index
                            newIndex += (left) ? -1 : 1;

                            // Loop around to the end.
                            if(newIndex < 0)
                            {
                                newIndex = weapons.Count - 1;
                            }
                            // Loop around back to the start.
                            else if(newIndex >= weapons.Count)
                            {
                                newIndex = 0;
                            }
                        }
                        else
                        {
                            newIndex = 0;
                        }       
                    }

                    // Set the current weapon.
                    currentWeapon = weapons[newIndex];

                    // TODO: update icon.
                }
            }
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