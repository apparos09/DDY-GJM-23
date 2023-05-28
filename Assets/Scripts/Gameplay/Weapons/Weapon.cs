using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The weapons class.
    public abstract class Weapon : MonoBehaviour
    {
        // The weapon enum type.
        public enum weaponType { none, punch, gunSlow, gunMid, gunFast, runPower, swimPower }

        // The owner of the weapon.
        public Player owner;

        // The type of the weapon.
        public weaponType weapon = weaponType.none;

        // The power of the weapon.
        public float power = 10.0F;

        // The number of uses a weapon has.
        public int uses = -1;

        // The maximum amount of uses.
        public int maxUses = -1;

        // If 'true', a weapon can be used indefinitely.
        public bool infiniteUse = false;

        // If 'true', the player can move and attack at the same time using this weapon.
        public bool canMoveAndAttack = true;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
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

        // Restores the number of uses for the weapon to its max.
        public void RestoreUsesToMax()
        {
            uses = maxUses;
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

        // Called when a weapon was used.
        public void OnUseWeapon(int timesUsed)
        {
            // Not infinite use, so reduce uses count.
            if (!infiniteUse)
            {
                uses -= timesUsed;

                // Uses now zero.
                if (uses < 0)
                    uses = 0;
            }
        }


        // Update is called once per frame
        protected virtual void Update()
        {
        }
    }
}