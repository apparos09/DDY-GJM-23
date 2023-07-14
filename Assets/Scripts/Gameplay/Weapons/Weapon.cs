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
        public weaponType type = weaponType.none;

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

        // Force applied to push back enemies.
        public float knockbackForce = 250.0F;

        // If the push force should be used.
        public bool useKnockback = true;

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


        // If the weapon is usable, return true. If not, return false.
        public bool IsUsable()
        {
            if (infiniteUse || uses > 0)
                return true;
            else
                return false;

        }

        // Adds uses to the weapon.
        public void AddUses(int amount)
        {
            // If the weapon is infinite use, don't change the use count.
            if (!infiniteUse)
            {
                uses += amount;
                uses = Mathf.Clamp(uses, 0, maxUses);
            }
        }

        // Reduces uses of the weapon.
        public void RemoveUses(int amount)
        {
            // If the weapon is infinite use, don't change the use count.
            if (!infiniteUse)
            {
                uses -= amount;
                uses = Mathf.Clamp(uses, 0, maxUses);
            }
        }

        // Restores the number of uses for the weapon to its max.
        public void RestoreUsesToMax()
        {
            uses = maxUses;
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

        // Applies push force to the enemy.
        public void ApplyKnockback(Enemy enemy)
        {
            ApplyKnockback(owner.gameObject, enemy.gameObject, knockbackForce);
        }

        // Applies push force to the enemy.
        public static void ApplyKnockback(GameObject attacker, GameObject target, float knockbackForce)
        {
            // Gets the direction vector.
            Vector3 distVec = target.transform.position - attacker.transform.position;

            // No z-movement.
            distVec.z = 0.0F;

            // Move to the right.
            if (distVec == Vector3.zero)
                distVec = Vector3.right;

            // Gives the distance vector for the knockback force.
            ApplyKnockback(distVec, target, knockbackForce);
        }

        // Applies push force to the enemy.
        public static void ApplyKnockback(Vector3 direc, GameObject target, float knockbackForce)
        {
            // Apply knockback.
            // The rigidbody.
            Rigidbody2D targetBody;

            // Tries to get the rigidbody.
            if (target.TryGetComponent(out targetBody))
            {
                // Rigidbody found, so apply force.
                targetBody.AddForce(direc.normalized * knockbackForce * Time.deltaTime * 10.0F, ForceMode2D.Impulse);
            }
            else
            {
                // Just translates the target instead. This will likely never be used, so it's never been tested.

                // No rigidbody, so use translate.
                target.transform.Translate(direc.normalized * knockbackForce * Time.deltaTime);
            }

        }


        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}