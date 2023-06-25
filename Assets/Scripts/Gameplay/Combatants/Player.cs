using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// The namespace for the Doomsday Game Jam (2023).
namespace DDY_GJM_23
{
    // The player for the game.
    public class Player : Combatant
    {
        // // The amount of lives the player has.
        // public int lives = 0;

        public const string PLAYER_TAG = "Player";

        [Header("Player")]

        // The number of player's deaths.
        public int deaths = 0;

        // The amount of scrap the player has on hand.
        public int scrapCount = 0;

        // The number of keys the player has.
        public int keyCount = 0;

        // The number of the keys used.
        public int keysUsed = 0;

        // The number of heals the player has.
        public int healCount = 0;

        // The number of heals used by the player.
        public int healsUsed = 0;

        // The amount the player gets healed for when they use a heal item (percentage.
        public float healAmount = 0.25F;

        [Header("Player/Inputs")]
        // Allows the player to input commands.
        [Tooltip("Enables all player inputs.")]
        public bool enableInputs = true;

        // Allows the player to make world inputs. Non-world inputs are for things like menu screens.
        [Tooltip("Enables player inputs on the game world. Non-world inputs are those for menu and UI elements.")]
        public bool enableWorldInputs = true;

        // The movement keys.
        public KeyCode moveLeftKey = KeyCode.A;
        public KeyCode moveRightKey = KeyCode.D;
        public KeyCode moveUpKey = KeyCode.W;
        public KeyCode moveDownKey = KeyCode.S;

        // The attack key.
        public KeyCode attackKey = KeyCode.Space;

        // The left weapon key and the right weapon key.
        public KeyCode leftWeaponKey = KeyCode.LeftArrow;
        public KeyCode rightWeaponKey = KeyCode.RightArrow;

        // The map key and heal key.
        public KeyCode mapKey = KeyCode.UpArrow;
        public KeyCode healKey = KeyCode.DownArrow;

        [Header("Player/Movement")]

        // The movement speed of the player.
        [Tooltip("The movement speed.")]
        public float speed = 75.0F;

        // The max speed overall.
        [Tooltip("The overall max speed.")]
        public float maxSpeed = 15.0F;

        // The maximum run speed. Default max speed.
        [Tooltip("The max run speed.")]
        public float maxRunSpeed = 12.0F;

        // The maximum swim speed.
        [Tooltip("The max swim speed.")]
        public float maxSwimSpeed = 2.0F;

        // The direction the player is facing.
        private Vector2 facingDirec = Vector2.down;

        // The inputs for moves on a given frame.
        private Vector2 moveInputs = Vector2.zero;

        // Weapons
        [Header("Player/Weapons")]

        // The current weapon of the player.
        public Weapon currentWeapon;

        // The list of weapons.
        public List<Weapon> weapons;

        // The punch weapon.
        public Punch punch = null;
        
        // The slow gun.
        public GunSingle gunSlow = null;

        // The mid gun.
        public GunSingle gunMid = null;

        // The fast gun.
        public GunSingle gunFast = null;

        // The run power.
        public Punch runPower = null;

        // The swim power.
        public Punch swimPower = null;

        

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Finding weapons
            // Punch
            if (punch == null)
                punch = GetComponentInChildren<Punch>(true);


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
            // The new weapon.
            Weapon newWeapon;

            // Checks the type of weapon to add.
            switch (type)
            {
                case Weapon.weaponType.punch: // Punch
                case Weapon.weaponType.runPower: // Run
                case Weapon.weaponType.swimPower: // Swim

                    // The punch object.
                    Punch punchWeapon = null;

                    // Checks the type of punch being used.
                    switch (type)
                    {
                        case Weapon.weaponType.punch:
                            punchWeapon = punch;
                            break;

                        case Weapon.weaponType.runPower:
                            punchWeapon = runPower;
                            break;

                        case Weapon.weaponType.swimPower:
                            punchWeapon = swimPower;
                            break;
                    }

                    // New weapon set.
                    newWeapon = punchWeapon;

                    break;

                case Weapon.weaponType.gunSlow: // Slow Gun
                case Weapon.weaponType.gunMid: // Mid Gun
                case Weapon.weaponType.gunFast: // Fast Gun

                    // The gun object.
                    GunSingle gun = null;

                    // Checks the type of gun being used.
                    switch (type)
                    {
                        case Weapon.weaponType.gunSlow:
                            gun = gunSlow;
                            break;

                        case Weapon.weaponType.gunMid:
                            gun = gunMid;
                            break;

                        case Weapon.weaponType.gunFast:
                            gun = gunFast;
                            break;
                    }

                    // Set the new weapon.
                    newWeapon = gun;

                    break;

                default:
                    newWeapon = null;
                    break;
            }

            // Put the newWeapon in the list if it exists.
            if (newWeapon != null)
            {
                // The new weapon isn't in the list, so add it.
                if (!weapons.Contains(newWeapon))
                {
                    weapons.Add(newWeapon);
                    newWeapon.owner = this;
                }

                // Give the weapon its max bullet count.
                newWeapon.RestoreUsesToMax();
            }
        }

        // Removes a weapon from the player.
        public void RemoveWeapon(Weapon.weaponType type)
        {
            // The weapon to be removed.
            Weapon weapon = null;

            // Checks the type of weapon to remove.
            switch (type)
            {
                case Weapon.weaponType.punch:
                    weapon = punch;
                    break;

                case Weapon.weaponType.gunSlow:
                    weapon = gunSlow;
                    break;

                case Weapon.weaponType.gunMid:
                    weapon = gunMid;
                    break;

                case Weapon.weaponType.gunFast:
                    weapon = gunFast;
                    break;

                case Weapon.weaponType.runPower:
                    weapon = runPower;
                    break;

                case Weapon.weaponType.swimPower:
                    weapon = swimPower;
                    break;

                default:
                    weapon = null;
                    break;
            }

            // If the weapon is in the list.
            if(weapons.Contains(weapon))
            {
                // Remove the weapon.
                weapons.Remove(weapon);

                // If this is the current weapon.
                if(currentWeapon == weapon)
                {
                    // Set to null.
                    currentWeapon = null;
                }
            }
        }

        // Has the weapon.
        public bool HasWeapon(Weapon weapon)
        {
            // Checks if the weapon is in the list.
            bool result = weapons.Contains(weapon);

            // Returns the result.
            return result;
        }

        // Has the weapon.
        public bool HasWeapon(Weapon.weaponType type)
        {
            bool result = false;

            // Goes through each weapon.
            foreach (Weapon weapon in weapons)
            {
                // Weapon exists.
                if (weapon != null)
                {
                    // If the types match.
                    if (weapon.type == type)
                    {
                        result = true;
                        break;
                    }

                }
            }

            // Returns the result.
            return result;
        }


        // Sets the weapon using the provided index. Sets weapon to null if index is invalid.
        public void SetWeapon(int index)
        {
            // May get set by weapon.
            poisonImmune = false;

            // Valid index.
            if(index >= 0 && index < weapons.Count) 
            {
                currentWeapon = weapons[index];
            }
            else
            {
                currentWeapon = null;
            }

            // Checks if the weapon exists.
            if(currentWeapon != null)
            {
                // Checks if the weapon gives the player poison immunity.
                if (currentWeapon.type == Weapon.weaponType.swimPower)
                    poisonImmune = true;
                else
                    poisonImmune = false;
            }

            // Grabs game UI.
            GameplayUI gameUI = GameplayManager.Instance.gameUI;

            // Update the weapon.
            if (gameUI != null)
                gameUI.UpdateWeaponInfo();
        }

        // Gives weapon uses to the player.
        public void GiveWeaponUses(Weapon.weaponType type, int uses)
        {
            // Checks the weapon type.
            if(type == Weapon.weaponType.none) // No type, so refill set weapon.
            {
                // Provide uses to the weapon.
                if (currentWeapon != null)
                {
                    currentWeapon.AddUses(uses);
                }
            }
            else
            {
                // Goes through each weapon.
                foreach(Weapon weapon in weapons)
                {
                    // The types match.
                    if(weapon.type == type)
                    {
                        weapon.AddUses(uses);
                        break;
                    }
                }
            }
        }


        // Restores the amount of uses to all of the player's weapons.
        public void RestoreAllWeaponUsesToMax()
        {
            // Restores each weapon.
            foreach(Weapon weapon in weapons)
            {
                weapon.RestoreUsesToMax();
            }
        }

        // On the death of the player.
        protected override void OnDeath()
        {
            // Increase death count.
            deaths++;

            // The player loses all their scraps, keys, and heals.
            scrapCount = 0;
            keyCount = 0;
            healCount = 0;

            // Gets the gameplay manager.
            GameplayManager gameManager = GameplayManager.Instance;

            // The home base has been set, so go to it.
            if(gameManager.homeBase != null)
            {
                // Set to max health.
                SetHealthToMax();

                // Return to home base.
                transform.position = gameManager.homeBase.transform.position;
            }
        }

        // Updates the inputs for the player.
        private void UpdateInputs()
        {
            // MOVEMENT INPUT
            // The movement directions.
            Vector2 movement = Vector2.zero;

            // TODO: find a way to have gradual slow down for tile movement instead of stopping instantly.

            // Movement - Horizontal (Left, Right)
            if(Input.GetKey(moveLeftKey) && enableWorldInputs) // Left
            {
                movement.x = -1;
            }
            else if(Input.GetKey(moveRightKey) && enableWorldInputs) // Right
            {
                movement.x = 1;
            }
            //else if(Input.GetKeyUp(moveLeftKey) || Input.GetKeyUp(moveRightKey))
            //{
            //    // Stop horizontal velocity.
            //    Vector2 velocity = new Vector2(0, rigidbody.velocity.y);
            //    rigidbody.velocity = velocity;
            //}


            // Movement - Vertical (Up, Down)
            if (Input.GetKey(moveUpKey) && enableWorldInputs) // Up
            {
                movement.y = 1;
            }
            else if (Input.GetKey(moveDownKey) && enableWorldInputs) // Down
            {
                movement.y = -1;
            }
            //else if (Input.GetKeyUp(moveUpKey) || Input.GetKeyUp(moveDownKey))
            //{
            //    // Stop vertical velocity.
            //    Vector2 velocity = new Vector2(rigidbody.velocity.x, 0);
            //    rigidbody.velocity = velocity;
            //}


            // ATTACK
            // The user is attacking.
            if (Input.GetKeyDown(attackKey) && enableWorldInputs)
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

                // Averages out the friction.
                float friction = GetAveragedFrictionFactor();

                // There is friction be applied.
                if(friction > 0)
                {
                    // Calculates the friction force.
                    float fricForce = 1 / (friction);

                    // Alter the player's force.
                    force *= fricForce;
                }
                
                // Uses rigidbody for movement (original) (not needed) [OLD]
                rigidbody.AddForce(force, ForceMode2D.Impulse);


                // CLAMPING SPEED
                if(currentTiles.Count > 0) // On tiles
                {
                    // The speeds are set.
                    if (maxRunSpeed != 0 && maxSwimSpeed != 0)
                    {
                        // The number of run and swim tiles.
                        int runTiles = GetSolidTileCount();
                        int swimTiles = GetLiquidTileCount();

                        // Checks if the run and swim powers are on.
                        bool runPowerOn = currentWeapon.type == Weapon.weaponType.runPower;
                        bool swimPowerOn = currentWeapon.type == Weapon.weaponType.swimPower;

                        // The run and swim speeds.
                        // If the player has certain items equipped, then they move at maxSpeed.
                        float runSpeed = (runPowerOn) ? maxSpeed : maxRunSpeed;
                        float swimSpeed = (swimPowerOn) ? maxSpeed : maxSwimSpeed;

                        // Calculates the mixed speed.
                        float mixedSpeed = (runSpeed * runTiles + swimSpeed * swimTiles) / (runTiles + swimTiles);

                        // Add force again.

                        // If the run power is on, and the player is on run tiles.
                        if (runPowerOn && runTiles != 0)
                            rigidbody.AddForce(force * 1.25F, ForceMode2D.Impulse);

                        // If the swim power is on, and the player is on swim tiles.
                        if(swimPowerOn && swimTiles != 0)
                            rigidbody.AddForce(force * 1.5F, ForceMode2D.Impulse);

                        // Clamp velocity.
                        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, mixedSpeed);
                    }
                    else
                    {
                        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);
                    }
                }
                else // No tiles
                {
                    rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);
                }
                
                

                // Uses transform for movement.
                // transform.Translate(force);
            }

            // Saves the inputs from the player.
            moveInputs = movement.normalized;

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
                    SetWeapon(newIndex);
                }
            }

            // OTHER //

            // Heal
            if (Input.GetKeyDown(healKey) && enableWorldInputs)
            {
                // If the player has heals, reduce the count and increase their health.
                if(healCount > 0)
                {
                    healCount--;
                    healsUsed++;
                    health += maxHealth * healAmount;
                }
            }

            // Toggle Map (this is NOT a world input).
            if (Input.GetKeyDown(mapKey))
            {
                // Turn on the map.
                GameplayManager.Instance.ToggleMap();
            }
            
        }

        // Update is called once per frame
        protected override void Update()
        {
            // The player hasn't moved yet for this frame.
            moveInputs = Vector2.zero;

            // Base update.
            base.Update();

            // Updates the player's inputs.
            if(enableInputs)
            {
                UpdateInputs();
            }


            // Checks if the player's velocity is not zero.
            if (rigidbody.velocity != Vector2.zero)
            {
                // Checks if the player moved on a given axis.
                bool movedX = moveInputs.x != 0;
                bool movedY = moveInputs.y != 0;

                // If the player did not move on on axis.
                if (!movedX || !movedY)
                {
                    // The new velocity.
                    Vector2 newVelocity = rigidbody.velocity;

                    // Gets the resulting velocity.
                    Vector2 result = GetVelocityWithFriction(rigidbody.velocity, rigidbody.mass);


                    // Checks if the new velocity changed anything.
                    if(newVelocity == rigidbody.velocity) // Did not change.
                    {
                        // No x-movement, so set to 0.
                        if (!movedX)
                            newVelocity.x = 0;

                        // No y-movement, so set to 0.
                        if (!movedY)
                            newVelocity.y = 0;
                    }
                    else // Changed
                    {
                        // Reduce x velocity.
                        if (!movedX && rigidbody.velocity.x != 0)
                            newVelocity.x = result.x;

                        // Reduce y velocity.
                        if (!movedY && rigidbody.velocity.y != 0)
                            newVelocity.y = result.y;
                    }

                    // Set the velocity.
                    rigidbody.velocity = newVelocity;
                }
            }
                
        }
    }
}