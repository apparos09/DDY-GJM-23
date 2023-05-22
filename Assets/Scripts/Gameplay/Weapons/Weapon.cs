using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The weapons class.
    public abstract class Weapon : MonoBehaviour
    {
        // The weapon enum type.
        public enum weaponType { none, punch }

        // The type of the weapon.
        protected weaponType weapon = weaponType.none;

        // The number of uses a weapon has.
        public int uses = -1;

        // If 'true', a weapon can be used indefinitely.
        public bool infiniteUse = false;

        [Header("Other")]

        // OTHER
        // The owner of the weapon.
        public Player owner;

        // Constructor
        public Weapon(weaponType type, Player owner)
        {
            // Sets the weapon type.
            weapon = type;

            // Sets the owner.
            this.owner = owner;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // Returns the weapon type.
        public weaponType WeaponType
        {
            get { return weapon; }
        }

        // If the weapon is usable, return true. If not, return false.
        public bool IsUsable()
        {
            if (infiniteUse || uses > 0)
                return true;
            else
                return false;

        }

        // Uses the weapon.
        public abstract void UseWeapon();


        // Update is called once per frame
        protected virtual void Update()
        {
        }
    }
}